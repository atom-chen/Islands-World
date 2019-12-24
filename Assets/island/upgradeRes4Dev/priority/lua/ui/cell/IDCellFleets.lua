-- xx单元
local _cell = {}
---@type Coolape.CLCellLua
local csSelf = nil
local transform = nil
---@type NetProtoIsland.ST_fleetinfor
local mData = nil
local uiobjs = {}

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    ---@type UIToggle
    uiobjs.toggle = csSelf:GetComponent("UIToggle")
    uiobjs.LabelName = getCC(transform, "LabelName", "UILabel")
    uiobjs.LabelState = getCC(transform, "LabelState", "UILabel")
    uiobjs.LabelPos = getCC(transform, "LabelPos", "UILabel")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
    mData = data
    uiobjs.LabelName.text = mData.name
    uiobjs.LabelState.text = bio2number(mData.status) .. ""
    local pos = bio2number(mData.curpos)
    local x = IDWorldMap.grid.grid:GetRow(pos)
    local y = IDWorldMap.grid.grid:GetColumn(pos)
    uiobjs.LabelPos.text = joinStr("X:", x, " Y:", y)
end

function _cell.setToggle(val)
    uiobjs.toggle:Set(val)
    uiobjs.toggle.value = val
end

-- 取得数据
function _cell.getData()
    return mData
end

--------------------------------------------
return _cell
