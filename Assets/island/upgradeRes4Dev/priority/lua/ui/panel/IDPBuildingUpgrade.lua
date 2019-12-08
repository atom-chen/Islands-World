-- xx界面
do
    local IDPBuildingUpgrade = {}

    local csSelf = nil;
    local transform = nil;
    local uiobjs = {}
    local mData
    local buildingCamera
    local buildingAttrRoot
    local attr
    ---@type NetProtoIsland.ST_building
    local serverData

    -- 初始化，只会调用一次
    function IDPBuildingUpgrade.init(csObj)
        csSelf = csObj;
        transform = csObj.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.Table = getCC(transform, "Table", "UITable")
        uiobjs.attrRoot = getChild(uiobjs.Table.transform, "1")
        uiobjs.LabelNextOpen = getCC(uiobjs.Table.transform, "2/LabelNextOpen", "UILabel")
        uiobjs.LabelLev = getCC(transform, "LabelLev", "UILabel")
        uiobjs.LabelDesc = getCC(transform, "LabelDesc", "UILabel")
        uiobjs.conditionRoot = getCC(uiobjs.Table.transform, "3/Table", "UITable")
        uiobjs.conditionCellPrefab = getChild(uiobjs.conditionRoot.transform, "00000").gameObject
        uiobjs.BtnTable = getCC(transform, "BtnTable", "UIGrid")
        uiobjs.LabelCostTime = getCC(uiobjs.BtnTable.transform, "ButtonUpgrade/LabelCostTime", "UILabel")
        uiobjs.LabelCostDiamond = getCC(uiobjs.BtnTable.transform, "ButtonUpgradeImm/LabelCostDiamond", "UILabel")
    end

    -- 设置数据
    function IDPBuildingUpgrade.setData(paras)
        mData = paras
        attr = mData.attr
        serverData = mData.serverData
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPBuildingUpgrade.show()
        if buildingCamera == nil then
            CLUIOtherObjPool.borrowObjAsyn("BuildingCamera",
                    function(name, go, orgs)
                        buildingCamera = go:GetComponent("CLCellLua")
                        buildingCamera.transform.parent = transform
                        buildingCamera.transform.localScale = Vector3.one
                        buildingCamera.transform.localPosition = Vector3(-537, 100, 0)
                        SetActive(go, true)
                        buildingCamera:init(mData, nil)
                    end)
        end

        uiobjs.LabelLev.text = joinStr(bio2number(serverData.lev), "➚", bio2number(serverData.lev) + 1)
        uiobjs.LabelDesc.text = LGet(attr.DescKey)
        IDPBuildingUpgrade.showNextLevOpen()
        IDPBuildingUpgrade.showConditions()
        local cooldownTime = DBCfg.getGrowingVal(bio2number(attr.BuildTimeMin) * 60, bio2number(attr.BuildTimeMax) * 60, bio2number(attr.BuildTimeCurve), (bio2number(serverData.lev) + 1) / bio2number(attr.MaxLev))
        uiobjs.LabelCostTime.text = DateEx.ToTimeStr2(cooldownTime * 1000)

        IDPBuildingUpgrade.showAttr(
                function()
                    uiobjs.Table.repositionNow = true
                    --uiobjs.Table:Reposition()
                end)
    end

    -- 当加载好通用框的回调
    function IDPBuildingUpgrade.onShowFrame(cs)
        local title = LGet(attr.NameKey)
        csSelf.frameObj:init({ title = title, panel = csSelf })
    end

    -- 属性处理
    function IDPBuildingUpgrade.showAttr(callback)
        if buildingAttrRoot == nil then
            CLUIOtherObjPool.borrowObjAsyn("AttrUIPoc",
                    function(name, go, orgs)
                        buildingAttrRoot = go:GetComponent("CLCellLua")
                        buildingAttrRoot.transform.parent = uiobjs.attrRoot
                        buildingAttrRoot.transform.localScale = Vector3.one
                        buildingAttrRoot.transform.localPosition = Vector3.zero
                        SetActive(go, true)
                        buildingAttrRoot:init({ attr = attr, serverData = serverData, type = IDConst.AttrType.buildingNextOpen, maxRow = 2 }, nil)
                        if callback then
                            callback()
                        end
                    end)
        else
            buildingAttrRoot:init({ attr = attr, serverData = serverData, type = IDConst.AttrType.buildingNextOpen, maxRow = 2 }, nil)
        end
    end

    function IDPBuildingUpgrade.showNextLevOpen()
        local list = IDUtl.nextOpen(attr, bio2number(serverData.lev), IDConst.UnitType.building)
        if #list > 0 then
            local str = ""
            for i, v in ipairs(list) do
                if v.addVal > 0 then
                    str = joinStr(str, v.name, "+", v.addVal, ",")
                else
                    str = joinStr(str, v.name, v.addVal, ",")
                end
            end
            uiobjs.LabelNextOpen.text = str
        else
            uiobjs.LabelNextOpen.text = LGet("None")
        end
    end

    -- 显示升级条件
    function IDPBuildingUpgrade.showConditions()
        local list = IDUtl.getBuildingUpgradeConditions(bio2number(attr.ID), bio2number(serverData.lev) + 1)
        CLUIUtl.resetList4Lua(uiobjs.conditionRoot, uiobjs.conditionCellPrefab, list, IDPBuildingUpgrade.initCellCondition)
    end

    function IDPBuildingUpgrade.initCellCondition(cell, data)
        cell:init(data, nil)
    end

    -- 刷新
    function IDPBuildingUpgrade.refresh()
        IDPBuildingUpgrade.showUpgradeImmDiam()
    end

    function IDPBuildingUpgrade.showUpgradeImmDiam()
        local leftMinutes = 0
        local needDiam = "0"
        local needRes = IDUtl.getBuildingUpgradeNeedRes(bio2number(attr.ID), bio2number(serverData.lev) + 1)
        local total = 0
        for k,v in pairs(needRes) do
            total = total + v
        end
        needDiam = IDUtl.res2Diam(total)

        local state = bio2number(serverData.state)
        if state == IDConst.BuildingState.upgrade then
            -- 正在升级
            leftMinutes = (bio2number(serverData.endtime) - DateEx.nowMS) / 60000
        elseif state == IDConst.BuildingState.normal then
            -- 空闲状态
            local persent = (bio2number(serverData.lev) + 1) / bio2number(attr.MaxLev)
            leftMinutes = DBCfg.getGrowingVal(bio2number(attr.BuildTimeMin),
                    bio2number(attr.BuildTimeMax),
                    bio2number(attr.BuildTimeCurve), persent)
        end

        if leftMinutes > 0 then
            leftMinutes = math.ceil(leftMinutes)
            needDiam = tostring(needDiam + IDUtl.minutes2Diam(leftMinutes))
        end
        uiobjs.LabelCostDiamond.text = needDiam
    end

    -- 关闭页面
    function IDPBuildingUpgrade.hide()
        if buildingCamera then
            CLUIOtherObjPool.returnObj(buildingCamera.gameObject)
            SetActive(buildingCamera.gameObject, false)
            buildingCamera = nil
        end
        if buildingAttrRoot then
            CLUIOtherObjPool.returnObj(buildingAttrRoot.gameObject)
            SetActive(buildingAttrRoot.gameObject, false)
            buildingAttrRoot = nil
        end
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPBuildingUpgrade.procNetwork (cmd, succ, msg, paras)
        if (succ == NetSuccess) then
            if (cmd == NetProtoIsland.cmds.upLevBuilding) then
                hideHotWheel()
                hideTopPanel(csSelf)
            elseif cmd == NetProtoIsland.cmds.upLevBuildingImm then
                hideHotWheel()
                hideTopPanel(csSelf)
            end
        end
    end

    -- 处理ui上的事件，例如点击等
    function IDPBuildingUpgrade.uiEventDelegate(go)
        local goName = go.name;
        if goName == "ButtonUpgrade" then
            -- 升级
            showHotWheel()
            CLLNet.send(NetProtoIsland.send.upLevBuilding(bio2number(serverData.idx)))
        elseif goName == "ButtonUpgradeImm" then
            -- 立即升级
            showHotWheel()
            CLLNet.send(NetProtoIsland.send.upLevBuildingImm(bio2number(serverData.idx)))
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPBuildingUpgrade.hideSelfOnKeyBack()
        return true
    end

    --------------------------------------------
    return IDPBuildingUpgrade;
end
