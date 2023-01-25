from inc_noesis import *

DLOP_VTX = 0x04
DLOP_TRI4 = 0xB1
DLOP_CLEARGEOMETRYMODE = 0xB6
DLOP_SETGEOMETRYMODE = 0xB7
DLOP_ENDDL = 0xB8
DLOP_SETOTHERMODEL = 0xB9
DLOP_SETOTHERMODEH = 0xBA
DLOP_TEXTURE = 0xBB
DLOP_TRI1 = 0xBF
DLOP_SETENVCOLOR = 0xFB
DLOP_SETCOMBINE = 0xFC
DLOP_SETTILE = 0xF5
DLOP_INVALID = -1

DL_COMMANDSIZE = 8

DL_UCODEMODE_UC0 = 0

GEOMETRYMODE_ZBUFFER = (1 << 0)
GEOMETRYMODE_TEXTURE = (1 << 1)
GEOMETRYMODE_SHADING = (1 << 2)
GEOMETRYMODE_SHADING_SMOOTH = (1 << 9)
GEOMETRYMODE_CULL_FRONT = (1 << 12)
GEOMETRYMODE_CULL_BACK = (1 << 13)
GEOMETRYMODE_FOG = (1 << 16)
GEOMETRYMODE_LIGHTING = (1 << 17)
GEOMETRYMODE_TEXGEN = (1 << 18)
GEOMETRYMODE_TEXGEN_LINEAR = (1 << 19)

OTHERMODEL_ALPHACOMPARE = 0 #2 bits
OTHERMODEL_ZSRCSEL = 2
OTHERMODEL_RM_AA_EN = 3
OTHERMODEL_RM_Z_CMP = 4
OTHERMODEL_RM_Z_UPD = 5
OTHERMODEL_RM_IM_RD = 6
OTHERMODEL_RM_CLR_ON_CVG = 7
OTHERMODEL_RM_CVG_DST = 8 #2 bits
OTHERMODEL_RM_ZMODE = 10 #2 bits
OTHERMODEL_RM_CVG_X_ALPHA = 12
OTHERMODEL_RM_ALPHA_CVG_SEL = 13
OTHERMODEL_RM_FORCE_BL = 14
OTHERMODEL_RM_BLENDER = 16

OTHERMODEL_CVG_DST_CLAMP = 0
OTHERMODEL_CVG_DST_WRAP = 1
OTHERMODEL_CVG_DST_FULL = 2
OTHERMODEL_CVG_DST_SAVE = 3

OTHERMODEL_ZMODE_OPA = 0
OTHERMODEL_ZMODE_INTER = 1
OTHERMODEL_ZMODE_XLU = 2
OTHERMODEL_ZMODE_DEC = 3

OTHERMODEH_ALPHADITHER = 4
OTHERMODEH_RGBDITHER = 6
OTHERMODEH_COMBKEY = 8
OTHERMODEH_TEXTCONV = 9
OTHERMODEH_TEXTFILT = 12
OTHERMODEH_TEXTLUT = 14
OTHERMODEH_TEXTLOD = 16
OTHERMODEH_TEXTDETAIL = 17
OTHERMODEH_TEXTPERSP = 19
OTHERMODEH_CYCLETYPE = 20
OTHERMODEH_COLORDITHER = 22
OTHERMODEH_PIPELINE = 23

OTHERMODEH_TEXTDETAIL_CLAMP = 0
OTHERMODEH_TEXTDETAIL_SHARPEN = 1
OTHERMODEH_TEXTDETAIL_DETAIL = 2

OTHERMODEH_CYCLETYPE_1CYCLE = 0
OTHERMODEH_CYCLETYPE_2CYCLE = 1
OTHERMODEH_CYCLETYPE_COPY = 2
OTHERMODEH_CYCLETYPE_FILL = 3

COMBINER_DEFAULT = 0x00FFFFFFFFFFFFFF #all 0's by default

