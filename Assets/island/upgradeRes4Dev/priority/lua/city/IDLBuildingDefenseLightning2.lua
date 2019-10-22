---@public 防御建筑:末日之门
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseLightning2:IDLBuildingDefense
IDLBuildingDefenseLightning2 = class("IDLBuildingDefenseLightning2", IDLBuildingDefense)

function IDLBuildingDefenseLightning2:doSearchTarget()
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
    self.targets = IDLBattle.searchTarget(self, 3)
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)

    -- 设置目标
    self:setTarget(self.targets[1])

    if self.targets and #self.targets > 0 then
        -- 炮面向目标
        self:lookatTarget(self.target, true)
        for i, v in ipairs(self.targets) do
            self:fire(v)
        end
    end
end

function IDLBuildingDefenseLightning2:fire(target)
    local dir = target.transform.position - self.ejector.transform.position
    SetActive(self.ejector.gameObject, true)
    self.ejector:fire(self.csSelf, target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
end

---@param bullet Coolape.CLBulletBase
function IDLBuildingDefenseLightning2:onBulletHit(bullet)
    local target = bullet.target.luaTable
    if target then
        local damage = self:getDamage(target)
        target:onHurt(damage, self)
    end
end

--------------------------------------------
return IDLBuildingDefenseLightning2
