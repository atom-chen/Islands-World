---@class System.IO.Directory

local m = { }
---public String[] GetFiles(String path)
---public String[] GetFiles(String path, String searchPattern)
---public String[] GetFiles(String path, String searchPattern, SearchOption searchOption)
---@return table
---@param String path
---@param String searchPattern
---@param optional SearchOption searchOption
function m.GetFiles(path, searchPattern, searchOption) end
---public String[] GetDirectories(String path)
---public String[] GetDirectories(String path, String searchPattern)
---public String[] GetDirectories(String path, String searchPattern, SearchOption searchOption)
---@return table
---@param String path
---@param String searchPattern
---@param optional SearchOption searchOption
function m.GetDirectories(path, searchPattern, searchOption) end
---public String[] GetFileSystemEntries(String path)
---public String[] GetFileSystemEntries(String path, String searchPattern)
---public String[] GetFileSystemEntries(String path, String searchPattern, SearchOption searchOption)
---@return table
---@param String path
---@param String searchPattern
---@param optional SearchOption searchOption
function m.GetFileSystemEntries(path, searchPattern, searchOption) end
---public IEnumerable`1 EnumerateDirectories(String path)
---public IEnumerable`1 EnumerateDirectories(String path, String searchPattern)
---public IEnumerable`1 EnumerateDirectories(String path, String searchPattern, SearchOption searchOption)
---@return IEnumerable`1
---@param String path
---@param String searchPattern
---@param optional SearchOption searchOption
function m.EnumerateDirectories(path, searchPattern, searchOption) end
---public IEnumerable`1 EnumerateFiles(String path)
---public IEnumerable`1 EnumerateFiles(String path, String searchPattern)
---public IEnumerable`1 EnumerateFiles(String path, String searchPattern, SearchOption searchOption)
---@return IEnumerable`1
---@param String path
---@param String searchPattern
---@param optional SearchOption searchOption
function m.EnumerateFiles(path, searchPattern, searchOption) end
---public IEnumerable`1 EnumerateFileSystemEntries(String path)
---public IEnumerable`1 EnumerateFileSystemEntries(String path, String searchPattern)
---public IEnumerable`1 EnumerateFileSystemEntries(String path, String searchPattern, SearchOption searchOption)
---@return IEnumerable`1
---@param String path
---@param String searchPattern
---@param optional SearchOption searchOption
function m.EnumerateFileSystemEntries(path, searchPattern, searchOption) end
---public String GetDirectoryRoot(String path)
---@return String
---@param optional String path
function m.GetDirectoryRoot(path) end
---public DirectoryInfo CreateDirectory(String path)
---public DirectoryInfo CreateDirectory(String path, DirectorySecurity directorySecurity)
---@return DirectoryInfo
---@param String path
---@param optional DirectorySecurity directorySecurity
function m.CreateDirectory(path, directorySecurity) end
---public Void Delete(String path)
---public Void Delete(String path, Boolean recursive)
---@param String path
---@param optional Boolean recursive
function m.Delete(path, recursive) end
---public Boolean Exists(String path)
---@return bool
---@param optional String path
function m.Exists(path) end
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
---public DateTime GetCreationTime(String path)
---@return DateTime
---@param optional String path
function m.GetCreationTime(path) end
---public DateTime GetCreationTimeUtc(String path)
---@return DateTime
---@param optional String path
function m.GetCreationTimeUtc(path) end
---public String GetCurrentDirectory()
---@return String
function m.GetCurrentDirectory() end
---public String[] GetLogicalDrives()
---@return table
function m.GetLogicalDrives() end
---public DirectoryInfo GetParent(String path)
---@return DirectoryInfo
---@param optional String path
function m.GetParent(path) end
---public Void Move(String sourceDirName, String destDirName)
---@param optional String sourceDirName
---@param optional String destDirName
function m.Move(sourceDirName, destDirName) end
---public Void SetAccessControl(String path, DirectorySecurity directorySecurity)
---@param optional String path
---@param optional DirectorySecurity directorySecurity
function m.SetAccessControl(path, directorySecurity) end
---public Void SetCreationTime(String path, DateTime creationTime)
---@param optional String path
---@param optional DateTime creationTime
function m.SetCreationTime(path, creationTime) end
---public Void SetCreationTimeUtc(String path, DateTime creationTimeUtc)
---@param optional String path
---@param optional DateTime creationTimeUtc
function m.SetCreationTimeUtc(path, creationTimeUtc) end
---public Void SetCurrentDirectory(String path)
---@param optional String path
function m.SetCurrentDirectory(path) end
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
---public DirectorySecurity GetAccessControl(String path)
---public DirectorySecurity GetAccessControl(String path, AccessControlSections includeSections)
---@return DirectorySecurity
---@param String path
---@param optional AccessControlSections includeSections
function m.GetAccessControl(path, includeSections) end
System.IO.Directory = m
return m
