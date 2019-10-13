---@class Coolape.ZipEx
local m = { }
---public ZipEx .ctor()
---@return ZipEx
function m.New() end
---public Void Zip(String SrcFile, String DstFile)
---@param optional String SrcFile
---@param optional String DstFile
function m.Zip(SrcFile, DstFile) end
---public Void UnZip(String SrcFile, String DstDir)
---public Void UnZip(String SrcFile, String DstFile, Int32 BufferSize)
---public Void UnZip(String zipedFile, String strDirectory, String password, Boolean overWrite)
---@param String zipedFile
---@param String strDirectory
---@param optional String password
---@param optional Boolean overWrite
function m.UnZip(zipedFile, strDirectory, password, overWrite) end
---public Void ZipFile(String fileToZip, String zipedFile)
---public Void ZipFile(String fileToZip, String zipedFile, Int32 compressionLevel, Int32 blockSize)
---@param String fileToZip
---@param String zipedFile
---@param optional Int32 compressionLevel
---@param optional Int32 blockSize
function m.ZipFile(fileToZip, zipedFile, compressionLevel, blockSize) end
---public Void ZipFileDirectory(String strDirectory, String zipedFile)
---@param optional String strDirectory
---@param optional String zipedFile
function m.ZipFileDirectory(strDirectory, zipedFile) end
Coolape.ZipEx = m
return m
