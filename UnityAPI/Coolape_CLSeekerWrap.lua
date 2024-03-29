---@class Coolape.CLSeeker : UnityEngine.MonoBehaviour
---@field public mAStarPathSearch Coolape.CLAStarPathSearch
---@field public target UnityEngine.Transform
---@field public targetPos UnityEngine.Vector3
---@field public speed System.Single
---@field public turningSpeed System.Single
---@field public endReachedDistance System.Single
---@field public canSearchPath System.Boolean
---@field public canMove System.Boolean
---@field public autoMoveOnFinishSeek System.Boolean
---@field public movingBy Coolape.CLSeeker.MovingBy
---@field public unscaledTime System.Boolean
---@field public showPath System.Boolean
---@field public pathList System.Collections.Generic.List1UnityEngine.Vector3
---@field public onFinishSeekCallback System.Object
---@field public onMovingCallback System.Object
---@field public onArrivedCallback System.Object
---@field public isSeekTargetLoop System.Boolean
---@field public seekTargetLoopIntvalSec System.Single
---@field public nextSeekTargetTime System.Single
---@field public mTransform UnityEngine.Transform
---@field public cellSize System.Single
local m = { }
---public CLSeeker .ctor()
---@return CLSeeker
function m.New() end
---public Void Start()
function m:Start() end
---public Void init(Object onFinishSeekCallback, Object onMovingCallback, Object onArrivedCallback)
---@param optional Object onFinishSeekCallback
---@param optional Object onMovingCallback
---@param optional Object onArrivedCallback
function m:init(onFinishSeekCallback, onMovingCallback, onArrivedCallback) end
---public List`1 seekTarget(Transform target)
---public List`1 seekTarget(Transform target, Single searchIntvalSec)
---@return List`1
---@param Transform target
---@param optional Single searchIntvalSec
function m:seekTarget(target, searchIntvalSec) end
---public Void cancelSeekTarget()
function m:cancelSeekTarget() end
---public Void seekAsyn(Vector3 toPos)
---@param optional Vector3 toPos
function m:seekAsyn(toPos) end
---public List`1 seek(Vector3 toPos)
---public List`1 seek(Vector3 toPos, Single endReachDis)
---@return List`1
---@param Vector3 toPos
---@param optional Single endReachDis
function m:seek(toPos, endReachDis) end
---public Void Update()
function m:Update() end
---public Void FixedUpdate()
function m:FixedUpdate() end
---public Void startMove()
function m:startMove() end
---public Void stopMove()
function m:stopMove() end
Coolape.CLSeeker = m
return m
