-- xx单元
do
    local _cell = {}
    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local btns = {}
    local mData = nil
    local default_radius = 142;
    local attr
    ---@type IDDBBuilding
    local serverData

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj
        transform = csSelf.transform
        uiobjs.gameObject = csSelf.gameObject
        uiobjs.Label = getCC(transform, "Label", "UILabel")
        uiobjs.followTarget = csSelf:GetComponent("UIFollowTarget")
        uiobjs.followTarget:setCamera(MyCfg.self.mainCamera, MyCfg.self.uiCamera)
        uiobjs.SpriteCircle = getChild(transform, "SpriteCircle"):GetComponent("TweenSpriteFill");
        uiobjs.btnsRoot = getChild(transform, "btnsRoot")
        uiobjs.btnPrefab = getCC(uiobjs.btnsRoot, "00000", "CLCellLua")
        table.insert(btns, uiobjs.btnPrefab)
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    ---@param data={target=目标,offset=偏移, buttonList＝{{icon=图标,bg=背景图, nameKey=显示名称,callback=点击回调}}, params=点击的回调参数}
    function _cell.show ( go, data )
        mData = data
        if mData.target and mData.target then
            attr = data.target.attr
            serverData = data.target.serverData

            if mData.target.isBuilding and serverData and bio2number(attr.GID) ~= IDConst.BuildingGID.tree and bio2number(attr.GID) ~= IDConst.BuildingGID.decorate then
                uiobjs.Label.text = joinStr(LGet(attr.NameKey), " ", string.format(LGet("LevelWithNum"), bio2number(serverData.lev)))
            else
                if attr then
                    uiobjs.Label.text = LGet(attr.NameKey)
                else
                    uiobjs.Label.text = ""
                end
            end
        else
            uiobjs.Label.text = ""
        end

        if mData.target then
            uiobjs.followTarget:setTarget(mData.target.transform, (mData.offset or Vector3.zero))
        else
            uiobjs.followTarget:setTargetPosition(mData.targetPosition or Vector3.zero, (mData.offset or Vector3.zero))
        end
        _cell.showButtons()

        uiobjs.SpriteCircle.transform.localScale = Vector3.one * ((mData.radius and mData.radius or default_radius) / default_radius)
        uiobjs.SpriteCircle:ResetToBeginning();
        uiobjs.SpriteCircle:Play(true);
    end

    function _cell.showButtons(r)
        r = r or default_radius;

        local list = mData.buttonList
        for i = 2, #list do
            local go = NGUITools.AddChild(uiobjs.btnsRoot.gameObject, uiobjs.btnPrefab.gameObject)
            table.insert(btns, go:GetComponent("CLCellLua"))
        end

        local d;
        local cellAngle = 0;
        local pos;
        if #(list) > 0 then
            cellAngle = 360 / #(list)
        end

        for i, v in ipairs(btns) do
            if (list[i] ~= nil) then
                d = list[i];
                d.delay = i * 0.05;
                v:init(d, _cell.onClickBtn);
                pos = AngleEx.getCirclePointV2(Vector2.zero, r, 90 - cellAngle * (i - 1));
                v.transform.localPosition = Vector3(pos.x, pos.y, 0);
                NGUITools.SetActive(v.gameObject, true);
            else
                NGUITools.SetActive(v.gameObject, false);
            end
        end
    end

    function _cell.refreshSize(r)
        uiobjs.SpriteCircle.transform.localScale = Vector3.one * (r / default_radius)
        CLLPMapPopMenu.showButtons(r, true)
    end

    function _cell.onClickBtn(cell)
        local d = cell.luaTable.getData()
        Utl.doCallback(d.callback, mData.params, d)

        if mData.target then
            local building = mData.target
            local isTile = building.isTile
            if IDMainCity.newBuildUnit == nil and (not isTile) then
                IDMainCity.onClickOcean()
            end
        end
    end

    -- 取得数据
    function _cell.getData ( )
        return mData
    end

    --------------------------------------------
    return _cell
end
