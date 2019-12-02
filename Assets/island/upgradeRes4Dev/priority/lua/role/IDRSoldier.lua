require("role.IDRoleBase")
---@class IDRSoldier:IDRoleBase 士兵
IDRSoldier = class("IDRSoldier", IDRoleBase)

---@param selfObj MyUnit
function IDRSoldier:__init(selfObj, other)
    if IDRSoldier.super.__init(self, selfObj, other) then
        ---@type Coolape.MyTween
        self.tween = self.gameObject:GetComponent("MyTween")
    end
    return false
end
function IDRSoldier:init(selfObj, id, star, lev, _isOffense, other)
    IDRSoldier.super.init(self, selfObj, id, star, lev, _isOffense, other)
    self.isSeekUseRay = false
    self.seeker.endReachedDistance = self.MinAttackRange
    self.seeker.mAStarPathSearch = IDMainCity.astar4Tile
    self.seeker.mAStarPathSearch:addGridStateChgCallback(self:wrapFunction4CS(self.onAstarChgCallback))
    self.grid = IDMainCity.grid.grid
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

function IDRSoldier:onSearchPath(pathList, canReach)
    IDRSoldier.super.onSearchPath(self, pathList, canReach)
    self.currIndex = self.grid:GetCellIndex(self.transform.position)
    self.oldIndex = self.currIndex
    self:runOrSwim()
end

function IDRSoldier:onMoving()
    IDRSoldier.super.onMoving(self)

    self.currIndex = self.grid:GetCellIndex(self.transform.position)
    local tmpPos = self.transform.position
    tmpPos.y = IDMainCity.getPosOffset(self.currIndex).y
    self.transform.position = tmpPos
    self:repositionShadow()

    if self.currIndex ~= self.oldIndex then
        self:runOrSwim()
        self.oldIndex = self.currIndex
    end
end

---@public 刷新影子的位置
function IDRSoldier:repositionShadow()
    if self.shadow then
        self.tmpPos = self.transform.position
        self.shadow.position = self.tmpPos
    end
end

---@public 走还是游泳
function IDRSoldier:runOrSwim()
    if IDMainCity.isOnTheLandOrBeach(self.currIndex) then
        self:playAction("run")
    else
        --//TODO:在水里，可以游泳，但是只是在水里一会，超过时间就会死掉
    end
end

function IDRSoldier:onArrived()
    IDRSoldier.super.onArrived(self)
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

---@public 跳过去
function IDRSoldier:jumpTo(pos, callback)
    self.tween:flyout(pos, 0.8, 1, nil, callback, true)
end

return IDRSoldier
