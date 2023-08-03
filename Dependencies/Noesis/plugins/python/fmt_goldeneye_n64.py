#basic GoldenEye N64 support for Noesis. thanks to Zoinkity for making a lot of the specs used here available.
#the rest (like the animations) was reverse engineered with a debugger-enabled (and violently hacked to make the debugger usable) build of Mupen64Plus.
from inc_noesis import *
from inc_n64 import DisplayListContext, DLMaterialEncapsulation
import os

#can potentially support different rom images/regions just by adding tuple entries, e.g.
#(0x21990, 0x1F850, 0x219D0) #U, E, J
DATA_SEGMENT_OFFSETS = (0x21990,)
DATA_SEGMENT_ADDRESSES = (0x80020D90,)

RESOURCE_TABLE_OFFSETS = (0x252C4,)
PROP_TABLE_OFFSETS = (0x19498,)
CHAR_TABLE_OFFSETS = (0x1D080,)
GUN_TABLE_OFFSETS = (0x12B94,)
AMMO_TABLE_OFFSETS = (0x116DC,)
IMAGE_TABLE_OFFSETS = (0x28570,)

ANIM_TYPE0_HEADERS_OFFSETS = (0x28E980,)
ANIM_TYPE0_ROTDATA_OFFSETS = (0x124AC0,)
ANIM_TYPE0_HEADEROFFSETS_OFFSETS = (0x8FDC,)
ANIM_TYPE0_NAME = "AnimBundle_Type0.ge64_anm"

IMAGE_BUNDLE_ID = "GE64_IMG_BUNDLE "
IMAGE_BUNDLE_VERSION = 1
IMAGE_BUNDLE_NAME = "ImageBundle.ge64_img"

ANIM_BUNDLE_ID = "GE64_ANM"
ANIM_BUNDLE_VERSION = 1
ANIM_BUNDLE_MAPPING_TYPE_0 = 0

MODEL_HEADER_ID = "GE64_MDL"
MODEL_HEADER_VERSION = 1

RESTYPE_UNSPECIFIED = 0xFFFFFFFF
RESTYPE_PROP = 0
RESTYPE_CHAR = 1
RESTYPE_GUN = 2
RESTYPE_AMMO = 3

def registerNoesisTypes():
	handle = noesis.register("GoldenEye N64 ROM", ".z64")
	noesis.setHandlerExtractArc(handle, geExtractArc)
	
	handle = noesis.register("GoldenEye N64 Images", ".ge64_img")
	noesis.setHandlerTypeCheck(handle, imgCheckType)
	noesis.setHandlerLoadRGBA(handle, imgLoadRGBA)
	
	handle = noesis.register("GoldenEye N64 Map", ".ge64_bg")
	noesis.setHandlerTypeCheck(handle, bgCheckType)
	noesis.setHandlerLoadModel(handle, bgLoadModel)

	handle = noesis.register("GoldenEye N64 Model", ".ge64_mdl")
	noesis.setHandlerTypeCheck(handle, mdlCheckType)
	noesis.setHandlerLoadModel(handle, mdlLoadModel)
	noesis.addOption(handle, "-ge64minlod", "ignore nodes with min lod dist > <arg>.", noesis.OPTFLAG_WANTARG)
	noesis.addOption(handle, "-ge64head", "loads a head model.", noesis.OPTFLAG_WANTARG)

	handle = noesis.register("GoldenEye N64 Animations", ".ge64_anm")
	noesis.setHandlerTypeCheck(handle, anmCheckType)
	noesis.setHandlerLoadModel(handle, anmLoadModel)
	noesis.addOption(handle, "-ge64anmmdl", "specify model to pair with animation.", noesis.OPTFLAG_WANTARG)
	noesis.addOption(handle, "-ge64anmflip", "use alternate direction mapping.", 0)
	
	return 1

def geGetRegionIndex(f):
	for regionIndex in range(0, len(DATA_SEGMENT_OFFSETS)):
		segOffset = DATA_SEGMENT_OFFSETS[regionIndex]
		f.seek(segOffset, os.SEEK_SET)
		markerTest = f.read(2)
		if markerTest[0] == 0x11 and markerTest[1] == 0x72:
			return regionIndex
	return -1

def geDecompress(data, offset):
	if data[offset] != 0x11 or data[offset + 1] != 0x72:
		return None
	seg = data[offset + 2 :]
	windowSize = -15
	return rapi.decompInflate(seg, rapi.getInflatedSize(seg, windowSize), windowSize)

class ImageTableEntry:
	def __init__(self, bs, romOffset):
		self.romOffset = romOffset
		self.f0 = bs.readUByte()
		self.f1 = bs.readUByte()
		self.imageSize = bs.readUShort()
		self.f2 = bs.readUByte()
		self.f3 = bs.readUByte()
		self.f4 = bs.readUByte()
		self.f5 = bs.readUByte()

	def isValid(self):
		return self.imageSize != 0xFFFF

class ImageBundle:
	def __init__(self, data):
		self.data = data
		self.bs = NoeBitStream(self.data, NOE_BIGENDIAN)
		self.entries = []

	def parseHeader(self):
		bs = self.bs
		try:
			if len(self.data) <= 0x10:
				return 0
			id = bs.readBytes(len(IMAGE_BUNDLE_ID))
			if id != IMAGE_BUNDLE_ID.encode("ASCII"):
				return 0
			self.ver = bs.readUInt()
			if self.ver != IMAGE_BUNDLE_VERSION:
				return 0
			bs.readUInt()
			bs.readUInt()
			bs.readUInt()
			self.imageCount = bs.readUInt()
			self.imageTableSize = bs.readUInt()
			self.imageRomSize = bs.readUInt()
			if self.imageCount <= 0 or self.imageTableSize <= 0 or self.imageRomSize <= 0:
				return 0
			self.imageTableOffset = bs.tell()
			self.imageRomOffset = self.imageTableOffset + self.imageTableSize
			return 1
		except:
			pass
		return 0

	def parseEntries(self):
		bs = self.bs
		bs.seek(self.imageTableOffset, NOESEEK_ABS)
		curImageOffset = 0
		for entryIndex in range(0, self.imageCount):
			entry = ImageTableEntry(bs, curImageOffset)
			if not entry.isValid():
				break
			curImageOffset += entry.imageSize
			self.entries.append(entry)
			
	def loadTexture(self, entryIndex, imageDecodeFlags):
		if entryIndex < 0 or entryIndex >= len(self.entries):
			return None, 0
		entry = self.entries[entryIndex]
		imageOffset = self.imageRomOffset + entry.romOffset
		imageData = self.data[imageOffset : imageOffset + entry.imageSize]
		width, height, memoryWidth, rgba = rapi.callExtensionMethod("ge64_imgdecode", imageData, imageDecodeFlags)
		texName = "ge64_img_%04i"%entryIndex
		return NoeTexture(texName, memoryWidth, height, rgba, noesis.NOESISTEX_RGBA32), width
	
def imgCheckType(data):
	imgBundle = ImageBundle(data)
	if not imgBundle.parseHeader():
		return 0
	return 1
	
def imgLoadRGBA(data, texList):
	imgBundle = ImageBundle(data)
	imgBundle.parseHeader()
	imgBundle.parseEntries()
	imageDecodeFlags = 0
	for entryIndex in range(0, len(imgBundle.entries)):
		tex, usableWidth = imgBundle.loadTexture(entryIndex, imageDecodeFlags)
		texList.append(tex)
	return 1
	
