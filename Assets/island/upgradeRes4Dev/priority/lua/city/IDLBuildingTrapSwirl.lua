---@public 海怪陷阱
require("city.IDLBuildingTrap")

---@class IDLBuildingTrapSwirl:IDLBuildingTrap
IDLBuildingTrapSwirl = class("IDLBuildingTrapSwirl", IDLBuildingTrap)

function IDLBuildingTrapSwirl:__init(selfObj)
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapSwirl.super
    if base.__init(self, selfObj) then
        ---@type Coolape.MyTween
        self.tweenPos = self.gameObject:GetComponent("MyTween")
        self.spin = self.csSelf:GetComponent("Spin")
        self.spin.enabled = false
        ---@type UnityEngine.Transform
        self.targetsRoot = getChild(self.transform, "targetsRoot")
        return true
    end
    return false
end

function IDLBuildingTrapSwirl:doAttack()
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapSwirl.super
    base.doAttack(self)

    if self.isTrigered then
        -- 说明已经触发了
        self.spin.enabled = true
        local toPos = self.transform.position
        toPos.y = 3
        self.tweenPos:flyout(toPos, 2, 0, nil, nil, true)
    end
end

function IDLBuildingTrapSwirl:fire()
    local pos = self.transform.position
    pos.y = 0
    CLEffect.play(self.attr.AttackEffect, pos)
    self:doFire()

    -- 等待xx秒自爆
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.onDead), bio2Int(self.attr.DamageKeepTime) / 1000)
end

function IDLBuildingTrapSwirl:doFire()
    if self.isDead then
        return
    end
    -- 取得目标
    local pos = self.transform.position
    pos.y = 0
    local targets = IDLBattle.searcher.getTargetsInRange(self, pos, self.MaxAttackRange)
    self.targets = targets
    ---@param target IDLUnitBase
    for i, target in ipairs(targets) do
        target:pause()
        target.transform.parent = self.targetsRoot
    end

    -- //TODO:陷阱的配置表时还没有音效
    SoundEx.playSound(self.attr.AttackSound, 1, 2)

    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doTargetsHurt), targets, bio2number(self.attr.AttackSpeedMS) / 1000)
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doFire), bio2number(self.attr.AttackSpeedMS) / 1000)
end

function IDLBuildingTrapSwirl:doTargetsHurt(targets)
    ---@param target IDLUnitBase
    for i, target in ipairs(targets) do
        target:onHurt(self:getDamage(target), self)
    end
end

function IDLBuildingTrapSwirl:clean()
    IDLBuildingTrapSwirl.super.clean(self)
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.doTargetsHurt))
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.onDead))
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.doFire))
    self.spin.enabled = false
    self.tweenPos:stop()
end

--------------------------------------------
return IDLBuildingTrapSwirl
