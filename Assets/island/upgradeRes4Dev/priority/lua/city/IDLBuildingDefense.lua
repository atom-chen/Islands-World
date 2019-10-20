---@public 防御建筑
require("city.IDLBuilding")

---@class IDLBuildingDefense:IDLBuilding
IDLBuildingDefense = class("IDLBuildingDefense", IDLBuilding)
local abs = math.abs
function IDLBuildingDefense:__init(selfObj, other)
    if self.isFinishInited then
        return
    end
    self:getBase(IDLBuildingDefense).__init(self, selfObj, other)
    ---@type TweenRotation
    self.bodyRotate = getCC(self.csSelf.mbody, "pao/pao_sz", "TweenRotation")
    ---@type Coolape.CLEjector 发射器
    self.ejector = getCC(self.csSelf.mbody, "pao/pao_sz/cannon", "CLEjector")
end

function IDLBuildingDefense:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingDefense).init(self, selfObj, id, star, lev, _isOffense, other)
    ---@type IDRoleBase 攻击目标
    self.target = nil

    -- 最远攻击距离
    local lev = self.serverData and bio2number(self.serverData.lev) or 1
    self.MaxAttackRange =
        DBCfg.getGrowingVal(
        bio2number(self.attr.AttackRangeMin),
        bio2number(self.attr.AttackRangeMax),
        bio2number(self.attr.AttackRangeCurve),
        lev / bio2number(self.attr.MaxLev)
    )
    self.MaxAttackRange = self.MaxAttackRange / 100
    -- 最近攻击距离
    self.MinAttackRange = bio2number(self.attr.MinAttackRange) / 100
    --  子弹
    local Bullets = bio2number(self.attr.Bullets)
    if Bullets > 0 then
        ---@type DBCFBulletData
        self.bulletAttr = DBCfg.getBulletByID(Bullets)
    end

    if GameModeSub.city == IDWorldMap.mode then
        self:idel()
    end
end

function IDLBuildingDefense:idel()
    self.csSelf:cancelInvoke4Lua(self.idel)
    if self.bodyRotate == nil then
        return
    end
    self.bodyRotate.duration = 0.5
    self.bodyRotate.from = self.bodyRotate.transform.localEulerAngles
    self.bodyRotate.to = Vector3(0, 0, NumEx.NextInt(0, 360))
    self.bodyRotate.duration = NumEx.NextInt(3, 8) / 5
    self.bodyRotate:ResetToBeginning()
    self.bodyRotate:Play(true)
    self.csSelf:invoke4Lua(self.idel, NumEx.NextInt(25, 50) / 10)
end

function IDLBuildingDefense:OnPress(go, isPress)
    self:getBase(IDLBuildingDefense).OnPress(self, go, isPress)

    self.OnPressed = isPress
    if isPress then
        self:showAttackRang()
    else
        self:hideAttackRang()
    end
end

---@public 显示攻击范围
function IDLBuildingDefense:showAttackRang()
    -- 最小攻击范围
    local MinAttackRange = self.MinAttackRange
    if MinAttackRange > 0 then
        if self.attackMinRang == nil then
            self:loadRang(
                Color.red,
                MinAttackRange,
                function(rangObj)
                    self.attackMinRang = rangObj
                end
            )
        else
            SetActive(self.attackMinRang.gameObject, true)
        end
    end
    -- 最远攻击范围
    local MaxAttackRange = self.MaxAttackRange
    if MaxAttackRange > 0 then
        if self.attackMaxRang == nil then
            self:loadRang(
                Color.white,
                MaxAttackRange,
                function(rangObj)
                    self.attackMaxRang = rangObj
                end
            )
        else
            SetActive(self.attackMaxRang.gameObject, true)
        end
    end
end

