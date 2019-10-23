---@public 防御建筑:烈焰式火箭炮
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseMissile2:IDLBuildingDefense
IDLBuildingDefenseMissile2 = class("IDLBuildingDefenseMissile2", IDLBuildingDefense)

function IDLBuildingDefenseMissile2:doSearchTarget()
	self.targets = IDLBattle.searchTarget(self, 2)

    -- 设置目标
    self:setTarget(self.targets[1])
end

function IDLBuildingDefenseMissile2:fire()
    local target = self.target
	SetActive(self.ejector.gameObject, true)
	-- 发射两个导弹
	self.ejector:fire(0, 1, 1, 0, 0, self.csSelf, target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
	InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.fire2), 0.2)
end

function IDLBuildingDefenseMissile2:fire2()
	---@type IDLUnitBase
	local target = self.targets[2] or self.target
	self.ejector:fire(1, 1, 1, 0, 0.3, self.csSelf, target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
end

--------------------------------------------
return IDLBuildingDefenseMissile2
