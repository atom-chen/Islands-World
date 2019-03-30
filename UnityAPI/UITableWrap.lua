---@class UITable : UIWidgetContainer
---@field public columns System.Int32
---@field public direction UITable.Direction
---@field public sorting UITable.Sorting
---@field public pivot UIWidget.Pivot
---@field public cellAlignment UIWidget.Pivot
---@field public hideInactive System.Boolean
---@field public keepWithinPanel System.Boolean
---@field public padding UnityEngine.Vector2
---@field public onReposition UITable.OnReposition
---@field public onCustomSort System.Comparison1UnityEngine.Transform
---@field public repositionNow System.Boolean

local m = { }
---public UITable .ctor()
---@return UITable
function m.New() end
---public List`1 GetChildList()
---@return List`1
function m:GetChildList() end
---public Void Start()
function m:Start() end
---public Void Reposition()
function m:Reposition() end
UITable = m
return m
