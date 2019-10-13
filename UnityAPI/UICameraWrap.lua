---@class UICamera : UnityEngine.MonoBehaviour
---@field public list BetterList1UICamera
---@field public GetKeyDown UICamera.GetKeyStateFunc
---@field public GetKeyUp UICamera.GetKeyStateFunc
---@field public GetKey UICamera.GetKeyStateFunc
---@field public GetAxis UICamera.GetAxisFunc
---@field public onScreenResize UICamera.OnScreenResize
---@field public eventType UICamera.EventType
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
---@field public onCustomInput UICamera.OnCustomInput
---@field public showTooltips System.Boolean
---@field public lastTouchPosition UnityEngine.Vector2
---@field public lastWorldPosition UnityEngine.Vector3
---@field public lastHit UnityEngine.RaycastHit
---@field public lastHit2d UnityEngine.RaycastHit2D
---@field public current UICamera
---@field public currentCamera UnityEngine.Camera
---@field public currentScheme UICamera.ControlScheme
---@field public currentTouchID System.Int32
---@field public currentKey UnityEngine.KeyCode
---@field public currentTouch UICamera.MouseOrTouch
---@field public inputHasFocus System.Boolean
---@field public fallThrough UnityEngine.GameObject
---@field public onClick UICamera.VoidDelegate
---@field public onDoubleClick UICamera.VoidDelegate
---@field public onHover UICamera.BoolDelegate
---@field public onPress UICamera.BoolDelegate
---@field public onSelect UICamera.BoolDelegate
---@field public onScroll UICamera.FloatDelegate
---@field public onDrag UICamera.VectorDelegate
---@field public onDragStart UICamera.VoidDelegate
---@field public onDragOver UICamera.ObjectDelegate
---@field public onDragOut UICamera.ObjectDelegate
---@field public onDragEnd UICamera.VoidDelegate
---@field public onDrop UICamera.ObjectDelegate
---@field public onKey UICamera.KeyCodeDelegate
---@field public onTooltip UICamera.BoolDelegate
---@field public onMouseMove UICamera.MoveDelegate
---@field public controller UICamera.MouseOrTouch
---@field public activeTouches System.Collections.Generic.List1UICamera.MouseOrTouch
---@field public isDragging System.Boolean
---@field public hoveredObject UnityEngine.GameObject
---@field public GetInputTouchCount UICamera.GetTouchCountCallback
---@field public GetInputTouch UICamera.GetTouchCallback
---@field public currentRay UnityEngine.Ray
---@field public cachedCamera UnityEngine.Camera
---@field public isOverUI System.Boolean
---@field public selectedObject UnityEngine.GameObject
---@field public dragCount System.Int32
---@field public mainCamera UnityEngine.Camera
---@field public eventHandler UICamera
---@field public dragThreshold System.Single
local m = { }
---public UICamera .ctor()
---@return UICamera
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
---public UICamera FindCameraForLayer(Int32 layer)
---@return UICamera
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
---public Void ProcessRelease(Boolean isMouse, Single drag)
---@param Boolean isMouse
---@param Single drag
function m:ProcessRelease(isMouse, drag) end
---public Void ProcessTouch(Boolean pressed, Boolean released)
---@param optional Boolean pressed
---@param optional Boolean released
function m:ProcessTouch(pressed, released) end
---public Void ShowTooltip(Boolean val)
---@param optional Boolean val
function m:ShowTooltip(val) end
UICamera = m
return m
