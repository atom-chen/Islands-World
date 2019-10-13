---@class Coolape.MyTween : UnityEngine.MonoBehaviour
---@field public speed System.Single
---@field public turningSpeed System.Single
---@field public high System.Single
---@field public obstrucLayer UnityEngine.LayerMask
---@field public obsDistance System.Single
---@field public shadow UnityEngine.Transform
---@field public shadowHeight System.Single
---@field public curveSpeed UnityEngine.AnimationCurve
---@field public curveHigh UnityEngine.AnimationCurve
---@field public orgParams System.Object
---@field public ignoreTimeScale System.Boolean
---@field public isMoveNow System.Boolean
---@field public runOnStart System.Boolean
---@field public style UITweener.Style
---@field public from UnityEngine.Vector3
---@field public to UnityEngine.Vector3
---@field public transform UnityEngine.Transform
local m = { }
---public MyTween .ctor()
---@return MyTween
function m.New() end
---public Void flyout(Vector3 toPos, Single speed, Single ihight, Object onMovingCallback, Object finishCallback, Boolean isWoldPos)
---public Void flyout(Vector3 toPos, Single speed, Single ihight, Single angleOffset, Object onMovingCallback, Object finishCallback, Boolean isWoldPos)
---public Void flyout(Vector3 dir, Single distance, Single speed, Single hight, Single angleOffset, Object onMovingCallback, Object finishCallback, Boolean isWoldPos)
---public Void flyout(Vector3 toPos, Single speed, Single ihight, Single angleOffset, Object onMovingCallback, Object finishCallback, Object orgs, Boolean isWoldPos)
---public Void flyout(Vector3 dirFrom, Vector3 dirTo, Single distance, Single speed, Single hight, Single angleOffset, Object onMovingCallback, Object finishCallback, Boolean isWoldPos)
---@param Vector3 dirFrom
---@param Vector3 dirTo
---@param Single distance
---@param optional Single speed
---@param optional Single hight
---@param optional Single angleOffset
---@param optional Object onMovingCallback
---@param optional Object finishCallback
---@param optional Boolean isWoldPos
function m:flyout(dirFrom, dirTo, distance, speed, hight, angleOffset, onMovingCallback, finishCallback, isWoldPos) end
---public Void refreshToPos(Vector3 toPos, Single angleOffset)
---@param optional Vector3 toPos
---@param optional Single angleOffset
function m:refreshToPos(toPos, angleOffset) end
---public Void onFinishTween()
function m:onFinishTween() end
---public Void stop()
function m:stop() end
---public Void Update()
function m:Update() end
---public Void FixedUpdate()
function m:FixedUpdate() end
---public Void moveForward(Single speed)
---public Void moveForward(Single speed, Object onMovingCallback)
---@param Single speed
---@param optional Object onMovingCallback
function m:moveForward(speed, onMovingCallback) end
---public Void stopMoveForward()
function m:stopMoveForward() end
Coolape.MyTween = m
return m
