---@public 陷阱建筑
require("city.IDLBuildingDefense")

---@class IDLBuildingTrap:IDLBuildingDefense
IDLBuildingTrap = class("IDLBuildingTrap", IDLBuildingDefense)

function IDLBuildingTrap:init(selfObj, id, star, lev, _isOffense, other)
    self.isTrap = true
    if self.bodyRotate == nil then
        -- 先把self.bodyRotate设值，在调用父类的方法时就不会有再设值了
        self.bodyRotate = selfObj.mbody:GetComponent("TweenRotation")
    end
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingTrap).init(self, selfObj, id, star, lev, _isOffense, other)
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
    SetActive(self.csSelf.mbody.gameObject, true)
    self:loadShadow()
end

---@public 把自己隐藏起来
function IDLBuildingTrap:hide()
    SetActive(self.csSelf.mbody.gameObject, false)
    self:hideAttackRang()
    self:hideShadow()
end

---@public 显示攻击范围
function IDLBuildingTrap:showAttackRang()
    -- 触发半径
    local lev = self.serverData and bio2number(self.serverData.lev) or 1
    local TriggerRadius = DBCfg.getGrowingVal(
        bio2number(self.attr.TriggerRadiusMin), 
        bio2number(self.attr.TriggerRadiusMax), 
        bio2number(self.attr.TriggerRadiusCurve), 
        lev / bio2number(self.attr.MaxLev))
        TriggerRadius = TriggerRadius/100

    if TriggerRadius > 0 then
        if self.attackMinRang == nil then
            self:loadRang(Color.blue, TriggerRadius, function(rangObj)
                self.attackMinRang = rangObj
            end)
        else
            SetActive(self.attackMinRang.gameObject, true)
        end
    end

    local lev = self.serverData and bio2number(self.serverData.lev) or 1
    local MaxAttackRange = DBCfg.getGrowingVal(bio2number(self.attr.AttackRangeMin), bio2number(self.attr.AttackRangeMax), bio2number(self.attr.AttackRangeCurve), lev / bio2number(self.attr.MaxLev))
    MaxAttackRange = MaxAttackRange / 100
    -- 最远攻击范围
    if MaxAttackRange > 0 then
        if self.attackMaxRang == nil then
            self:loadRang(Color.white, MaxAttackRange, function(rangObj)
                self.attackMaxRang = rangObj
            end)
        else
            SetActive(self.attackMaxRang.gameObject, true)
        end
    end
end

function IDLBuildingTrap:hideAttackRang()
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

function IDLBuildingTrap:clean()
    self:getBase(IDLBuildingTrap).clean(self)
    self:hideAttackRang()
end

--------------------------------------------
return IDLBuildingTrap
