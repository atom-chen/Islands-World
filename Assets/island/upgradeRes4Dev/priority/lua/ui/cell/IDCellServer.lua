-- xx单元
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local mData = nil;
    local uiobjs = {}

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.Background = getCC(transform, "Background", "UISprite")
        uiobjs.Label = getCC(transform, "Label", "UILabel")
        uiobjs.LabelStat = getCC(transform, "LabelStat", "UILabel")
        uiobjs.LabelNew = getCC(transform, "LabelNew", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show ( go, data )
        mData = data;
        uiobjs.Label.text = mData.name
        local stateDesc = ""
        local status = mData.status
        if status == 2 then
            stateDesc = joinStr("[00ffff]", LGet("StateCrowded"),"[-]")
        elseif status == 3 then
            stateDesc = LGet("StateMaintain")
            stateDesc = joinStr("[ff0000]", LGet("StateMaintain"),"[-]")
        else
            stateDesc = joinStr("[00ff00]", LGet("StateNomal"),"[-]")
        end
        uiobjs.LabelStat.text = stateDesc
        SetActive(uiobjs.LabelNew.gameObject, mData.isnew and true or false)
    end

    -- 取得数据
    function _cell.getData ( )
        return mData;
    end

    --------------------------------------------
    return _cell;
end
