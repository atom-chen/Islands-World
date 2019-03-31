---@class Coolape.CLUIRenderQueue : UnityEngine.MonoBehaviour
---@field public _panel UIPanel
---@field public depth System.Int32
---@field public currRenderQueue System.Int32
---@field public isSharedMaterial System.Boolean
---@field public isForceUpdateSetRenderQueue System.Boolean
---@field public panel UIPanel
---@field public mDepth System.Int32
---@field public mWidget UIWidget

local m = { }
---public CLUIRenderQueue .ctor()
---@return CLUIRenderQueue
function m.New() end
---public Void Start()
function m:Start() end
---public Void reset()
---public Void reset(Boolean forceUpdate)
---@param Boolean forceUpdate
function m:reset(forceUpdate) end
---public Void setRenderQueueExe()
function m:setRenderQueueExe() end
---public Void setRenderQueue(Boolean isForce)
---@param optional Boolean isForce
function m:setRenderQueue(isForce) end
Coolape.CLUIRenderQueue = m
return m
