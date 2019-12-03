-- 通用ui框1
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    ---@type _ParamFrameData
    local mData = nil;
    local uiobjs = {}

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        uiobjs.BtnClose = getChild(transform, "SpriteClose").gameObject
        uiobjs.LabelTitle = getCC(transform, "LabelTitle", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data;
        if mData.hideClose then
            SetActive(uiobjs.BtnClose, false)
        else
            SetActive(uiobjs.BtnClose, true)
        end
        if mData.hideTitle then
            SetActive(uiobjs.LabelTitle.gameObject, false)
        else
            uiobjs.LabelTitle.text = mData.title
            SetActive(uiobjs.LabelTitle.gameObject, true)
        end
    end

    function _cell.uiEventDelegate(go)
        local goName = go.name
        if goName == "SpriteClose" then
            if mData.closeCallback then
                Utl.doCallback(mData.closeCallback)
            else
                hideTopPanel(mData.panel);
            end
        end
    end

    -- 取得数据
    function _cell.getData ()
        return mData;
    end

    --------------------------------------------
    return _cell;
end