def tryToLoadImageBundle():
	loadPath = rapi.getDirForFilePath(rapi.getLastCheckedName())
	filePath = os.path.join(loadPath, IMAGE_BUNDLE_NAME)
	if not os.path.exists(filePath):
		filePath = os.path.join(loadPath, "..\\" + IMAGE_BUNDLE_NAME)
	if os.path.exists(filePath):
		with open(filePath, "rb") as f:
			imgBundle = ImageBundle(f.read())
			if imgBundle.parseHeader():
				imgBundle.parseEntries()
				return imgBundle
	return None

class StandardVertex:
	def __init__(self, data):
		posX, posY, posZ, resv, s, t = noeUnpack(">hhhhhh", data[:12])
		self.pos = (posX, posY, posZ)
		self.st = (s, t)
		self.color = (data[12] / 255.0, data[13] / 255.0, data[14] / 255.0, data[15] / 255.0)

class BgRoom:
	def __init__(self, bg, bs):
		self.bg = bg
		self.pointDataOffset = bs.readUInt() & 0xFFFFFF
		self.disp0DataOffset = bs.readUInt() & 0xFFFFFF
		self.disp1DataOffset = bs.readUInt() & 0xFFFFFF
		self.pos = NoeVec3((bs.readFloat(), bs.readFloat(), bs.readFloat()))
		self.pointData = None
		self.disp0Data = None
		self.disp1Data = None

	def loadData(self, data):
		self.pointData = geDecompress(data, self.pointDataOffset)
		if self.disp0DataOffset:
			self.disp0Data = geDecompress(data, self.disp0DataOffset)
		if self.disp1DataOffset:
			self.disp1Data = geDecompress(data, self.disp1DataOffset)
	
class GEResource:
	def __init__(self, data):
		self.data = data
		self.bs = NoeBitStream(self.data, NOE_BIGENDIAN)
		self.materials = []
		self.textures = []
		self.setImageBundle(None)
		self.matDict = {} #name to index
		self.texDict = {} #bundle id to index
		self.resInit()
		
	def resInit(self):
		pass
		
	def setImageBundle(self, imageBundle):
		self.imageBundle = imageBundle

	def loadCommon(self):
		self.setImageBundle(tryToLoadImageBundle())
		
	def createModelMats(self):
		return NoeModelMaterials(self.textures, self.materials)

	def findOrCreateMaterial(self, mtlEnc):
		mtlName = mtlEnc.generateMaterialName()
		imgId = mtlEnc.tmemAddr
		imgLookup = imgId | (mtlEnc.stMode[0] << 16) | (mtlEnc.stMode[1] << 24)
		if mtlName not in self.matDict:
			mtl = mtlEnc.generateMaterial()
			#for testing with fog
			#mtl.flags2 |= noesis.NMATFLAG2_PREFERPPL
			if imgId >= 0 and not mtlEnc.getStateFlags() & DLMaterialEncapsulation.StateFlag_NoTexture:
				tex = None
				if imgLookup not in self.texDict:
					if self.imageBundle:
						tex, memWidth = self.imageBundle.loadTexture(imgId, 0)
						if tex:
							mtlEnc.setTextureWrapFlags(tex)
							tex.name += "-" + mtlEnc.generateSTModeSuffix()
							texIndex = len(self.textures)
							self.texDict[imgLookup] = texIndex
							self.textures.append(tex)
				else:
					tex = self.textures[self.texDict[imgLookup]]
					
				if tex:
					mtl.setTexture(tex.name)
			self.matDict[mtlName] = len(self.materials)
			self.materials.append(mtl)
		else:
			mtl = self.materials[self.matDict[mtlName]]
			
		tex = self.textures[self.texDict[imgLookup]] if imgLookup in self.texDict else None
		return mtl, tex
		
class LevelBg(GEResource):
	def resInit(self):
		self.rooms = []
		
	def parseHeader(self):
		try:
			if len(self.data) < 16:
				return 0
			bs = self.bs
			bs.readUInt() #reserved
			self.roomDataOffset = bs.readUInt() & 0xFFFFFF
			self.portalDataOffset = bs.readUInt() & 0xFFFFFF
			self.visDataOffset = bs.readUInt() & 0xFFFFFF
			if self.roomDataOffset >= len(self.data):
				return 0
			if self.portalDataOffset >= len(self.data):
				return 0
			if self.visDataOffset >= len(self.data):
				return 0
			bs.seek(self.roomDataOffset, NOESEEK_ABS)
			firstRoom = BgRoom(self, bs)
			if firstRoom.pointDataOffset == 0: #normally expect first entry to be empty
				firstRoom = BgRoom(self, bs)
				
			if firstRoom.pointDataOffset == 0:
				return 0
			bs.seek(firstRoom.pointDataOffset, NOESEEK_ABS)
			#expect first room's point data to begin with a compression tag
			id = bs.readBytes(2)
			if id[0] != 0x11 or id[1] != 0x72:
				return 0
			return 1
		except:
			pass
		return 0
		
	def loadRooms(self):
		bs = self.bs
		bs.seek(self.roomDataOffset, NOESEEK_ABS)
		while True:
			room = BgRoom(self, bs)
			if room.pointDataOffset == 0:
				if len(self.rooms) == 0: #normally expect first entry to be empty
					room = BgRoom(self, bs)
				else: #last room is empty to act as a terminator
					break
			room.loadData(self.data)
			if room.pointData: #if point data points to nothing, skip this room
				self.rooms.append(room)

def bgCheckType(data):
	bg = LevelBg(data)
	if not bg.parseHeader():
		return 0
	return 1

def geCalculateUv(dlc, tex, vertST):
	s, t = dlc.calculateST(tex.width, tex.height, vertST)
	return (s, 1.0 - t)

def geDetermineColorScale(dlc, mtlEnc):
	#this is a hackaround for fully replicating the logic which generates combiner (and other) ops
	#from 0xC0 commands which we're just handling with a custom handler. (see 0x7F0CE118) instead
	#of handling it properly, just do some hackery to determine whether we want to use the envcolor
	#alpha. for now, just base it on whether we've decided to use blending.
	rgba = mtlEnc.getRgbaScale()[:]
	if mtlEnc.getStateFlags() & DLMaterialEncapsulation.StateFlag_AnyAlpha:
		rgba[3] = dlc.getEnvColor()[3]

	return rgba

def geReadRoomVerts(dlc, destOffset, readAddr, readSize):
	room = dlc.getUserData()
	offset = readAddr & 0x00FFFFFF
	return room.pointData[offset : offset + readSize]

def geDrawRoomTris(dlc, drawList, vertexBuffer, vertexSize):
	room = dlc.getUserData()
	bg = room.bg
	
	mtlEnc = DLMaterialEncapsulation(dlc)
	mtl, tex = bg.findOrCreateMaterial(mtlEnc)
	baseColor = geDetermineColorScale(dlc, mtlEnc)
	mtl.setDiffuseColor(None)
	rapi.rpgSetMaterial(mtl.name)
	rapi.immBegin(noesis.RPGEO_TRIANGLE)
	for tri in drawList:
		for index in tri:
			vertOffset = index * vertexSize
			vert = StandardVertex(vertexBuffer[vertOffset : vertOffset + vertexSize])
			if tex:
				rapi.immUV2(geCalculateUv(dlc, tex, vert.st))
			color = [vert.color[i] * baseColor[i] for i in range(0, 4)]
			rapi.immColor4(color)
			rapi.immVertex3(vert.pos)
	rapi.immEnd()
	
def geTranslateClampMode(clampMode):
	return ((clampMode & 1) << 1) | ((clampMode & 2) >> 1)

