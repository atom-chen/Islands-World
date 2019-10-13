---@class Coolape.CLVerManager : Coolape.CLBaseLua
---@field public isPrintDownload System.Boolean
---@field public downLoadTimes4Failed System.Int32
---@field public baseUrl System.String
---@field public platform System.String
---@field public self Coolape.CLVerManager
---@field public resVer System.String
---@field public versPath System.String
---@field public fverVer System.String
---@field public verPriority System.String
---@field public localPriorityVer System.Collections.Hashtable
---@field public verOthers System.String
---@field public otherResVerOld System.Collections.Hashtable
---@field public otherResVerNew System.Collections.Hashtable
---@field public haveUpgrade System.Boolean
---@field public is2GNetUpgrade System.Boolean
---@field public is3GNetUpgrade System.Boolean
---@field public is4GNetUpgrade System.Boolean
---@field public mVerverPath System.String
---@field public mVerPrioriPath System.String
---@field public mVerOtherPath System.String
---@field public wwwMap System.Collections.Hashtable
---@field public wwwTimesMap System.Collections.Hashtable
---@field public clientVersion System.String
local m = { }
---public CLVerManager .ctor()
---@return CLVerManager
function m.New() end
---public Void initStreamingAssetsPackge(Callback onFinisInitStreaming)
---@param optional Callback onFinisInitStreaming
function m:initStreamingAssetsPackge(onFinisInitStreaming) end
---public Hashtable toMap(Byte[] buffer)
---@return Hashtable
---@param optional Byte[] buffer
function m:toMap(buffer) end
---public Void getNewestRes4Lua(String path, CLAssetType t, Object onGetAsset, Boolean autoRealseAB, Object originals)
---@param optional String path
---@param optional CLAssetType t
---@param optional Object onGetAsset
---@param optional Boolean autoRealseAB
---@param optional Object originals
function m:getNewestRes4Lua(path, type, onGetAsset, autoRealseAB, originals) end
---public Void getNewestRes(String path, CLAssetType t, Object onGetAsset, Boolean autoRealseAB, Object[] originals)
---@param optional String path
---@param optional CLAssetType t
---@param optional Object onGetAsset
---@param optional Boolean autoRealseAB
---@param optional Object[] originals
function m:getNewestRes(path, type, onGetAsset, autoRealseAB, originals) end
---public Void onGetNewstRes(UnityWebRequest www, String url, String path, CLAssetType t, Object content, Boolean needSave, Object onGetAsset, Boolean autoRealseAB, Object[] originals)
---@param optional UnityWebRequest www
---@param optional String url
---@param optional String path
---@param optional CLAssetType t
---@param optional Object content
---@param optional Boolean needSave
---@param optional Object onGetAsset
---@param optional Boolean autoRealseAB
---@param optional Object[] originals
function m:onGetNewstRes(www, url, path, type, content, needSave, onGetAsset, autoRealseAB, originals) end
---public Void setWWWListner(Object addWWWcb, Object rmWWWcb)
---@param optional Object addWWWcb
---@param optional Object rmWWWcb
function m:setWWWListner(addWWWcb, rmWWWcb) end
---public Void addWWW(UnityWebRequest www, String path, String url)
---@param optional UnityWebRequest www
---@param optional String path
---@param optional String url
function m:addWWW(www, path, url) end
---public Void rmWWW(String url)
---@param optional String url
function m:rmWWW(url) end
---public Texture getAtalsTexture4Edit(String path)
---@return Texture
---@param optional String path
function m:getAtalsTexture4Edit(path) end
---public Boolean checkNeedDownload(String path)
---@return bool
---@param optional String path
function m:checkNeedDownload(path) end
---public Boolean isVerNewest(String path, String ver)
---@return bool
---@param optional String path
---@param optional String ver
function m:isVerNewest(path, ver) end
---public Void getCurrentRes()
function m:getCurrentRes() end
Coolape.CLVerManager = m
return m
