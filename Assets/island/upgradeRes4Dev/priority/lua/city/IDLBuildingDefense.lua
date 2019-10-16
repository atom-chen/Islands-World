---@public 防御建筑
require("city.IDLBuilding")

---@class IDLBuildingDefense:IDLBuilding
IDLBuildingDefense = class("IDLBuildingDefense", IDLBuilding)

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
    if self.target == nil or self.target.idDead then
        -- 重新寻敌
        self.target = IDLBattle.searchTarget(self)
    else
        local dis = Vector3.Distance(self.transform.position, self.target.transform.position)
        if dis > self.MaxAttackRange or dis < self.MinAttackRange then
            -- 重新寻敌
            self.target = IDLBattle.searchTarget(self)
        end
    end
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)

    if self.target then
        self.csSelf.mTarget = self.target.csSelf
        local dir = self.target.transform.position - self.transform.position
        -- 炮面向目标
        self:lookatTarget(self.target)
        SoundEx.playSound(self.attr.AttackSound, 1, 3)
        self:fire(self.target)
    end
end

---@public 开炮
function IDLBuildingDefense:fire(target)
    local dir = target.transform.position - self.transform.position
    SetActive(self.ejector.gameObject, true)
    self.ejector:fire(self.csSelf, self.target.csSelf, self.bulletAttr, nil, self:wrapFunc(self.onBulletHit))
end

---@public
---@param bullet Coolape.CLBulletBase
function IDLBuildingDefense:onBulletHit(bullet)
    ---@type DBCFBulletData
    local bulletAttr = bullet.attr
    CLEffect.play(bulletAttr.HitEffect, bullet.transform.position)
end

-- 炮口面向目标
function IDLBuildingDefense:lookatTarget(target, callback)
    local v1 = Vector2(self.bodyRotate.transform.position.x, self.bodyRotate.transform.transform.position.z)
    local v2 = Vector2(target.transform.position.x, target.transform.position.z)
    local toAn = MyUtl.calculateAngle(v1, v2)

    self.bodyRotate.duration = 0.2
    self.bodyRotate.from = self.bodyRotate.transform.localEulerAngles
    self.bodyRotate.to = Vector3(0, 0, toAn)
    -- if (needCallback) then
    --     self.bodyRotate.onFinished = onFinishCannonLookAt
    -- else
    --     self.bodyRotate.onFinished = nil
    -- end
    self.bodyRotate:Reset()
    self.bodyRotate:Play(true)
end

function IDLBuildingDefense:clean()
    self.target = nil
    self.csSelf:cancelInvoke4Lua()
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.doAttack))
    self:getBase(IDLBuildingDefense).clean(self)
    self:hideAttackRang()
end

--------------------------------------------
return IDLBuildingDefense