def dlHandlerGETexture(dlc, data):
	d0, d1 = noeUnpack(">II", data)
	opType = (d0 & 7)
	sMode = geTranslateClampMode((d0 >> 22) & 3)
	tMode = geTranslateClampMode((d0 >> 20) & 3)
	stOffset = 0 #(d0 >> 18) & 3
	if opType == 0:
		#use shift for dist < f0 case
		dist = d1 >> 24
		if dist < 0xF0:
			sShift = (d0 >> 14) & 15
			tShift = (d0 >> 10) & 15
		else:
			sShift = tShift = 0
		imgId = d1 & 4095
		otherImgId = -1
	elif opType == 1:
		#always use the detail texture (shift applies to other image)
		sShift = 0 #(d0 >> 14) & 15
		tShift = 0 #(d0 >> 10) & 15
		imgId = d1 & 4095
		otherImgId = (d1 >> 12) & 4095
	elif opType == 2 or opType == 3 or opType == 4:
		sShift = tShift = 0
		imgId = d1 & 4095
		otherImgId = -1
	#there are actually 8 possible values (see 7F0CE298), but code only handles 0-4.

	tileIndex = 0
	#hijacking tmemAddr to store the image id
	dlc.setTextureTileData(tileIndex, 0, 0, 0, imgId, 0)
	dlc.setTextureTileST(tileIndex, sMode, 0, sShift, tMode, 0, tShift)
	dlc.setTextureTileOffset(tileIndex, stOffset, stOffset)

def bgLoadModel(data, mdlList):
	scale = 1.0
	bg = LevelBg(data)
	bg.parseHeader()
	bg.loadCommon()
	bg.loadRooms()

	ctx = rapi.rpgCreateContext()

	roomTrans = NoeMat43()
	roomTrans[0] *= scale
	roomTrans[1] *= scale
	roomTrans[2] *= scale
	
	dlc = DisplayListContext()
	dlc.setVertexReadCallback(geReadRoomVerts)
	dlc.setDrawTrianglesCallback(geDrawRoomTris)
	dlc.setCommandHandler(0xC0, dlHandlerGETexture)
	
	for roomIndex in range(0, len(bg.rooms)):
		room = bg.rooms[roomIndex]
		dlc.setUserData(room)
		rapi.rpgSetName("room%04i"%roomIndex)
		roomTrans[3] = room.pos * scale
		rapi.rpgSetTransform(roomTrans)
		if room.disp0Data:
			dlc.runCommands(room.disp0Data)
		if room.disp1Data:
			dlc.runCommands(room.disp1Data)

	rapi.rpgOptimize()
	mdl = rapi.rpgConstructModel()
	if mdl:
		mdl.setModelMaterials(bg.createModelMats())
		mdlList.append(mdl)

	return 1

def geGetPairedFilePath(path):
	if not os.path.exists(path):
		#convert to relative to last checked file if it doesn't seem to be an abs path
		loadPath = rapi.getDirForFilePath(rapi.getLastCheckedName())
		path = os.path.join(loadPath, path)
	return path	
	
class ModelHeader:
	def __init__(self, bs):
		self.r0 = bs.readUInt()
		self.bcAddr = bs.readUInt()
		self.r1 = bs.readUInt()
		self.handleNodeCount = bs.readUShort()
		self.groupCount = bs.readUShort()
		self.f0 = bs.readFloat()
		self.r2 = bs.readUShort()
		self.imageCount = bs.readUShort()
		self.r3 = bs.readUInt()
		self.r4 = bs.readUInt()

class ModelSubHeader:
	def __init__(self, bs):
		self.entryCount = bs.readUShort()
		self.r0 = bs.readUShort()
		self.dataAddr = bs.readUInt()
		self.partWordCount = bs.readUShort()
	def asBytes(self):
		return noePack(">HHIH", self.entryCount, self.r0, self.dataAddr, self.partWordCount)

class AnimHeader:
	def __init__(self, bs):
		self.rotDataOffset = bs.readUInt()
		self.frameCount = bs.readUShort()
		self.bitsPerChannel = bs.readUByte()
		self.u0 = bs.readUByte()
		self.offset0 = bs.readUInt()
		self.translationBPC = bs.readUShort()
		self.frameSize = bs.readUShort() #in bits
		self.offset1 = bs.readUInt()

class ModelAnimMapEntry:
	def __init__(self, bs):
		self.t0 = bs.readUByte()
		self.typeId = bs.readUByte()
		self.chanIndices = []
		self.chanIndices.append(bs.readUShort())
		self.chanIndices.append(bs.readUShort())

class ModelImage:
	def __init__(self, bs):
		self.u0 = bs.readUShort()
		self.imageIndex = bs.readUShort()
		self.width = bs.readUByte()
		self.height = bs.readUByte()
		self.b0 = bs.readUShort()
		self.b1 = bs.readUInt()

class ModelNode:
	def __init__(self, typeId, offsetId, nodeIndex, fileOrderIndex, mdl, bs):
		self.typeId = typeId
		self.offsetId = offsetId
		self.nodeIndex = nodeIndex
		self.fileOrderIndex = fileOrderIndex
		self.mdl = mdl
		self.r0 = bs.readUShort()
		self.dataOffset = bs.readUInt() & 0x00FFFFFF
		self.parentOffset = bs.readUInt()
		self.nextOffset = bs.readUInt()
		self.prevOffset = bs.readUInt()
		self.childOffset = bs.readUInt()
		self.modelTransform = NoeMat43()
		self.boneIndex = -1
		self.drawOrder = -1
		self.nodeInit(bs)
		
	def isJoint(self):
		return False
		
	def isDrawable(self):
		return False
		
	def getLodPrefix(self):
		return ""
		
	def getJointName(self):
		return "bone%04X" % self.offsetId + self.mdl.getModelSuffix()

	def getMeshName(self):
		return "mesh%04X" % self.offsetId + self.mdl.getModelSuffix()
		
	def getParentIndex(self):
		if self.parentOffset == 0:
			return -1
		return self.mdl.loadOrFindNodeIndex(self.parentOffset)
	def getPrevIndex(self):
		if self.prevOffset == 0:
			return -1
		return self.mdl.loadOrFindNodeIndex(self.prevOffset)
	def getNextIndex(self):
		if self.nextOffset == 0:
			return -1
		return self.mdl.loadOrFindNodeIndex(self.nextOffset)
	def getChildIndex(self):
		if self.childOffset == 0:
			return -1
		return self.mdl.loadOrFindNodeIndex(self.childOffset)
		
	def getLocalTransform(self):
		return NoeMat43()

	def nodeInit(self, bs):
		pass

	def loadData(self, bs):
		pass
		
	def draw(self, dlc):
		pass

class ModelNodePosition(ModelNode):
	def loadData(self, bs):
		self.loadPosDataCommon(bs, False)
		
	def loadPosDataCommon(self, bs, isType2):
		self.localTransform = NoeMat43()
		self.localTransform[3] = NoeVec3((bs.readFloat(), bs.readFloat(), bs.readFloat()))
		self.animMap = []
		self.animMap.append(bs.readUShort())
		self.animMap.append(bs.readUShort())
		if isType2:
			self.animMap.append(0xFFFF)
			self.animMap.append(0xFFFF)
			self.childNodeOffset = 0
		else:
			self.animMap.append(bs.readUShort())
			self.animMap.append(bs.readUShort())
			self.childNodeOffset = bs.readUInt()
		self.radius = bs.readFloat()

	def isJoint(self):
		return True
		
	def getLocalTransform(self):
		return self.localTransform

class ModelNodePosition2(ModelNodePosition):
	def loadData(self, bs):
		self.loadPosDataCommon(bs, True)
		
