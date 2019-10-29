---@class IDLBattleSearcher 战场寻敌器
local IDLBattleSearcher = {}

---@class BuildingRangeInfor
---@field public index number 网格的index
---@field public dis number 距离

-- 建筑攻击范围数据 key=IDLBuilding.instanceID, val = BuildingRangeInfor
local buildingsRange = {}
-- 建筑数据，是从城市里取的原始数据
local buildings = {}

-- 每个角色所在网格的index数据。key=角色对象，val=网格的index
local rolesIndex = {}
-- 进攻方（舰船），记录的是每个网格上有哪些舰船
local offense = {}
-- 防守方（舰船），记录的是每个网格上有哪些舰船
local defense = {}
local defenseUnits = {}
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
    ---@param b IDLBuilding
    for k, b in pairs(_buildings) do
        if not (b.isTrap or b.isTree or bio2number(b.attr.GID) == IDConst.BuildingGID.decorate) then
            buildings[k] = b
        end

        if bio2Int(b.attr.GID) == IDConst.BuildingGID.defense then -- 防御炮
            local MaxAttackRange =
                DBCfg.getGrowingVal(
                bio2number(b.attr.AttackRangeMin),
                bio2number(b.attr.AttackRangeMax),
                bio2number(b.attr.AttackRangeCurve),
                bio2number(b.serverData.lev) / bio2number(b.attr.MaxLev)
            )
            MaxAttackRange = MaxAttackRange / 100
            local size = IDLBattleSearcher.calculateSize(MaxAttackRange)
            -- 取得可攻击范围内的格子
            local cells = grid:getOwnGrids(b.gridIndex, NumEx.getIntPart(size))

            local MinAttackRange = bio2Int(b.attr.MinAttackRange) / 100
            -- 按照离建筑的远近排序
            local list = IDLBattleSearcher.sortGridCells(b, MinAttackRange, MaxAttackRange, cells)
            buildingsRange[b.instanceID] = list
        elseif bio2Int(b.attr.GID) == IDConst.BuildingGID.trap or bio2Int(b.attr.ID) == IDConst.BuildingID.AllianceID then -- 陷阱\联盟港口，主要处理触发半径
            local triggerR =
                DBCfg.getGrowingVal(
                bio2number(b.attr.TriggerRadiusMin),
                bio2number(b.attr.TriggerRadiusMax),
                bio2number(b.attr.TriggerRadiusCurve),
                bio2number(b.serverData.lev) / bio2number(b.attr.MaxLev)
            )
            triggerR = triggerR / 100
            local size = IDLBattleSearcher.calculateSize(triggerR)
            -- 取得可攻击范围内的格子
            local cells = grid:getOwnGrids(b.gridIndex, NumEx.getIntPart(size))

            -- 按照离建筑的远近排序
            local list = IDLBattleSearcher.sortGridCells(b, 0, triggerR, cells)
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

---@param building IDLBuilding
function IDLBattleSearcher.debugBuildingAttackRange(building)
    for k, obj in ipairs(IDLBattleSearcher._debugRangs or {}) do
        CLThingsPool.returnObj(obj)
        SetActive(obj, false)
    end
    IDLBattleSearcher._debugRangs = {}

    local cells = buildingsRange[building.instanceID]
    for i, v in ipairs(cells or {}) do
        CLThingsPool.borrowObjAsyn(
            "MapTileSize",
            function(name, obj, orgs)
                obj.transform.position = grid.grid:GetCellCenter(v.index)
                obj.transform.localScale = Vector3.one * 0.1
                obj.transform.localEulerAngles = Vector3.zero
                SetActive(obj, true)
                table.insert(IDLBattleSearcher._debugRangs, obj)
            end
        )
    end
end

---@public 要取得圆的范围，因此取得了圆的外切正方形的边长
function IDLBattleSearcher.calculateSize(r)
    return r * 2
    -- return NumEx.getIntPart(math.sqrt(2 * (r * r)) + 0.5)
end

---@public 刷新舰船的位置
---@param unit IDRoleBase
function IDLBattleSearcher.refreshUnit(unit)
    --//注意所有移动的战斗单元需要定时刷新
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
        map[unit] = unit
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
        map[unit] = unit
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
---@param targetsNum number 目标数量
function IDLBattleSearcher.searchTarget(unit, targetsNum)
    if unit.isBuilding then
        -- 说明是建筑的防御设施
        return IDLBattleSearcher.buildingSearchRole4Def(unit, targetsNum)
    else
        -- 说明是角色
        return IDLBattleSearcher.searchTarget4Role(unit, targetsNum)
    end
end

