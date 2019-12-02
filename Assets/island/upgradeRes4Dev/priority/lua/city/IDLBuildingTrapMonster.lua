---@public 海怪陷阱
require("city.IDLBuildingTrap")

---@class IDLBuildingTrapMonster:IDLBuildingTrap
IDLBuildingTrapMonster = class("IDLBuildingTrapMonster", IDLBuildingTrap)

function IDLBuildingTrapMonster:__init(selfObj)
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapMonster.super
    if base.__init(self, selfObj) then
        -- 墨汁特效
        self.inkEffect = getChild(self.transform, "effect").gameObject
        return true
    end
    return false
end

function IDLBuildingTrapMonster:show()
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapMonster.super
    base.show(self)
    SetActive(self.inkEffect, true)
end

function IDLBuildingTrapMonster:hide()
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapMonster.super
    base.hide(self)
    SetActive(self.inkEffect, false)
end

function IDLBuildingTrapMonster:fire()
    local targets = IDLBattle.searcher.getTargetsInRange(self, self.transform.position, self.MaxAttackRange)
    self.targets = targets
    ---@param target IDLUnitBase
    for i, target in ipairs(targets) do
        -- 先把目标固定住
        target:pause()
    end
    CLThingsPool.borrowObjAsyn(
        self.attr.AttackEffect,
        function(name, go, orgs)
            if self.attackEffectObj then
                CLThingsPool.returnObj(go)
                SetActive(go, false)
                return
            end
            ---@type Coolape.CLCellLua
            self.attackEffectObj = go:GetComponent("CLCellLua")
            self.attackEffectObj.transform.position = self.transform.position
            SetActive(self.attackEffectObj.gameObject, true)
            self.attackEffectObj:init(self, nil)
            self.attackEffectObj.luaTable.play(self:wrapFunc(self.onFinishAttack))
            SoundEx.playSound(self.attr.AttackSound, 1, 3)
        end
    )
end

function IDLBuildingTrapMonster:onFinishAttack()
    self:onDead()
    ---@param target IDLUnitBase
    for i, target in ipairs(self.targets) do
        -- 秒杀所有目标
        target:onHurt(self:getDamage(target), self)
    end
    self:cleanMonsterAttackEffect()
end

function IDLBuildingTrapMonster:cleanMonsterAttackEffect()
    if self.attackEffectObj then
        CLThingsPool.returnObj(self.attackEffectObj.gameObject)
        SetActive(self.attackEffectObj.gameObject, false)
        self.attackEffectObj = nil
    end
end

function IDLBuildingTrapMonster:clean()
    ---@type IDLBuildingTrap
    local base = IDLBuildingTrapMonster.super
    base.clean(self)
    self:cleanMonsterAttackEffect()
end

--------------------------------------------
return IDLBuildingTrapMonster