class ModelNodeDisplayList(ModelNode):
	def loadData(self, bs):
		self.disp0Offset = bs.readUInt() & 0x00FFFFFF
		self.disp1Offset = bs.readUInt() & 0x00FFFFFF
		self.u0 = bs.readUShort()
		self.u1 = bs.readUShort()
		self.pointsOffset = bs.readUInt() & 0x00FFFFFF
		self.pointCount = bs.readUShort()
		self.tag = bs.readUByte()
		self.r0 = bs.readUByte()
		mdl = self.mdl
		mdl.displayListNodes.append(self.nodeIndex)

	def isDrawable(self):
		return True
		
	def readPointData(self, dlc, destOffset, offset, readSize):
		mdl = self.mdl
		pointsOffset = mdl.dataOffset + offset
		vertStride = dlc.vertexSize
		vertCount = readSize // vertStride
		
		#hack for the only 2 models where this happens. todo - handle this correctly
		boneIndex = self.boneIndex if destOffset == 0 else 0
		for vertIndex in range(0, vertCount):
			vertOffset = destOffset + vertIndex * vertStride
			mdl.setBoneIndicesAtOffset(vertOffset, (boneIndex, boneIndex))
		return mdl.data[pointsOffset : pointsOffset + readSize]

	def draw(self, dlc):
		mdl = self.mdl
		if self.disp0Offset > 0:
			self.drawDisp(dlc, self.disp0Offset)
		if self.disp1Offset > 0:
			self.drawDisp(dlc, self.disp1Offset)
			
	def drawDisp(self, dlc, dispOffset):
		mdl = self.mdl
		dispOffset = mdl.dataOffset + dispOffset
		dlc.runCommands(mdl.data[dispOffset:])
		
class ModelNodeLODInfo(ModelNode):
	def loadData(self, bs):
		self.distMin = bs.readFloat()
		self.distMax = bs.readFloat()
		self.dispNodeOffset = bs.readUInt()
		self.tag = bs.readUShort()
		self.r0 = bs.readUShort()
		
	def getLodPrefix(self):
		return "lod%.02f_%.02f_"%(self.distMin, self.distMax)
		
class ModelNodeHead(ModelNode):
	def loadData(self, bs):
		self.bounds = bs.readBytes(4 * 6) #unused by Noesis
		self.headOffset = bs.readUInt()
		self.neckOffset = bs.readUInt()
		self.r0 = bs.readUShort()
		self.tag = bs.readUShort()
		if noesis.optWasInvoked("-ge64head"):
			mdlName = geGetPairedFilePath(noesis.optGetArg("-ge64head"))
			with open(mdlName, "rb") as f:
				data = f.read()
				headMdl = PropModel(data)
				headMdl.parseHeader()
				headMdl.loadData()
				pmdl = self.mdl
				pmdl.addChildModel(headMdl, self.offsetId)
				

class ModelMergePoint:
	def __init__(self, bs):
		self.xyzData = bs.readBytes(6)
		self.id = bs.readUShort()
		self.nodeOffset = bs.readUInt()
		self.pointIndex = bs.readUShort()
		self.r0 = bs.readUShort()

class ModelNodeDisplayListMerge(ModelNodeDisplayList):
	def loadData(self, bs):
		self.disp0Offset = bs.readUInt() & 0x00FFFFFF
		self.disp1Offset = bs.readUInt() & 0x00FFFFFF
		self.pointsOffset = bs.readUInt() & 0x00FFFFFF
		self.pointCount = bs.readUShort()
		self.pointMergeCount = bs.readUShort()
		self.mergeOffset = bs.readUInt() & 0x00FFFFFF
		self.dupIndicesOffset = bs.readUInt() & 0x00FFFFFF
		self.tag = bs.readUShort()
		self.r0 = bs.readUShort()

		mdl = self.mdl
		
		self.mergePoints = []
		bs.seek(mdl.dataOffset + self.mergeOffset)
		for mergePointIndex in range(0, self.pointMergeCount):
			self.mergePoints.append(ModelMergePoint(bs))

		mergeToVertIndex = {}
		bs.seek(mdl.dataOffset + self.dupIndicesOffset)
		for pointIndex in range(0, self.pointCount):
			dupPositionIndex = bs.readUShort()
			if dupPositionIndex != 0xFFFF:
				mergeToVertIndex[dupPositionIndex] = pointIndex
				
		usedMergeCount = 0
		self.mergeIndices = []
		for pointIndex in range(0, self.pointCount):
			if pointIndex in mergeToVertIndex:
				refPointIndex = mergeToVertIndex[pointIndex]
				mergeIndex = self.mergeIndices[refPointIndex]
				self.mergeIndices.append(mergeIndex)
			else:
				mergePoint = self.mergePoints[usedMergeCount]
				if mergePoint.id != pointIndex:
					print("Warning: Unexpected merge point mismatch:", mergePoint.id, "vs", pointIndex)
				self.mergeIndices.append(usedMergeCount)
				usedMergeCount += 1
				
		mdl.displayListNodes.append(self.nodeIndex)

	def readPointData(self, dlc, destOffset, offset, readSize):
		mdl = self.mdl
		bs = mdl.bs
		vertStride = dlc.vertexSize
		vertsOut = bytearray()
		vertCount = readSize // vertStride
		if (offset % vertStride) != 0 or (destOffset % vertStride) != 0:
			print("Warning: Unexpected offset:", offset, destOffset)
		srcStart = offset // vertStride
		pointsOffset = mdl.dataOffset + self.pointsOffset
		for vertIndex in range(0, vertCount):
			absVertIndex = srcStart + vertIndex
			pointOffset = pointsOffset + absVertIndex * vertStride
			vertData = mdl.data[pointOffset : pointOffset + vertStride]
			
			mergeIndex = self.mergeIndices[absVertIndex]
			mergePoint = self.mergePoints[mergeIndex]
			
			boneIndex = self.boneIndex
			blendBoneIndex = self.boneIndex
			if mergePoint.nodeOffset != 0:
				nodeIndex = mdl.loadOrFindNodeIndex(mergePoint.nodeOffset)
				if nodeIndex >= 0 and mergePoint.pointIndex != 0xFFFF:
					mergeNode = mdl.nodes[nodeIndex]
					boneIndex = mergeNode.boneIndex
					otherMergePoint = mergeNode.mergePoints[mergePoint.pointIndex]
					vertData = otherMergePoint.xyzData + vertData[6:]
					#slight hack, only take the bone as the weight influence if it takes precedence in the hierarchy
					if mergeNode.drawOrder < self.drawOrder:
						blendBoneIndex = mergeNode.boneIndex
				else:
					print("Warning: No merge node:", mergePoint.nodeOffset, mergePoint.pointIndex)
				
			mdl.setBoneIndicesAtOffset(destOffset + vertIndex * vertStride, (boneIndex, blendBoneIndex))
			vertsOut += vertData
		return vertsOut
		
geModelNodeTypes = {
	0x02 : ModelNodePosition,
	0x04 : ModelNodeDisplayList,
	0x08 : ModelNodeLODInfo,
	0x09 : ModelNodeHead,
	0x15 : ModelNodePosition2,
	0x18 : ModelNodeDisplayListMerge,
}

def geReadNodeVerts(dlc, destOffset, readAddr, readSize):
	node = dlc.getUserData()
	offset = readAddr & 0x00FFFFFF
	return node.readPointData(dlc, destOffset, offset, readSize)

