---@public 冰冻地雷
require("city.IDLBuildingTrap")

---@class IDLBuildingTrapFrozen:IDLBuildingTrap
IDLBuildingTrapFrozen = class("IDLBuildingTrapFrozen", IDLBuildingTrap)

function IDLBuildingTrapFrozen:fire()
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapFrozen.super
    base.fire(self)

    -- 冰冻//TODO:目前没有冰冻累的爆的特殊
    ---@param target IDLUnitBase
    for i, target in ipairs(self.targets or {}) do
        if not target.isDead then
            target:frozen(bio2Int(self.attr.DamageKeepTime) / 1000)
        end
    end
end

--------------------------------------------
return IDLBuildingTrapFrozen
