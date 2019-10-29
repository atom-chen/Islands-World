---@public 联盟港口
require("city.IDLBuildingDefense")

---@class IDLBuildingAlliance:IDLBuildingDefense
IDLBuildingAlliance = class("IDLBuildingAlliance", IDLBuildingDefense)

---@param selfObj MyUnit
function IDLBuildingAlliance:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingAlliance).init(self, selfObj, id, star, lev, _isOffense, other)
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
end

---@public 显示攻击范围
function IDLBuildingAlliance:showAttackRang()
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
end

function IDLBuildingAlliance:doAttack()
    if GameMode.battle ~= MyCfg.mode or self.isDead or self.isTrigered then
        return
    end
    self:doSearchTarget()
    if self.target then
        self.isTrigered = true
        self:fire()
    else
        InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doAttack), bio2number(self.attr.AttackSpeedMS) / 1000)
    end
end

function IDLBuildingAlliance:fire()
    --//TODO:聪明港口可以出动联盟的兵了
end

function IDLBuildingAlliance:clean()
    self:getBase(IDLBuildingAlliance).clean(self)
    self:hideAttackRang()
end

--------------------------------------------
return IDLBuildingAlliance