def geDrawNodeTris(dlc, drawList, vertexBuffer, vertexSize):
	node = dlc.getUserData()
	mdl = node.mdl

	#special handling for loaded heads
	forceBlendBone = -1
	if mdl.parentMdl:
		rootMdl = mdl.parentMdl
		blendNodeIndex = rootMdl.loadOrFindNodeIndex(mdl.parentMdlRootId)
		if blendNodeIndex >= 0:
			forceBlendBone = rootMdl.nodes[blendNodeIndex].boneIndex
	else:
		rootMdl = mdl

	mtlEnc = DLMaterialEncapsulation(dlc)		
	mtl, tex = rootMdl.findOrCreateMaterial(mtlEnc)
	baseColor = geDetermineColorScale(dlc, mtlEnc)
	rapi.rpgSetMaterial(mtl.name)	
	rapi.immBegin(noesis.RPGEO_TRIANGLE)	
	for tri in drawList:
		for index in tri:
			vertOffset = index * vertexSize
			vert = StandardVertex(vertexBuffer[vertOffset : vertOffset + vertexSize])
			if tex:
				rapi.immUV2(geCalculateUv(dlc, tex, vert.st))
			color = [vert.color[i] * baseColor[i] for i in range(0, 4)]
			rapi.immColor4(color)
			if forceBlendBone >= 0:
				rapi.immBoneWeight([1.0])
				rapi.immBoneIndex([forceBlendBone])
				rapi.immVertex3((mdl.rootTransform * NoeVec3(vert.pos)).vec3)
			else:
				boneIndex, blendBoneIndex = mdl.getBoneIndicesAtOffset(vertOffset)
				if boneIndex >= 0:
					bone = mdl.bones[boneIndex]
					rapi.immBoneWeight([1.0])
					rapi.immBoneIndex([blendBoneIndex])
					rapi.immVertex3((bone.getMatrix() * NoeVec3(vert.pos)).vec3)
				else:
					rapi.immBoneWeight(None)
					rapi.immBoneIndex(None)
					rapi.immVertex3(vert.pos)
	rapi.immEnd()
	
class AnimBundle(GEResource):
	def parseHeader(self):
		try:
			if len(self.data) < 24:
				return 0
			bs = self.bs
			if bs.readBytes(len(ANIM_BUNDLE_ID)) != ANIM_BUNDLE_ID.encode("ASCII"):
				return 0
			self.animDirection = 1 if noesis.optWasInvoked("-ge64anmflip") else 0
			self.ver = bs.readUInt()
			self.mappingType = bs.readUInt()
			self.offsetsSize = bs.readUInt()
			self.headersSize = bs.readUInt()
			self.rotDataSize = bs.readUInt()
			self.offsetsOffset = bs.tell()
			self.headersOffset = self.offsetsOffset + self.offsetsSize
			self.rotDataOffset = self.headersOffset + self.headersSize
			endOffset = self.rotDataOffset + self.rotDataSize
			if self.offsetsSize == 0 or self.headersSize == 0 or self.rotDataSize == 0 or endOffset > len(self.data):
				return 0
			return 1
		except:
			pass
		return 0
		
	def createAnimations(self, pmdl):
		anims = []
		boneCount = len(pmdl.bones)
		if boneCount > 0:
			defaultFramerate = 30.0
			localMats = []
			for boneIndex in range(0, boneCount):
				bone = pmdl.bones[boneIndex]
				localMat = bone.getMatrix()
				if bone.parentIndex >= 0:
					parentBone = pmdl.bones[bone.parentIndex]
					localMat = parentBone.getMatrix().inverse() * localMat
				localMats.append(localMat)
			bs = self.bs
			offsetCount = self.offsetsSize // 4
			bs.seek(self.offsetsOffset, NOESEEK_ABS)
			offsets = []
			for i in range(0, offsetCount):
				offset = bs.readUInt()
				if offset > 1:
					offsets.append(offset)

			headers = []
			for offset in offsets:
				bs.seek(self.headersOffset + offset, NOESEEK_ABS)
				hdr = AnimHeader(bs)
				headers.append(hdr)

			for animIndex in range(0, len(headers)):
				hdr = headers[animIndex]
				animName = "anim%04i"%animIndex
				animMats = []
				for frameIndex in range(0, hdr.frameCount):
					for boneIndex in range(0, boneCount):
						bone = pmdl.bones[boneIndex]
						nodeIndex = bone.ge64NodeIndex
						node = pmdl.nodes[nodeIndex]
						
						animEntryIndex = node.animMap[self.mappingType]
						if animEntryIndex < len(pmdl.animMapEntries):
							animMapEntry = pmdl.animMapEntries[animEntryIndex]
							chanIndex = animMapEntry.chanIndices[self.animDirection]
							chanFlags = 0 if bone.parentIndex >= 0 else 1
							#the decode is tucked away in native land for speed, but it's relatively simple -
							#rotations are all local and linear with each channel packed according to bitsPerChannel, no fancy delta compression or anything
							frameMat = rapi.callExtensionMethod("ge64_animdecode", hdr, self.animDirection, chanIndex, chanFlags, frameIndex, localMats[boneIndex], self.data, self.headersOffset, self.rotDataOffset)
							if not frameMat:
								frameMat = localMats[boneIndex]
							animMats.append(frameMat)

				anim = NoeAnim(animName, pmdl.bones, hdr.frameCount, animMats, defaultFramerate)
				anims.append(anim)

		return anims

def anmCheckType(data):
	animBundle = AnimBundle(data)
	if not animBundle.parseHeader():
		return 0	
	return 1
	
def anmLoadModel(data, mdlList):
	animBundle = AnimBundle(data)
	animBundle.parseHeader()
	
	mdlData = None
	if noesis.optWasInvoked("-ge64anmmdl"):
		mdlName = geGetPairedFilePath(noesis.optGetArg("-ge64anmmdl"))
		with open(mdlName, "rb") as f:
			mdlData = f.read()
	else:
		mdlData = rapi.loadPairedFile("GoldenEye N64 Model", ".ge64_mdl")
		
	if not mdlData:
		print("Error: Model is required to load animation bundle.")
		return 0

	pmdl, mdl = geLoadAndConstructModel(mdlData)

	if mdl:
		anims = animBundle.createAnimations(pmdl)
		if len(anims) > 0:
			mdl.setAnims(anims)
		mdlList.append(mdl)
	return 1