#this implementation is far from complete (even for a given ucode binary, but also given all of the unique ucode variations
#in the wild) and obviously can't faithfully handle lots of runtime behavior, like effects achieved by changing states between
#pipeline syncs. the idea is for this thing to also encapsulate state and handle mapping it into materials, so it's kind of a
#strange meld of very high-level rsp and rdp functionality.
#if you end up supporting more n64 games in Noesis and extend/fix stuff in this implementation as a result, let me know. as
#long as your stuff is right and you adhere to the style in here and don't go off and do pythonic-snowflake bullshit, i'll be
#happy to incorporate your changes and credit you!
class DisplayListContext:
	def __init__(self, ucMode = DL_UCODEMODE_UC0, indexDenom = 10):
		self.ucMode = ucMode
		self.indexDenom = indexDenom
		self.setDefaultCommandHandlers()
		self.setUserData(None)
		self.resetDisplayState()
		self.vertexBuffer = bytearray(0x100)
		self.vertexSize = 0
		self.setVertexReadCallback(None)
		self.setDrawTrianglesCallback(None)

	def setDefaultCommandHandlers(self):
		self.commandHandlers = {}
		self.setTerminationCode(DLOP_ENDDL)
		self.setCommandHandler(DLOP_VTX, dlHandlerVtx)
		self.setCommandHandler(DLOP_TRI4, dlHandlerTri4)
		self.setCommandHandler(DLOP_CLEARGEOMETRYMODE, dlHandlerClearGeometryMode)
		self.setCommandHandler(DLOP_SETGEOMETRYMODE, dlHandlerSetGeometryMode)
		self.setCommandHandler(DLOP_SETOTHERMODEL, dlHandlerSetOtherModeL)
		self.setCommandHandler(DLOP_SETOTHERMODEH, dlHandlerSetOtherModeH)
		self.setCommandHandler(DLOP_TEXTURE, dlHandlerTexture)
		self.setCommandHandler(DLOP_TRI1, dlHandlerTri1)
		self.setCommandHandler(DLOP_SETENVCOLOR, dlHandlerSetEnvColor)
		self.setCommandHandler(DLOP_SETCOMBINE, dlHandlerSetCombine)
		self.setCommandHandler(DLOP_SETTILE, dlHandlerSetTile)

	def setCommandHandler(self, dlOp, handler):
		self.commandHandlers[dlOp] = handler
		
	#just use DLOP_INVALID if you don't want to break on any command. in that case runCommands will run to the end of the supplied buffer.
	def setTerminationCode(self, endOp):
		self.endOp = endOp
		
	def setVertexReadCallback(self, cb):
		self.vertexReadCallback = cb

	def setDrawTrianglesCallback(self, cb):
		self.drawTrianglesCallback = cb #it's the callback method's responsibility to check getCullMode to determine winding

	def setUserData(self, userData):
		self.userData = userData
		
	def getUserData(self):
		return self.userData
		
	def getUcMode(self):
		return self.ucMode
		
	def getIndexDenom(self):
		return self.indexDenom
		
	def getCullMode(self):
		return (self.geometryMode >> 12) & 3 #just pile both cull bits in and hand it back as "mode"
		
	def getCurrentTextureTile(self):
		return self.tiles[self.texTileIndex]

	def runCommands(self, data):
		commandCount = len(data) // DL_COMMANDSIZE
		offset = 0
		while offset < commandCount * DL_COMMANDSIZE:
			cmd = data[offset]
			if cmd == self.endOp:
				break
			if cmd in self.commandHandlers:
				self.commandHandlers[cmd](self, data[offset : offset + DL_COMMANDSIZE])
			offset += DL_COMMANDSIZE
		return offset
		
	def calculateST(self, texWidth, texHeight, vertST):
		if texWidth <= 0 or texHeight <= 0:
			return vertST
		tile = self.getCurrentTextureTile()

		st = []
		texDims = (texWidth, texHeight)
		for i in range(0, 2):
			shiftScale = 1.0
			if tile.stShift[i]:
				if tile.stShift[i] > 10:
					shiftScale = (1 << (16 - tile.stShift[i]))
				else:
					shiftScale = 1.0 / (1 << tile.stShift[i])
			scale = (self.texScales[i] * shiftScale) / texDims[i]
			offset = (tile.stOffset[i] / 4.0) / texDims[i]
			st.append(vertST[i] * scale - offset)
		return st
		
	def readVertices(self, vertCount, bufferOffset, readAddr, readSize):
		self.vertexSize = readSize // vertCount
		vertData = self.vertexReadCallback(self, bufferOffset, readAddr, readSize) if self.vertexReadCallback else bytearray(readSize)
		self.vertexBuffer[bufferOffset : bufferOffset + readSize] = vertData
	
	def drawTriangles(self, drawList):
		if self.drawTrianglesCallback and len(drawList) > 0:
			self.drawTrianglesCallback(self, drawList, self.vertexBuffer, self.vertexSize)
	
	def setGeometryMode(self, geometryMode):
		self.geometryMode = geometryMode
	def getGeometryMode(self):
		return self.geometryMode
		
	def setOtherModeL(self, otherModeL):
		self.otherModeL = otherModeL
	def getOtherModeL(self):
		return self.otherModeL

	def setOtherModeH(self, otherModeH):
		self.otherModeH = otherModeH
	def getOtherModeH(self):
		return self.otherModeH
		
	def setCombineSourceBits(self, combineSourceBits):
		self.combineSourceBits = combineSourceBits
	def getCombineSourceBits(self):
		return self.combineSourceBits
		
	def setEnvColor(self, envColor):
		self.envColor = envColor
	def getEnvColor(self):
		return self.envColor
		
	def setTextureTileDescScale(self, descIndex, maxMipCount, descEnable, scaleS, scaleT):
		self.texDescEnable = descEnable
		if self.texDescEnable: #in hardware, does rest of state still update even when not enabling? just guessing here
			self.texTileIndex = descIndex
			self.texMaxMipCount = maxMipCount
			self.texScales = (
				scaleS if scaleS != 0.0 else 1.0 / 32.0,
				scaleT if scaleT != 0.0 else 1.0 / 32.0
			)
			
	def setTextureTileData(self, descIndex, colorFormat, bpp, stride, tmemAddr, palIndex):
		tile = self.tiles[descIndex]
		tile.colorFormat = colorFormat
		tile.bpp = bpp
		tile.stride = stride
		tile.tmemAddr = tmemAddr
		tile.palIndex = palIndex
		
	def setTextureTileST(self, descIndex, sMode, sMask, sShift, tMode, tMask, tShift):
		tile = self.tiles[descIndex]
		tile.stMode = (sMode, tMode)
		tile.stMask = (sMask, tMask)
		tile.stShift = (sShift, tShift)

	def setTextureTileOffset(self, descIndex, sOffset, tOffset):
		tile = self.tiles[descIndex]
		tile.stOffset = (sOffset, tOffset)
	
	def resetDisplayState(self):
		self.setGeometryMode(0)
		self.setOtherModeL(0)
		self.setOtherModeH(0)
		self.setCombineSourceBits(COMBINER_DEFAULT)
		self.setEnvColor((1.0, 1.0, 1.0, 1.0))
		self.setTextureTileDescScale(0, 1, 1, 0.0, 0.0)
		self.tiles = [DLTile() for i in range(0, 8)]
		self.texTileIndex = 0

