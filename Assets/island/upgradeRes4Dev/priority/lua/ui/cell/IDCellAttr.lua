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
        uiobjs.SpriteIcon = getCC(transform, "SpriteIcon", "UISprite")
        uiobjs.Label = getCC(transform, "Label", "UILabel")
        uiobjs.LabelVal = getCC(transform, "LabelVal", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show ( go, data )
        mData = data;
        uiobjs.Label.text = mData.name
        if mData.addValue then
            if mData.addValue > 0 then
                uiobjs.LabelVal.text = joinStr(mData.value, "[00ff00]", "+", mData.addValue, "[-]")
            elseif mData.addValue < 0 then
                uiobjs.LabelVal.text = joinStr(mData.value, "[00ff00]", mData.addValue, "[-]")
            else
                uiobjs.LabelVal.text = joinStr(mData.value, "")
            end
        else
            uiobjs.LabelVal.text = joinStr(mData.value, "")
        end

        if isNilOrEmpty(mData.icon) then
            SetActive(uiobjs.SpriteIcon.gameObject, false)
        else
            CLUIUtl.setSpriteFit(uiobjs.SpriteIcon, mData.icon, 60)
            SetActive(uiobjs.SpriteIcon.gameObject, true)
        end
    end

    -- 取得数据
    function _cell.getData ( )
        return mData;
    end

    --------------------------------------------
    return _cell;
end
