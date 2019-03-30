---@class System.IO.File

local m = { }
---public Void AppendAllText(String path, String contents)
---public Void AppendAllText(String path, String contents, Encoding encoding)
---@param String path
---@param optional String contents
---@param optional Encoding encoding
function m.AppendAllText(path, contents, encoding) end
---public StreamWriter AppendText(String path)
---@return StreamWriter
---@param optional String path
function m.AppendText(path) end
---public Void Copy(String sourceFileName, String destFileName)
---public Void Copy(String sourceFileName, String destFileName, Boolean overwrite)
---@param String sourceFileName
---@param optional String destFileName
---@param optional Boolean overwrite
function m.Copy(sourceFileName, destFileName, overwrite) end
---public FileStream Create(String path)
---public FileStream Create(String path, Int32 bufferSize)
---public FileStream Create(String path, Int32 bufferSize, FileOptions options)
---public FileStream Create(String path, Int32 bufferSize, FileOptions options, FileSecurity fileSecurity)
---@return FileStream
---@param String path
---@param Int32 bufferSize
---@param FileOptions options
---@param optional FileSecurity fileSecurity
function m.Create(path, bufferSize, options, fileSecurity) end
---public StreamWriter CreateText(String path)
---@return StreamWriter
---@param optional String path
function m.CreateText(path) end
---public Void Delete(String path)
---@param optional String path
function m.Delete(path) end
---public Boolean Exists(String path)
---@return bool
---@param optional String path
function m.Exists(path) end
---public FileSecurity GetAccessControl(String path)
---public FileSecurity GetAccessControl(String path, AccessControlSections includeSections)
---@return FileSecurity
---@param String path
---@param optional AccessControlSections includeSections
function m.GetAccessControl(path, includeSections) end
---public FileAttributes GetAttributes(String path)
---@return number
---@param optional String path
function m.GetAttributes(path) end
---public DateTime GetCreationTime(String path)
---@return DateTime
---@param optional String path
function m.GetCreationTime(path) end
---public DateTime GetCreationTimeUtc(String path)
---@return DateTime
---@param optional String path
function m.GetCreationTimeUtc(path) end
---public DateTime GetLastAccessTime(String path)
---@return DateTime
---@param optional String path
function m.GetLastAccessTime(path) end
---public DateTime GetLastAccessTimeUtc(String path)
---@return DateTime
---@param optional String path
function m.GetLastAccessTimeUtc(path) end
---public DateTime GetLastWriteTime(String path)
---@return DateTime
---@param optional String path
function m.GetLastWriteTime(path) end
---public DateTime GetLastWriteTimeUtc(String path)
---@return DateTime
---@param optional String path
function m.GetLastWriteTimeUtc(path) end
---public Void Move(String sourceFileName, String destFileName)
---@param optional String sourceFileName
---@param optional String destFileName
function m.Move(sourceFileName, destFileName) end
---public FileStream Open(String path, FileMode mode)
---public FileStream Open(String path, FileMode mode, FileAccess access)
---public FileStream Open(String path, FileMode mode, FileAccess access, FileShare share)
---@return FileStream
---@param String path
---@param FileMode mode
---@param optional FileAccess access
---@param optional FileShare share
function m.Open(path, mode, access, share) end
---public FileStream OpenRead(String path)
---@return FileStream
---@param optional String path
function m.OpenRead(path) end
---public StreamReader OpenText(String path)
---@return StreamReader
---@param optional String path
function m.OpenText(path) end
---public FileStream OpenWrite(String path)
---@return FileStream
---@param optional String path
function m.OpenWrite(path) end
---public Void Replace(String sourceFileName, String destinationFileName, String destinationBackupFileName)
---public Void Replace(String sourceFileName, String destinationFileName, String destinationBackupFileName, Boolean ignoreMetadataErrors)
---@param String sourceFileName
---@param optional String destinationFileName
---@param optional String destinationBackupFileName
---@param optional Boolean ignoreMetadataErrors
function m.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors) end
---public Void SetAccessControl(String path, FileSecurity fileSecurity)
---@param optional String path
---@param optional FileSecurity fileSecurity
function m.SetAccessControl(path, fileSecurity) end
---public Void SetAttributes(String path, FileAttributes fileAttributes)
---@param optional String path
---@param optional FileAttributes fileAttributes
function m.SetAttributes(path, fileAttributes) end
---public Void SetCreationTime(String path, DateTime creationTime)
---@param optional String path
---@param optional DateTime creationTime
function m.SetCreationTime(path, creationTime) end
---public Void SetCreationTimeUtc(String path, DateTime creationTimeUtc)
---@param optional String path
---@param optional DateTime creationTimeUtc
function m.SetCreationTimeUtc(path, creationTimeUtc) end
---public Void SetLastAccessTime(String path, DateTime lastAccessTime)
---@param optional String path
---@param optional DateTime lastAccessTime
function m.SetLastAccessTime(path, lastAccessTime) end
---public Void SetLastAccessTimeUtc(String path, DateTime lastAccessTimeUtc)
---@param optional String path
---@param optional DateTime lastAccessTimeUtc
function m.SetLastAccessTimeUtc(path, lastAccessTimeUtc) end
---public Void SetLastWriteTime(String path, DateTime lastWriteTime)
---@param optional String path
---@param optional DateTime lastWriteTime
function m.SetLastWriteTime(path, lastWriteTime) end
---public Void SetLastWriteTimeUtc(String path, DateTime lastWriteTimeUtc)
---@param optional String path
---@param optional DateTime lastWriteTimeUtc
function m.SetLastWriteTimeUtc(path, lastWriteTimeUtc) end
---public Byte[] ReadAllBytes(String path)
---@return table
---@param optional String path
function m.ReadAllBytes(path) end
---public String[] ReadAllLines(String path)
---public String[] ReadAllLines(String path, Encoding encoding)
---@return table
---@param String path
---@param optional Encoding encoding
function m.ReadAllLines(path, encoding) end
---public String ReadAllText(String path)
---public String ReadAllText(String path, Encoding encoding)
---@return String
---@param String path
---@param optional Encoding encoding
function m.ReadAllText(path, encoding) end
---public Void WriteAllBytes(String path, Byte[] bytes)
---@param optional String path
---@param optional Byte[] bytes
function m.WriteAllBytes(path, bytes) end
---public Void WriteAllLines(String path, String[] contents)
---public Void WriteAllLines(String path, String[] contents, Encoding encoding)
---@param String path
---@param optional String[] contents
---@param optional Encoding encoding
function m.WriteAllLines(path, contents, encoding) end
---public Void WriteAllText(String path, String contents)
---public Void WriteAllText(String path, String contents, Encoding encoding)
---@param String path
---@param optional String contents
---@param optional Encoding encoding
function m.WriteAllText(path, contents, encoding) end
---public Void Encrypt(String path)
---@param optional String path
function m.Encrypt(path) end
---public Void Decrypt(String path)
---@param optional String path
function m.Decrypt(path) end
System.IO.File = m
return m