class DLTile:
	def __init__(self):
		self.colorFormat = 0
		self.bpp = 0
		self.stride = 0
		self.tmemAddr = 0
		self.palIndex = 0
		self.stMode = (0, 0)
		self.stMask = (0, 0)
		self.stShift = (0, 0)
		self.stOffset = (0, 0)

COMBINER_SRC_COMBINEDCOLOR = 0
COMBINER_SRC_TEXEL0COLOR = 1
COMBINER_SRC_TEXEL1COLOR = 2
COMBINER_SRC_PRIMCOLOR = 3
COMBINER_SRC_SHADECOLOR = 4
COMBINER_SRC_ENVCOLOR = 5
COMBINER_SRC_KEYCENTER = 6
COMBINER_SRC_KEYSCALE = 7
COMBINER_SRC_COMBINEDALPHA = 8
COMBINER_SRC_TEXEL0ALPHA = 9
COMBINER_SRC_TEXEL1ALPHA = 10
COMBINER_SRC_PRIMALPHA = 11
COMBINER_SRC_SHADEALPHA = 12
COMBINER_SRC_ENVALPHA = 13
COMBINER_SRC_LODFRAC = 14
COMBINER_SRC_PRIMLODFRAC = 15
COMBINER_SRC_NOISE = 16
COMBINER_SRC_CONVK4 = 17
COMBINER_SRC_CONVK5 = 18
COMBINER_SRC_CONST1 = 19
COMBINER_SRC_CONST0 = 20

