#the BYLE format is only used in source art, not runtime, so you won't find these files in the game data.
from inc_noesis import *

def registerNoesisTypes():
	handle = noesis.register("SCUMM BYLE", ".byl")
	noesis.setHandlerTypeCheck(handle, bylCheckType)
	noesis.setHandlerLoadRGBA(handle, bylLoadRGBA)
	noesis.addOption(handle, "-scummbylnocanv", "don't render to canvas.", 0)
	noesis.addOption(handle, "-scummbylseq", "use sequence data.", 0)
	noesis.addOption(handle, "-scummbylsubpart", "when using sequences, try to sub head/body parts.", 0)
	noesis.addOption(handle, "-scummbyllopal", "force global palette into low 16 colors.", 0)
	noesis.addOption(handle, "-scummbylnomir", "avoid exporting mirrored sequences.", 0)
	return 1

class BYLEChunk:
	def __init__(self, bs):
		self.offset = bs.tell()
		self.id = bs.readUInt()
		self.size = bs.readUInt()
		self.headerSize = bs.tell() - self.offset
		
class BYLEHeader:
	def __init__(self, bs):
		bs.setEndian(NOE_LITTLEENDIAN)
		self.u0 = bs.readUShort()
		self.celCount = bs.readUShort()
		bs.setEndian(NOE_BIGENDIAN)

class BYLEVar:
	def __init__(self, bs):
		bs.setEndian(NOE_LITTLEENDIAN)
		self.u0 = bs.readUInt()
		self.u1 = bs.readUInt()
		self.u2 = bs.readUShort()
		self.u3 = bs.readUShort()
		self.u4 = bs.readShort()
		self.u5 = bs.readShort()
		self.u6 = bs.readShort()
		self.u7 = bs.readShort()
		self.transparentIndex = bs.readShort()
		bs.setEndian(NOE_BIGENDIAN)

class BYLECelHeaderType1:
	def __init__(self, bs):
		bs.setEndian(NOE_LITTLEENDIAN)
		self.groupId = bs.readUByte()
		if self.isValid():
			self.partId = bs.readUByte()
			self.x = bs.readShort()
			self.y = bs.readShort()
			self.width = bs.readShort()
			self.height = bs.readShort()
			self.childX = bs.readShort()
			self.childY = bs.readShort()
		bs.setEndian(NOE_BIGENDIAN)
		
	def isValid(self):
		return self.groupId != 0xFF

class BYLECelHeader:
	def __init__(self, bs):
		bs.setEndian(NOE_LITTLEENDIAN)
		self.groupId = bs.readUShort()
		self.partId = bs.readUShort()
		self.x = bs.readShort()
		self.y = bs.readShort()
		self.width = bs.readShort()
		self.height = bs.readShort()
		self.childX = bs.readShort()
		self.childY = bs.readShort()
		bs.setEndian(NOE_BIGENDIAN)

