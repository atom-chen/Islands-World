---@public 暴风控制器
require("city.IDLBuildingTrap")

---@class IDLBuildingTrapIceStorm:IDLBuildingTrap
IDLBuildingTrapIceStorm = class("IDLBuildingTrapIceStorm", IDLBuildingTrap)

function IDLBuildingTrapIceStorm:__init(selfObj, other)
    if self:getBase(IDLBuildingTrapIceStorm).__init(self, selfObj, other) then
        ---@type Coolape.MyTween
        self.tweenPos = self.gameObject:GetComponent("MyTween")

        self.spin = self.csSelf:GetComponent("Spin")
        return true
    end
    return false
end

function IDLBuildingTrapIceStorm:doAttack()
    ---@type IDLBuildingTrap
    local base = self:getBase(IDLBuildingTrapIceStorm)
    base.doAttack(self)

    if self.isTrigered then
        -- 说明已经触发了
        self.spin.enabled = true
        local toPos = self.transform.position
        toPos.y = 4
        self.tweenPos:flyout(toPos, 2, 0, nil, nil, true)
    end
end

function IDLBuildingTrapIceStorm:fire()
    local pos = self.transform.position
    pos.y = 0
    CLEffect.play(self.attr.AttackEffect, pos)
    self:doFire()
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.onDead), bio2Int(self.attr.DamageKeepTime) / 1000)
end

function IDLBuildingTrapIceStorm:doFire()
    if self.isDead then
        return
    end
    -- 取得目标
    local pos = self.transform.position
    pos.y = 0
    local targets = IDLBattle.searcher.getTargetsInRange(self, pos, self.MaxAttackRange)
    self.targets = targets

    -- 自爆
    -- //TODO:陷阱的配置表时还没有音效
    SoundEx.playSound(self.attr.AttackSound, 1, 2)

    ---@param target IDLUnitBase
    for i, target in ipairs(targets) do
        target:onHurt(self:getDamage(target), self)
        target:frozen(bio2Int(self.attr.DamageKeepTime) / 1000)
    end
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doFire), bio2number(self.attr.AttackSpeedMS) / 1000)
end

function IDLBuildingTrapIceStorm:clean()
    self:getBase(IDLBuildingTrapIceStorm).clean(self)
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.onDead))
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.doFire))
    self.spin.enabled = false
    self.tweenPos:stop()
end

--------------------------------------------
return IDLBuildingTrapIceStorm
