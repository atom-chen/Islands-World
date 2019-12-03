---@public 数据number通用
---@class _Param4SliderNumber
---@field public min number 最小值
---@field public max number 最大值
---@field public default number 默认值
---@field public onValChg function 当数值有变化时的回调
---@field public isSlenderMode boolean 是否是细长版的
---@field public transScale number 缩放值

local _cell = {}
---@type Coolape.CLCellLua
local csSelf = nil
---@type UnityEngine.Transform
local transform = nil
---@type _Param4SliderNumber
local mData = nil
local uiobjs = {}
local mode = 0

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    ---@type UISprite
    uiobjs.bg = csSelf:GetComponent("UISprite")
    ---@type UISlider
    uiobjs.Slider = getCC(transform, "Slider", "UISlider")
    uiobjs.InputNum = getCC(transform, "InputNum", "UIInput")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
    mData = data
    mode = 0
    mData.min = mData.min or 0
    mData.default = mData.default or mData.min
    local persent = (mData.default - mData.min) / (mData.max - mData.min)
    uiobjs.Slider.value = persent
    uiobjs.InputNum.value = tostring(mData.default)
    if mData.isSlenderMode then
        uiobjs.bg.height = 220
        uiobjs.bg.width = 710
        uiobjs.InputNum.transform.localPosition = Vector3(572, 0, 0)
        uiobjs.Slider.transform.localPosition = Vector3(0, 0, 0)
    else
        uiobjs.bg.height = 220
        uiobjs.bg.width = 800
        uiobjs.InputNum.transform.localPosition = Vector3(0, -45, 0)
        uiobjs.Slider.transform.localPosition = Vector3(0, 40, 0)
    end
    
    transform.localScale = Vector3.one * (mData.transScale or 1)
end

-- 取得数据
function _cell.getData()
    return mData
end

function _cell.onNotifyLua(go, paras)
    local goName = go.name
    if goName == "Slider" or goName == "Thumb" then
        mode = 1
    elseif goName == "InputNum" then
        mode = 2
        _cell.setSliderValue()
    end
end

function _cell.uiEventDelegate(go)
    local goName = go.name
    if goName == "ButtonAdd" then
        mode = 0
        _cell.offsetValue(1)
    elseif goName == "ButtonCut" then
        mode = 0
        _cell.offsetValue(-1)
    elseif goName == "Slider" then
        if mode == 1 then
            _cell.setInputValue()
        end
        if mData.onValChg then
            -- mData.onValChg(_cell.getValue())
            Utl.doCallback(mData.onValChg, _cell.getValue())
        end
    elseif goName == "InputNum" then
        if mode == 2 then
            _cell.setSliderValue()
        end
    end
end

function _cell.offsetValue(offset)
    local val = _cell.getValue()
    val = val + offset
    if val > mData.max then
        val = mData.max
    end
    if val < mData.min then
        val = mData.min
    end
    _cell.setInputValue(val)
    _cell.setSliderValue()
end

function _cell.setSliderValue(val)
    if val then
        uiobjs.Slider.value = val
    else
        local curVal = _cell.getValue()
        local persent = (curVal - mData.min) / (mData.max - mData.min)
        uiobjs.Slider.value = persent
    end
end

function _cell.setInputValue(val)
    if val then
        uiobjs.InputNum.value = tostring(val)
    else
        local persent = uiobjs.Slider.value
        uiobjs.InputNum.value = tostring(mData.min + NumEx.getIntPart(persent * (mData.max - mData.min)))
    end
end

function _cell.getValue()
    if isNilOrEmpty(uiobjs.InputNum.value) then
        return mData.min
    end
    return tonumber(uiobjs.InputNum.value)
end
--------------------------------------------
return _cell
