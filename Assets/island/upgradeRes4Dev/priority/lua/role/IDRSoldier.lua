require("role.IDRoleBase")
---@class IDRSoldier:IDRoleBase 士兵
IDRSoldier = class("IDRSoldier", IDRoleBase)

function IDRSoldier:init(selfObj, id, star, lev, _isOffense, other)
    self:getBase(IDRSoldier).init(self, selfObj, id, star, lev, _isOffense, other)
    self.isSeekUseRay = false
    self.seeker.endReachedDistance = self.MinAttackRange
    self.seeker.mAStarPathSearch = IDMainCity.astar4Tile
    self.seeker.mAStarPathSearch:addGridStateChgCallback(self:wrapFunction4CS(self.onAstarChgCallback))
end

function IDRSoldier:onSearchPath(pathList, canReach)
    self:getBase(IDRSoldier).onSearchPath(self, pathList, canReach)
    self:playAction("run")
end

function IDRSoldier:searchPath(toPos)
    self.isSeekUseRay = false
    self.seeker:seekAsyn(toPos)
end

function IDRSoldier:onCannotReach4AttackTarget()
    if self.isSeekUseRay or self.target == nil then
        return
    end
    self.isSeekUseRay = true
    local endReachedDistance = self.MaxAttackRange
    if self.target.isBuilding then
        endReachedDistance = endReachedDistance + self.target.size / 2
    end
    self.seeker:seek(self.target.transform.position, endReachedDistance)
end

function IDRSoldier:onMoving()
    self:playAction("run")
    self:getBase(IDRSoldier).onMoving(self)
end

function IDRSoldier:onArrived()
    self:getBase(IDRSoldier).onArrived(self)
    if self.target then
        Utl.RotateTowards(self.transform, self.target.transform.position - self.transform.position)
    end
end

---@param target IDLUnitBase
function IDRSoldier:fire(target)
    self:playAction(
        "attack",
        ActCBtoList(90, self:wrapFunc(self.onFinsihFire), 100, self:wrapFunc(self.onActionFinish))
    )
    CLEffect.play(self.attr.AttackEffect, self.transform.position)
    SoundEx.playSound(self.attr.AttackSound, 1, 3)
end

function IDRSoldier:onFinsihFire(act)
    if self.target then
        local damage = self:getDamage(self.target)
        self.target:onHurt(damage, self)
    end
end

return IDRSoldier
