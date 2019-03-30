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
    ---@param data={target=目标,offset=偏移, buttonList＝{{icon=图标,bg=背景图, nameKey=显示名称,callback=点击回调}}}
    function _cell.show ( go, data )
        mData = data
        attr = data.target.attr
        serverData = data.target.serverData

        if serverData and bio2number(attr.GID) ~= IDConst.BuildingGID.tree and bio2number(attr.GID) ~= IDConst.BuildingGID.decorate then
            uiobjs.Label.text = joinStr(LGet(attr.NameKey), " ", string.format(LGet("LevelWithNum"), bio2number(serverData.lev)))
        else
            if attr then
                uiobjs.Label.text = LGet(attr.NameKey)
            else
                uiobjs.Label.text = ""
            end
        end
        _cell.prepareData()
        uiobjs.followTarget:setTarget(mData.target.transform, (mData.offset or Vector3.zero))
        _cell.showButtons()

        uiobjs.SpriteCircle.transform.localScale = Vector3.one * ((mData.radius and mData.radius or default_radius) / default_radius)
        uiobjs.SpriteCircle:ResetToBeginning();
        uiobjs.SpriteCircle:Play(true);
    end

    function _cell.prepareData()
        mData.buttonList = {}
        local building = mData.target
        local isTile = building.isTile
        if IDMainCity.newBuildUnit == building then
            -- 说明是新建
            -- 取消
            table.insert(mData.buttonList, { nameKey = "Cancel", callback = IDMainCity.cancelCreateBuilding, icon = "public_guest_bt_delete", bg = "public_edit_circle_bt_shipshop_n" })
            -- 建造
            table.insert(mData.buttonList, { nameKey = "Okay", callback = IDMainCity.doCreateBuilding, icon = "public_guest_checkbox_check", bg = "public_edit_circle_bt_management" })
        else
            if isTile then
                -- 扩建
                table.insert(mData.buttonList, { nameKey = "Extend", callback = IDMainCity.showExtendTile, icon = "icon_build", bg = "public_edit_circle_bt_management" })
                -- 移除
                table.insert(mData.buttonList, { nameKey = "Remove", callback = _cell.removeTile, icon = "public_guest_bt_delete", bg = "public_edit_circle_bt_shipshop_n" })
            else
                local attr = building.attr
                local attrid = bio2number(attr.ID)
                local attrgid = bio2number(attr.GID)
                -- 详情
                table.insert(mData.buttonList, { nameKey = "Detail", callback = _cell.showBuildingDetail, icon = "icon_detail", bg = "public_edit_circle_bt_management" })

                -- 升级加速
                if attrid ~= IDConst.activityCenterBuildingID
                        and attrid ~= IDConst.MailBoxBuildingID
                        and attrgid ~= IDConst.BuildingGID.tree then
                    -- 活动中心、邮箱 不需要升级
                    if bio2number(serverData.state) == IDConst.BuildingState.normal then
                        if bio2number(serverData.lev) < bio2number(attr.MaxLev) then
                            -- 升级
                            table.insert(mData.buttonList, { nameKey = "Upgrade", callback = _cell.showBuildingUpgrade, icon = "icon_build", bg = "public_edit_circle_bt_management" })
                        end
                    elseif bio2number(serverData.state) == IDConst.BuildingState.upgrade then
                        -- 立即
                        table.insert(mData.buttonList, { nameKey = "SpeedUp", callback = _cell.speedUpBuild, icon = "icon_arrow", bg = "public_edit_circle_bt_management" })
                    elseif bio2number(serverData.state) == IDConst.BuildingState.renew then
                        -- 修复
                        table.insert(mData.buttonList, { nameKey = "Renew", callback = nil, icon = "icon_build", bg = "public_edit_circle_bt_management" })
                    end
                end

                if building == IDMainCity.Headquarters then
                    -- 说明是主基地
                    --todo:
                end
                if attrid == 6 or attrid == 8 or attrid == 10 then
                    -- 收集
                    --table.insert(mData.buttonList, { nameKey = "Renew", callback = nil, icon = "icon_build", bg = "public_edit_circle_bt_management" })
                end
                if attrid == 2 then
                    -- 造船厂
                    if bio2number(serverData.lev) > 0 then
                        table.insert(mData.buttonList, { nameKey = "BuildShip", callback = _cell.buildShip, icon = "icon_ship", bg = "public_edit_circle_bt_management" })
                    end
                end

                if attrgid == IDConst.BuildingGID.tree or MyCfg.self.isEditScene or __EditorMode__ then
                    -- 移除
                    table.insert(mData.buttonList, { nameKey = "Remove", callback = _cell.removeBuilding, icon = "public_guest_bt_delete", bg = "public_edit_circle_bt_shipshop_n" })
                end
            end
        end
    end

    function _cell.showBuildingDetail(data)
        getPanelAsy("PanelBuildingInfor", onLoadedPanelTT, data)
    end

    function _cell.showBuildingUpgrade(data)
        getPanelAsy("PanelBuildingUpgrade", onLoadedPanelTT, data)
    end

    function _cell.removeTile(data)
        local idx = bio2number(data.target.mData.idx)
        showHotWheel()
        net:send(NetProtoIsland.send.rmTile(idx))
    end

    function _cell.removeBuilding(data)
        local idx = bio2number(serverData.idx)
        showHotWheel()
        net:send(NetProtoIsland.send.rmBuilding(idx))
    end

    -- 加速
    function _cell.speedUpBuild(data)
        local diam = 0
        local state = bio2number(serverData.state)
        if state == IDConst.BuildingState.upgrade then
            -- 正在升级
            local leftMinutes = (bio2number(serverData.endtime) - DateEx.nowMS) / 60000
            leftMinutes = math.ceil(leftMinutes)
            diam = IDUtl.minutes2Diam(leftMinutes)
        end

        CLUIUtl.showConfirm(
                string.format(LGet("MsgUseDiamSpeedUp"), diam),
                function()
                    showHotWheel()
                    net:send(NetProtoIsland.send.upLevBuildingImm(bio2number(serverData.idx)))
                end,
                nil
        )
    end

    -- 造船
    function _cell.buildShip(data)
        getPanelAsy("PanelBuildShip", onLoadedPanelTT, data)
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
        Utl.doCallback(d.callback, mData)

        local building = mData.target
        local isTile = building.isTile

        if IDMainCity.newBuildUnit == nil and (not isTile) then
            IDMainCity.onClickOcean()
        end
    end

    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.refresh( paras )
        --[[
        if(paras == 1) then   -- 刷新血
          -- TODO:
        elseif(paras == 2) then -- 刷新状态
          -- TODO:
        end
        --]]
    end

    -- 取得数据
    function _cell.getData ( )
        return mData
    end

    --------------------------------------------
    return _cell
end