#combiner equation is (A - B) * C + D
class DLCombinerInterpreter:
	def __init__(self, combinerBits):
		self.bits = combinerBits

	def getColorAMapped(self, colorA):
		colorAMap = {
			0 : COMBINER_SRC_COMBINEDCOLOR,
			1 : COMBINER_SRC_TEXEL0COLOR,
			2 : COMBINER_SRC_TEXEL1COLOR,
			3 : COMBINER_SRC_PRIMCOLOR,
			4 : COMBINER_SRC_SHADECOLOR,
			5 : COMBINER_SRC_ENVCOLOR,
			6 : COMBINER_SRC_CONST1,
			7 : COMBINER_SRC_NOISE
		}
		if colorA in colorAMap:
			return colorAMap[colorA]
		return COMBINER_SRC_CONST0
	def getColorBMapped(self, colorB):
		colorBMap = {
			0 : COMBINER_SRC_COMBINEDCOLOR,
			1 : COMBINER_SRC_TEXEL0COLOR,
			2 : COMBINER_SRC_TEXEL1COLOR,
			3 : COMBINER_SRC_PRIMCOLOR,
			4 : COMBINER_SRC_SHADECOLOR,
			5 : COMBINER_SRC_ENVCOLOR,
			6 : COMBINER_SRC_KEYSCALE,
			7 : COMBINER_SRC_CONVK4
		}
		if colorB in colorBMap:
			return colorBMap[colorB]
		return COMBINER_SRC_CONST0
	def getColorCMapped(self, colorC):
		colorCMap = {
			0 : COMBINER_SRC_COMBINEDCOLOR,
			1 : COMBINER_SRC_TEXEL0COLOR,
			2 : COMBINER_SRC_TEXEL1COLOR,
			3 : COMBINER_SRC_PRIMCOLOR,
			4 : COMBINER_SRC_SHADECOLOR,
			5 : COMBINER_SRC_ENVCOLOR,
			6 : COMBINER_SRC_KEYSCALE,
			7 : COMBINER_SRC_COMBINEDALPHA,
			8 : COMBINER_SRC_TEXEL0ALPHA,
			9 : COMBINER_SRC_TEXEL1ALPHA,
			10 : COMBINER_SRC_PRIMALPHA,
			11 : COMBINER_SRC_SHADEALPHA,
			12 : COMBINER_SRC_ENVALPHA,
			13 : COMBINER_SRC_LODFRAC,
			14 : COMBINER_SRC_PRIMLODFRAC,
			15 : COMBINER_SRC_CONVK5
		}
		if colorC in colorCMap:
			return colorCMap[colorC]
		return COMBINER_SRC_CONST0
	def getColorDMapped(self, colorD):
		colorDMap = {
			0 : COMBINER_SRC_COMBINEDCOLOR,
			1 : COMBINER_SRC_TEXEL0COLOR,
			2 : COMBINER_SRC_TEXEL1COLOR,
			3 : COMBINER_SRC_PRIMCOLOR,
			4 : COMBINER_SRC_SHADECOLOR,
			5 : COMBINER_SRC_ENVCOLOR,
			6 : COMBINER_SRC_CONST1
		}
		if colorD in colorDMap:
			return colorDMap[colorD]
		return COMBINER_SRC_CONST0
		
	def getAlphaABDMapped(self, alphaA):
		alphaAMap = {
			0 : COMBINER_SRC_COMBINEDALPHA,
			1 : COMBINER_SRC_TEXEL0ALPHA,
			2 : COMBINER_SRC_TEXEL1ALPHA,
			3 : COMBINER_SRC_PRIMALPHA,
			4 : COMBINER_SRC_SHADEALPHA,
			5 : COMBINER_SRC_ENVALPHA,
			6 : COMBINER_SRC_CONST1
		}
		if alphaA in alphaAMap:
			return alphaAMap[alphaA]
		return COMBINER_SRC_CONST0
	def getAlphaCMapped(self, alphaC):
		alphaCMap = {
			0 : COMBINER_SRC_LODFRAC,
			1 : COMBINER_SRC_TEXEL0ALPHA,
			2 : COMBINER_SRC_TEXEL1ALPHA,
			3 : COMBINER_SRC_PRIMALPHA,
			4 : COMBINER_SRC_SHADEALPHA,
			5 : COMBINER_SRC_ENVALPHA,
			6 : COMBINER_SRC_PRIMLODFRAC
		}
		if alphaC in alphaCMap:
			return alphaCMap[alphaC]
		return COMBINER_SRC_CONST0

	def getMode1ColorA(self):
		return (self.bits >> 52) & 15
	def getMode1ColorB(self):
		return (self.bits >> 28) & 15
	def getMode1ColorC(self):
		return (self.bits >> 47) & 31
	def getMode1ColorD(self):
		return (self.bits >> 15) & 7

	def getMode2ColorA(self):
		return (self.bits >> 37) & 15
	def getMode2ColorB(self):
		return (self.bits >> 24) & 15
	def getMode2ColorC(self):
		return (self.bits >> 32) & 31
	def getMode2ColorD(self):
		return (self.bits >> 6) & 7
		
	def getMode1AlphaA(self):
		return (self.bits >> 44) & 7
	def getMode1AlphaB(self):
		return (self.bits >> 12) & 7
	def getMode1AlphaC(self):
		return (self.bits >> 41) & 7
	def getMode1AlphaD(self):
		return (self.bits >> 9) & 7

	def getMode2AlphaA(self):
		return (self.bits >> 21) & 7
	def getMode2AlphaB(self):
		return (self.bits >> 3) & 7
	def getMode2AlphaC(self):
		return (self.bits >> 18) & 7
	def getMode2AlphaD(self):
		return self.bits & 7

	def getMode1ColorAMapped(self):
		return self.getColorAMapped(self.getMode1ColorA())
	def getMode1ColorBMapped(self):
		return self.getColorBMapped(self.getMode1ColorB())
	def getMode1ColorCMapped(self):
		return self.getColorCMapped(self.getMode1ColorC())
	def getMode1ColorDMapped(self):
		return self.getColorDMapped(self.getMode1ColorD())

	def getMode2ColorAMapped(self):
		return self.getColorAMapped(self.getMode2ColorA())
	def getMode2ColorBMapped(self):
		return self.getColorBMapped(self.getMode2ColorB())
	def getMode2ColorCMapped(self):
		return self.getColorCMapped(self.getMode2ColorC())
	def getMode2ColorDMapped(self):
		return self.getColorDMapped(self.getMode2ColorD())

	def getMode1AlphaAMapped(self):
		return self.getAlphaABDMapped(self.getMode1AlphaA())
	def getMode1AlphaBMapped(self):
		return self.getAlphaABDMapped(self.getMode1AlphaB())
	def getMode1AlphaCMapped(self):
		return self.getAlphaCMapped(self.getMode1AlphaC())
	def getMode1AlphaDMapped(self):
		return self.getAlphaABDMapped(self.getMode1AlphaD())

	def getMode2AlphaAMapped(self):
		return self.getAlphaABDMapped(self.getMode2AlphaA())
	def getMode2AlphaBMapped(self):
		return self.getAlphaABDMapped(self.getMode2AlphaB())
	def getMode2AlphaCMapped(self):
		return self.getAlphaCMapped(self.getMode2AlphaC())
	def getMode2AlphaDMapped(self):
		return self.getAlphaABDMapped(self.getMode2AlphaD())
		
