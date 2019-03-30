---@class Coolape.CLBehaviour4Lua : Coolape.CLBaseLua
---@field public flclean XLua.LuaFunction
---@field public flApplicationQuit XLua.LuaFunction
---@field public flStart XLua.LuaFunction
---@field public flAwake XLua.LuaFunction
---@field public flOnTriggerEnter XLua.LuaFunction
---@field public flOnTriggerExit XLua.LuaFunction
---@field public flOnTriggerStay XLua.LuaFunction
---@field public flOnCollisionEnter XLua.LuaFunction
---@field public flOnCollisionExit XLua.LuaFunction
---@field public flOnApplicationPause XLua.LuaFunction
---@field public flOnApplicationFocus XLua.LuaFunction
---@field public flOnBecameInvisible XLua.LuaFunction
---@field public flOnBecameVisible XLua.LuaFunction
---@field public flOnControllerColliderHit XLua.LuaFunction
---@field public flOnDestroy XLua.LuaFunction
---@field public flOnDisable XLua.LuaFunction
---@field public flOnEnable XLua.LuaFunction
---@field public flOnWillRenderObject XLua.LuaFunction
---@field public flOnPreRender XLua.LuaFunction
---@field public flOnPostRender XLua.LuaFunction
---@field public flOnClick XLua.LuaFunction
---@field public flOnPress XLua.LuaFunction
---@field public flOnDrag XLua.LuaFunction
---@field public flUIEventDelegate XLua.LuaFunction

local m = { }
---public CLBehaviour4Lua .ctor()
---@return CLBehaviour4Lua
function m.New() end
---public Void setLua()
function m:setLua() end
---public Void initGetLuaFunc()
function m:initGetLuaFunc() end
---public Void OnApplicationQuit()
function m:OnApplicationQuit() end
---public Void clean()
function m:clean() end
---public Void Start()
function m:Start() end
---public Void Awake()
function m:Awake() end
---public Void OnTriggerEnter(Collider other)
---@param optional Collider other
function m:OnTriggerEnter(other) end
---public Void OnTriggerExit(Collider other)
---@param optional Collider other
function m:OnTriggerExit(other) end
---public Void OnTriggerStay(Collider other)
---@param optional Collider other
function m:OnTriggerStay(other) end
---public Void OnCollisionEnter(Collision collision)
---@param optional Collision collision
function m:OnCollisionEnter(collision) end
---public Void OnCollisionExit(Collision collisionInfo)
---@param optional Collision collisionInfo
function m:OnCollisionExit(collisionInfo) end
---public Void OnApplicationPause(Boolean pauseStatus)
---@param optional Boolean pauseStatus
function m:OnApplicationPause(pauseStatus) end
---public Void OnApplicationFocus(Boolean focusStatus)
---@param optional Boolean focusStatus
function m:OnApplicationFocus(focusStatus) end
---public Void OnBecameInvisible()
function m:OnBecameInvisible() end
---public Void OnBecameVisible()
function m:OnBecameVisible() end
---public Void OnControllerColliderHit(ControllerColliderHit hit)
---@param optional ControllerColliderHit hit
function m:OnControllerColliderHit(hit) end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void OnDisable()
function m:OnDisable() end
---public Void OnEnable()
function m:OnEnable() end
---public Void OnWillRenderObject()
function m:OnWillRenderObject() end
---public Void OnPreRender()
function m:OnPreRender() end
---public Void OnPostRender()
function m:OnPostRender() end
---public Void OnClick()
function m:OnClick() end
---public Void OnPress(Boolean isPressed)
---@param optional Boolean isPressed
function m:OnPress(isPressed) end
---public Void OnDrag(Vector2 delta)
---@param optional Vector2 delta
function m:OnDrag(delta) end
---public Void uiEventDelegate(GameObject go)
---@param optional GameObject go
function m:uiEventDelegate(go) end
Coolape.CLBehaviour4Lua = m
return m
