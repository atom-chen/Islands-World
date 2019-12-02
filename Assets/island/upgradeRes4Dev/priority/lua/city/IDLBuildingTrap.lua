---@public 陷阱建筑
require("city.IDLBuildingDefense")

---@class IDLBuildingTrap:IDLBuildingDefense
IDLBuildingTrap = class("IDLBuildingTrap", IDLBuildingDefense)

---@param selfObj MyUnit
function IDLBuildingTrap:init(selfObj, id, star, lev, _isOffense, other)
    self.isTrap = true
    if self.bodyRotate == nil then
        -- 先把self.bodyRotate设值，在调用父类的方法时就不会有再设值了
        self.bodyRotate = selfObj.mbody:GetComponent("TweenRotation")
    end
    -- 通过这种模式把self传过去，不能 self.super:init()
    IDLBuildingTrap.super.init(self, selfObj, id, star, lev, _isOffense, other)
    -- 是否已经触发了
    self.isTrigered = false

    local lev = self.serverData and bio2number(self.serverData.lev) or 1
    -- 触发半径
    self.TriggerRadius =
        DBCfg.getGrowingVal(
        bio2number(self.attr.TriggerRadiusMin),
        bio2number(self.attr.TriggerRadiusMax),
        bio2number(self.attr.TriggerRadiusCurve),
        lev / bio2number(self.attr.MaxLev)
    )
    self.TriggerRadius = self.TriggerRadius / 100

    if MyCfg.mode == GameMode.battle then
        self:hide()
    else
        self:show()
    end
end

function IDLBuildingTrap:idel()
    self.csSelf:cancelInvoke4Lua(self.idel)
    if self.bodyRotate == nil then
        return
    end
    self.bodyRotate.from = self.bodyRotate.transform.localEulerAngles
    self.bodyRotate.to = Vector3(0, NumEx.NextInt(0, 360), 0)
    self.bodyRotate.duration = NumEx.NextInt(3, 8) / 5
    self.bodyRotate:ResetToBeginning()
    self.bodyRotate:Play(true)
    self.csSelf:invoke4Lua(self.idel, NumEx.NextInt(25, 50) / 10)
end

function IDLBuildingTrap:show()
    SetActive(self.body.gameObject, true)
    self:loadShadow()
end

---@public 把自己隐藏起来
function IDLBuildingTrap:hide()
    SetActive(self.body.gameObject, false)
    self:hideAttackRang()
    self:hideShadow()
end

---@public 显示攻击范围
function IDLBuildingTrap:showAttackRang()
    if MyCfg.mode == GameMode.battle then
        -- 战斗中不能显示陷阱的范围，不然就穿邦了
        return
    end

    -- 触发半径
    local TriggerRadius = self.TriggerRadius
    if TriggerRadius > 0 then
        if self.attackMinRangObj == nil then
            self:loadRang(
                Color.blue,
                TriggerRadius,
                function(rangObj)
                    if self.attackMinRangObj then
                        CLUIOtherObjPool.returnObj(rangObj)
                        SetActive(rangObj, false)
                    else
                        self.attackMinRangObj = rangObj
                    end
                end
            )
        else
            SetActive(self.attackMinRangObj.gameObject, true)
        end
    end

    local MaxAttackRange = self.MaxAttackRange
    -- 最远攻击范围
    if MaxAttackRange > 0 then
        if self.attackMaxRangObj == nil then
            self:loadRang(
                Color.white,
                MaxAttackRange,
                function(rangObj)
                    if self.attackMaxRangObj then
                        CLUIOtherObjPool.returnObj(rangObj)
                        SetActive(rangObj, false)
                    else
                        self.attackMaxRangObj = rangObj
                    end
                end
            )
        else
            SetActive(self.attackMaxRangObj.gameObject, true)
        end
    end
end

function IDLBuildingTrap:doAttack()
    if GameMode.battle ~= MyCfg.mode or self.isDead or self.isTrigered then
        return
    end
    self:doSearchTarget()
    if self.target then
        self.isTrigered = true
        self:show()
        -- 显示一会再爆
        InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.fire), bio2number(self.attr.AttackSpeedMS) / 1000)
    else
        InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)
    end
end

function IDLBuildingTrap:fire()
    -- 取得目标
    local targets = IDLBattle.searcher.getTargetsInRange(self, self.transform.position, self.MaxAttackRange)
    self.targets = targets

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

function IDLBuildingTrap:iamDie()
    SetActive(self.gameObject, false)
    self:hide()
    IDLBattle.someOneDead(self)
end

function IDLBuildingTrap:clean()
    IDLBuildingTrap.super.clean(self)
    self:hideAttackRang()
end

--------------------------------------------
return IDLBuildingTrap