#pulls relevant info from the dlc in order to construct a unique material on demand.
#nowhere near level of completeness required to handle various multi-pass, blending, and other state requirements.
class DLMaterialEncapsulation:
	StateFlag_AlphaBlend = (1 << 0)
	StateFlag_AlphaBlendWithWrite = (1 << 1)
	StateFlag_AlphaTest = (1 << 2)
	StateFlag_AnyAlpha = StateFlag_AlphaBlend | StateFlag_AlphaBlendWithWrite | StateFlag_AlphaTest
	StateFlag_NoTexture = (1 << 3)

	GameHack_None = 0
	
	def __init__(self, dlc, gameHack = GameHack_None):
		ol = dlc.getOtherModeL()
		oh = dlc.getOtherModeH()
		tile = dlc.getCurrentTextureTile()
		self.gameHack = gameHack
		self.tmemAddr = tile.tmemAddr if dlc.texDescEnable else -1
		self.stMode = tile.stMode
		self.flags = 0
		self.flags2 = 0
		self.stateFlags = 0
		self.defaultBlend = 0
		self.blendSrc = ""
		self.blendDst = ""
		self.alphaTest = 0.0
		self.rgbaScale = [1.0, 1.0, 1.0, 1.0]
		
		zWrite = (ol >> OTHERMODEL_RM_Z_UPD) & 1
		alphaAsCvg = (ol >> OTHERMODEL_RM_ALPHA_CVG_SEL) & 1
		if dlc.getCullMode() == 0:
			self.flags |= noesis.NMATFLAG_TWOSIDED
		self.zMode = (ol >> OTHERMODEL_RM_ZMODE) & 3
		if self.zMode == OTHERMODEL_ZMODE_DEC:
			self.flags2 |= noesis.NMATFLAG2_DECAL
		
		combinerBits = dlc.getCombineSourceBits()
		if combinerBits != COMBINER_DEFAULT:
			ci = DLCombinerInterpreter(combinerBits)
			cycleType = (oh >> OTHERMODEH_CYCLETYPE) & 3
			is2Cycle = cycleType == OTHERMODEH_CYCLETYPE_2CYCLE
			if is2Cycle:
				#hack just to cover a common case
				if ci.getMode2ColorCMapped() == COMBINER_SRC_ENVCOLOR:
					self.rgbaScale[:3] = dlc.getEnvColor()[:3]
				if ci.getMode2AlphaCMapped() == COMBINER_SRC_ENVALPHA:
					self.rgbaScale[3] = dlc.getEnvColor()[3]
			else:
				if ci.getMode1ColorAMapped() == COMBINER_SRC_CONST0 and ci.getMode1ColorBMapped() == COMBINER_SRC_CONST0 and ci.getMode1ColorCMapped() == COMBINER_SRC_CONST0:
					dSrc = ci.getMode1ColorDMapped()
					if dSrc != COMBINER_SRC_TEXEL0COLOR and dSrc != COMBINER_SRC_TEXEL1COLOR:
						self.stateFlags |= DLMaterialEncapsulation.StateFlag_NoTexture

		#todo - add some hardcoded blending modes based on states
		if ((ol >> OTHERMODEL_RM_FORCE_BL) & 1) != 0:
			if (self.flags2 & noesis.NMATFLAG2_DECAL) and not zWrite:
				self.blendSrc = "GL_SRC_ALPHA"
				self.blendDst = "GL_ONE_MINUS_SRC_ALPHA"
				self.stateFlags |= DLMaterialEncapsulation.StateFlag_AlphaBlend
			else:
				self.stateFlags |= DLMaterialEncapsulation.StateFlag_AlphaBlendWithWrite
				self.defaultBlend = 1
				self.alphaTest = 0.002
		elif not alphaAsCvg:
			self.defaultBlend = 1
			self.stateFlags |= DLMaterialEncapsulation.StateFlag_AlphaTest
		elif combinerBits == COMBINER_DEFAULT and self.flags2 & noesis.NMATFLAG2_DECAL:
			#if set up as decal but no combiner setting, assume default blend
			self.defaultBlend = 1
			self.stateFlags |= DLMaterialEncapsulation.StateFlag_AlphaTest

	def generateSTModeSuffix(self):
		return "st%i,%i"%(self.stMode[0], self.stMode[1])

	def generateMaterialName(self):
		return "t%X-f1%i-f2%i-sf%i-"%(self.tmemAddr, self.flags, self.flags2, self.stateFlags) + self.generateSTModeSuffix()
		
	def getStateFlags(self):
		return self.stateFlags

	def getRgbaScale(self):
		return self.rgbaScale
		
	def generateMaterial(self):
		mat = NoeMaterial(self.generateMaterialName(), "")
		mat.setFlags(self.flags)
		mat.setFlags2(self.flags2)
		mat.setDefaultBlend(self.defaultBlend)
		if len(self.blendSrc) > 0 and len(self.blendDst) > 0:
			mat.setBlendMode(self.blendSrc, self.blendDst)
		if self.alphaTest != 0.0:
			mat.setAlphaTest(self.alphaTest)
			
		return mat
		
	def setTextureWrapFlags(self, tex):
		if self.stMode[0] == 1:
			tex.flags |= noesis.NTEXFLAG_WRAP_MIRROR_REPEAT
		elif self.stMode[0] == 2:
			tex.flags |= noesis.NTEXFLAG_WRAP_CLAMP
		else:
			tex.flags |= noesis.NTEXFLAG_WRAP_REPEAT
		if self.stMode[0] != self.stMode[1]:
			tex.flags |= noesis.NTEXFLAG_WRAP_SEP_ST
			if self.stMode[1] == 1:
				tex.flags |= noesis.NTEXFLAG_WRAP_T_MIRROR_REPEAT
			elif self.stMode[1] == 2:
				tex.flags |= noesis.NTEXFLAG_WRAP_T_CLAMP
			else:
				tex.flags |= noesis.NTEXFLAG_WRAP_T_REPEAT
		
