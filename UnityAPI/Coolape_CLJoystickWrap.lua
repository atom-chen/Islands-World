---@class Coolape.CLJoystick : UIEventListener
---@field public joystickUI UnityEngine.Transform
---@field public joystickMoveDis System.Single

local m = { }
---public CLJoystick .ctor()
---@return CLJoystick
function m.New() end
---public Void init(Object onPress, Object onClick, Object onDrag)
---@param optional Object onPress
---@param optional Object onClick
---@param optional Object onDrag
function m:init(onPress, onClick, onDrag) end
Coolape.CLJoystick = m
return m
