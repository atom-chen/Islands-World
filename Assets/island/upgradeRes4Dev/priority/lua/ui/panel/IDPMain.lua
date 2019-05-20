-- 主界面
IDPMain = {}

local csSelf = nil
local transform = nil
local uiobjs = {}

-- 初始化，只会调用一次
function IDPMain.init(csObj)
    csSelf = csObj
    transform = csObj.transform
    --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
    uiobjs.cityRoot = getChild(transform, "cityRoot")
    uiobjs.worldRoot = getChild(transform, "worldRoot")
    uiobjs.publicRoot = getChild(transform, "public")
    local TableRes = getChild(uiobjs.publicRoot, "AnchorTopLeft/TableRes")
    uiobjs.public = {}
    uiobjs.public.ProgressBarFood = getCC(TableRes, "ProgressBarFood", "UISlider")
    uiobjs.public.LabelFood = getCC(uiobjs.public.ProgressBarFood.transform, "Label", "UILabel")
    uiobjs.public.spriteFood = getCC(uiobjs.public.ProgressBarFood.transform, "SpriteIcon", "UISprite")
    uiobjs.public.ProgressBarGold = getCC(TableRes, "ProgressBarGold", "UISlider")
    uiobjs.public.LabelGold = getCC(uiobjs.public.ProgressBarGold.transform, "Label", "UILabel")
    uiobjs.public.spriteGold = getCC(uiobjs.public.ProgressBarGold.transform, "SpriteIcon", "UISprite")
    uiobjs.public.ProgressBarOil = getCC(TableRes, "ProgressBarOil", "UISlider")
    uiobjs.public.LabelOil = getCC(uiobjs.public.ProgressBarOil.transform, "Label", "UILabel")
    uiobjs.public.spriteOil = getCC(uiobjs.public.ProgressBarOil.transform, "SpriteIcon", "UISprite")
    uiobjs.public.LabelDiam = getCC(TableRes, "ProgressBarDiam/Label", "UILabel")
    uiobjs.public.spriteDiam = getCC(TableRes, "ProgressBarDiam/SpriteIcon", "UISprite")
end

-- 设置数据
function IDPMain.setData(paras)
end

-- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
function IDPMain.show()
    CLUIDrag4World.setCanClickPanel(csSelf.name)
end

-- 刷新
function IDPMain.refresh()
    IDPMain.refreshRes()
end

---@public 刷新资源
function IDPMain.refreshRes()
    uiobjs.public.LabelDiam.text = tostring(bio2number(IDDBPlayer.myself.diam))
    local res = IDDBCity.curCity:getRes()
    uiobjs.public.LabelFood.text = tostring(res.food)
    uiobjs.public.LabelGold.text = tostring(res.gold)
    uiobjs.public.LabelOil.text = tostring(res.oil)
    uiobjs.public.ProgressBarFood.value = res.food / res.maxfood
    uiobjs.public.ProgressBarGold.value = res.gold / res.maxgold
    uiobjs.public.ProgressBarOil.value = res.oil / res.maxoil
end

-- 关闭页面
function IDPMain.hide()
    CLUIDrag4World.removeCanClickPanel(csSelf.name)
end

---@public 当游戏模式变化（主要是从世界到主城的切换时ui的变化）
function IDPMain.onChgMode()
    if IDWorldMap.mode == GameModeSub.city then
        SetActive(uiobjs.worldRoot.gameObject, false)
        IDPMain.refreshCityInfor()
    elseif IDWorldMap.mode == GameModeSub.map or IDWorldMap.mode == GameModeSub.mapBtwncity then
        SetActive(uiobjs.cityRoot.gameObject, false)
        IDPMain.refreshWorldInfor()
    end
end

---@public 刷新主城信息
function IDPMain.refreshCityInfor()
    SetActive(uiobjs.cityRoot.gameObject, true)
end

---@public 刷新世界信息
function IDPMain.refreshWorldInfor()
    SetActive(uiobjs.worldRoot.gameObject, true)
end

-- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
function IDPMain.procNetwork(cmd, succ, msg, paras)
    if (succ == NetSuccess) then
        if cmd == NetProtoIsland.cmds.newBuilding then
            hideHotWheel()
            IDMainCity.onfinsihCreateBuilding(paras.building)
        elseif cmd == NetProtoIsland.cmds.onPlayerChg then
            uiobjs.LabelDiam.text = tostring(bio2number(IDDBPlayer.myself.diam))
        elseif cmd == NetProtoIsland.cmds.onBuildingChg then
            IDMainCity.onBuildingChg(paras.building)
            local attr = DBCfg.getBuildingByID(bio2number(paras.building.attrid))
            if
                bio2number(attr.ID) == IDConst.headquartersBuildingID or
                    (attr and bio2number(attr.GID) == IDConst.BuildingGID.resource)
             then
                -- 刷新数据
                IDPMain.refreshRes()
            end
        elseif cmd == NetProtoIsland.cmds.newTile then
            hideHotWheel()
            IDMainCity.addTile(paras.tile)
        elseif cmd == NetProtoIsland.cmds.rmTile then
            hideHotWheel()
            IDMainCity.doRemoveTile(bio2number(paras.idx))
        elseif cmd == NetProtoIsland.cmds.rmBuilding then
            hideHotWheel()
            IDMainCity.doRemoveBuilding(bio2number(paras.idx))
        elseif cmd == NetProtoIsland.cmds.onFinishBuildingUpgrade then
            IDMainCity.onFinishBuildingUpgrade(paras.building)
        elseif cmd == NetProtoIsland.cmds.collectRes then
            hideHotWheel()
            if bio2number(paras.resVal) > 0 then
                local msg = joinStr(IDUtl.getResNameByType(bio2number(paras.resType)), " +", bio2number(paras.resVal))
                CLAlert.add(msg, Color.green, 1)
            end
            ---@type IDDBBuilding
            local b = paras.building
            IDMainCity.onFinishCollectRes(b)
        elseif cmd == NetProtoIsland.cmds.upLevBuildingImm then
            hideHotWheel()
        end
    end
end

-- 处理ui上的事件，例如点击等
function IDPMain.uiEventDelegate(go)
    local goName = go.name
    if goName == "ButtonBuildings" then
        getPanelAsy("PanelBuildings", onLoadedPanelTT)
    elseif goName == "ButtonSetting" then
        getPanelAsy("PanelSetting", onLoadedPanelTT)
    end
end

-- 当按了返回键时，关闭自己（返值为true时关闭）
function IDPMain.hideSelfOnKeyBack()
    return false
end

---@public 取得资源图标对像
function IDPMain.getResIconObj(resType)
    if resType == IDConst.ResType.food then
        return uiobjs.public.spriteFood
    elseif resType == IDConst.ResType.gold then
        return uiobjs.public.spriteGold
    elseif resType == IDConst.ResType.oil then
        return uiobjs.public.spriteOil
    elseif resType == IDConst.ResType.diam then
        return uiobjs.public.spriteDiam
    end
end

--------------------------------------------
return IDPMain