class BYLECel:
	def __init__(self, byleFile):
		self.byleFile = byleFile
		self.celHeader = None
		self.bitmapData = None

	def initType1(self, palIndices, bs):
		hdr = BYLECelHeaderType1(bs)
		if not hdr.isValid():
			self.celHeader = None
			return
		self.celHeader = hdr
		self.bitmapData = bytearray(hdr.width * hdr.height)
		#expand to 8bpp direct palette indices
		for x in range(0, hdr.width, 2):
			for y in range(0, hdr.height):
				c = bs.readUByte()
				self.bitmapData[y * hdr.width + x] = palIndices[c >> 4]
				self.bitmapData[y * hdr.width + x + 1] = palIndices[c & 15]
		
	def initType2(self, baseChunk, bs):
		chunkEnd = baseChunk.offset + baseChunk.size
		while bs.tell() < chunkEnd:
			chunk = BYLEChunk(bs)
			if chunk.id == 0x43454C48: #CELH
				self.celHeader = BYLECelHeader(bs)
			elif chunk.id == 0x424D4150: #BMAP
				self.bitmapData = bs.readBytes(chunk.size - chunk.headerSize)
			bs.seek(chunk.offset + chunk.size, NOESEEK_ABS)

	def isValid(self):
		return self.celHeader and self.bitmapData

	def getBounds(self, parentObject):
		hdr = self.celHeader
		if parentObject:
			hdrX, hdrY = parentObject.calculateCelPosition(self)
		else:
			hdrX, hdrY = hdr.x, hdr.y		
		return hdrX, hdrY, hdrX + hdr.width, hdrY + hdr.height
		
	def getRowOrderedBitmap(self, flipFlags):
		hdr = self.celHeader
		data = self.bitmapData
		if self.byleFile.type == 1:
			#already row-ordered, but still need to handle flipFlags
			if flipFlags == 0:
				return data
			else:
				ordered = bytearray(hdr.width * hdr.height)
				for x in range(0, hdr.width):
					for y in range(0, hdr.height):
						dstX = hdr.width - x - 1 if flipFlags & 1 else x
						dstY = hdr.height - y - 1 if flipFlags & 2 else y
						ordered[dstY * hdr.width + dstX] = data[y * hdr.width + x]
				return ordered

		ordered = bytearray(hdr.width * hdr.height)
		for x in range(0, hdr.width):
			for y in range(0, hdr.height):
				dstX = hdr.width - x - 1 if flipFlags & 1 else x
				dstY = hdr.height - y - 1 if flipFlags & 2 else y
				ordered[dstY * hdr.width + dstX] = data[x * hdr.height + y]
		return ordered
			
	def generateRgba(self, clut, transparentIndex, flipFlags):
		if not clut:
			return None
		hdr = self.celHeader
		if hdr.width <= 0 or hdr.height <= 0:
			return None
		rgbaPal = rapi.imageDecodeRaw(clut, len(clut) // 3, 1, "r8g8b8")
		if transparentIndex >= 0:
			rgbaPal[transparentIndex * 4 + 3] = 0
		return rapi.imageDecodeRawPal(self.getRowOrderedBitmap(flipFlags), rgbaPal, hdr.width, hdr.height, 8, "r8g8b8a8")
			
	def paintToCanvas(self, rgba, canvasRgba, canvasBaseX, canvasBaseY, canvasWidth, canvasHeight, frame):
		hdr = self.celHeader
		if frame:
			hdrX, hdrY = frame.calculateCelPosition(self)
		else:
			hdrX, hdrY = hdr.x, hdr.y
		canvasX = hdrX - canvasBaseX
		canvasY = hdrY - canvasBaseY
		rapi.imageBlit32(canvasRgba, canvasWidth, canvasHeight, canvasX, canvasY, rgba, hdr.width, hdr.height, 0, 0, 0, 0, noesis.BLITFLAG_ALPHABLEND)
			
	def createTexture(self, name, clut, transparentIndex, canvasBaseX, canvasBaseY, canvasWidth, canvasHeight):
		rgba = self.generateRgba(clut, transparentIndex, 0)
		if not rgba:
			return None

		if canvasWidth > 0 and canvasHeight > 0:
			canvasRgba = bytearray(canvasWidth * canvasHeight * 4)
			self.paintToCanvas(rgba, canvasRgba, canvasBaseX, canvasBaseY, canvasWidth, canvasHeight, None)
			return NoeTexture(name, canvasWidth, canvasHeight, canvasRgba, noesis.NOESISTEX_RGBA32)
		else:
			hdr = self.celHeader
			return NoeTexture(name, hdr.width, hdr.height, rgba, noesis.NOESISTEX_RGBA32)

class BYLESequenceFrame:
	def __init__(self, byleSeq, cels, seqs, groupList, partLookup, direction, frameIndex):
		byleFile = byleSeq.byleFile
		self.byleSeq = byleSeq
		self.cels = []
		self.frameIndex = frameIndex
		self.direction = direction
		self.applyFlip = True
		for groupIndex in range(0, byleFile.maxGroupCount):
			partList = groupList[groupIndex] if groupIndex < len(groupList) else []
			partFrameIndex = frameIndex
			if len(partList) == 0 and noesis.optWasInvoked("-scummbylsubpart"):
				stubSeqIndex = 4 + direction
				if not stubSeqIndex in seqs:
					stubSeqIndex = 4
				if stubSeqIndex in seqs:
					stubGroupList = seqs[stubSeqIndex]
					if groupIndex < len(stubGroupList):
						partList = stubGroupList[groupIndex]
						partFrameIndex = 0
			
			if len(partList) > 0:
				#not quite right as parts don't obey individually tracked repeat/hold/etc. commands, but good enough for now
				partId = partList[partFrameIndex] if partFrameIndex < len(partList) else partList[-1]
				partKey = byleFile.generatePartLookupKey(groupIndex, partId)
				if partKey in partLookup:
					cel = partLookup[partKey]
					self.cels.append(cel)
					
	def calculateBounds(self):
		byleFile = self.byleSeq.byleFile
		self.bounds = byleFile.calcCanvasRect(self.cels, self)

	def getBounds(self, parentObject):
		return self.bounds
		
	def getFlipFlags(self):
		return 1 if self.direction == 1 and self.applyFlip else 0
		
	def calculateCelPosition(self, cel):
		hdr = cel.celHeader
		hdrX, hdrY = hdr.x, hdr.y
	
		#not totally sure if this is the right way to handle this, but looks right
		for otherCel in self.cels:
			otherHdr = otherCel.celHeader
			if otherHdr.groupId < hdr.groupId:
				hdrX += otherHdr.childX
				hdrY -= otherHdr.childY
		
		flipFlags = self.getFlipFlags()
		if flipFlags:
			seqMinX, seqMinY, seqMaxX, seqMaxY = self.byleSeq.getBounds(None)
			if flipFlags & 1:
				deltaX = hdrX - seqMinX
				hdrX = seqMaxX - hdr.width - deltaX
			if flipFlags & 2:
				deltaY = hdrY - seqMinY
				hdrY = seqMaxY - hdr.width - deltaY
		return hdrX, hdrY

class BYLESequence:
	def __init__(self, byleFile, cels, seqs, seqNames, partLookup, seqIndex):
		self.byleFile = byleFile
		self.seqIndex = seqIndex
		self.frames = []
		self.bounds = None
		frameCount = 0
		groupList = seqs[seqIndex]
		for partList in groupList:
			frameCount = max(frameCount, len(partList))
		if frameCount > 0:
			self.direction = seqIndex & 3
			nameIndex = seqIndex // 4
			self.name = seqNames[nameIndex] if nameIndex < len(seqNames) else "unknown"
			for frameIndex in range(0, frameCount):
				self.frames.append(BYLESequenceFrame(self, cels, seqs, groupList, partLookup, self.direction, frameIndex))
		self.calculateBounds()

	def calculateBounds(self):
		byleFile = self.byleFile
		for frame in self.frames:
			frame.applyFlip = False
			frame.calculateBounds()
			frame.applyFlip = True
		self.bounds = byleFile.calcCanvasRect(self.frames, self)
		
	def getBounds(self, parentObject):
		return self.bounds

class BYLEFile:
	def __init__(self, data):
		self.data = data
		self.bs = NoeBitStream(data, NOE_BIGENDIAN)
		self.maxGroupCount = 16

	def determineType(self):
		if len(self.data) < 8:
			return 0
		bs = self.bs
		bs.seek(0, NOESEEK_ABS)
		chunk = BYLEChunk(bs)
		if chunk.id == 0x42594C45 and chunk.size <= len(self.data):
			self.type = 2
			return 1
		if chunk.id == 0x80050100:
			self.type = 1
			return 1
		return 0
		
	def generatePartLookupKey(self, groupId, partId):
		return groupId | (partId << 16)
		
	def parseSequenceData(self, bs, endOffset):
		frameDict = {}
		firstStopCmdId = 0x7D
		while bs.tell() < endOffset:
			frameId = bs.readUByte()
			if frameId == 0xFF:
				break
			groupList = []
			partList = []
			groupId = 0
			while groupId < self.maxGroupCount:
				partId = bs.readUByte()
				if partId >= firstStopCmdId: #hold/loop/etc., for now don't care which
					if partId == 0xFF and len(partList) == 0:
						#explicit "kill part" command, make sure there's a bogus entry to avoid placing with subparts option
						partList.append(0x10000)
					elif len(partList) == 1 and partList[0] == 0x7A:
						#another hack - for 7A, explicitly clear list so part can be subbed
						partList = []
					groupId += 1
					#some commands, such as 7B, will just be added in as part id's.
					#in the case of 7B, this is "hide", so it will just naturally work out because no part will be found for the id.
					groupList.append(partList)
					partList = []
				else:
					partList.append(partId)
			frameDict[frameId] = groupList
		return frameDict
		
	def parseSequenceNames(self, bs):
		seqNames = []
		bs.setEndian(NOE_LITTLEENDIAN)
		seqCount = bs.readUShort() if self.type == 2 else 31
		for seqIndex in range(0, seqCount):
			seqNameData = bs.readBytes(80)
			firstZ = seqNameData.find(b"\0")
			if firstZ >= 0:
				seqNameData = seqNameData[:firstZ]
			seqNames.append(noeStrFromBytes(seqNameData))
		bs.setEndian(NOE_BIGENDIAN)
		return seqNames
	
	def calcCanvasRect(self, objects, parentObject = None):
		canvasMinX = 999999
		canvasMinY = 999999
		canvasMaxX = -999999
		canvasMaxY = -999999
		for object in objects:
			minX, minY, maxX, maxY = object.getBounds(parentObject)
			canvasMinX = min(canvasMinX, minX)
			canvasMinY = min(canvasMinY, minY)
			canvasMaxX = max(canvasMaxX, maxX)
			canvasMaxY = max(canvasMaxY, maxY)
		return canvasMinX, canvasMinY, canvasMaxX, canvasMaxY

	def createTextures(self, texList):
		bs = self.bs
		bs.seek(0, NOESEEK_ABS)
		byleHeader = None
		byleVar = None
		clut = None
		cels = []
		maxSeqCount = 124
		seqs = {}
		seqNames = []
		if self.type == 1:
			bs.seek(19, NOESEEK_ABS)
			palIndices = bs.readBytes(16)
			clut = bs.readBytes(768)
			if noesis.optWasInvoked("-scummbyllopal"):
				globalPal = bytearray((0x00, 0x00, 0x00, 0x00, 0x00, 0xAA, 0x00, 0xAA, 0x00, 0x00, 0xAA, 0xAA, 0xAA, 0x00, 0x00, 0xAA, 0x00, 0xAA, 0xAA, 0x55, 0x00, 0xAA, 0xAA, 0xAA, 0x55, 0x55, 0x55, 0x55, 0x55, 0xFF, 0x55, 0xFF, 0x55, 0x55, 0xFF, 0xFF, 0xFF, 0x55, 0x55, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF))
				clut = globalPal + clut[16*3:]
			bs.seek(255, NOESEEK_REL)
			transparentIndex = palIndices[bs.readUByte()]			
			bs.seek(804, NOESEEK_REL)
			dataSize = len(self.data)
			seqs = self.parseSequenceData(bs, dataSize - bs.tell())
			while bs.tell() < dataSize:
				cel = BYLECel(self)
				cel.initType1(palIndices, bs)
				if cel.isValid():
					cels.append(cel)
				else:
					break
			seqNames = self.parseSequenceNames(bs)
		else:
			while bs.tell() < len(self.data):
				chunk = BYLEChunk(bs)
				if chunk.size <= 0:
					break #invalid chunk
					
				if chunk.id == 0x42594C45: #BYLE
					continue #special-case, continue parsing through chunk contents
				elif chunk.id == 0x42594844: #BYHD
					byleHeader = BYLEHeader(bs)
				elif chunk.id == 0x42564152: #BVAR
					byleVar = BYLEVar(bs)
				elif chunk.id == 0x434C5554: #CLUT
					clut = bs.readBytes(chunk.size - chunk.headerSize)
				elif chunk.id == 0x43454C20: #CEL
					cel = BYLECel(self)
					cel.initType2(chunk, bs)
					cels.append(cel)
				elif chunk.id == 0x43534551: #CSEQ
					seqs = self.parseSequenceData(bs, chunk.offset + chunk.size)
				elif chunk.id == 0x43485458: #CHTX
					seqNames = self.parseSequenceNames(bs)
				bs.seek(chunk.offset + chunk.size, NOESEEK_ABS)

			transparentIndex = byleVar.transparentIndex if byleVar else -1

		useCanvas = not noesis.optWasInvoked("-scummbylnocanv")
		useSeqs = len(seqs) > 0 and noesis.optWasInvoked("-scummbylseq")
		recenterCels = True
		if useSeqs:
			useCanvas = True #force canvas if using sequence data

		byleSeqs = []
		if useSeqs:
			partLookup = {}
			for cel in cels:
				hdr = cel.celHeader
				partKey = self.generatePartLookupKey(hdr.groupId, hdr.partId)
				partLookup[partKey] = cel
			for seqIndex in range(0, maxSeqCount):
				if seqIndex in seqs:
					byleSeqs.append(BYLESequence(self, cels, seqs, seqNames, partLookup, seqIndex))
					
		canvasBaseX = 0
		canvasBaseY = 0
		canvasWidth = 0
		canvasHeight = 0
		if useCanvas:
			rectObjects = byleSeqs if useSeqs else cels
			canvasMinX, canvasMinY, canvasMaxX, canvasMaxY = self.calcCanvasRect(rectObjects)
			if recenterCels:
				#recenter all of the cels
				canvasCenterX = canvasMinX + (canvasMaxX - canvasMinX) // 2
				canvasCenterY = canvasMinY + (canvasMaxY - canvasMinY) // 2
				for cel in cels:
					hdr = cel.celHeader
					hdr.x -= canvasCenterX
					hdr.y -= canvasCenterY
				if useSeqs:
					for byleSeq in byleSeqs:
						byleSeq.calculateBounds()
				canvasMinX, canvasMinY, canvasMaxX, canvasMaxY = self.calcCanvasRect(rectObjects)
			if canvasMaxX > canvasMinX and canvasMaxY > canvasMinY:
				canvasWidth = canvasMaxX - canvasMinX
				canvasHeight = canvasMaxY - canvasMinY
				canvasBaseX = canvasMinX
				canvasBaseY = canvasMinY

		if useSeqs:
			dirNames = ("Right", "Left", "Forward", "Back")
			for byleSeq in byleSeqs:
				frameCount = len(byleSeq.frames)
				if frameCount > 0:
					if noesis.optWasInvoked("-scummbylnomir"):
						if byleSeq.direction == 1 and byleSeq.seqIndex - 1 in seqs:
							continue
					print("Sequence \"" + byleSeq.name + "\" (" + dirNames[byleSeq.direction] + ") at frame", len(texList), "-", frameCount, "frame(s).")
					for byleFrame in byleSeq.frames:
						canvasRgba = bytearray(canvasWidth * canvasHeight * 4)
						for cel in byleFrame.cels:
							rgba = cel.generateRgba(clut, transparentIndex, byleFrame.getFlipFlags())
							if rgba:
								cel.paintToCanvas(rgba, canvasRgba, canvasBaseX, canvasBaseY, canvasWidth, canvasHeight, byleFrame)
						tex = NoeTexture("seq%04iframe%04i"%(byleSeq.seqIndex, byleFrame.frameIndex), canvasWidth, canvasHeight, canvasRgba, noesis.NOESISTEX_RGBA32)
						texList.append(tex)
							
		else:
			for celIndex in range(0, len(cels)):
				cel = cels[celIndex]
				celTex = cel.createTexture("cel%04i"%celIndex, clut, transparentIndex, canvasBaseX, canvasBaseY, canvasWidth, canvasHeight)
				if celTex:
					texList.append(celTex)

def bylCheckType(data):
	byl = BYLEFile(data)
	if not byl.determineType():
		return 0
	return 1

def bylLoadRGBA(data, texList):
	byl = BYLEFile(data)
	byl.determineType()
	byl.createTextures(texList)
	return 1
