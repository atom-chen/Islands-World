---@class IDCellWorldTileHudParam 大地图块hud参数
---@field public target UnityEngine.Transform
---@field public offset UnityEngine.Vector3
---@field public data NetProtoIsland.ST_mapCell
---@field public attr DBCFMapTileData

-- 大地图块hud
local _cell = {}
---@type Coolape.CLCellLua
local csSelf = nil
local transform = nil
---@type IDCellWorldTileHudParam
local mData = nil
local uiobjs = {}

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    --[[
    上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
	--]]
    ---@type UIFollowTarget
    uiobjs.followTarget = csSelf:GetComponent("UIFollowTarget")
    uiobjs.followTarget:setCamera(MyCfg.self.mainCamera, MyCfg.self.uiCamera)
	uiobjs.LabelName = getCC(transform, "LabelName", "UILabel")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
---@param data IDCellWorldTileHudParam
function _cell.show(go, data)
	mData = data
	uiobjs.followTarget:setTarget(mData.target, mData.offset or Vector3.zero)
	local gid = bio2Int(mData.attr.GID)
	if gid == IDConst.WorldmapCellType.user then
		uiobjs.LabelName.text = mData.data.name
	else
		uiobjs.LabelName.text = LGet(mData.attr.Name)
	end
end

-- 取得数据
function _cell.getData()
    return mData
end

--------------------------------------------
return _cell
