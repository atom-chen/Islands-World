---@class Coolape.MyMainCamera : UnityEngine.MonoBehaviour
---@field public isbeControled System.Boolean
---@field public list BetterList1Coolape.MyMainCamera
---@field public GetKeyDown Coolape.MyMainCamera.GetKeyStateFunc
---@field public GetKeyUp Coolape.MyMainCamera.GetKeyStateFunc
---@field public GetKey Coolape.MyMainCamera.GetKeyStateFunc
---@field public GetAxis Coolape.MyMainCamera.GetAxisFunc
---@field public onScreenResize Coolape.MyMainCamera.OnScreenResize
---@field public eventType Coolape.MyMainCamera.EventType
---@field public eventsGoToColliders System.Boolean
---@field public eventReceiverMask UnityEngine.LayerMask
---@field public debug System.Boolean
---@field public useMouse System.Boolean
---@field public useTouch System.Boolean
---@field public allowMultiTouch System.Boolean
---@field public useKeyboard System.Boolean
---@field public useController System.Boolean
---@field public stickyTooltip System.Boolean
---@field public tooltipDelay System.Single
---@field public longPressTooltip System.Boolean
---@field public mouseDragThreshold System.Single
---@field public mouseClickThreshold System.Single
---@field public touchDragThreshold System.Single
---@field public touchClickThreshold System.Single
---@field public rangeDistance System.Single
---@field public scrollAxisName System.String
---@field public verticalAxisName System.String
---@field public horizontalAxisName System.String
---@field public commandClick System.Boolean
---@field public submitKey0 UnityEngine.KeyCode
---@field public submitKey1 UnityEngine.KeyCode
---@field public cancelKey0 UnityEngine.KeyCode
---@field public cancelKey1 UnityEngine.KeyCode
---@field public onCustomInput Coolape.MyMainCamera.OnCustomInput
---@field public showTooltips System.Boolean
---@field public lastTouchPosition UnityEngine.Vector2
---@field public lastWorldPosition UnityEngine.Vector3
---@field public lastHit UnityEngine.RaycastHit
---@field public current Coolape.MyMainCamera
---@field public currentCamera UnityEngine.Camera
---@field public currentScheme Coolape.MyMainCamera.ControlScheme
---@field public currentTouchID System.Int32
---@field public currentKey UnityEngine.KeyCode
---@field public currentTouch Coolape.MyMainCamera.MouseOrTouch
---@field public inputHasFocus System.Boolean
---@field public fallThrough UnityEngine.GameObject
---@field public onClick Coolape.MyMainCamera.VoidDelegate
---@field public onDoubleClick Coolape.MyMainCamera.VoidDelegate
---@field public onHover Coolape.MyMainCamera.BoolDelegate
---@field public onPress Coolape.MyMainCamera.BoolDelegate
---@field public onSelect Coolape.MyMainCamera.BoolDelegate
---@field public onScroll Coolape.MyMainCamera.FloatDelegate
---@field public onDrag Coolape.MyMainCamera.VectorDelegate
---@field public onDragStart Coolape.MyMainCamera.VoidDelegate
---@field public onDragOver Coolape.MyMainCamera.ObjectDelegate
---@field public onDragOut Coolape.MyMainCamera.ObjectDelegate
---@field public onDragEnd Coolape.MyMainCamera.VoidDelegate
---@field public onDrop Coolape.MyMainCamera.ObjectDelegate
---@field public onKey Coolape.MyMainCamera.KeyCodeDelegate
---@field public onTooltip Coolape.MyMainCamera.BoolDelegate
---@field public onMouseMove Coolape.MyMainCamera.MoveDelegate
---@field public controller Coolape.MyMainCamera.MouseOrTouch
---@field public activeTouches System.Collections.Generic.List1Coolape.MyMainCamera.MouseOrTouch
---@field public isDragging System.Boolean
---@field public hoveredObject UnityEngine.GameObject
---@field public GetInputTouchCount Coolape.MyMainCamera.GetTouchCountCallback
---@field public GetInputTouch Coolape.MyMainCamera.GetTouchCallback
---@field public currentRay UnityEngine.Ray
---@field public cachedCamera UnityEngine.Camera
---@field public isOverUI System.Boolean
---@field public selectedObject UnityEngine.GameObject
---@field public dragCount System.Int32
---@field public mainCamera UnityEngine.Camera
---@field public eventHandler Coolape.MyMainCamera
---@field public dragThreshold System.Single
local m = { }
---public MyMainCamera .ctor()
---@return MyMainCamera
function m.New() end
---public Boolean IsPressed(GameObject go)
---@return bool
---@param optional GameObject go
function m.IsPressed(go) end
---public Int32 CountInputSources()
---@return number
function m.CountInputSources() end
---public Boolean Raycast(Vector3 inPos)
---@return bool
---@param optional Vector3 inPos
function m.Raycast(inPos) end
---public Boolean IsHighlighted(GameObject go)
---@return bool
---@param optional GameObject go
function m.IsHighlighted(go) end
---public MyMainCamera FindCameraForLayer(Int32 layer)
---@return MyMainCamera
---@param optional Int32 layer
function m.FindCameraForLayer(layer) end
---public Void Notify(GameObject go, String funcName, Object obj)
---@param optional GameObject go
---@param optional String funcName
---@param optional Object obj
function m.Notify(go, funcName, obj) end
---public MouseOrTouch GetMouse(Int32 button)
---@return MouseOrTouch
---@param optional Int32 button
function m.GetMouse(button) end
---public MouseOrTouch GetTouch(Int32 id)
---@return MouseOrTouch
---@param optional Int32 id
function m.GetTouch(id) end
---public Void RemoveTouch(Int32 id)
---@param optional Int32 id
function m.RemoveTouch(id) end
---public Void Update()
function m:Update() end
---public Void LateUpdate()
function m:LateUpdate() end
---public Void ProcessMouse()
function m:ProcessMouse() end
---public Void ProcessTouches()
function m:ProcessTouches() end
---public Void ProcessOthers()
function m:ProcessOthers() end
---public Void ProcessRelease()
function m:ProcessRelease() end
---public Void ProcessTouch(Boolean pressed, Boolean released)
---@param optional Boolean pressed
---@param optional Boolean released
function m:ProcessTouch(pressed, released) end
---public Void ShowTooltip(Boolean val)
---@param optional Boolean val
function m:ShowTooltip(val) end
Coolape.MyMainCamera = m
return m
