from inc_noesis import *
import os

FMS_SECTOR_SIZE = 128
FMS_SECTOR_USABLE = 125
FMS_DIR_SECTOR = 361
FMS_DIR_SECTOR_COUNT = 8
FMS_MAX_SIZE = 720 * FMS_SECTOR_SIZE
FMS_USED_BIT = (1 << 6)

EXTRACT_UNUSED_DIR_ENTRIES = False

def registerNoesisTypes():
	handle = noesis.register("Atari 810 FMS Image", ".fms")
	noesis.setHandlerExtractArc(handle, fmsExtractArc)
	return 1

def fmsSectorToOffset(sector):
	return (sector - 1) * FMS_SECTOR_SIZE

class FmsDirEntry:
	def __init__(self, bs, entryIndex):
		self.index = entryIndex
		self.flags = bs.readUByte()
		self.sectorCount = bs.readUShort()
		self.sectorStartIndex = bs.readUShort()
		if self.isValid():
			self.name = noeStrFromBytes(bs.readBytes(8)).rstrip(" ")
			extName = noeStrFromBytes(bs.readBytes(3)).rstrip(" ")
			if extName:
				self.name += "." + extName
		else:
			self.name = ""
			bs.readBytes(11)
				
	def isValid(self):
		if not EXTRACT_UNUSED_DIR_ENTRIES and not self.flags & FMS_USED_BIT:
			return False
		return self.sectorCount > 0
	
def fmsReadSectors(bs, sectorReadCount, sectorStartIndex, recordIndex):
	data = bytearray()
	try:
		currentSectorIndex = sectorStartIndex
		for sectorReadIndex in range(sectorReadCount):
			bs.seek(fmsSectorToOffset(currentSectorIndex), NOESEEK_ABS)
			sectorData = bs.readBytes(FMS_SECTOR_USABLE)
			secAndCheck = bs.readUByte()
			expectedRecordIndex = secAndCheck >> 2
			if expectedRecordIndex != recordIndex:
				print("	Warning: Record index", recordIndex, "doesn't match in sector", sectorReadIndex, "/", currentSectorIndex, "-", expectedRecordIndex)
			currentSectorIndex = ((secAndCheck & 3) << 8) | bs.readUByte()
			sizeByte = bs.readUByte()
			#some reports that sector is only shortened if bit 0x80 is set, but dealing with data where that isn't the case
			sectorData = sectorData[:sizeByte & 0x7F]
			data += sectorData
			if currentSectorIndex == 0:
				break
	except:
		print("	Warning: File is corrupt/incomplete.")
	return data

def fmsExtractArc(fileName, fileLen, justChecking):
	dirOffset = fmsSectorToOffset(FMS_DIR_SECTOR)
	if fileLen <= dirOffset or fileLen > FMS_MAX_SIZE:
		return 0

	entries = []
	
	try:
		with open(fileName, "rb") as f:
			bs = NoeBitStream(f.read())
			bs.seek(dirOffset, NOESEEK_ABS)
			maxEntryCount = (FMS_DIR_SECTOR_COUNT * FMS_SECTOR_SIZE) // 16
			for entryIndex in range(maxEntryCount):
				entry = FmsDirEntry(bs, entryIndex)
				if entry.isValid():
					entries.append(entry)
	except:
		return 0

	if not entries:
		return 0
		
	if justChecking:
		return 1
		
	for entry in entries:
		print("Writing", entry.name)
		data = fmsReadSectors(bs, entry.sectorCount, entry.sectorStartIndex, entry.index)
		rapi.exportArchiveFile(entry.name, data)

	return 1
