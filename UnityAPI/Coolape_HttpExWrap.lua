---@class Coolape.HttpEx

local m = { }
---public HttpEx .ctor()
---@return HttpEx
function m.New() end
---public String readString(HttpWebResponse response)
---public String readString(String url, Nullable`1 timeout)
---public String readString(String url, Byte[] data, Nullable`1 timeout)
---@return String
---@param String url
---@param Byte[] data
---@param optional Nullable`1 timeout
function m.readString(url, data, timeout) end
---public Byte[] readBytes(String url, Nullable`1 timeout)
---public Byte[] readBytes(String url, Byte[] data, Nullable`1 timeout)
---@return table
---@param String url
---@param optional Byte[] data
---@param optional Nullable`1 timeout
function m.readBytes(url, data, timeout) end
---public HttpWebResponse CreateGetHttpResponse(String url, Nullable`1 timeout)
---@return HttpWebResponse
---@param optional String url
---@param optional Nullable`1 timeout
function m.CreateGetHttpResponse(url, timeout) end
---public HttpWebResponse CreatePostHttpResponse(String url, Byte[] data, Nullable`1 timeout)
---public HttpWebResponse CreatePostHttpResponse(String url, IDictionary`2 parameters, Nullable`1 timeout, Encoding requestEncoding)
---@return HttpWebResponse
---@param String url
---@param optional IDictionary`2 parameters
---@param optional Nullable`1 timeout
---@param optional Encoding requestEncoding
function m.CreatePostHttpResponse(url, parameters, timeout, requestEncoding) end
---public Hashtable get2json(String url)
---@return Hashtable
---@param optional String url
function m.get2json(url) end
---public String get2str(String url)
---@return String
---@param optional String url
function m.get2str(url) end
---public Byte[] get2bytes(String url)
---@return table
---@param optional String url
function m.get2bytes(url) end
---public Byte[] get2(String host, Int32 port, String path)
---public Byte[] get2(String host, Int32 port, String path, Int32 timeout)
---@return table
---@param String host
---@param optional Int32 port
---@param optional String path
---@param optional Int32 timeout
function m.get2(host, port, path, timeout) end
---public Byte[] post2(String host, Int32 port, String path, Byte[] buf)
---public Byte[] post2(String host, Int32 port, String path, Byte[] buf, Int32 timeout)
---@return table
---@param String host
---@param optional Int32 port
---@param optional String path
---@param optional Byte[] buf
---@param optional Int32 timeout
function m.post2(host, port, path, buf, timeout) end
---public String readLine(Stream stream)
---@return String
---@param optional Stream stream
function m.readLine(stream) end
Coolape.HttpEx = m
return m
