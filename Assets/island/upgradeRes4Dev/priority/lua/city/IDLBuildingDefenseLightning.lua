---@public 防御建筑:死亡电塔
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseLightning:IDLBuildingDefense
IDLBuildingDefenseLightning = class("IDLBuildingDefenseLightning", IDLBuildingDefense)

function IDLBuildingDefenseLightning:onBulletHit(bullet)
    if self.target == nil or self.target.isDead then
        return
    end
    local damage = self:getDamage(self.target)
    self.target:onHurt(damage, self)
end

--------------------------------------------
return IDLBuildingDefenseLightning
