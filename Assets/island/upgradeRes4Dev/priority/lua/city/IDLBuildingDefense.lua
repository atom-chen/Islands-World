---@public 防御建筑
require("city.IDLBuilding")

---@class IDLBuildingDefense
IDLBuildingDefense = class("IDLBuildingDefense", IDLBuilding)

function IDLBuildingDefense:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingDefense).init(self, selfObj, id, star, lev, _isOffense, other)
    if self.bodyRotate == nil then
        self.bodyRotate = getCC(self.csSelf.mbody, "pao/pao_sz", "TweenRotation")
    end
    if GameMode.city == MyCfg.mode then
        self:idel()
    end
end

function IDLBuildingDefense:idel()
    self.csSelf:cancelInvoke4Lua(self.idel)
    if self.bodyRotate == nil then
        return
    end
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
    local MinAttackRange = bio2number(self.attr.MinAttackRange) / 100
    if MinAttackRange > 0 then
        if self.attackMinRang == nil then
            CLUIOtherObjPool.borrowObjAsyn("Rang",
                    function(name, obj, orgs)
                        if (not self.OnPressed) or (not self.gameObject.activeInHierarchy) or self.attackMinRang ~= nil then
                            CLUIOtherObjPool.returnObj(obj)
                            return
                        end
                        self.attackMinRang = obj:GetComponent("CLCellLua")
                        SetActive(obj, true)
                        self.attackMinRang.transform.parent = self.transform
                        self.attackMinRang.transform.position = self.transform.position + IDMainCity.offset4Building
                        self.attackMinRang:init(nil, nil)
                        self.attackMinRang.luaTable.showRang(Color.red, MinAttackRange * 2)
                    end)
        else
            self.attackMinRang.luaTable.showRang(Color.red, MinAttackRange * 2)
        end
    end
    local lev = self.serverData and bio2number(self.serverData.lev) or 1
    local MaxAttackRange = DBCfg.getGrowingVal(bio2number(self.attr.AttackRangeMin), bio2number(self.attr.AttackRangeMax), bio2number(self.attr.AttackRangeCurve), lev / bio2number(self.attr.MaxLev))
    MaxAttackRange = MaxAttackRange / 100
    -- 最远攻击范围
    if MaxAttackRange > 0 then
        if self.attackMaxRang == nil then
            CLUIOtherObjPool.borrowObjAsyn("Rang",
                    function(name, obj, orgs)
                        if (not self.OnPressed) or (not self.gameObject.activeInHierarchy) or self.attackMaxRang ~= nil then
                            CLUIOtherObjPool.returnObj(obj)
                            return
                        end
                        self.attackMaxRang = obj:GetComponent("CLCellLua")
                        self.attackMaxRang.transform.parent = self.transform
                        self.attackMaxRang.transform.position = self.transform.position
                        SetActive(obj, true)
                        self.attackMaxRang:init(nil, nil)
                        self.attackMaxRang.luaTable.showRang(Color.white, MaxAttackRange * 2)
                    end)
        else
            self.attackMaxRang.luaTable.showRang(Color.white, MaxAttackRange * 2)
        end
    end
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

function IDLBuildingDefense:clean()
    self.csSelf:cancelInvoke4Lua()
    self:getBase(IDLBuildingDefense).clean(self)
    self:hideAttackRang()
end

--------------------------------------------
return IDLBuildingDefense
