---@class IDLBattleSearcher 战场寻敌器
local IDLBattleSearcher = {}

---@class BuildingRangeInfor
---@field public index number 网格的index
---@field public dis number 距离

-- 建筑攻击范围数据 key=IDLBuilding, val = BuildingRangeInfor
local buildingsRange = {}
-- 建筑数据，是从城市里取的原始数据
local buildings = {}

-- 每个角色所在网格的index数据。key=角色对象，val=网格的index
local rolesIndex = {}
-- 进攻方（舰船），记录的是每个网格上有哪些舰船
local offense = {}
-- 防守方（舰船），记录的是每个网格上有哪些舰船
local defense = {}
-- 距离缓存,key=两个网格的index拼接，val=距离
local disCache = {}

---@type CLGrid
local grid

-- 初始化
---@param city IDMainCity
function IDLBattleSearcher.init(city)
    grid = city.grid
    IDLBattleSearcher.wrapBuildingInfor(city.getBuildings())
end

---@public 包装建筑的数据
function IDLBattleSearcher.wrapBuildingInfor(_buildings)
    buildings = _buildings
    ---@param b IDLBuilding
    for k, b in pairs(buildings) do
        if bio2Int(b.attr.GID) == IDConst.BuildingGID.defense then -- 防御炮
            local MaxAttackRange =
                DBCfg.getGrowingVal(
                bio2number(b.attr.AttackRangeMin) / 100,
                bio2number(b.attr.AttackRangeMax) / 100,
                bio2number(b.attr.AttackRangeCurve),
                bio2number(b.serverData.lev) / bio2number(b.attr.MaxLev)
            )
            -- 取得可攻击范围内的格子
            local cells = grid:getOwnGrids(b.gridIndex, MaxAttackRange)

            local MinAttackRange = bio2Int(b.attr.MinAttackRange) / 100
            -- 按照离建筑的远近排序
            local list = IDLBattleSearcher.sortGridCells(b, MinAttackRange, MaxAttackRange, cells)
            buildingsRange[b.instanceID] = list
        elseif bio2Int(b.attr.GID) == IDConst.BuildingGID.trap or bio2Int(b.attr.ID) == IDConst.dockyardBuildingID then -- 陷阱\造船厂，主要处理触发半径
            local triggerR =
                DBCfg.getGrowingVal(
                bio2number(b.attr.TriggerRadiusMin) / 100,
                bio2number(b.attr.TriggerRadiusMax) / 100,
                bio2number(b.attr.TriggerRadiusCurve),
                bio2number(b.serverData.lev) / bio2number(b.attr.MaxLev)
            )
            local size = IDLBattleSearcher.calculateSize(triggerR)
            -- 取得可攻击范围内的格子
            local cells = grid:getOwnGrids(b.gridIndex, size)

            -- 按照离建筑的远近排序
            local list = IDLBattleSearcher.sortGridCells(b, 0, 0, cells)
            buildingsRange[b.instanceID] = list
        end
    end
end

---@public 按照离建筑的远近排序
---@param building IDLBuilding
function IDLBattleSearcher.sortGridCells(building, min, max, cells)
    local count = cells.Count
    local list = {}
    local buildingPos = building.transform.position
    buildingPos.y = 0
    local index, pos, dis
    -- 准备要排序的数据
    for i = 0, count - 1 do
        index = cells[i]
        pos = grid.grid:GetCellCenter(index)
        pos.y = 0
        dis = Vector3.Distance(buildingPos, pos)
        if dis >= min and (dis <= max or max <= 0) then
            -- 只有可攻击范围的才处理
            table.insert(list, {index = index, dis = dis})
        end
    end
    CLQuickSort.quickSort(
        list,
        function(a, b)
            return a.dis < b.dis
        end
    )
    return list
end

---@public 要取得圆的范围，因此取得了圆的外切正方形的边长
function IDLBattleSearcher.calculateSize(r)
    return NumEx.getIntPart(math.sqrt(2 * (r * r)) + 0.5)
end

---@public 刷新舰船的位置
---@param unit IDRoleBase
function IDLBattleSearcher.refreshUnit(unit)
    local index = grid.grid:GetCellIndex(unit.transform.position)
    if unit.isOffense then
        local oldIndex = rolesIndex[unit]
        if oldIndex and oldIndex ~= index then
            -- 先清除掉旧的数据
            local map = offense[oldIndex] or {}
            map[unit] = nil
            offense[oldIndex] = map
        end
        local map = offense[index] or {}
        map[unit] = index
        offense[index] = map
    else
        local oldIndex = rolesIndex[unit]
        if oldIndex and oldIndex ~= index then
            -- 先清除掉旧的数据
            local map = defense[oldIndex] or {}
            map[unit] = nil
            defense[oldIndex] = map
        end
        local map = defense[index] or {}
        map[unit] = index
        defense[index] = map
    end
    -- 最后再更新舰船的位置
    rolesIndex[unit] = index
end

---@public 取得两个网格间的距离
function IDLBattleSearcher.getDistance(index1, index2)
    local key = joinStr(index1, "_", index2)
    local dis = disCache[key]
    if dis then
        return dis
    else
        local pos1 = grid.grid:GetCellCenter(index1)
        local pos2 = grid.grid:GetCellCenter(index2)
        dis = Vector3.Distance(pos1, pos2)
        disCache[key] = dis
        return dis
    end
end

---@public 寻敌
function IDLBattleSearcher.searchTarget(unit)
    if unit.isBuilding then
        -- 说明是建筑的防御设施
        return IDLBattleSearcher.searchRole4Def(unit)
    else
        -- 说明是角色
    end
end

---@public 防御设施寻敌人
---@param building IDLBuilding
function IDLBattleSearcher.searchRole4Def(building)
    local cells = buildingsRange[building.instanceID]
    local taget, preferedTarget
    local PreferedTargetType = bio2number(building.attr.PreferedTargetType)
    ---@param v BuildingRangeInfor
    for i, v in ipairs(cells) do
        local map = offense[v.index]
        if map then
            ---@param role IDRoleBase
            for role, v2 in pairs(map) do
                if role and (not role.idDead) then
                    if not taget then
                        taget = role
                    end
                    if PreferedTargetType > 0 then
                        -- 有优先攻击类型
                        if bio2Int(role.attr.GID) == PreferedTargetType then
                            PreferedTargetType = role
                            return PreferedTargetType
                        end
                    else
                        return taget
                    end
                end
            end
        end
    end
    return preferedTarget or taget
end

---@public 角色寻敌
---@param role IDRoleBase
function IDLBattleSearcher.search4Role(role)
    if role.isOffense then
        -- 取得角色的index
        -- 取得离角色最近的目标，注意要考虑优先攻击目标
    else
        --//TODO:防守方的舰船寻敌
    end
end

function IDLBattleSearcher.clean()
    buildingsRange = {}
    buildings = nil
    offense = {}
    defense = {}
    rolesIndex = {}
end

--------------------------------------------
return IDLBattleSearcher