---@public 防御设施寻敌人
---@param building IDLBuilding
---@param targetsNum number 目标数量默认是1个
function IDLBattleSearcher.buildingSearchRole4Def(building, targetsNum)
    targetsNum = targetsNum or 1

    local cells = buildingsRange[building.instanceID]
    local targets, targetsPrefered = {}, {}
    local PreferedTargetType = bio2number(building.attr.PreferedTargetType)
    ---@param v BuildingRangeInfor
    for i, v in ipairs(cells or {}) do
        local map = offense[v.index]
        if map then
            ---@param role IDRoleBase
            for role, v2 in pairs(map) do
                if role then
                    -- 可攻击地面、飞行单位否？
                    if IDLBattleSearcher.isTarget(building, role) then
                        table.insert(targets, role)

                        if PreferedTargetType > 0 then
                            -- 有优先攻击类型
                            if bio2Int(role.attr.GID) == PreferedTargetType then
                                table.insert(targetsPrefered, role)
                            end
                        end
                    end
                end

                -- 处理跳出
                if targetsNum == 1 then
                    if PreferedTargetType > 0 then
                        -- 有优先目标类型
                        if targetsPrefered[1] then
                            return targetsPrefered[1]
                        end
                    else
                        if targets[i] then
                            return targets[i]
                        end
                    end
                else
                    if PreferedTargetType > 0 then
                        -- 有优先目标类型
                        if #targetsPrefered >= targetsNum then
                            return targetsPrefered
                        end
                    else
                        if #targets >= targetsNum then
                            return targets
                        end
                    end
                end
            end
        end
    end

    if targetsNum == 1 then
        return targetsPrefered[1] or targets[1]
    else
        if PreferedTargetType > 0 then
            if #targetsPrefered < targetsNum and #targets <= targetsNum then
                -- targets是包含targetsPrefered的
                return targets
            elseif #targetsPrefered < targetsNum and #targets > targetsNum then
                ---@param role IDRoleBase
                for i, role in ipairs(targets) do
                    if bio2number(role.attr.GID) ~= PreferedTargetType then
                        -- 说明之前是没有加入到优先目标列表里的
                        table.insert(targetsPrefered, role)
                    end
                    if #targetsPrefered == targetsNum then
                        return targetsPrefered
                    end
                end
            else
                -- 其它情况不存在，因为上面的for循环里已经处理了
                printe("居然print到这里了！！！这种是情况不存在，因为上面的for循环里已经处理了！bug！bug！")
            end
        else
            return targets
        end
    end
end

---@public 角色寻敌(注意角色只能找一个目标，不可能同时找多个目标)
---@param role IDRoleBase
function IDLBattleSearcher.searchTarget4Role(role)
    local tempList = {}
    local dis = 0

    local roleIndex = rolesIndex[role] or -1
    if roleIndex <= -1 then
        printe("取得角色的网格index错误，应该是有bug！")
        return
    end
    local PreferedTargetType = bio2number(role.attr.PreferedTargetType)
    if role.isOffense then
        -- 取得角色的index
        -- 取得离角色最近的目标，注意要考虑优先攻击目标
        ---@param b IDDBBuilding
        for k, b in pairs(buildings) do
            if IDLBattleSearcher.isTarget(role, b) then
                dis = IDLBattleSearcher.getDistance(roleIndex, b.gridIndex)
                table.insert(tempList, {unit = b, dis = dis})
            end
        end
        if #tempList > 0 then
            CLQuickSort.quickSort(
                tempList,
                function(a, b)
                    return a.dis < b.dis
                end
            )
            local target, preferedTarget = nil, nil
            ---@type IDLBuilding
            local building
            -- 处理优先攻击目标
            for i, d in ipairs(tempList) do
                building = d.unit
                if target == nil then
                    target = building
                end
                if PreferedTargetType > 0 then
                    if bio2number(building.attr.GID) == PreferedTargetType then
                        preferedTarget = building
                        return preferedTarget
                    end
                else
                    return target
                end
            end
            return (preferedTarget or target)
        else
            -- 说明没有建筑目标，再找找舰船目标
            return IDLBattleSearcher.roleSearch4Role(role, defense)
        end
    else
        --//防守方的舰船寻敌
        return IDLBattleSearcher.roleSearch4Role(role, offense)
    end
end

