-- 公共处理
require("city.IDLBuilding")
require("city.IDLBuildingRes")
require("city.IDLBuildingStore")
require("city.IDLBuildingDefense")
require("city.IDLBuildingDockyard")
require("city.IDLBuildingTrap")
require("city.IDLTree")
require("role.IDRoleBase")
require("role.IDRWorker")
require("role.IDRShip")
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
                    table.insert(list, { name = LGet("Tile"), currVal = bio2number(headquartersOpen.Tiles), addVal = diff })
                end
                -- 工人
                diff = bio2number(headquartersOpenNext.Workers) - bio2number(headquartersOpen.Workers)
                if diff > 0 then
                    table.insert(list, { name = LGet("Worker"), currVal = bio2number(headquartersOpen.Tiles), addVal = diff })
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
                        table.insert(list, { name = LGet(bAttr.NameKey), currVal = bio2number(headquartersOpen[keyBuilding]), addVal = diff })
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
        d.value = DBCfg.getGrowingVal(bio2number(attr.BuildCostFoodMin), bio2number(attr.BuildCostFoodMax), bio2number(attr.BuildCostFoodCurve), lev / bio2number(attr.MaxLev))
        d.other = {}
        table.insert(list, d)
    end
    -- 金币
    if bio2number(attr.BuildCostGoldMax) > 0 then
        d = {}
        d.icon = "res_gold"
        d.value = DBCfg.getGrowingVal(bio2number(attr.BuildGoldFoodMin), bio2number(attr.BuildCostGoldMax), bio2number(attr.BuildCostGoldCurve), lev / bio2number(attr.MaxLev))
        d.other = {}
        table.insert(list, d)
    end
    -- 油
    if bio2number(attr.BuildCostOilMax) > 0 then
        d = {}
        d.icon = "res_shiyou"
        d.value = DBCfg.getGrowingVal(bio2number(attr.BuildCostOilMin), bio2number(attr.BuildCostOilMax), bio2number(attr.BuildCostOilCurve), lev / bio2number(attr.MaxLev))
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
        map.food = DBCfg.getGrowingVal(bio2number(attr.BuildCostFoodMin), bio2number(attr.BuildCostFoodMax), bio2number(attr.BuildCostFoodCurve), lev / bio2number(attr.MaxLev))
    end
    -- 金币
    if bio2number(attr.BuildCostGoldMax) > 0 then
        map.gold = DBCfg.getGrowingVal(bio2number(attr.BuildGoldFoodMin), bio2number(attr.BuildCostGoldMax), bio2number(attr.BuildCostGoldCurve), lev / bio2number(attr.MaxLev))
    end
    -- 油
    if bio2number(attr.BuildCostOilMax) > 0 then
        map.oil = DBCfg.getGrowingVal(bio2number(attr.BuildCostOilMin), bio2number(attr.BuildCostOilMax), bio2number(attr.BuildCostOilCurve), lev / bio2number(attr.MaxLev))
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
    if gid == IDConst.BuildingGID.com or gid == IDConst.BuildingGID.spec then
        if id == IDConst.dockyardBuildingID then
            buildingLua = IDLBuildingDockyard.new()
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
        buildingLua = IDLBuildingDefense.new()
    elseif gid == IDConst.BuildingGID.trap then
        -- 陷阱建筑
        buildingLua = IDLBuildingTrap.new()
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
        role = IDRShip.new()
    else
        role = IDRoleBase.new()
    end
    return role
end

---@public 取得角色的预制件名
function IDUtl.getRolePrefabName(id)
    local attr = DBCfg.getRoleByID(id)
    if attr then
        return attr.PrefabName
    end
end

function IDUtl.newMapTileLua(type)
    if type == 1 then
        return IDWorldTile.new()
    end
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
        return ""
    elseif type == IDConst.ResType.gold then
        return ""
    elseif type == IDConst.ResType.oil then
        return ""
    elseif type == IDConst.ResType.diam then
        return ""
    end
end

local playOneFlyPic = function(params)
    local initPos = params.initPos
    CLUIOtherObjPool.borrowObjAsyn("FlyPic",
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
            end)
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
        InvokeEx.invokeByUpdate(playOneFlyPic,
                { initPos = initPos, from = from, to = to, icon = icon, sound = sound },
                (i - 1) * 0.05)
    end
end

--------------------------------------------
return IDUtl
