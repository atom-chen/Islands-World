---@public 防御建筑:死亡电塔
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseLightning:IDLBuildingDefense
IDLBuildingDefenseLightning = class("IDLBuildingDefenseLightning", IDLBuildingDefense)


function IDLBuildingDefenseLightning:fire()
    local target = self.target
    SetActive(self.ejector.gameObject, true)
    self.ejector:fire(self.csSelf, target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
end

function IDLBuildingDefenseLightning:onBulletHit(bullet)
    if self.target == nil or self.target.isDead then
        return
    end
    local damage = self:getDamage(self.target)
    self.target:onHurt(damage, self)
end

--------------------------------------------
return IDLBuildingDefenseLightning
