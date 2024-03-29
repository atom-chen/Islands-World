﻿-- 公共处理
require("city.IDLBuilding")
require("city.IDLBuildingHeadquarters")
require("city.IDLBuildingRes")
require("city.IDLBuildingStore")
require("city.IDLBuildingDefense")
require("city.IDLBuildingDefenseMissile2")
require("city.IDLBuildingDefenseLightning")
require("city.IDLBuildingDefenseLightning2")
require("city.IDLBuildingDockyard")
require("city.IDLBuildingTrap")
require("city.IDLBuildingTrapAirBomb")
require("city.IDLBuildingTrapFrozen")
require("city.IDLBuildingTrapIceStorm")
require("city.IDLBuildingTrapMonster")
require("city.IDLBuildingTrapSwirl")
require("city.IDLBuildingAlliance")
require("city.IDLTree")
require("role.IDRoleBase")
require("role.IDRWorker")
require("role.IDRShip")
require("role.IDRSoldier")
require("role.IDRShipLandCraft")
require("role.IDRShipBreaker")
require("worldmap.IDWorldTile")

IDUtl = {}

---@public 下级开放
---@param attr 配制
---@param lev 等级
---@param type 类型 {1:建筑;}
function IDUtl.nextOpen(attr, lev, type)
    local list = {}
    if type == IDConst.UnitType.building then
        -- 建筑
        if bio2number(attr.MaxLev) < lev then
            -- 已经是最高等级了
            return list
        end

        if bio2number(attr.ID) == 1 then
            -- 主基地的下级开放
            local headquartersOpen = DBCfg.getHeadquartersLevsDataByLev(lev)
            local headquartersOpenNext = DBCfg.getHeadquartersLevsDataByLev(lev + 1)
            if headquartersOpenNext then
                -- 地块
                local diff = bio2number(headquartersOpenNext.Tiles) - bio2number(headquartersOpen.Tiles)
                if diff > 0 then
                    table.insert(
                        list,
                        {name = LGet("Tile"), currVal = bio2number(headquartersOpen.Tiles), addVal = diff}
                    )
                end
                -- 工人
                diff = bio2number(headquartersOpenNext.Workers) - bio2number(headquartersOpen.Workers)
                if diff > 0 then
                    table.insert(
                        list,
                        {name = LGet("Worker"), currVal = bio2number(headquartersOpen.Tiles), addVal = diff}
                    )
                end
            end

            -- 各种建筑开放数量
            local key = "Building"
            local keyBuilding
            local diff = 0
            for buildingID = 1, 39 do
                keyBuilding = joinStr(key, buildingID)
                if headquartersOpen[keyBuilding] then
                    diff = bio2number(headquartersOpenNext[keyBuilding]) - bio2number(headquartersOpen[keyBuilding])
                    if diff > 0 then
                        local bAttr = DBCfg.getBuildingByID(buildingID)
                        table.insert(
                            list,
                            {
                                name = LGet(bAttr.NameKey),
                                currVal = bio2number(headquartersOpen[keyBuilding]),
                                addVal = diff
                            }
                        )
                    end
                end
            end
        end
    end

    return list
end