def otherModeHandlerGeneric(setFunction, getFunction, data):
	shift, count, bits = noeUnpack(">BBI", data[2:])
	mask = ((1 << count) - 1) << shift
	setFunction((getFunction() & ~mask) | bits)

def dlHandlerVtx(dlc, data):
	vertCountOffset, readSize, readAddr = noeUnpack(">BHI", data[1:])
	vertCount = (vertCountOffset >> 4) + 1
	bufferOffset = (vertCountOffset & 15) << 4
	dlc.readVertices(vertCount, bufferOffset, readAddr, readSize)

def dlHandlerTri4(dlc, data):
	drawList = []
	for i in range(0, 4):
		xy = data[7 - i]
		z = data[3 - (i >> 1)]
		if i & 1:
			z >>= 4
		else:
			z &= 15
		x = xy & 15
		y = xy >> 4
		if x != y or x != z:
			drawList.append((x, y, z))
	dlc.drawTriangles(drawList)
	
def dlHandlerClearGeometryMode(dlc, data):
	dlc.setGeometryMode(dlc.getGeometryMode() & ~noeUnpack(">I", data[4:])[0])

def dlHandlerSetGeometryMode(dlc, data):
	dlc.setGeometryMode(noeUnpack(">I", data[4:])[0])

def dlHandlerSetOtherModeL(dlc, data):
	otherModeHandlerGeneric(dlc.setOtherModeL, dlc.getOtherModeL, data)

