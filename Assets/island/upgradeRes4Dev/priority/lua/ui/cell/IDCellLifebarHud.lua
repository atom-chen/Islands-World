---@class IDCellLifebarHud 血条

local _cell = {}
local csSelf = nil
local transform = nil
local mData = nil
local uiobjs = {}

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    ---@type HUDText
    uiobjs.hudText = csSelf:GetComponent("HUDText")

    ---@type UIFollowTarget
    uiobjs.followTarget = csSelf:GetComponent("UIFollowTarget")
    uiobjs.followTarget:setCamera(MyCfg.self.mainCamera, MyCfg.self.uiCamera)

    ---@type UISlider
    uiobjs.ProgressBar = getCC(transform, "Progress Bar", "UISlider")
    ---@type UISprite
    uiobjs.Foreground = getCC(uiobjs.ProgressBar.transform, "Foreground", "UISprite")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
    mData = data
    ---@type IDLUnitBase
    local unit = mData.unit
    uiobjs.followTarget:setTarget(unit.transform, mData.offset or Vector3.zero)
    if unit.isOffense then
        uiobjs.Foreground.color = Color.green
    else
        uiobjs.Foreground.color = Color.red
    end

    local persent = bio2number(unit.data.curHP) / bio2number(unit.data.HP)
    uiobjs.ProgressBar.value = persent
    local color
    if mData.damage > 0 then
        -- 减血
        color = Color.red
    else
        -- 加血
        color = Color.green
	end
    uiobjs.hudText:Add(-1*mData.damage, color, 0, 1)
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
