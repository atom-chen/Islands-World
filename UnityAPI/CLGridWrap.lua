---@class CLGrid : UIEventListener
---@field public numRows System.Int32
---@field public numCols System.Int32
---@field public numGroundRows System.Int32
---@field public numGroundCols System.Int32
---@field public cellSize System.Single
---@field public OffsetX System.Single
---@field public OffsetY System.Single
---@field public RowOffsetX System.Single
---@field public OffsetZ System.Single
---@field public gridLineHight System.Single
---@field public showGrid System.Boolean
---@field public showGridRange System.Boolean
---@field public grid Coolape.GridBase
---@field public lineName System.String

local m = { }
---public CLGrid .ctor()
---@return CLGrid
function m.New() end
---public Void Start()
function m:Start() end
---public Void show()
function m:show() end
---public Void onSetPrefab(Object[] paras)
---@param optional Object[] paras
function m:onSetPrefab(paras) end
---public Void showRect()
function m:showRect() end
---public Void drawLine(Vector3 startPos, Vector3 endPos, Color color)
---@param optional Vector3 startPos
---@param optional Vector3 endPos
---@param optional Color color
function m:drawLine(startPos, endPos, color) end
---public Void doDrawLine(Object[] orgs)
---@param optional Object[] orgs
function m:doDrawLine(orgs) end
---public Void reShow()
function m:reShow() end
---public Void clean()
function m:clean() end
---public Void hide()
function m:hide() end
---public Vector3 getGridPos(Vector3 pos)
---@return Vector3
---@param optional Vector3 pos
function m:getGridPos(pos) end
---public Vector3 getPos(Int32 x, Int32 y, Int32 z)
---@return Vector3
---@param optional Int32 x
---@param optional Int32 y
---@param optional Int32 z
function m.getPos(x, y, z) end
---public List`1 getOwnGrids(Int32 center, Int32 size)
---@return List`1
---@param optional Int32 center
---@param optional Int32 size
function m:getOwnGrids(center, size) end
CLGrid = m
return m
