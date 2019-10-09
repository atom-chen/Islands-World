-- xx单元
local _cell = {}
local csSelf = nil
local transform = nil
---@type WrapBattleUnitData
local mData = nil
local uiobjs = {}

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    --[[
    上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
	--]]
    uiobjs.toggle = csSelf:GetComponent("UIToggle")
    uiobjs.SpriteIcon = getCC(transform, "SpriteIcon", "UISprite")
    uiobjs.LabelName = getCC(transform, "LabelName", "UILabel")
    uiobjs.LabelNum = getCC(transform, "LabelNum", "UILabel")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
	mData = data
    uiobjs.SpriteIcon.spriteName = mData.icon
    uiobjs.LabelName.text = mData.name
    uiobjs.LabelNum.text = joinStr(bio2number(mData.num), "")
end

-- 注意，c#侧不会在调用show时，调用refresh
function _cell.refresh(paras)
    --[[
    if(paras == 1) then   -- 刷新血
      -- TODO:
    elseif(paras == 2) then -- 刷新状态
      -- TODO:
    end
    --]]
end

-- 取得数据
function _cell.getData()
    return mData
end

--------------------------------------------
return _cell
