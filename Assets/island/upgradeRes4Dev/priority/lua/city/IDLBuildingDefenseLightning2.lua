---@public 防御建筑:末日之门
require("city.IDLBuildingDefense")

---@class IDLBuildingDefenseLightning2:IDLBuildingDefense
IDLBuildingDefenseLightning2 = class("IDLBuildingDefenseLightning2", IDLBuildingDefense)

function IDLBuildingDefenseLightning2:doAttack()
    if GameMode.battle ~= MyCfg.mode or self.isDead then
        return
    end

    self:doSearchTarget()
    if self.target then
        -- 炮面向目标
        self:lookatTarget(self.target, true)
        for i, target in ipairs(self.targets) do
            self:fire(target)
        end
    end
    -- 再次攻击
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)
end

function IDLBuildingDefenseLightning2:doSearchTarget()
    self.targets = IDLBattle.searchTarget(self, 3)

    -- 设置目标
    self:setTarget(self.targets[1])
end

function IDLBuildingDefenseLightning2:fire(target)
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
