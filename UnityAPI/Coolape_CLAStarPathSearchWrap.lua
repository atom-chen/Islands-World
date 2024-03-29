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
---@field public needCachePaths System.Boolean
---@field public isFilterPathByRay System.Boolean
---@field public isSoftenPath System.Boolean
---@field public softenPathType Coolape.CLAIPathUtl.SoftenPathType
---@field public softenFactor System.Int32
---@field public grid Coolape.GridBase
---@field public nodesMap System.Collections.Generic.Dictionary2System.Int32Coolape.CLAStarNode
---@field public showGrid System.Boolean
---@field public showObstruct System.Boolean
---@field public isIninted System.Boolean
---@field public originPos UnityEngine.Vector3
---@field public OnGridStateChgCallbacks System.Collections.ArrayList
local m = { }
---public CLAStarPathSearch .ctor()
---@return CLAStarPathSearch
function m.New() end
---public Void Start()
function m:Start() end
---public Void init()
---public Void init(Vector3 origin)
---@param Vector3 origin
function m:init(origin) end
---public Void scan()
function m:scan() end
---public Void scanRange(Vector3 center, Int32 r)
---public Void scanRange(Int32 centerIndex, Int32 r)
---@param optional Int32 centerIndex
---@param optional Int32 r
function m:scanRange(centerIndex, r) end
---public Void addGridStateChgCallback(Object callback)
---@param optional Object callback
function m:addGridStateChgCallback(callback) end
---public Void removeGridStateChgCallback(Object callback)
---@param optional Object callback
function m:removeGridStateChgCallback(callback) end
---public Void searchPathAsyn(Vector3 from, Vector3 to, Object finishSearchCallback)
---@param optional Vector3 from
---@param optional Vector3 to
---@param optional Object finishSearchCallback
function m:searchPathAsyn(from, to, finishSearchCallback) end
---public Boolean getCachePath(Vector3 from, Vector3 to, List`1& vectorList, Boolean& canReach)
---@return bool
---@param optional Vector3 from
---@param optional Vector3 to
---@param optional List`1& vectorList
---@param optional Boolean& canReach
function m:getCachePath(from, to, vectorList, canReach) end
---public Boolean searchPath(Vector3 from, Vector3 to, List`1& vectorList)
---public Boolean searchPath(Vector3 from, Vector3 to, List`1& vectorList, Boolean& isCachePath, Boolean notPocSoftenPath)
---@return bool
---@param Vector3 from
---@param Vector3 to
---@param optional List`1& vectorList
---@param optional Boolean& isCachePath
---@param optional Boolean notPocSoftenPath
function m:searchPath(from, to, vectorList, isCachePath, notPocSoftenPath) end
---public Void softenPath(List`1& vectorList)
---@param optional List`1& vectorList
function m:softenPath(vectorList) end
---public Boolean isObstructNode(Vector3 pos)
---public Boolean isObstructNode(Int32 index)
---@return bool
---@param optional Int32 index
function m:isObstructNode(index) end
---public Void filterPath(List`1& list)
---@param optional List`1& list
function m:filterPath(list) end
---public Single distance(CLAStarNode node1, CLAStarNode node2)
---@return number
---@param optional CLAStarNode node1
---@param optional CLAStarNode node2
function m:distance(node1, node2) end
---public Void Update()
function m:Update() end
Coolape.CLAStarPathSearch = m
return m
