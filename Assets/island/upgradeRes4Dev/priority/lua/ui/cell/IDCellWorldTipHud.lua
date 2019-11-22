-- xx单元
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local mData = nil; --[[
    mData.target : gameobject
    mData.data:数据
    mData.offset:位置偏移
    --]]
    local uiobjs = {}

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        ---@type UIFollowTarget
        uiobjs.followTarget = csSelf:GetComponent("UIFollowTarget")
        uiobjs.followTarget:setCamera(MyCfg.self.mainCamera, MyCfg.self.uiCamera)
        uiobjs.spriteIcon = getCC(transform, "SpriteIcon", "UISprite")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        ---@type IDCellWorldTileHudParam
        mData = data
        uiobjs.followTarget:setTarget(mData.target, mData.offset or Vector3.zero)
    end


    -- 取得数据
    function _cell.getData ()
        return mData;
    end

    --------------------------------------------
    return _cell;
end
