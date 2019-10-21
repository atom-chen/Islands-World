---@public 防御建筑:烈焰式火箭炮
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseMissile2:IDLBuildingDefense
IDLBuildingDefenseMissile2 = class("IDLBuildingDefenseMissile2", IDLBuildingDefense)

function IDLBuildingDefenseMissile2:fire()
    local target = self.target
    local dir = target.transform.position - self.ejector.transform.position
	SetActive(self.ejector.gameObject, true)
	-- 发射两个导弹
	self.ejector:fire(0, 1, 1, 0, 0, self.csSelf, self.target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
	InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.fire2), 0.2)
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
end

function IDLBuildingDefenseMissile2:fire2()
    self.ejector:fire(1, 1, 1, 0, 0.3, self.csSelf, self.target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
end

--------------------------------------------
return IDLBuildingDefenseMissile2
