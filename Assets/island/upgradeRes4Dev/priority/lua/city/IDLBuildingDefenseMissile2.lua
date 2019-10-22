---@public 防御建筑:烈焰式火箭炮
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseMissile2:IDLBuildingDefense
IDLBuildingDefenseMissile2 = class("IDLBuildingDefenseMissile2", IDLBuildingDefense)

function IDLBuildingDefenseMissile2:doSearchTarget()
    -- local target = self.target
    -- if target == nil or target.isDead then
    --     -- 重新寻敌
    --     target = IDLBattle.searchTarget(self)
    -- else
    --     local dis = Vector3.Distance(self.transform.position, target.transform.position)
    --     -- dis减掉0.6，是因为寻敌时用的网格的index来计算的距离，因为可有可以对象在网格边上的情况
    --     if (dis - 0.6) > self.MaxAttackRange or dis < self.MinAttackRange then
    --         -- 重新寻敌
    --         target = IDLBattle.searchTarget(self)
    --     end
	-- end
	self.targets = IDLBattle.searchTarget(self, 2)
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)

    -- 设置目标
    self:setTarget(self.targets[1])

	if self.targets and #self.targets > 0 then
        -- 炮面向目标
        self:lookatTarget(self.target, false, self:wrapFunc(self.fire))
    end
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
