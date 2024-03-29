---@class Coolape.GridBase
---@field public Width System.Single
---@field public Height System.Single
---@field public Origin UnityEngine.Vector3
---@field public NumberOfCells System.Int32
---@field public Left System.Single
---@field public Right System.Single
---@field public Top System.Single
---@field public Bottom System.Single
---@field public CellSize System.Single
local m = { }
---public GridBase .ctor()
---@return GridBase
function m.New() end
---public Void init(Vector3 origin, Int32 numRows, Int32 numCols, Single cellSize)
---@param optional Vector3 origin
---@param optional Int32 numRows
---@param optional Int32 numCols
---@param optional Single cellSize
function m:init(origin, numRows, numCols, cellSize) end
---public Void DebugDraw(Vector3 origin, Int32 numRows, Int32 numCols, Single cellSize, Color color)
---@param optional Vector3 origin
---@param optional Int32 numRows
---@param optional Int32 numCols
---@param optional Single cellSize
---@param optional Color color
function m.DebugDraw(origin, numRows, numCols, cellSize, color) end
---public Vector3 GetNearestCellCenter(Vector3 pos)
---@return Vector3
---@param optional Vector3 pos
function m:GetNearestCellCenter(pos) end
---public Vector3 GetCellCenter(Int32 index)
---public Vector3 GetCellCenter(Int32 col, Int32 row)
---@return Vector3
---@param Int32 col
---@param optional Int32 row
function m:GetCellCenter(col, row) end
---public Vector3 GetCellPosition(Int32 index)
---@return Vector3
---@param optional Int32 index
function m:GetCellPosition(index) end
---public Int32 GetCellIndex(Vector3 pos)
---public Int32 GetCellIndex(Int32 col, Int32 row)
---@return number
---@param Int32 col
---@param optional Int32 row
function m:GetCellIndex(col, row) end
---public Int32 GetCellIndexClamped(Vector3 pos)
---@return number
---@param optional Vector3 pos
function m:GetCellIndexClamped(pos) end
---public Bounds GetCellBounds(Int32 index)
---@return Bounds
---@param optional Int32 index
function m:GetCellBounds(index) end
---public Bounds GetGridBounds()
---@return Bounds
function m:GetGridBounds() end
---public Int32 GetRow(Int32 index)
---@return number
---@param optional Int32 index
function m:GetRow(index) end
---public Int32 GetColumn(Int32 index)
---@return number
---@param optional Int32 index
function m:GetColumn(index) end
---public Int32 GetX(Int32 index)
---@return number
---@param optional Int32 index
function m:GetX(index) end
---public Int32 GetY(Int32 index)
---@return number
---@param optional Int32 index
function m:GetY(index) end
---public Boolean IsInBounds(Int32 index)
---public Boolean IsInBounds(Vector3 pos)
---public Boolean IsInBounds(Int32 col, Int32 row)
---@return bool
---@param Int32 col
---@param optional Int32 row
function m:IsInBounds(col, row) end
---public Int32 LeftIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:LeftIndex(index) end
---public Int32 RightIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:RightIndex(index) end
---public Int32 UpIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:UpIndex(index) end
---public Int32 DownIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:DownIndex(index) end
---public Int32 LeftUpIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:LeftUpIndex(index) end
---public Int32 RightUpIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:RightUpIndex(index) end
---public Int32 LeftDownIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:LeftDownIndex(index) end
---public Int32 RightDownIndex(Int32 index)
---@return number
---@param optional Int32 index
function m:RightDownIndex(index) end
---public List`1 getCircleCells(Vector3 centerPos, Int32 r)
---@return List`1
---@param optional Vector3 centerPos
---@param optional Int32 r
function m:getCircleCells(centerPos, r) end
---public List`1 getAroundCells(Int32 center, Int32 size)
---@return List`1
---@param optional Int32 center
---@param optional Int32 size
function m:getAroundCells(center, size) end
---public List`1 getCells(Int32 center, Int32 size)
---@return List`1
---@param optional Int32 center
---@param optional Int32 size
function m:getCells(center, size) end
Coolape.GridBase = m
return m
