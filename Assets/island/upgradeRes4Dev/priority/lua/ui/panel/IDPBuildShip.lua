-- 造船界面
do
    local IDPBuildShip = {}

    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local mData
    local dockyardAttr
    ---@type IDDBBuilding
    local dockyardServerData
    local selectedCell
    local roleAttr

    -- 初始化，只会调用一次
    function IDPBuildShip.init(csObj)
        csSelf = csObj
        transform = csObj.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.Grid = getCC(transform, "PanelList/Grid", "CLUILoopGrid")
        uiobjs.roleRoot = getCC(transform, "InforLeft/roleRoot", "CLUIRenderQueue")
        uiobjs.LabelName = getCC(transform, "InforLeft/LabelName", "UILabel")
        uiobjs.InforRight = getChild(transform, "InforRight")
        uiobjs.LabelSpace = getCC(uiobjs.InforRight, "LabelSpace", "UILabel")
        uiobjs.SpaceProgressBar = getCC(uiobjs.LabelSpace.transform, "Progress Bar", "UISlider")
        uiobjs.ButtonBuild = getChild(uiobjs.InforRight, "ButtonBuild")
        uiobjs.LabelCostTime = getCC(uiobjs.ButtonBuild, "LabelCostTime", "UILabel")
        uiobjs.AttrRoot = getChild(uiobjs.InforRight, "AttrRoot")
        uiobjs.NumRoot = getChild(uiobjs.InforRight, "NumRoot")
        uiobjs.BuildProgress = getCC(uiobjs.InforRight, "BuildProgress", "UISlider")
        uiobjs.BuildProgressLabel = getCC(uiobjs.BuildProgress.transform, "Label", "UILabel")
        uiobjs.ButtonImm = getChild(uiobjs.InforRight, "ButtonImm")
        uiobjs.LabelDockyardBusy = getChild(uiobjs.InforRight, "LabelDockyardBusy").gameObject
    end

    -- 设置数据
    function IDPBuildShip.setData(paras)
        mData = paras
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPBuildShip.show()
        dockyardAttr = mData.attr
        dockyardServerData = mData.serverData

        SetActive(uiobjs.ButtonBuild.gameObject, false)
        SetActive(uiobjs.ButtonImm.gameObject, false)
        SetActive(uiobjs.BuildProgress.gameObject, false)
        SetActive(uiobjs.LabelDockyardBusy, false)
        local list = IDPBuildShip.wrapShipList()
        IDPBuildShip.showShipList(list, false)
    end

    ---@public 包装舰船列表数据
    function IDPBuildShip.wrapShipList()
        local shipList = DBCfg.getRolesByGID(IDConst.RoleGID.ship)
        local dockyardShips = IDDBCity.curCity:getShipsByDockyardId(bio2number(dockyardServerData.idx))
        --if dockyardShips == nil then
        --    net:send(NetProtoIsland.send.getShipsByBuildingIdx(bio2number(dockyardServerData.idx)))
        --end
        dockyardShips = dockyardShips or {}
        local dockyardLev = bio2number(dockyardServerData.lev)
        local list = {}
        for i, v in ipairs(shipList) do
            local d = {}
            d.attr = v
            d.count = dockyardShips[bio2number(v.ID)] or 0
            d.isLocked = dockyardLev < bio2number(v.ArsenalLev)
            table.insert(list, d)
        end
        return list
    end

    function IDPBuildShip.showShipList(list, refreshContentOnly)
        if refreshContentOnly then
            uiobjs.Grid:refreshContentOnly(list)
        else
            uiobjs.Grid:setList(list, IDPBuildShip.initShipCell)
        end
    end

    function IDPBuildShip.initShipCell(cell, data)
        cell:init(data, IDPBuildShip.onClickShipCell)
        if selectedCell == nil or selectedCell == cell then
            IDPBuildShip.onClickShipCell(cell)
        end
    end

    function IDPBuildShip.onClickShipCell(cell)
        local data = cell.luaTable.getData()
        if data.isLocked then
            return
        end
        if selectedCell then
            selectedCell.luaTable.setSelected(false)
        end
        selectedCell = cell
        if selectedCell then
            selectedCell.luaTable.setSelected(true)
        end

        -- attr
        IDPBuildShip.showAttr()
        -- number
        local dockyardState = bio2number(dockyardServerData.state)
        local buildShipId = bio2number(dockyardServerData.val)
        if dockyardState == IDConst.BuildingState.normal then
            IDPBuildShip.loadSliderNum()
        elseif dockyardState == IDConst.BuildingState.working then
            if buildShipId == bio2number(data.attr.ID) then
                IDPBuildShip.working()
            else
                -- 正在繁忙
                IDPBuildShip.busy()
            end
        else
            -- 正在繁忙
            IDPBuildShip.busy()
        end

        -- 显示选中数据
        IDPBuildShip.showShip()
    end

    function IDPBuildShip.working()
        SetActive(uiobjs.LabelDockyardBusy, false)
        SetActive(uiobjs.ButtonBuild.gameObject, false)
        SetActive(uiobjs.ButtonImm.gameObject, true)
        SetActive(uiobjs.BuildProgress.gameObject, true)
        if uiobjs.sliderNum then
            SetActive(uiobjs.sliderNum.gameObject, false)
        end
        IDPBuildShip.cooldownBuild()
    end

    function IDPBuildShip.cooldownBuild()
        csSelf:cancelInvoke4Lua(IDPBuildShip.cooldownBuild)
        local starttime = bio2number(dockyardServerData.starttime)
        local endtime = bio2number(dockyardServerData.endtime)
        local num = bio2number(dockyardServerData.val2)
        local diff = endtime - starttime
        local diff2 = endtime - DateEx.nowMS
        local persent = diff2 / diff
        uiobjs.BuildProgressLabel.text = joinStr(LGet("Number"), ":", num, " ", LGet("Time"), ":", DateEx.toStrCn(diff2))
        uiobjs.BuildProgress.value = persent
        csSelf:invoke4Lua(IDPBuildShip.cooldownBuild, 0.5)
    end

    function IDPBuildShip.busy()
        csSelf:cancelInvoke4Lua(IDPBuildShip.cooldownBuild)
        SetActive(uiobjs.LabelDockyardBusy, true)
        SetActive(uiobjs.ButtonBuild.gameObject, false)
        SetActive(uiobjs.ButtonImm.gameObject, false)
        SetActive(uiobjs.BuildProgress.gameObject, false)
        if uiobjs.sliderNum then
            SetActive(uiobjs.sliderNum.gameObject, false)
        end
    end

    function IDPBuildShip.showShip()
        local data = selectedCell.luaTable.getData()
        uiobjs.LabelName.text = LGet(data.attr.NameKey)
        local totalSpace = DBCfg.getGrowingVal(bio2number(dockyardAttr.ComVal1Min), bio2number(dockyardAttr.ComVal1Max), bio2number(dockyardAttr.ComVal1Curve), bio2number(dockyardServerData.lev) / bio2number(dockyardAttr.MaxLev))
        local usedSpace = IDDBCity.curCity:getDockyardUsedSpace(bio2number(dockyardServerData.idx))
        uiobjs.LabelSpace.text = joinStr(usedSpace, "/", totalSpace)
        uiobjs.SpaceProgressBar.value = usedSpace / totalSpace

        IDPBuildShip.loadShip(IDUtl.getRolePrefabName(bio2number(data.attr.ID)))
    end

    function IDPBuildShip.loadShip(shipName)
        if uiobjs.shipGo then
            -- 先释放之前加载的
            CLRolePool.returnObj(uiobjs.shipGo)
            SetActive(uiobjs.shipGo.gameObject, false)
            NGUITools.SetLayer(uiobjs.shipGo.gameObject, LayerMask.NameToLayer("Unit"))
            uiobjs.shipGo = nil
        end
        CLRolePool.borrowObjAsyn(shipName, IDPBuildShip.onLoadShip)
    end

    function IDPBuildShip.onLoadShip(name, ship, orgs)
        if not csSelf.gameObject.activeInHierarchy then
            CLRolePool.returnObj(ship)
            SetActive(ship.gameObject, false)
            return
        end
        uiobjs.shipGo = ship
        uiobjs.roleRoot.transform.localEulerAngles = Vector3(0, 130, 0)
        uiobjs.shipGo.transform.parent = uiobjs.roleRoot.transform
        uiobjs.shipGo.transform.localPosition = Vector3.zero
        uiobjs.shipGo.transform.localScale = Vector3.one * 250
        uiobjs.shipGo.transform.localEulerAngles = Vector3.zero
        NGUITools.SetLayer(uiobjs.shipGo.gameObject, LayerMask.NameToLayer("UI"))
        SetActive(uiobjs.shipGo.gameObject, true)
        uiobjs.roleRoot:reset(true)
        --csSelf:invoke4Lua(
        --        function()
        --            uiobjs.roleRoot:setRenderQueue(true)
        --        end, 0.2)
    end

    function IDPBuildShip.showAttr(callback)
        local data = selectedCell.luaTable.getData()
        if roleAttr == nil then
            CLUIOtherObjPool.borrowObjAsyn("AttrUIPoc",
                    function(name, go, orgs)
                        roleAttr = go:GetComponent("CLCellLua")
                        roleAttr.transform.parent = uiobjs.AttrRoot
                        roleAttr.transform.localScale = Vector3.one
                        roleAttr.transform.localPosition = Vector3.zero
                        SetActive(go, true)
                        roleAttr:init({ attr = data.attr, serverData = nil, type = IDConst.AttrType.ship4Build, maxRow = 4 }, nil)
                        if callback then
                            callback()
                        end
                    end)
        else
            roleAttr:init({ attr = data.attr, serverData = nil, type = IDConst.AttrType.ship4Build, maxRow = 4 }, nil)
        end
    end


    -- 当加载好通用框的回调
    function IDPBuildShip.onShowFrame(cs)
        local title = LGet("Arsenal")
        csSelf.frameObj:init({ title = title, panel = csSelf })
    end

    function IDPBuildShip.loadSliderNum()
        csSelf:cancelInvoke4Lua(IDPBuildShip.cooldownBuild)
        SetActive(uiobjs.ButtonBuild.gameObject, true)
        SetActive(uiobjs.ButtonImm.gameObject, false)
        SetActive(uiobjs.BuildProgress.gameObject, false)
        SetActive(uiobjs.LabelDockyardBusy, false)

        local totalSpace = DBCfg.getGrowingVal(bio2number(dockyardAttr.ComVal1Min), bio2number(dockyardAttr.ComVal1Max), bio2number(dockyardAttr.ComVal1Curve), bio2number(dockyardServerData.lev) / bio2number(dockyardAttr.MaxLev))
        local usedSpace = IDDBCity.curCity:getDockyardUsedSpace(bio2number(dockyardServerData.idx))
        local freeSpace = totalSpace - usedSpace
        local cellData = selectedCell.luaTable.getData()
        local maxNum = NumEx.getIntPart(freeSpace / bio2number(cellData.attr.SpaceSize))
        local default = 1
        if maxNum < 1 then
            IDPBuildShip.showSpaceFull()
            return
        end
        local data = { min = 0, max = maxNum, default = default, onValChg = IDPBuildShip.onNumChg }
        if uiobjs.sliderNum == nil then
            CLUIOtherObjPool.borrowObjAsyn("SliderNumber",
                    function(name, go, orgs)
                        uiobjs.sliderNum = go:GetComponent("CLCellLua")
                        uiobjs.sliderNum.transform.parent = uiobjs.NumRoot
                        uiobjs.sliderNum.transform.localScale = Vector3.one
                        uiobjs.sliderNum.transform.localPosition = Vector3.zero
                        SetActive(go, true)
                        uiobjs.sliderNum:init(data, nil)
                    end)
        else
            SetActive(uiobjs.sliderNum.gameObject, true)
            uiobjs.sliderNum:init(data, nil)
        end
    end

    function IDPBuildShip.hideSliderNum()
        if uiobjs.sliderNum then
            CLUIOtherObjPool.returnObj(uiobjs.sliderNum.gameObject)
            SetActive(uiobjs.sliderNum.gameObject, false)
            uiobjs.sliderNum = nil
        end
    end

    function IDPBuildShip.onNumChg(n)
        local totalSpace = DBCfg.getGrowingVal(bio2number(dockyardAttr.ComVal1Min), bio2number(dockyardAttr.ComVal1Max), bio2number(dockyardAttr.ComVal1Curve), bio2number(dockyardServerData.lev) / bio2number(dockyardAttr.MaxLev))
        local usedSpace = IDDBCity.curCity:getDockyardUsedSpace(bio2number(dockyardServerData.idx))
        local cellData = selectedCell.luaTable.getData()
        local shipSpaceSize = bio2number(cellData.attr.SpaceSize)
        local BuildTimeS = bio2number(cellData.attr.BuildTimeS)

        usedSpace = usedSpace + shipSpaceSize * n
        local costTimes = n * (BuildTimeS / 10) * 1000 -- 转成毫秒
        uiobjs.LabelCostTime.text = DateEx.toStrEn(costTimes)

        uiobjs.LabelSpace.text = joinStr(usedSpace, "/", totalSpace)
        uiobjs.SpaceProgressBar.value = usedSpace / totalSpace
    end

    -- 当空间满时显示
    function IDPBuildShip.showSpaceFull()
        if uiobjs.sliderNum then
            SetActive(uiobjs.sliderNum.gameObject, false)
        end
    end

    -- 刷新
    function IDPBuildShip.refresh()
    end

    -- 关闭页面
    function IDPBuildShip.hide()
        csSelf:cancelInvoke4Lua()
        IDPBuildShip.hideSliderNum()
        if roleAttr then
            CLUIOtherObjPool.returnObj(roleAttr.gameObject)
            SetActive(roleAttr.gameObject, false)
            roleAttr = nil
        end

        if uiobjs.shipGo then
            CLRolePool.returnObj(uiobjs.shipGo)
            SetActive(uiobjs.shipGo.gameObject, false)
            NGUITools.SetLayer(uiobjs.shipGo.gameObject, LayerMask.NameToLayer("Unit"))
            uiobjs.shipGo = nil
        end
        uiobjs.roleRoot:reset()

        if selectedCell then
            selectedCell.luaTable.setSelected(false)
            selectedCell = nil
        end
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPBuildShip.procNetwork (cmd, succ, msg, paras)
        if succ == NetSuccess then
            if cmd == NetProtoIsland.cmds.getShipsByBuildingIdx or cmd == NetProtoIsland.cmds.onBuildingChg then
                dockyardServerData = IDDBCity.curCity.buildings[bio2number(dockyardServerData.idx)]
                local list = IDPBuildShip.wrapShipList()
                IDPBuildShip.showShipList(list, true)
            elseif cmd == NetProtoIsland.cmds.buildShip then
                hideHotWheel()
                dockyardServerData = IDDBCity.curCity.buildings[bio2number(dockyardServerData.idx)]
                local list = IDPBuildShip.wrapShipList()
                IDPBuildShip.showShipList(list, true)
            end
        end
    end

    function IDPBuildShip.dragShip(go)
        local delta = UICamera.currentTouch.delta
        local angle = uiobjs.roleRoot.transform.localEulerAngles
        angle.y = angle.y - delta.x
        uiobjs.roleRoot.transform.localEulerAngles = angle
    end

    -- 处理ui上的事件，例如点击等
    function IDPBuildShip.uiEventDelegate(go)
        local goName = go.name
        if (goName == "ButtonBuild") then
            if uiobjs.sliderNum then
                local num = uiobjs.sliderNum.luaTable.getValue()
                if num <= 0 then
                    CLAlert.add(LGet("MsgPleaseInputNumber"), Color.yellow, 1)
                    return
                end
                showHotWheel()
                local cellData = selectedCell.luaTable.getData()
                net:send(NetProtoIsland.send.buildShip(
                        bio2number(dockyardServerData.idx),
                        bio2number(cellData.attr.ID), num))
            end
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPBuildShip.hideSelfOnKeyBack()
        return true
    end

    --------------------------------------------
    return IDPBuildShip
end
