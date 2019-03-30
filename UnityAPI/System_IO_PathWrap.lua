---@class System.IO.Path
---@field public AltDirectorySeparatorChar System.Char
---@field public DirectorySeparatorChar System.Char
---@field public PathSeparator System.Char
---@field public VolumeSeparatorChar System.Char

local m = { }
---public String ChangeExtension(String path, String extension)
---@return String
---@param optional String path
---@param optional String extension
function m.ChangeExtension(path, extension) end
---public String Combine(String path1, String path2)
---@return String
---@param optional String path1
---@param optional String path2
function m.Combine(path1, path2) end
---public String GetDirectoryName(String path)
---@return String
---@param optional String path
function m.GetDirectoryName(path) end
---public String GetExtension(String path)
---@return String
---@param optional String path
function m.GetExtension(path) end
---public String GetFileName(String path)
---@return String
---@param optional String path
function m.GetFileName(path) end
---public String GetFileNameWithoutExtension(String path)
---@return String
---@param optional String path
function m.GetFileNameWithoutExtension(path) end
---public String GetFullPath(String path)
---@return String
---@param optional String path
function m.GetFullPath(path) end
---public String GetPathRoot(String path)
---@return String
---@param optional String path
function m.GetPathRoot(path) end
---public String GetTempFileName()
---@return String
function m.GetTempFileName() end
---public String GetTempPath()
---@return String
function m.GetTempPath() end
---public Boolean HasExtension(String path)
---@return bool
---@param optional String path
function m.HasExtension(path) end
---public Boolean IsPathRooted(String path)
---@return bool
---@param optional String path
function m.IsPathRooted(path) end
---public Char[] GetInvalidFileNameChars()
---@return table
function m.GetInvalidFileNameChars() end
---public Char[] GetInvalidPathChars()
---@return table
function m.GetInvalidPathChars() end
---public String GetRandomFileName()
---@return String
function m.GetRandomFileName() end
System.IO.Path = m
return m