def dlHandlerSetOtherModeH(dlc, data):
	otherModeHandlerGeneric(dlc.setOtherModeH, dlc.getOtherModeH, data)

def dlHandlerTexture(dlc, data):
	levelDescBits, enableBits, scaleS, scaleT = noeUnpack(">BBhh", data[2:])
	scales = (scaleS, scaleT)
	fScales = []
	scaleIToF = 1.0 / 2097152.0
	for i in range(0, 2):
		scale = scales[i]
		if scale == -1:
			fScale = 1.0 / 32.0
		elif scale == -32768:
			fScale = 1.0 / 64.0
		else:
			fScale = scale * scaleIToF
		fScales.append(fScale)
	descIndex = (levelDescBits & 7)
	maxMipCount = ((levelDescBits >> 3) & 7) + 1
	descEnable = (enableBits & 1)
	dlc.setTextureTileDescScale(descIndex, maxMipCount, descEnable, fScales[0], fScales[1])

def dlHandlerTri1(dlc, data):
	d = dlc.getIndexDenom()
	x = data[5] // d
	y = data[6] // d
	z = data[7] // d
	if x != y or x != z:
		dlc.drawTriangles([(x, y, z)])

def dlHandlerSetEnvColor(dlc, data):
	dlc.setEnvColor((data[4] / 255.0, data[5] / 255.0, data[6] / 255.0, data[7] / 255.0))
	
def dlHandlerSetCombine(dlc, data):
	dlc.setCombineSourceBits(noeUnpack(">Q", data)[0] & 0x00FFFFFFFFFFFFFF)

def dlHandlerSetTile(dlc, data):
	d0, d1 = noeUnpack(">II", data)
	colorFormat = (d0 >> 8) & 7
	bpp = (d0 >> 11) & 3
	stride = ((d0 >> 14) & 511) << 3 #in bytes, after shift
	tmemAddr = (d0 >> 23) & 511
	descIndex = (d1 >> 5) & 7
	palIndex = (d1 >> 8) & 15
	tMode = (d1 >> 12) & 3
	tMask = (d1 >> 14) & 15
	tShift = (d1 >> 18) & 15
	sMode = (d1 >> 22) & 3
	sMask = (d1 >> 24) & 15
	sShift = (d1 >> 28) & 15
	dlc.setTextureTileData(descIndex, colorFormat, bpp, stride, tmemAddr, palIndex)
	dlc.setTextureTileST(descIndex, sMode, sMask, sShift, tMode, tMask, tShift)