class PropModel(GEResource):
	def resInit(self):
		self.parentMdl = None
		self.parentMdlRootId = 0
		self.childMdls = []
		self.rootTransform = NoeMat43()

	def parseHeader(self):
		try:
			if len(self.data) < 32:
				return 0
			bs = self.bs
			if bs.readBytes(len(MODEL_HEADER_ID)) != MODEL_HEADER_ID.encode("ASCII"):
				return 0
			self.ver = bs.readUInt()
			if self.ver != MODEL_HEADER_VERSION:
				return 0
			self.resType = bs.readUInt()
			self.resDataSize = bs.readUInt()
			self.resHeaderSize = bs.readUInt()
			self.subHeaderSize = bs.readUInt()
			self.tableId = bs.readUInt()
			self.resDataOffset = bs.tell()
			self.resHeaderOffset = self.resDataOffset + self.resDataSize
			self.subHeaderOffset = self.resHeaderOffset + self.resHeaderSize
			self.dataOffset = self.subHeaderOffset + self.subHeaderSize
			if self.resHeaderOffset <= 0 or self.resHeaderSize <= 0 or (self.resHeaderOffset + self.resHeaderSize) >= len(self.data):
				return 0
			return 1
		except:
			pass
		return 0
		
	def loadData(self):
		bs = self.bs
		bs.seek(self.resHeaderOffset, NOESEEK_ABS)
		self.rh = ModelHeader(bs)
		self.animMapEntries = []
		if self.subHeaderSize > 0:
			bs.seek(self.subHeaderOffset, NOESEEK_ABS)
			self.sh = ModelSubHeader(bs)
			shDataSize = self.subHeaderSize - (bs.tell() - self.subHeaderOffset)
			animMapEntryCount = shDataSize // 6
			for animMapEntryIndex in range(0, animMapEntryCount):
				self.animMapEntries.append(ModelAnimMapEntry(bs))
		else:
			self.sh = None
		rh = self.rh
		bs.seek(self.dataOffset, NOESEEK_ABS)
		self.handleOffsets = []
		nodesToLoad = []

		self.offsetToBoneIndex = {}
		self.displayListNodes = []
		
		for handleNodeIndex in range(0, rh.handleNodeCount):
			nodeOffset = bs.readUInt()
			nodesToLoad.append(nodeOffset)
			self.handleOffsets.append(nodeOffset)
		self.imageRefs = []
		for imageRefIndex in range(0, rh.imageCount):
			self.imageRefs.append(ModelImage(bs))

		#start a cascade from the first node in the file
		self.firstNodeOffset = bs.tell() - self.dataOffset
		if self.firstNodeOffset == 0: #hack so that it isn't considered "null"
			self.firstNodeOffset = 0x05000000
		nodesToLoad.append(self.firstNodeOffset)
			
		self.nodeOffsetToIndex = {}
		self.nodeIndexToBoneIndex = {}
		self.nodes = []
		while len(nodesToLoad) > 0:
			nodeOffset = nodesToLoad.pop()
			self.loadOrFindNodeIndex(nodeOffset)
			
		self.bones = []
		for nodeIndex in range(0, len(self.nodes)):
			node = self.nodes[nodeIndex]
			if node.isJoint():
				if nodeIndex not in self.nodeIndexToBoneIndex:
					boneIndex = len(self.bones)
					self.nodeIndexToBoneIndex[nodeIndex] = boneIndex
					node.modelTransform = self.calculateNodeModelTransform(nodeIndex)
					bone = NoeBone(boneIndex, node.getJointName(), node.modelTransform, None, -1)
					bone.ge64NodeIndex = nodeIndex
					self.bones.append(bone)
		#set parent indices as needed
		for nodeIndex in range(0, len(self.nodes)):
			if nodeIndex in self.nodeIndexToBoneIndex:
				boneIndex = self.nodeIndexToBoneIndex[nodeIndex]
				bone = self.bones[boneIndex]
				nodeParentIndex = self.findJointParentIndex(nodeIndex)
				if nodeParentIndex in self.nodeIndexToBoneIndex:
					bone.parentIndex = self.nodeIndexToBoneIndex[nodeParentIndex]
		#now assign applicable bone indices for all nodes including non-joints
		for nodeIndex in range(0, len(self.nodes)):
			node = self.nodes[nodeIndex]
			node.boneIndex = self.findFirstJointIndex(nodeIndex)

		self.currentDrawOrder = 0
		for nodeIndex in range(0, len(self.nodes)):
			self.setDrawOrder(nodeIndex)

	def setDrawOrder(self, nodeIndex):
		node = self.nodes[nodeIndex]
		if node.drawOrder >= 0:
			return
		parentIndex = node.getParentIndex()
		if parentIndex >= 0:
			self.setDrawOrder(parentIndex)
		prevIndex = node.getPrevIndex()
		if prevIndex >= 0:
			self.setDrawOrder(prevIndex)
		node.drawOrder = self.currentDrawOrder
		self.currentDrawOrder += 1
		nextIndex = node.getNextIndex()
		if nextIndex >= 0:
			self.setDrawOrder(nextIndex)
		childIndex = node.getChildIndex()
		if childIndex >= 0:
			self.setDrawOrder(childIndex)
	
	def findDisplayListNodeOwner(self, vertOffset, vertStride):
		bestCandidate = -1
		for nodeIndex in self.displayListNodes:
			node = self.nodes[nodeIndex]
			pointsOffset = node.pointsOffset
			pointsOffsetEnd = pointsOffset + node.pointCount * vertStride
			if vertOffset >= pointsOffset and vertOffset < pointsOffsetEnd:
				if bestCandidate < 0 or self.nodes[bestCandidate].drawOrder < node.drawOrder:
					bestCandidate = nodeIndex
		return bestCandidate
	
	def findJointParentIndex(self, nodeIndex):
		node = self.nodes[nodeIndex]
		parentIndex = node.getParentIndex()
		if parentIndex >= 0:
			parentNode = self.nodes[parentIndex]
			if not parentNode.isJoint():
				return self.findJointParentIndex(parentIndex)
			#fall through to return this index since the node is a joint
		return parentIndex
		
	def findFirstJointIndex(self, nodeIndex):
		node = self.nodes[nodeIndex]
		if node.isJoint():
			return self.nodeIndexToBoneIndex[nodeIndex]
		parentJointNodeIndex = self.findJointParentIndex(nodeIndex)
		if parentJointNodeIndex >= 0:
			return self.nodeIndexToBoneIndex[parentJointNodeIndex]
		return -1
	
	def calculateNodeModelTransform(self, nodeIndex):
		node = self.nodes[nodeIndex]
		parentIndex = node.getParentIndex()
		if parentIndex >= 0:
			parentTransform = self.calculateNodeModelTransform(parentIndex)
			return parentTransform * node.getLocalTransform()
		return node.getLocalTransform()
	
	def loadOrFindNodeIndex(self, nodeOffset):
		if nodeOffset == 0:
			return -1
		nodeOffset &= 0x00FFFFFF
		if nodeOffset in self.nodeOffsetToIndex:
			return self.nodeOffsetToIndex[nodeOffset]

		bs = self.bs
		streamOffset = self.dataOffset + nodeOffset
		if streamOffset >= len(self.data):
			return -1
		bs.seek(streamOffset, NOESEEK_ABS)

		t0 = bs.readUByte()
		typeId = bs.readUByte()
		nodeType = geModelNodeTypes[typeId] if typeId in geModelNodeTypes else ModelNode
		nodeIndex = len(self.nodes)
		fileOrderIndex = (nodeOffset - (self.firstNodeOffset & 0x00FFFFFF)) // 24
		node = nodeType(typeId, nodeOffset, nodeIndex, fileOrderIndex, self, bs)
		#print("load node: %02X/%02X @ %04X, parent %04X"%(t0, typeId, nodeOffset, node.parentOffset), "-", self.dataOffset + node.dataOffset)
		self.nodes.append(node)
		self.nodeOffsetToIndex[nodeOffset] = nodeIndex
		#load the data if applicable
		if node.dataOffset != 0:
			bs.seek(self.dataOffset + node.dataOffset, NOESEEK_ABS)
			node.loadData(bs)
		#make sure nodes pointed to by this one are loaded too
		self.loadOrFindNodeIndex(node.parentOffset)
		self.loadOrFindNodeIndex(node.nextOffset)
		self.loadOrFindNodeIndex(node.prevOffset)
		self.loadOrFindNodeIndex(node.childOffset)
		
	def getParentLodNodeIndex(self, nodeIndex):
		node = self.nodes[nodeIndex]
		lodPrefix = node.getLodPrefix()
		if len(lodPrefix) == 0:
			parentIndex = node.getParentIndex()
			if parentIndex >= 0:
				return self.getParentLodNodeIndex(parentIndex)
		else:
			return nodeIndex
		return -1

	def getParentLodPrefix(self, nodeIndex):
		lodNodeIndex = self.getParentLodNodeIndex(nodeIndex)
		if lodNodeIndex >= 0:
			return self.nodes[lodNodeIndex].getLodPrefix()
		return ""

	def getParentLodMinDist(self, nodeIndex):
		lodNodeIndex = self.getParentLodNodeIndex(nodeIndex)
		if lodNodeIndex >= 0:
			return self.nodes[lodNodeIndex].distMin
		return 0.0
		
	def setBoneIndicesAtOffset(self, offset, boneIndices):
		self.offsetToBoneIndex[offset] = boneIndices
	def getBoneIndicesAtOffset(self, offset):
		return self.offsetToBoneIndex[offset]
		
	def drawNodes(self):
		dlc = DisplayListContext()
		dlc.setVertexReadCallback(geReadNodeVerts)
		dlc.setDrawTrianglesCallback(geDrawNodeTris)
		dlc.setCommandHandler(0xC0, dlHandlerGETexture)
		
		#draw in hierarchy order, in case it matters for render states (doesn't matter for transforms, since we have a flat vertex handler that just checkes hierarchy values)
		sortedIndices = sorted([x for x in range(0, len(self.nodes))], key=lambda a: self.nodes[a].drawOrder)
	
		for nodeIndex in sortedIndices:
			self.drawNode(nodeIndex, dlc)
			
		#now draw child models if applicable
		for childMdl in self.childMdls:
			rootNodeIndex = self.loadOrFindNodeIndex(childMdl.parentMdlRootId)
			if rootNodeIndex >= 0:
				childMdl.rootTransform = self.calculateNodeModelTransform(rootNodeIndex)
			childMdl.drawNodes()
			
	def drawNode(self, nodeIndex, dlc):
		node = self.nodes[nodeIndex]
		if node.isDrawable():
			dlc.setUserData(node)
			boneIndex = node.boneIndex
			if noesis.optWasInvoked("-ge64minlod"):
				minLod = float(noesis.optGetArg("-ge64minlod"))
				if self.getParentLodMinDist(nodeIndex) > minLod:
					return
			rapi.rpgSetName(self.getParentLodPrefix(nodeIndex) + node.getMeshName())
			node.draw(dlc)
			
	def getModelSuffix(self):
		return "_ch%04X"%self.parentMdlRootId if self.parentMdl else ""
		
	def addChildModel(self, childMdl, nodeParentId):
		childMdl.parentMdl = self
		childMdl.parentMdlRootId = nodeParentId
		self.childMdls.append(childMdl)
			
