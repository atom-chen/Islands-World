---@class Coolape.CLSeekerByRay : Coolape.CLSeeker
---@field public obstructMask UnityEngine.LayerMask
---@field public rayDistance System.Single
---@field public rayHeight System.Single
---@field public rayDirs Coolape.CLSeekerByRay.SearchDirs
---@field public maxSearchTimes System.Int32
---@field public isSoftenPath System.Boolean
---@field public softenPathType Coolape.CLAIPathUtl.SoftenPathType
---@field public softenFactor System.Int32
---@field public dirNum System.Int32
---@field public originPosition UnityEngine.Vector3
---@field public cellSize System.Single

local m = { }
---public CLSeekerByRay .ctor()
---@return CLSeekerByRay
function m.New() end
---public Void Start()
function m:Start() end
---public Void resetCache()
function m:resetCache() end
---public Void getPoints(Vector3 center, Vector3 eulerAngles, Single r, List`1& left, List`1& right)
---@param optional Vector3 center
---@param optional Vector3 eulerAngles
---@param optional Single r
---@param optional List`1& left
---@param optional List`1& right
function m:getPoints(center, eulerAngles, r, left, right) end
---public List`1 seek(Vector3 toPos)
---@return List`1
---@param optional Vector3 toPos
function m:seek(toPos) end
---public List`1 trySearchPathLeft(Vector3 toPos)
---@return List`1
---@param optional Vector3 toPos
function m:trySearchPathLeft(toPos) end
---public List`1 trySearchPathRight(Vector3 toPos)
---@return List`1
---@param optional Vector3 toPos
function m:trySearchPathRight(toPos) end
---public List`1 doSearchPath(Vector3 toPos)
---@return List`1
---@param optional Vector3 toPos
function m:doSearchPath(toPos) end
---public Boolean isInCircle(Vector3 pos, List`1 list)
---@return bool
---@param optional Vector3 pos
---@param optional List`1 list
function m:isInCircle(pos, list) end
---public Boolean canReach(Vector3 from, Vector3 to, Single endReachedDis)
---@return bool
---@param optional Vector3 from
---@param optional Vector3 to
---@param optional Single endReachedDis
function m:canReach(from, to, endReachedDis) end
---public List`1 getShortList(List`1 list1, List`1 list2)
---@return List`1
---@param optional List`1 list1
---@param optional List`1 list2
function m:getShortList(list1, list2) end
---public Void filterPath(List`1& list)
---@param optional List`1& list
function m:filterPath(list) end
---public _Dir getTargetDir(Vector3 fromPos, Vector3 toPos)
---@return number
---@param optional Vector3 fromPos
---@param optional Vector3 toPos
function m:getTargetDir(fromPos, toPos) end
Coolape.CLSeekerByRay = m
return m
