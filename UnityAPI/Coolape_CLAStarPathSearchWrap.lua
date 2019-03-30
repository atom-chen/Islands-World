---@class Coolape.CLAStarPathSearch : UnityEngine.MonoBehaviour
---@field public current Coolape.CLAStarPathSearch
---@field public numRows System.Int32
---@field public numCols System.Int32
---@field public cellSize System.Single
---@field public numNeighbours Coolape.CLAStarPathSearch.NumNeighbours
---@field public scanType Coolape.CLAStarPathSearch.ScanType
---@field public obstructMask UnityEngine.LayerMask
---@field public passableMask UnityEngine.LayerMask
---@field public rayDis4Scan System.Single
---@field public rayDirection Coolape.CLAStarPathSearch.RayDirection
---@field public isAutoScan System.Boolean
---@field public isFilterPathByRay System.Boolean
---@field public isSoftenPath System.Boolean
---@field public softenPathType Coolape.CLAIPathUtl.SoftenPathType
---@field public softenFactor System.Int32
---@field public grid Coolape.GridBase
---@field public nodesMap System.Collections.Generic.Dictionary2System.Int32Coolape.CLAStarNode
---@field public showGrid System.Boolean
---@field public isIninted System.Boolean

local m = { }
---public CLAStarPathSearch .ctor()
---@return CLAStarPathSearch
function m.New() end
---public Void Start()
function m:Start() end
---public Void init()
function m:init() end
---public Void scan()
function m:scan() end
---public Void scanRange(Vector3 center, Int32 r)
---@param optional Vector3 center
---@param optional Int32 r
function m:scanRange(center, r) end
---public Boolean searchPath(Vector3 from, Vector3 to, List`1& vectorList)
---@return bool
---@param optional Vector3 from
---@param optional Vector3 to
---@param optional List`1& vectorList
function m:searchPath(from, to, vectorList) end
---public Void filterPath(List`1& list)
---@param optional List`1& list
function m:filterPath(list) end
---public Single distance(CLAStarNode node1, CLAStarNode node2)
---@return number
---@param optional CLAStarNode node1
---@param optional CLAStarNode node2
function m:distance(node1, node2) end
Coolape.CLAStarPathSearch = m
return m
