---@class IDLBattleSeacher 战场寻敌器
local IDLBattleSeacher = {}

---@class BuildingRangeInfor
---@field public index number 网格的index
---@field public dis number 距离

-- 建筑攻击范围数据 k=IDLBuilding, v = BuildingRangeInfor
local buildingsRange = {}
-- 建筑数据
local buildings = {}
-- 进攻方（舰船）
local offense = {}
-- 防守方（舰船）
local defense = {}
---@type CLGrid
local grid

-- 初始化
---@param city IDMainCity
function IDLBattleSeacher.init(city)
    grid = city.grid
    IDLBattleSeacher.wrapBuildingInfor(city.getBuildings())
end

---@public 包装建筑的数据
function IDLBattleSeacher.wrapBuildingInfor(_buildings)
    buildings = _buildings
    ---@param b IDLBuilding
    for k, b in pairs(buildings) do
        if bio2Int(b.attr.GID) == IDConst.BuildingGID.defense then -- 防御炮
            local size = IDLBattleSeacher.calculateSize(bio2Int(b.attr.AttackRangeMax) / 100)
            -- 取得可攻击范围内的格子
            local cells = grid:getOwnGrids(b.gridIndex, size)

            local AttackRangeMin = bio2Int(building.attr.AttackRangeMin) / 100
            local AttackRangeMax = bio2Int(building.attr.AttackRangeMax) / 100
            -- 按照离建筑的远近排序
            local list = IDLBattleSeacher.sortGridCells(b, AttackRangeMin, AttackRangeMax, cells)
            buildingsRange[b] = list
        elseif bio2Int(b.attr.GID) == IDConst.BuildingGID.trap or bio2Int(b.attr.ID) == IDConst.dockyardBuildingID then -- 陷阱\造船厂，主要处理触发半径
            local triggerR =
                DBCfg.getGrowingVal(
                bio2number(b.attr.TriggerRadiusMin) / 100,
                bio2number(b.attr.TriggerRadiusMax) / 100,
                bio2number(b.attr.TriggerRadiusCurve),
                bio2number(b.serverData.lev) / bio2number(b.attr.MaxLev)
			)
            local size = IDLBattleSeacher.calculateSize(triggerR)
            -- 取得可攻击范围内的格子
            local cells = grid:getOwnGrids(b.gridIndex, size)

            -- 按照离建筑的远近排序
            local list = IDLBattleSeacher.sortGridCells(b, 0, 0, cells)
            buildingsRange[b] = list
        end
    end
end

---@public 按照离建筑的远近排序
---@param building IDLBuilding
function IDLBattleSeacher.sortGridCells(building, min, max, cells)
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
function IDLBattleSeacher.calculateSize(r)
    return NumEx.getIntPart(math.sqrt(2 * (r * r)) + 0.5)
end

---@param unit IDRoleBase
function IDLBattleSeacher.addUnit(unit, isOffense)
    local index = grid:GetCellIndex(unit.transform.position)
    if isOffense then
        local map = offense[index] or {}
        map[unit] = index
        offense[index] = map
    else
        local map = defense[index] or {}
        map[unit] = index
        defense[index] = map
    end
end

function IDLBattleSeacher.clean()
    offense = {}
    defense = {}
end

--------------------------------------------
return IDLBattleSeacher
