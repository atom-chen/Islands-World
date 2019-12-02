require("role.IDRShip")
---@class IDRShipBreaker:IDRShip 自爆
IDRShipBreaker = class("IDRShipBreaker", IDRShip)

function IDRShipBreaker:__init(csSelfObj, other)
    ---@type IDRShip
    local base = IDRShipBreaker.super
    if base.__init(self, csSelfObj, other) then
        ---@type Coolape.MyTween
        self.tweenPos = self.gameObject:GetComponent("MyTween")
        return true
    end
    return false
end

function IDRShipBreaker:dress(mode)
    if self.avata == nil then
        return
    end
    if mode == IDConst.dressMode.ice then
        for i = 1, 6 do
            self.avata:switch2xx(joinStr("body", i), "2", self:wrapFunc(self.onFinishDress))
        end
    else
        for i = 1, 6 do
            self.avata:switch2xx(joinStr("body", i), "1", self:wrapFunc(self.onFinishDress))
        end
    end
end

---@param target IDLUnitBase
function IDRShipBreaker:startFire(target)
    target = target or self.target
    if target == nil or target.isDead then
        self:setTarget(nil)
        self:doAttack()
        return
    end

    self.tweenPos:flyout(target.transform.position, 1.4, 0, nil, self:wrapFunc(self.doBomb), true)
end

---@public 当不能寻路到可攻击目标时
function IDRShipBreaker:onCannotReach4AttackTarget()
    self:onDead()
end

function IDRShipBreaker:doBomb()
    -- 取得目标
    local targets = IDLBattle.searcher.getTargetsInRange(self, self.transform.position, bio2number(self.attr.DamageAffectRang)/100)

    -- 自爆
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
    CLEffect.play(self.attr.AttackEffect, self.transform.position)
    self:onDead()

    ---@param target IDLUnitBase
    for i, target in ipairs(targets) do
        target:onHurt(self:getDamage(target), self)
    end
end

function IDRShipBreaker:clean()
    ---@type IDLBuildingTrap
    local base = IDRShipBreaker.super
    base.clean(self)
    self.tweenPos:stop()
end
return IDRShipBreaker
