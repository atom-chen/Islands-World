---@public 陷阱建筑
require("city.IDLBuildingDefense")

---@class IDLBuildingTrap
IDLBuildingTrap = class("IDLBuildingTrap", IDLBuildingDefense)

function IDLBuildingTrap:init(selfObj, id, star, lev, _isOffense, other)
    self.isTrap = true
    if self.bodyRotate == nil then
        -- 先把self.bodyRotate设值，在调用父类的方法时就不会有再设值了
        self.bodyRotate = selfObj.mbody:GetComponent("TweenRotation")
    end
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingTrap).init(self, selfObj, id, star, lev, _isOffense, other)
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

function IDLBuildingTrap:clean()
    self:getBase(IDLBuildingTrap).clean(self)
end

--------------------------------------------
return IDLBuildingTrap
