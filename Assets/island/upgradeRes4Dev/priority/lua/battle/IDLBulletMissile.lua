-- 导弹
local _cell = {}
local csSelf = nil
local transform = nil
local mData = nil
local lastSearchTime = 0

---@public 重新设置目标
---@param bullet Coolape.CLBulletBase
function _cell.resetTarget(bullet)
    if MyCfg.mode ~= GameMode.battle then
        return
    end

    if DateEx.nowMS - lastSearchTime < bullet.RefreshTargetMSec then
        -- 每隔0.5秒寻敌一次
        return
    end
    lastSearchTime = DateEx.nowMS

    ---@type IDLUnitBase
    local attacker = bullet.attacker.luaTable
    -- 取得半径
    local r = 0
    if attacker.isBuilding then
        ---@type IDLBuildingDefense
        local b = attacker
        r = b.MaxAttackRange
    else
        r = 4
    end
    local target = IDLBattle.searcher.getTarget(bullet.attacker, bullet.transform.position, r)
    if target then
        --[[ 注意：当target为nil时，不能设置bullet的target,
		因为可能bullet已经有目标了，而通过上面的方法只是再次取得更优的目标，
		只有当取得目标是才更新子弹的目标]]
        bullet.target = target
    end
end

--------------------------------------------
return _cell
