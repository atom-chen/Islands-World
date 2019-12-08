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
    local serverData
    local attr

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.table = csSelf:GetComponent("UITable")
        ---@type UIFollowTarget
        uiobjs.followTarget = csSelf:GetComponent("UIFollowTarget")
        uiobjs.followTarget:setCamera(MyCfg.self.mainCamera, MyCfg.self.uiCamera)
        uiobjs.Background = getCC(transform, "Background", "UISprite")
        uiobjs.Label = getCC(transform, "Label", "UILabel")
        uiobjs.spriteIcon = getCC(transform, "SpriteIcon", "UISprite")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data;
        ---@type NetProtoIsland.ST_building
        serverData = mData.data
        local attrid = bio2number(serverData.attrid)
        attr = DBCfg.getBuildingByID(attrid)
        mData.starttime = bio2number(serverData.starttime)
        mData.endtime = bio2number(serverData.endtime)
        mData.diff = mData.endtime - mData.starttime
        uiobjs.followTarget:setTarget(mData.target.transform, mData.offset or Vector3.zero)

        if mData.bgColor then
            uiobjs.Background.color = mData.bgColor
        end

        if isNilOrEmpty(mData.label) then
            SetActive(uiobjs.Label.gameObject, false)
        else
            SetActive(uiobjs.Label.gameObject, true)
            uiobjs.Label.text = mData.label
        end

        if isNilOrEmpty(mData.icon) then
            SetActive(uiobjs.spriteIcon.gameObject, false)
        else
            CLUIUtl.setSpriteFit(uiobjs.spriteIcon, mData.icon, 60)
            SetActive(uiobjs.spriteIcon.gameObject, true)
        end
    end

    function _cell.OnClick(go)
        if mData.onClick then
            mData.onClick(mData.target.luaTable)
        end
    end

    -- 取得数据
    function _cell.getData ()
        return mData;
    end

    --------------------------------------------
    return _cell;
end