---@public 取得建筑升级条件
---@param id 建筑id
function IDUtl.getBuildingUpgradeConditions(id, lev)
    local attr = DBCfg.getBuildingByID(id)
    if attr == nil then
        return
    end
    local list = {}
    local d
    -- 粮食
    if bio2number(attr.BuildCostFoodMax) > 0 then
        d = {}
        d.icon = "res_food"
        d.value =
            DBCfg.getGrowingVal(
            bio2number(attr.BuildCostFoodMin),
            bio2number(attr.BuildCostFoodMax),
            bio2number(attr.BuildCostFoodCurve),
            lev / bio2number(attr.MaxLev)
        )
        d.other = {}
        table.insert(list, d)
    end
    -- 金币
    if bio2number(attr.BuildCostGoldMax) > 0 then
        d = {}
        d.icon = "res_gold"
        d.value =
            DBCfg.getGrowingVal(
            bio2number(attr.BuildGoldFoodMin),
            bio2number(attr.BuildCostGoldMax),
            bio2number(attr.BuildCostGoldCurve),
            lev / bio2number(attr.MaxLev)
        )
        d.other = {}
        table.insert(list, d)
    end
    -- 油
    if bio2number(attr.BuildCostOilMax) > 0 then
        d = {}
        d.icon = "res_shiyou"
        d.value =
            DBCfg.getGrowingVal(
            bio2number(attr.BuildCostOilMin),
            bio2number(attr.BuildCostOilMax),
            bio2number(attr.BuildCostOilCurve),
            lev / bio2number(attr.MaxLev)
        )
        d.other = {}
        table.insert(list, d)
    end
    -- 其它建筑等级要求
    -- 主基地等级限制
    return list
end

---@public 取得建筑升级所要的资源
function IDUtl.getBuildingUpgradeNeedRes(id, lev)
    local attr = DBCfg.getBuildingByID(id)
    if attr == nil then
        return
    end
    local map = {}
    -- 粮食
    if bio2number(attr.BuildCostFoodMax) > 0 then
        map.food =
            DBCfg.getGrowingVal(
            bio2number(attr.BuildCostFoodMin),
            bio2number(attr.BuildCostFoodMax),
            bio2number(attr.BuildCostFoodCurve),
            lev / bio2number(attr.MaxLev)
        )
    end
    -- 金币
    if bio2number(attr.BuildCostGoldMax) > 0 then
        map.gold =
            DBCfg.getGrowingVal(
            bio2number(attr.BuildGoldFoodMin),
            bio2number(attr.BuildCostGoldMax),
            bio2number(attr.BuildCostGoldCurve),
            lev / bio2number(attr.MaxLev)
        )
    end
    -- 油
    if bio2number(attr.BuildCostOilMax) > 0 then
        map.oil =
            DBCfg.getGrowingVal(
            bio2number(attr.BuildCostOilMin),
            bio2number(attr.BuildCostOilMax),
            bio2number(attr.BuildCostOilCurve),
            lev / bio2number(attr.MaxLev)
        )
    end
    return map
end

---@public 分钟转钻石
function IDUtl.minutes2Diam(val)
    local ret = NumEx.getIntPart(val * bio2number(DBCfg.getConstCfg().Minute2DiamRate) / 100)
    return ret > 0 and ret or 1
end

---@public 资源转钻石
function IDUtl.res2Diam(val)
    return math.ceil(val / 100)
end