def mdlCheckType(data):
	pmdl = PropModel(data)
	if not pmdl.parseHeader():
		return 0	
	return 1

def geLoadAndConstructModel(data):
	pmdl = PropModel(data)
	pmdl.parseHeader()
	pmdl.loadCommon()
	pmdl.loadData()
	
	ctx = rapi.rpgCreateContext()
	
	pmdl.drawNodes()

	rapi.rpgOptimize()
	try:
		mdl = rapi.rpgConstructModel()
	except:
		mdl = None

	if not mdl and len(pmdl.bones) > 0:
		mdl = NoeModel()

	if mdl:
		mdl.setBones(pmdl.bones)
		mdl.setModelMaterials(pmdl.createModelMats())
		
	return pmdl, mdl
	
def mdlLoadModel(data, mdlList):
	pmdl, mdl = geLoadAndConstructModel(data)

	if mdl:
		mdlList.append(mdl)
	return 1

def geExtensionForRes(resName, resData):
	ln = resName.lower()
	if ln.startswith("bg"):
		bg = LevelBg(resData)
		if bg.parseHeader():
			return ".ge64_bg"
		else:
			return ".ge64_bd"
	elif ln.startswith("u"):
		return ".ge64_scn"
	elif ln.startswith("t"):
		return ".ge64_clp"
	elif ln.startswith("l"):
		return ".ge64_lng"
	elif ln.startswith("g"):
		return ".ge64_gnh" #gun without tabled header

	return ".ge64_bin"
	
class BaseTableEntry:
	def __init__(self, dataBs, tableId):
		self.headerAddr = -1
		self.tableId = tableId
		self.entryInit(dataBs)
	def entryInit(dataBs):
		pass
	def remainInTable(self):
		return True
	def getResType(self):
		return RESTYPE_UNSPECIFIED
	def getResData(self):
		return None
	def getResTypeHeaderSize(self):
		return 32
	def buildHeader(self, dataBs, dataSegAddr):
		if self.headerAddr < 0:
			return bytearray()
			
		headerOffset = self.headerAddr - dataSegAddr
		dataBs.seek(headerOffset, NOESEEK_ABS)
		resTypeHeaderSize = self.getResTypeHeaderSize()
		headerData = dataBs.readBytes(resTypeHeaderSize)
		subHeaderData = bytearray()
		if resTypeHeaderSize >= 32:
			dataBs.seek(headerOffset, NOESEEK_ABS)
			hdr = ModelHeader(dataBs)
			if hdr.bcAddr > 0:
				dataBs.seek(hdr.bcAddr - dataSegAddr, NOESEEK_ABS)
				subHdr = ModelSubHeader(dataBs)
				if subHdr.dataAddr > 0:
					dataBs.seek(subHdr.dataAddr - dataSegAddr, NOESEEK_ABS)
					subHeaderData += subHdr.asBytes()
					subHeaderData += dataBs.readBytes(subHdr.entryCount * 6)
			
		header = bytearray()
		header += MODEL_HEADER_ID.encode("ASCII") + noePack(">I", MODEL_HEADER_VERSION)
		resData = self.getResData()
		resDataSize = len(resData) if resData else 0
		header += noePack(">IIIII", self.getResType(), resDataSize, resTypeHeaderSize, len(subHeaderData), self.tableId)
		if resData:
			header += resData
		header += headerData
		header += subHeaderData
		return header

class ResEntry(BaseTableEntry):
	def entryInit(self, dataBs):
		self.id = dataBs.readUInt()
		self.nameAddr = dataBs.readUInt()
		self.dataOffset = dataBs.readUInt()
		self.name = None
	def isValid(self):
		return self.nameAddr != 0
	def remainInTable(self):
		return self.dataOffset != 0
		
class PropEntry(BaseTableEntry):
	def entryInit(self, dataBs):
		self.headerAddr = dataBs.readUInt()
		self.nameAddr = dataBs.readUInt()
		self.scale = dataBs.readFloat()
		self.name = None
	def isValid(self):
		return self.headerAddr != 0
	def getResType(self):
		return RESTYPE_PROP
	def getResData(self):
		return noePack(">f", self.scale)
		
class CharEntry(BaseTableEntry):
	def entryInit(self, dataBs):
		self.headerAddr = dataBs.readUInt()
		self.nameAddr = dataBs.readUInt()
		self.f0 = dataBs.readFloat()
		self.f1 = dataBs.readFloat()
		self.u0 = dataBs.readUInt()
		self.name = None
	def isValid(self):
		return self.headerAddr != 0
	def getResType(self):
		return RESTYPE_CHAR
	def getResData(self):
		return noePack(">ffI", self.f0, self.f1, self.u0)
		
class GunEntry(BaseTableEntry):
	def entryInit(self, dataBs):
		self.headerAddr = dataBs.readUInt()
		self.nameAddr = dataBs.readUInt()
		self.u0 = dataBs.readUInt()
		self.uAddr = dataBs.readUInt()
		self.u1 = dataBs.readUInt()
		self.data = dataBs.readBytes(36)
	def isValid(self):
		return self.u1 != 0
	def remainInTable(self):
		return self.nameAddr != 0
	def getResType(self):
		return RESTYPE_GUN
	def getResData(self):
		return noePack(">III", self.u0, self.uAddr, self.u1) + self.data
		
