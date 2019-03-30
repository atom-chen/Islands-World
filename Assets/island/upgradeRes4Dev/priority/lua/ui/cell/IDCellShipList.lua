-- xx单元
do
    local _cell = {}
    local csSelf = nil
    local transform = nil
    local mData = nil
    local uiobjs = {}
    local attr

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj
        transform = csSelf.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.offset = getChild(transform, "offset")
        uiobjs.Icon = getCC(uiobjs.offset, "Icon", "UISprite")
        uiobjs.Label = getCC(uiobjs.offset, "Label", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data
        attr = mData.attr
        uiobjs.Icon.spriteName = joinStr("roleIcon_", bio2number(attr.ID))
        uiobjs.Label.text = tostring(mData.count)
        CLUIUtl.setAllSpriteGray(csSelf.gameObject, mData.isLocked)
    end

    function _cell.setSelected(val)
        local pos = uiobjs.offset.localPosition
        if val then
            pos.y = 20
            uiobjs.offset.localPosition = pos
        else
            pos.y = 0
            uiobjs.offset.localPosition = pos
        end
    end

    -- 取得数据
    function _cell.getData ()
        return mData
    end

    --------------------------------------------
    return _cell
end