---@param attacker IDRoleBase
function IDLBattleSearcher.roleSearch4Role(attacker, shipInfor)
    local list = {}
    local dis = 0

    local roleIndex = rolesIndex[attacker] or -1
    for index, map in pairs(shipInfor) do
        dis = IDLBattleSearcher.getDistance(index, roleIndex)
        table.insert(list, {dis = dis, index = index})
    end
    CLQuickSort.quickSort(
        list,
        function(a, b)
            return a.dis < b.dis
        end
    )
    local PreferedTargetType = bio2number(attacker.attr.PreferedTargetType)
    local target, preferedTarget
    for i, v in ipairs(list) do
        local map = shipInfor[v.index]
        ---@param r IDRoleBase
        for k, r in pairs(map) do
            if IDLBattleSearcher.isTarget(attacker, r) then
                if target == nil then
                    target = r
                end
                if PreferedTargetType > 0 then
                    if bio2number(r.attr.GID) == PreferedTargetType then
                        preferedTarget = r
                        return preferedTarget
                    end
                else
                    return target
                end
            end
        end
    end
    return preferedTarget or target
end

---@param attacker IDLUnitBase
---@param unit IDLUnitBase
---@param onlyOnGroundOrSky 只找地面上或天空的单，1：地面，2：天空，其它值则都可以
function IDLBattleSearcher.isTarget(attacker, unit, onlyOnGroundOrSky)
    if unit.isDead then
        return false
    end
    ---@type IDLBuilding
    local b = attacker
    -- 可攻击地面、飞行单位否？
    if onlyOnGroundOrSky == 1 then
        -- 只找地面单元
        if unit.attr.IsFlying then
            return false
        else
            if b.attr.GroundTargets then
                return true
            else
                return false
            end
        end
    elseif onlyOnGroundOrSky == 2 then
        -- 只找飞行单元
        if unit.attr.IsFlying then
            if b.attr.AirTargets then
                return true
            else
                return false
            end
        else
            return false
        end
    else
        -- 都可以
        if ((unit.attr.IsFlying and b.attr.AirTargets) or ((not unit.attr.IsFlying) and b.attr.GroundTargets)) then
            return true
        else
            return false
        end
    end
end

---@public 取得范围内的最优目标
---@param attacker IDLUnitBase
---@param pos UnityEngine.Vector3
---@param r number 半径
function IDLBattleSearcher.getTarget(attacker, pos, r)
    local onlyOnGroundOrSky = pos.y <= 1 and 1 or 2
    local index = grid.grid:GetCellIndex(pos)
    local cells = grid:getOwnGrids(index, NumEx.getIntPart(r * 2))
    local list = nil
    if attacker.isOffense then
        list = defense
    else
        list = offense
    end
    local m, index2
    for i = 0, cells.Count - 1 do
        index2 = cells[i]
        if IDLBattleSearcher.getDistance(index, index2) <= r then
            m = list[index2]
            if m then
                for k, v in pairs(m) do
                    if IDLBattleSearcher.isTarget(attacker, v, onlyOnGroundOrSky) then
                        return v
                    end
                end
            end
        end
    end
    return nil
end

---@public 取得范围内的所有目标
---@param attacker IDLUnitBase
---@param pos UnityEngine.Vector3
---@param r number 半径
function IDLBattleSearcher.getTargetsInRange(attacker, pos, r)
    local onlyOnGroundOrSky = pos.y <= 1 and 1 or 2
    local index = grid.grid:GetCellIndex(pos)
    local cells = grid:getOwnGrids(index, NumEx.getIntPart(r * 2))
    local list = nil
    if attacker.isOffense then
        list = defense
    else
        list = offense
    end
    local ret = {}
    local m, index2
    for i = 0, cells.Count - 1 do
        index2 = cells[i]
        if IDLBattleSearcher.getDistance(index, index2) <= r then
            m = list[index2]
            if m then
                ---@param v IDLUnitBase
                for k, v in pairs(m) do
                    if IDLBattleSearcher.isTarget(attacker, v, onlyOnGroundOrSky) then
                        table.insert(ret, v)
                    end
                end
            end
        end
    end
    return ret
end

---@param unit IDLUnitBase
function IDLBattleSearcher.someOneDead(unit)
    if unit.isBuilding then
        ---@type IDLBuilding
        local b = unit
        buildingsRange[unit.instanceID] = nil
        buildings[bio2number(b.serverData.idx)] = nil
    else
        if unit.isOffense then
            local index = rolesIndex[unit]
            -- 先清除掉旧的数据
            local map = offense[index] or {}
            map[unit] = nil
            offense[index] = map
        else
            local index = rolesIndex[unit]
            -- 先清除掉旧的数据
            local map = defense[index] or {}
            map[unit] = nil
            defense[index] = map
        end
        rolesIndex[unit] = nil
    end
end

function IDLBattleSearcher.clean()
    buildingsRange = {}
    buildings = {}
    offense = {}
    defense = {}
    rolesIndex = {}

    for k, obj in ipairs(IDLBattleSearcher._debugRangs or {}) do
        CLThingsPool.returnObj(obj)
        SetActive(obj, false)
    end
    IDLBattleSearcher._debugRangs = {}
end

--------------------------------------------
return IDLBattleSearcher