---@public 加载范围圈
function IDLBuildingDefense:loadRang(color, r, callback)
    CLUIOtherObjPool.borrowObjAsyn(
        "Rang",
        function(name, obj, orgs)
            if (not self.OnPressed) or (not self.gameObject.activeInHierarchy) then
                CLUIOtherObjPool.returnObj(obj)
                return
            end
            local rangObj = obj:GetComponent("CLCellLua")
            rangObj.transform.parent = self.transform
            rangObj.transform.position = self.transform.position
            SetActive(obj, true)
            rangObj:init(nil, nil)
            rangObj.luaTable.showRang(color, r * 2)
            if callback then
                callback(rangObj)
            end
        end
    )
end

function IDLBuildingDefense:hideAttackRang()
    if self.attackMaxRang then
        CLUIOtherObjPool.returnObj(self.attackMaxRang.gameObject)
        SetActive(self.attackMaxRang.gameObject, false)
        self.attackMaxRang = nil
    end
    if self.attackMinRang then
        CLUIOtherObjPool.returnObj(self.attackMinRang.gameObject)
        SetActive(self.attackMinRang.gameObject, false)
        self.attackMinRang = nil
    end
end

function IDLBuildingDefense:SetActive(active)
    self:getBase(IDLBuildingDefense).SetActive(self, active)
    if active then
        self:idel()
    end
end

function IDLBuildingDefense:begainAttack()
    self.csSelf:cancelInvoke4Lua(self.idel)
    self:doAttack()
end

function IDLBuildingDefense:doAttack()
    if GameMode.battle ~= MyCfg.mode or self.isDead then
        return
    end
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
        local dir = self.target.transform.position - self.transform.position
        -- 炮面向目标
        self:lookatTarget(self.target, false, self:wrapFunc(self.fire))
    end
end

---@public 设置攻击目标
---@param target IDRoleBase
function IDLBuildingDefense:setTarget(target)
    if self.target == target then
        return
    end
    self.target = target
    if target then
        self.csSelf.mTarget = target.csSelf
    else
        self.csSelf.mTarget = nil
    end
end

---@public 开炮
---@param target IDRoleBase
function IDLBuildingDefense:fire()
    local target = self.target
    local dir = target.transform.position - self.ejector.transform.position
    SetActive(self.ejector.gameObject, true)
    self.ejector:fire(self.csSelf, self.target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
end

---@public
---@param bullet Coolape.CLBulletBase
function IDLBuildingDefense:onBulletHit(bullet)
    IDLBattle.onBulletHit(bullet)
end

---@public 取得伤害值
---@param target IDRoleBase
function IDLBuildingDefense:getDamage(target)
    if target == nil then
        return 0
    end
    local damage = bio2number(self.data.damage)
    local gid = bio2number(target.attr.GID)
    if bio2number(self.attr.PreferedTargetType) == gid then
        -- 优先攻击目标的伤害加成
        damage = damage * bio2number(self.attr.PreferedTargetDamageMod)
    end
    return math.floor(damage)
end

---@public 炮口面向目标
---@param target IDRoleBase
function IDLBuildingDefense:lookatTarget(target, imm, callback)
    self.bodyRotate.enabled = false
    local toAngel = Utl.getAngle(self.transform.position, target.transform.position)
    if imm then
        self.bodyRotate.transform.localEulerAngles = Vector3(0, 0, toAngel.y)
    else
        self.bodyRotate.duration = 0.1
        self.bodyRotate.from = self.bodyRotate.transform.localEulerAngles
        self.bodyRotate.to = Vector3(0, 0, toAngel.y)
        if abs(self.bodyRotate.from.z - self.bodyRotate.to.z) < 0.01 then
            if callback then
                Utl.doCallback(callback)
            end
        else
            self.bodyRotate:ResetToBeginning()
            self.bodyRotate:Play(true)
            if callback then
                self.csSelf:invoke4Lua(callback, 0.1)
            end
        end
    end
end

function IDLBuildingDefense:clean()
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.doAttack))
    self:setTarget(nil)
    self.csSelf:cancelInvoke4Lua()
    self:getBase(IDLBuildingDefense).clean(self)
    self:hideAttackRang()
end

--------------------------------------------
return IDLBuildingDefense
