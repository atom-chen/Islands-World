---@public 防御建筑:死亡电塔
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseLightning:IDLBuildingDefense
IDLBuildingDefenseLightning = class("IDLBuildingDefenseLightning", IDLBuildingDefense)

function IDLBuildingDefenseLightning:doSearchTarget()
    local target = self.target
    if target == nil or target.isDead then
        -- 重新寻敌
        target = IDLBattle.searchTarget(self)
    else
        local dis = Vector3.Distance(self.transform.position, target.transform.position)
        -- dis减掉0.6，是因为寻敌时用的网格的index来计算的距离，因为可有可以对象在网格边上的情况
        if (dis - 0.6) > self.MaxAttackRange or dis < self.MinAttackRange then
            -- 重新寻敌
            target = IDLBattle.searchTarget(self)
        end
    end
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)

    -- 设置目标
    self:setTarget(target)
    if target then
        self:lookatTarget(self.target, true)
        self:fire()
    end
end

function IDLBuildingDefenseLightning:fire()
    local target = self.target
    local dir = target.transform.position - self.ejector.transform.position
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
