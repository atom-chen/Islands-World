---@class IDLBattleSeacher 战场寻敌器
local IDLBattleSeacher = {}
-- 建筑攻击范围数据
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
		local size = bio2Int(b.attr.Size)
		local cells = grid:getOwnGrids(b.gridIndex, size)
		--//TODO:这里还有问题，要取得圆的范围，还不是正方形
    end
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