---@public 取得建筑的lua对象
function IDUtl.newBuildingLua(bAttr)
    local buildingLua
    local id = bio2number(bAttr.ID)
    local gid = bio2number(bAttr.GID)
    if id == IDConst.BuildingID.headquartersBuildingID then
        buildingLua = IDLBuildingHeadquarters.new()
    elseif gid == IDConst.BuildingGID.com or gid == IDConst.BuildingGID.spec then
        if id == IDConst.BuildingID.dockyardBuildingID then
            buildingLua = IDLBuildingDockyard.new()
        elseif id == IDConst.BuildingID.AllianceID then
            -- 联盟港口
            buildingLua = IDLBuildingAlliance.new()
        else
            -- 建筑
            buildingLua = IDLBuilding.new()
        end
    elseif gid == IDConst.BuildingGID.resource then
        if id == 6 or id == 8 or id == 10 then
            buildingLua = IDLBuildingRes.new()
        elseif id == 7 or id == 9 or id == 11 then
            buildingLua = IDLBuildingStore.new()
        else
            buildingLua = IDLBuilding.new()
        end
    elseif gid == IDConst.BuildingGID.defense then
        -- 防御建筑
        if id == IDConst.BuildingID.MortarDefenseID then
            buildingLua = IDLBuildingDefenseMissile2.new()
        elseif id == IDConst.BuildingID.ThunderboltID then
            buildingLua = IDLBuildingDefenseLightning.new()
        elseif id == IDConst.BuildingID.DestroyerRocketID then
            buildingLua = IDLBuildingDefenseLightning2.new()
        else
            buildingLua = IDLBuildingDefense.new()
        end
    elseif gid == IDConst.BuildingGID.trap then
        -- 陷阱建筑
        if id == IDConst.BuildingID.AirBombID then
            buildingLua = IDLBuildingTrapAirBomb.new()
        elseif id == IDConst.BuildingID.FrozenMineID then
            buildingLua = IDLBuildingTrapFrozen.new()
        elseif id == IDConst.BuildingID.IceStormID then
            buildingLua = IDLBuildingTrapIceStorm.new()
        elseif id == IDConst.BuildingID.trapMonsterID then
            buildingLua = IDLBuildingTrapMonster.new()
        elseif id == IDConst.BuildingID.trapSwirlID then
            buildingLua = IDLBuildingTrapSwirl.new()
        else
            buildingLua = IDLBuildingTrap.new()
        end
    elseif gid == IDConst.BuildingGID.tree then
        -- 树
        buildingLua = IDLTree.new()
    else
        buildingLua = IDLBuilding.new()
    end
    return buildingLua
end

---@public 取得角色的lua对象
function IDUtl.newRoleLua(id)
    local attr = DBCfg.getRoleByID(id)
    local gid = bio2number(attr.GID)
    local role
    if gid == IDConst.RoleGID.worker then
        role = IDRWorker.new()
    elseif gid == IDConst.RoleGID.ship then
        if id == 4 then
            -- 登陆船
            role = IDRShipLandCraft.new()
        elseif id == 8 then
            -- 毁灭者自爆炸弹
            role = IDRShipBreaker.new()
        else
            role = IDRShip.new()
        end
    elseif gid == IDConst.RoleGID.solider then
        role = IDRSoldier.new()
    else
        role = IDRoleBase.new()
    end
    return role
end

---@public 取得角色的预制件名
function IDUtl.getRolePrefabName(id)
    local attr = DBCfg.getRoleByID(id)
    if attr then
        --//TODO:当是登陆士兵的时候，还要考虑使用 AniInstancing
        return attr.PrefabName
    end
end

---@public 取得角色的icon
function IDUtl.getRoleIcon(id)
    return joinStr("roleIcon_", id)
end

function IDUtl.newMapTileLua(type)
    -- if type == IDConst.WorldmapCellType.user then
        return IDWorldTile.new()
    -- end
end

---@public 通过建筑的配置表id取得资源类型
function IDUtl.getResTypeByBuildingID(attrId)
    if attrId == 6 or attrId == 7 then
        return IDConst.ResType.food
    elseif attrId == 8 or attrId == 9 then
        return IDConst.ResType.oil
    elseif attrId == 10 or attrId == 11 then
        return IDConst.ResType.gold
    end
end

---@public 取得资源名
function IDUtl.getResNameByType(type)
    if type == IDConst.ResType.food then
        return LGet("Food")
    elseif type == IDConst.ResType.gold then
        return LGet("Gold")
    elseif type == IDConst.ResType.oil then
        return LGet("Oil")
    elseif type == IDConst.ResType.diam then
        return LGet("Diam")
    end
    return ""
end
---@public 取得资源图标
function IDUtl.getResIcon(type)
    if type == IDConst.ResType.food then
        return "res_food"
    elseif type == IDConst.ResType.gold then
        return "res_gold"
    elseif type == IDConst.ResType.oil then
        return "res_shiyou"
    elseif type == IDConst.ResType.diam then
        return "res_diamond"
    end
end