class AmmoEntry(BaseTableEntry):
	def entryInit(self, dataBs):
		self.headerAddr = dataBs.readUInt()
		self.nameAddr = dataBs.readUInt()
	def isValid(self):
		return self.headerAddr != 0
	def getResType(self):
		return RESTYPE_AMMO
		
def geCreateExportAnimSetData(romData, dataBs, mappingType, headerOffsetsOffset, romHeadersOffset, romRotDataOffset):
	romBs = NoeBitStream(romData, NOE_BIGENDIAN)
	dataBs.seek(headerOffsetsOffset, NOESEEK_ABS)
	maxOffset = 0
	headerOffsetsSize = 0
	rotDataSize = 0
	
	offsets = []
	while True:
		offset = dataBs.readUInt()
		if offset == 0:
			headerOffsetsSize = (dataBs.tell() - headerOffsetsOffset) - 4
			break
		elif offset > 1:
			maxOffset = max(offset, maxOffset)
			offsets.append(offset)
			
			romBs.seek(romHeadersOffset + offset, NOESEEK_ABS)
			animHdr = AnimHeader(romBs)
			
			animRotEnd = animHdr.rotDataOffset + animHdr.frameCount * (animHdr.frameSize >> 3)
			animRotEnd = (animRotEnd + 3) & ~3 #always aligned to 4 bytes
			rotDataSize = max(animRotEnd, rotDataSize)

	if maxOffset == 0 or headerOffsetsSize <= 0 or rotDataSize == 0:
		return None

	headersDataSize = maxOffset + 32 #header itself is always placed after translation data

	animSetData = bytearray()
	animSetData += ANIM_BUNDLE_ID.encode("ASCII")
	animSetData += noePack(">IIIII", ANIM_BUNDLE_VERSION, mappingType, headerOffsetsSize, headersDataSize, rotDataSize)
	dataBs.seek(headerOffsetsOffset, NOESEEK_ABS)	
	animSetData += dataBs.readBytes(headerOffsetsSize)
	animSetData += romData[romHeadersOffset : romHeadersOffset + headersDataSize]
	animSetData += romData[romRotDataOffset : romRotDataOffset + rotDataSize]
	return animSetData
		
def geReadTable(dataBs, tableOffset, dataSegAddr, entryType):
	entries = []
	entryMap = {}
	dataBs.seek(tableOffset, NOESEEK_ABS)
	tableIndex = 0
	while True:
		entry = entryType(dataBs, tableIndex)
		if not entry.isValid():
			break
		if entry.remainInTable():
			entries.append(entry)
		tableIndex += 1
			
	for index in range(0, len(entries)):
		entry = entries[index]
		dataBs.seek(entry.nameAddr - dataSegAddr, NOESEEK_ABS)
		entryName = dataBs.readString()
		entryMap[entryName] = index
		entry.name = entryName

	return entries, entryMap

def geExtractArc(fileName, fileLen, justChecking):
	if fileLen <= 0x1000 or fileLen >= 0x10000000:
		return 0
	with open(fileName, "rb") as f:
		f.seek(0x20, os.SEEK_SET)
		geId = f.read(9)
		if geId != "GOLDENEYE".encode("ASCII"):
			return 0
		regionIndex = geGetRegionIndex(f)
		if regionIndex < 0:
			return 0
			
		if justChecking:
			return 1

		f.seek(0, os.SEEK_SET)
		data = f.read()
		dataSeg = geDecompress(data, DATA_SEGMENT_OFFSETS[regionIndex])
		dataSegAddr = DATA_SEGMENT_ADDRESSES[regionIndex]
		if not dataSeg:
			print("Error: Compressed data segment not found.")
		else:
			dataBs = NoeBitStream(dataSeg, NOE_BIGENDIAN)
			resEntries, resMap = geReadTable(dataBs, RESOURCE_TABLE_OFFSETS[regionIndex], dataSegAddr, ResEntry)
			propEntries, propMap = geReadTable(dataBs, PROP_TABLE_OFFSETS[regionIndex], dataSegAddr, PropEntry)
			charEntries, charMap = geReadTable(dataBs, CHAR_TABLE_OFFSETS[regionIndex], dataSegAddr, CharEntry)
			gunEntries, gunMap = geReadTable(dataBs, GUN_TABLE_OFFSETS[regionIndex], dataSegAddr, GunEntry)
			ammoEntries, ammoMap = geReadTable(dataBs, AMMO_TABLE_OFFSETS[regionIndex], dataSegAddr, AmmoEntry)
			
			for resIndex in range(0, len(resEntries)):
				resEntry = resEntries[resIndex]
				if resIndex < len(resEntries) - 1:
					srcSize = 0
					nextRes = resIndex + 1
					while srcSize == 0: #handle aliased data
						srcSize = resEntries[nextRes].dataOffset - resEntry.dataOffset
						nextRes += 1
				else:
					srcSize = 16
				resData = geDecompress(data, resEntry.dataOffset)
				if not resData:
					resData = data[resEntry.dataOffset : resEntry.dataOffset + srcSize]
					
				if resEntry.name in propMap:
					resHeaderEntry = propEntries[propMap[resEntry.name]]
				elif resEntry.name in charMap:
					resHeaderEntry = charEntries[charMap[resEntry.name]]
				elif resEntry.name in gunMap:
					resHeaderEntry = gunEntries[gunMap[resEntry.name]]
				elif resEntry.name in ammoMap:
					resHeaderEntry = ammoEntries[ammoMap[resEntry.name]]
				else:
					resHeaderEntry = None
					
				if resHeaderEntry:
					resExt = ".ge64_mdl"
					resData = resHeaderEntry.buildHeader(dataBs, dataSegAddr) + resData
				else:
					resExt = geExtensionForRes(resEntry.name, resData)
					
				exName = resEntry.name + resExt
				print("Writing", exName)
				rapi.exportArchiveFile(exName, resData)

			resEndIndex = resMap["ob/ob_end.seg"]
			endEntry = resEntries[resIndex]
			imagesOffset = endEntry.dataOffset + 16

			imageTableOffset = IMAGE_TABLE_OFFSETS[regionIndex]
			curImageOffset = imagesOffset
			imageCount = 0
			dataBs.seek(imageTableOffset, NOESEEK_ABS)
			while True:
				entry = ImageTableEntry(dataBs, curImageOffset)
				if not entry.isValid():
					break
				imageCount += 1
				curImageOffset += entry.imageSize
			imageCount = imageCount
			imageTableSize = dataBs.tell() - imageTableOffset
			imageRomSize = curImageOffset - imagesOffset
			imgBundleData = bytearray()
			imgBundleData += IMAGE_BUNDLE_ID.encode("ASCII")
			imgBundleData += noePack(">IIII", IMAGE_BUNDLE_VERSION, 0, 0, 0)
			imgBundleData += noePack(">III", imageCount, imageTableSize, imageRomSize)
			imgBundleData += dataSeg[imageTableOffset : imageTableOffset + imageTableSize]
			imgBundleData += data[imagesOffset : imagesOffset + imageRomSize]
			exName = IMAGE_BUNDLE_NAME
			print("Writing", exName)
			rapi.exportArchiveFile(exName, imgBundleData)

			#possible todo - export other anim types
			animType0Data = geCreateExportAnimSetData(data, dataBs, ANIM_BUNDLE_MAPPING_TYPE_0, ANIM_TYPE0_HEADEROFFSETS_OFFSETS[regionIndex], ANIM_TYPE0_HEADERS_OFFSETS[regionIndex], ANIM_TYPE0_ROTDATA_OFFSETS[regionIndex])
			if animType0Data:
				exName = ANIM_TYPE0_NAME
				print("Writing", exName)
				rapi.exportArchiveFile(exName, animType0Data)

	return 1
