---@class UnityEngine.WWW : UnityEngine.CustomYieldInstruction
---@field public assetBundle UnityEngine.AssetBundle
---@field public bytes System.Byte
---@field public bytesDownloaded System.Int32
---@field public error System.String
---@field public isDone System.Boolean
---@field public progress System.Single
---@field public responseHeaders System.Collections.Generic.Dictionary2System.StringSystem.String
---@field public text System.String
---@field public texture UnityEngine.Texture2D
---@field public textureNonReadable UnityEngine.Texture2D
---@field public threadPriority UnityEngine.ThreadPriority
---@field public uploadProgress System.Single
---@field public url System.String
---@field public keepWaiting System.Boolean

local m = { }
---public WWW .ctor(String url)
---public WWW .ctor(String url, WWWForm form)
---public WWW .ctor(String url, Byte[] postData)
---public WWW .ctor(String url, Byte[] postData, Hashtable headers)
---public WWW .ctor(String url, Byte[] postData, Dictionary`2 headers)
---@return WWW
---@param String url
---@param Byte[] postData
---@param optional Dictionary`2 headers
function m.New(url, postData, headers) end
---public String EscapeURL(String s)
---public String EscapeURL(String s, Encoding e)
---@return String
---@param String s
---@param optional Encoding e
function m.EscapeURL(s, e) end
---public String UnEscapeURL(String s)
---public String UnEscapeURL(String s, Encoding e)
---@return String
---@param String s
---@param optional Encoding e
function m.UnEscapeURL(s, e) end
---public WWW LoadFromCacheOrDownload(String url, Int32 version)
---public WWW LoadFromCacheOrDownload(String url, Hash128 hash)
---public WWW LoadFromCacheOrDownload(String url, Int32 version, UInt32 crc)
---public WWW LoadFromCacheOrDownload(String url, Hash128 hash, UInt32 crc)
---public WWW LoadFromCacheOrDownload(String url, CachedAssetBundle cachedBundle, UInt32 crc)
---@return WWW
---@param String url
---@param optional CachedAssetBundle cachedBundle
---@param optional UInt32 crc
function m.LoadFromCacheOrDownload(url, cachedBundle, crc) end
---public Void LoadImageIntoTexture(Texture2D texture)
---@param optional Texture2D texture
function m:LoadImageIntoTexture(texture) end
---public Void Dispose()
function m:Dispose() end
---public AudioClip GetAudioClip()
---public AudioClip GetAudioClip(Boolean threeD)
---public AudioClip GetAudioClip(Boolean threeD, Boolean stream)
---public AudioClip GetAudioClip(Boolean threeD, Boolean stream, AudioType audioType)
---@return AudioClip
---@param Boolean threeD
---@param Boolean stream
---@param AudioType audioType
function m:GetAudioClip(threeD, stream, audioType) end
---public AudioClip GetAudioClipCompressed()
---public AudioClip GetAudioClipCompressed(Boolean threeD)
---public AudioClip GetAudioClipCompressed(Boolean threeD, AudioType audioType)
---@return AudioClip
---@param Boolean threeD
---@param AudioType audioType
function m:GetAudioClipCompressed(threeD, audioType) end
UnityEngine.WWW = m
return m
