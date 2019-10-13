---@class Coolape.FileEx
---@field public FileTextMap System.Collections.Hashtable
---@field public FileBytesMap System.Collections.Hashtable
local m = { }
---public FileEx .ctor()
---@return FileEx
function m.New() end
---public Boolean FileExists(String fn)
---@return bool
---@param optional String fn
function m.FileExists(fn) end
---public Void WriteAllBytes(String fn, Byte[] bytes)
---@param optional String fn
---@param optional Byte[] bytes
function m.WriteAllBytes(fn, bytes) end
---public Byte[] ReadAllBytes(String fn)
---@return table
---@param optional String fn
function m.ReadAllBytes(fn) end
---public Void WriteAllText(String fn, String str)
---@param optional String fn
---@param optional String str
function m.WriteAllText(fn, str) end
---public Void AppendAllText(String fn, String str)
---@param optional String fn
---@param optional String str
function m.AppendAllText(fn, str) end
---public String ReadAllText(String fn)
---@return String
---@param optional String fn
function m.ReadAllText(fn) end
---public Void Delete(String fn)
---@param optional String fn
function m.Delete(fn) end
---public Boolean DirectoryExists(String path)
---@return bool
---@param optional String path
function m.DirectoryExists(path) end
---public Boolean CreateDirectory(String path)
---@return bool
---@param optional String path
function m.CreateDirectory(path) end
---public String[] GetFiles()
---public String[] GetFiles(String fn)
---@return table
---@param String fn
function m.GetFiles(fn) end
---public Void SaveTexture2D(String fn, Byte[] data)
---@param optional String fn
---@param optional Byte[] data
function m.SaveTexture2D(fn, data) end
---public Texture2D LoadTexture2D(Int32 w, Int32 h, String fn)
---@return Texture2D
---@param optional Int32 w
---@param optional Int32 h
---@param optional String fn
function m.LoadTexture2D(w, h, fn) end
---public String readTextFromStreamingAssetsPath(String filepath)
---@return String
---@param optional String filepath
function m.readTextFromStreamingAssetsPath(filepath) end
---public Byte[] readBytesFromStreamingAssetsPath(String filepath)
---@return table
---@param optional String filepath
function m.readBytesFromStreamingAssetsPath(filepath) end
---public String readNewAllText(String fName)
---@return String
---@param optional String fName
function m.readNewAllText(fName) end
---public IEnumerator readNewAllTextAsyn(String fName, Object OnGet)
---@return IEnumerator
---@param optional String fName
---@param optional Object OnGet
function m.readNewAllTextAsyn(fName, OnGet) end
---public Byte[] readNewAllBytes(String fName)
---@return table
---@param optional String fName
function m.readNewAllBytes(fName) end
---public IEnumerator readNewAllBytesAsyn(String fName, Object OnGet)
---@return IEnumerator
---@param optional String fName
---@param optional Object OnGet
function m.readNewAllBytesAsyn(fName, OnGet) end
---public String getTextFromCache(String path)
---@return String
---@param optional String path
function m.getTextFromCache(path) end
---public Byte[] getBytesFromCache(String path)
---@return table
---@param optional String path
function m.getBytesFromCache(path) end
---public Void cleanCache()
function m.cleanCache() end
Coolape.FileEx = m
return m