---@public 取得资源音效
function IDUtl.getResSoundEffect(type)
    if type == IDConst.ResType.food then
        return "collect_Food"
    elseif type == IDConst.ResType.gold then
        return "collect_coins_01"
    elseif type == IDConst.ResType.oil then
        return "collect_oil_01"
    elseif type == IDConst.ResType.diam then
        return "collect_diamonds_01"
    end
end

local playOneFlyPic = function(params)
    local initPos = params.initPos
    CLUIOtherObjPool.borrowObjAsyn(
        "FlyPic",
        function(name, obj, orgs)
            local cell = obj:GetComponent("CLCellLua")
            cell.transform.parent = CLUIInit.self.transform
            if initPos then
                cell.transform.position = initPos
            end
            cell.transform.localScale = Vector3.one
            cell.transform.localEulerAngles = Vector3.zero
            SetActive(cell.gameObject, true)
            cell:init(params, nil)
        end
    )
end

---@public playFlyPics图标飞的效果
---@param initPos UnityEngine.Vector3 初始会位置（可以nil）
---@param from UnityEngine.Transform 起飞的transform（可以nil）
---@param to UnityEngine.Transform 目的地（不可为nil）
---@param icon string 图标
---@param sound string 音效 （可为nil）
---@param count number 飞的图标的数量，为nil是默认一个
function IDUtl.playFlyPics(initPos, from, to, icon, sound, count)
    count = count or 1
    for i = 1, count do
        InvokeEx.invokeByUpdate(
            playOneFlyPic,
            {initPos = initPos, from = from, to = to, icon = icon, sound = sound},
            (i - 1) * 0.05
        )
    end
end

---@public 隐藏弹出的菜单
function IDUtl.hidePopupMenus()
    if IDUtl.popupMenu then
        SetActive(IDUtl.popupMenu.gameObject, false)
    end
end

---@public 点击后的弹出菜单
---@param target LuaTable 目标对象，可以取得target.transform
---@param targetPosition UnityEngine.Vector3 popupMenu跟随的坐标
---@param buttonsList List 弹出的按钮列表{nameKey = 显示名的key,callback = 点击回调函数,icon = 按钮图标,bg = 按钮的背景}
---@param label string 点击后显示的文本
---@param params any 点击回调的参数
---@param offset UnityEngine.Vector3 位置的偏差
function IDUtl.showPopupMenus(target, targetPosition, buttonsList, label, params, offset)
    local d = {
        target = target,
        targetPosition = targetPosition,
        buttonList = buttonsList,
        label = label,
        params = params,
        offset = offset or Vector3.zero
    }
    if IDUtl.popupMenu == nil then
        showHotWheel()
        CLUIOtherObjPool.borrowObjAsyn(
            "PopupMenu",
            function(name, obj, orgs)
                if IDUtl.popupMenu then
                    CLUIOtherObjPool.returnObj(IDUtl.popupMenu.gameObject)
                    SetActive(IDUtl.popupMenu.gameObject, false)
                end
                IDUtl.popupMenu = obj:GetComponent("CLCellLua")
                obj.transform.parent = MyCfg.self.hud3dRoot
                obj.transform.localScale = Vector3.one
                SetActive(IDUtl.popupMenu.gameObject, true)
                IDUtl.popupMenu:init(d, nil)
                hideHotWheel()
            end
        )
    else
        SetActive(IDUtl.popupMenu.gameObject, true)
        IDUtl.popupMenu:init(d, nil)
    end
end

---@public 切换场景
function IDUtl.chgScene(mode, data, callback)
    local params = {}
    params.mode = mode
    params.data = data
    params.__finishCallback__ = callback
    getPanelAsy("PanelSceneManager", onLoadedPanel, params)
end

function IDUtl.clean()
    if IDUtl.popupMenu then
        CLUIOtherObjPool.returnObj(IDUtl.popupMenu.gameObject)
        SetActive(IDUtl.popupMenu.gameObject, false)
        IDUtl.popupMenu = nil
    end
end

--------------------------------------------
return IDUtl
