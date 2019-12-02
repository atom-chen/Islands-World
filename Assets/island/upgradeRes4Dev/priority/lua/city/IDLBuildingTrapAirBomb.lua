---@public 放空气球
require("city.IDLBuildingTrap")

---@class IDLBuildingTrapAirBomb:IDLBuildingTrap
IDLBuildingTrapAirBomb = class("IDLBuildingTrapAirBomb", IDLBuildingTrap)

function IDLBuildingTrapAirBomb:__init(csSelfObj, other)
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapAirBomb.super
    if base.__init(self, csSelfObj, other) then
        ---@type Coolape.MyTween
        self.tweenPos = self.gameObject:GetComponent("MyTween")
        return true
    end
    return false
end

function IDLBuildingTrapAirBomb:fire()
    -- 因为气球是可以动的，且触发到爆炸是等了一会的，再次寻敌
    self:doSearchTarget()
    if self.target == nil then
        self.isTrigered = false
        self:doAttack()
        return
    end
    self.tweenPos:flyout(self.target.transform.position, 1.4, 0, nil, self:wrapFunc(self.onArrive), true)
end

function IDLBuildingTrapAirBomb:onArrive()
    -- 取得目标
    local targets = IDLBattle.searcher.getTargetsInRange(self, self.transform.position, self.MaxAttackRange)

    -- 自爆
    -- //TODO:陷阱的配置表时还没有音效
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
    CLEffect.play(self.attr.AttackEffect, self.transform.position)
    self:onDead()

    ---@param target IDLUnitBase
    for i, target in ipairs(targets) do
        target:onHurt(self:getDamage(target), self)
    end
end

function IDLBuildingTrapAirBomb:clean()
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapAirBomb.super
    base.clean(self)
    self.tweenPos:stop()
end

--------------------------------------------
return IDLBuildingTrapAirBomb
