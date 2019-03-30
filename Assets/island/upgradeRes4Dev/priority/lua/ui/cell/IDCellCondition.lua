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
        uiobjs.label = csSelf:GetComponent("UILabel")
        uiobjs.SpriteIcon = getCC(transform, "SpriteIcon", "UISprite")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show ( go, data )
        mData = data;
        if isNilOrEmpty( mData.icon) then
            SetActive(uiobjs.SpriteIcon.gameObject, false)
        else
            CLUIUtl.setSpriteFit(uiobjs.SpriteIcon, mData.icon, 80)
            SetActive(uiobjs.SpriteIcon.gameObject, true)
        end
        uiobjs.label.text = tostring(mData.value)
    end

    -- 取得数据
    function _cell.getData ( )
        return mData;
    end

    --------------------------------------------
    return _cell;
end
