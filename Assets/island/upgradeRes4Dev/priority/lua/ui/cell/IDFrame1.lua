-- 通用ui框1
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local mData = nil;
    --[[
        mData.title:标题
        mData.closeCallback:关闭回调
        mData.panel:CLPanelLua
    --]]
    local uiobjs = {}

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.LabelTitle = getCC(transform, "LabelTitle", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data;
        uiobjs.LabelTitle.text = mData.title
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
