---@class UIGrid : UIWidgetContainer
---@field public arrangement UIGrid.Arrangement
---@field public sorting UIGrid.Sorting
---@field public pivot UIWidget.Pivot
---@field public maxPerLine System.Int32
---@field public cellWidth System.Single
---@field public cellHeight System.Single
---@field public animateSmoothly System.Boolean
---@field public hideInactive System.Boolean
---@field public keepWithinPanel System.Boolean
---@field public onReposition UIGrid.OnReposition
---@field public onCustomSort System.Comparison1UnityEngine.Transform
---@field public sorted System.Boolean
---@field public oldParentPos UnityEngine.Vector3
---@field public oldParentClipOffset UnityEngine.Vector2
---@field public repositionNow System.Boolean

local m = { }
---public UIGrid .ctor()
---@return UIGrid
function m.New() end
---public List`1 GetChildList()
---@return List`1
function m:GetChildList() end
---public Transform GetChild(Int32 index)
---@return Transform
---@param optional Int32 index
function m:GetChild(index) end
---public Int32 GetIndex(Transform trans)
---@return number
---@param optional Transform trans
function m:GetIndex(trans) end
---public Void AddChild(Transform trans)
---public Void AddChild(Transform trans, Boolean sort)
---@param Transform trans
---@param optional Boolean sort
function m:AddChild(trans, sort) end
---public Boolean RemoveChild(Transform t)
---@return bool
---@param optional Transform t
function m:RemoveChild(t) end
---public Void Start()
function m:Start() end
---public Int32 SortByName(Transform a, Transform b)
---@return number
---@param optional Transform a
---@param optional Transform b
function m.SortByName(a, b) end
---public Int32 SortHorizontal(Transform a, Transform b)
---@return number
---@param optional Transform a
---@param optional Transform b
function m.SortHorizontal(a, b) end
---public Int32 SortVertical(Transform a, Transform b)
---@return number
---@param optional Transform a
---@param optional Transform b
function m.SortVertical(a, b) end
---public Void Reposition()
function m:Reposition() end
---public Void ConstrainWithinPanel()
function m:ConstrainWithinPanel() end
---public Void Awake()
function m:Awake() end
---public Void changeOldClip(Vector3 pos, Vector2 clipOffset)
---@param optional Vector3 pos
---@param optional Vector2 clipOffset
function m:changeOldClip(pos, clipOffset) end
---public Void resetPosition()
function m:resetPosition() end
---public Int32 Count()
---@return number
function m:Count() end
UIGrid = m
return m
