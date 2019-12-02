-- 数据number通用
do
    local _cell = {}
    local csSelf = nil
    local transform = nil
    local mData = nil
    local uiobjs = {}
    local mode = 0

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj
        transform = csSelf.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.Slider = getCC(transform, "Slider", "UISlider")
        uiobjs.InputNum = getCC(transform, "InputNum", "UIInput")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data
        --[[
        mData.min:最小值
        mData.max:最大值
        mData.default:默认值
        mData.onValChg:当数值有变化时的回调
        --]]
        mode = 0
        mData.min = mData.min or 0
        mData.default = mData.default or mData.min
        local persent = (mData.default - mData.min) / (mData.max - mData.min)
        uiobjs.Slider.value = persent
        uiobjs.InputNum.value = tostring(mData.default)
    end

    -- 取得数据
    function _cell.getData ()
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
end
