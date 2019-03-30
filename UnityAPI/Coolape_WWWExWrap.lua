---@class Coolape.WWWEx : UnityEngine.MonoBehaviour
---@field public wwwMapUrl System.Collections.Hashtable
---@field public wwwMap4Check System.Collections.Hashtable
---@field public wwwMap4Get System.Collections.Hashtable

local m = { }
---public WWWEx .ctor()
---@return WWWEx
function m.New() end
---public Void newWWW(MonoBehaviour go, String url, CLAssetType t, Single checkProgressSec, Single timeOutSec, Object finishCallback, Object exceptionCallback, Object timeOutCallback, Object orgs)
---public Void newWWW(MonoBehaviour go, String url, String jsonMap, CLAssetType t, Single checkProgressSec, Single timeOutSec, Object finishCallback, Object exceptionCallback, Object timeOutCallback, Object orgs)
---public Void newWWW(MonoBehaviour go, String url, Object mapData, CLAssetType t, Single checkProgressSec, Single timeOutSec, Object finishCallback, Object exceptionCallback, Object timeOutCallback, Object orgs)
---@param MonoBehaviour go
---@param optional String url
---@param optional Object mapData
---@param optional CLAssetType t
---@param optional Single checkProgressSec
---@param optional Single timeOutSec
---@param optional Object finishCallback
---@param optional Object exceptionCallback
---@param optional Object timeOutCallback
---@param optional Object orgs
function m.newWWW(go, url, mapData, type, checkProgressSec, timeOutSec, finishCallback, exceptionCallback, timeOutCallback, orgs) end
---public Void newWWWPostBytes(MonoBehaviour go, String url, Byte[] mapData, CLAssetType t, Single checkProgressSec, Single timeOutSec, Object finishCallback, Object exceptionCallback, Object timeOutCallback, Object orgs)
---@param optional MonoBehaviour go
---@param optional String url
---@param optional Byte[] mapData
---@param optional CLAssetType t
---@param optional Single checkProgressSec
---@param optional Single timeOutSec
---@param optional Object finishCallback
---@param optional Object exceptionCallback
---@param optional Object timeOutCallback
---@param optional Object orgs
function m.newWWWPostBytes(go, url, mapData, type, checkProgressSec, timeOutSec, finishCallback, exceptionCallback, timeOutCallback, orgs) end
---public IEnumerator doNewWWW(MonoBehaviour go, String url, Object mapData, CLAssetType t, Single checkProgressSec, Single timeOutSec, Object finishCallback, Object exceptionCallback, Object timeOutCallback, Object orgs)
---@return IEnumerator
---@param optional MonoBehaviour go
---@param optional String url
---@param optional Object mapData
---@param optional CLAssetType t
---@param optional Single checkProgressSec
---@param optional Single timeOutSec
---@param optional Object finishCallback
---@param optional Object exceptionCallback
---@param optional Object timeOutCallback
---@param optional Object orgs
function m.doNewWWW(go, url, mapData, type, checkProgressSec, timeOutSec, finishCallback, exceptionCallback, timeOutCallback, orgs) end
---public Void checkWWWTimeout(MonoBehaviour go, WWW www, Single checkProgressSec, Single timeOutSec, Object timeoutCallback, Object orgs)
---@param optional MonoBehaviour go
---@param optional WWW www
---@param optional Single checkProgressSec
---@param optional Single timeOutSec
---@param optional Object timeoutCallback
---@param optional Object orgs
function m.checkWWWTimeout(go, www, checkProgressSec, timeOutSec, timeoutCallback, orgs) end
---public IEnumerator doCheckWWWTimeout(MonoBehaviour go, WWW www, Single checkProgressSec, Single timeOutSec, Object timeoutCallback, Single oldProgress, Single totalCostSec, Object orgs)
---@return IEnumerator
---@param optional MonoBehaviour go
---@param optional WWW www
---@param optional Single checkProgressSec
---@param optional Single timeOutSec
---@param optional Object timeoutCallback
---@param optional Single oldProgress
---@param optional Single totalCostSec
---@param optional Object orgs
function m.doCheckWWWTimeout(go, www, checkProgressSec, timeOutSec, timeoutCallback, oldProgress, totalCostSec, orgs) end
---public Void uncheckWWWTimeout(MonoBehaviour go, WWW www)
---@param optional MonoBehaviour go
---@param optional WWW www
function m.uncheckWWWTimeout(go, www) end
---public Void doCallback(Object callback, Object obj, Object orgs)
---@param optional Object callback
---@param optional Object obj
---@param optional Object orgs
function m.doCallback(callback, obj, orgs) end
---public WWW getWwwByUrl(String Url)
---@return WWW
---@param optional String Url
function m.getWwwByUrl(Url) end
---public Int32 getResponseCode(WWW request)
---@return number
---@param optional WWW request
function m.getResponseCode(request) end
---public Int32 parseResponseCode(String statusLine)
---@return number
---@param optional String statusLine
function m.parseResponseCode(statusLine) end
Coolape.WWWEx = m
return m
