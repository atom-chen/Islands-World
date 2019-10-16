require("role.IDRoleBase")
-- 航船
---@class IDRShip:IDRoleBase
IDRShip = class("IDRShip", IDRoleBase)

function IDRShip:init(selfObj, id, star, lev, _isOffense, other)
    self:getBase(IDRShip).init(self, selfObj, id, star, lev, _isOffense, other)
    self:chgState(RoleState.idel)

    self.flyHeigh = bio2number(self.attr.FlyHeigh)/10
    if self.attr.IsFlying then
        if self.seeker then
            self.seeker.rayHeight = self.flyHeigh
        end
    else
        if self.seeker then
            self.seeker.speed = bio2number(self.attr.MoveSpeed) / 100
            self.seeker.mAStarPathSearch = IDMainCity.astar4Ocean
            self.seeker.mAStarPathSearch:addGridStateChgCallback(self:wrapFunction4CS(self.onAstarChgCallback))
            self.seeker:init(self:wrapFunction4CS(self.onSearchPath), self:wrapFunction4CS(self.onMoving), self:wrapFunction4CS(self.onArrived))
        end
        self:showTrail() --先去掉，因为有gc，为卡顿，后续再想办法
    end
end

function IDRShip:__init(selfObj, other)
    self:getBase(IDRShip).__init(self, selfObj, other)
    ---@type Coolape.CLSeekerByRay
    self.seeker = self.csSelf:GetComponent("CLSeekerByRay")
    if self.seeker == nil then
        self.seeker = self.csSelf:GetComponent("CLSeeker")
    end
    self.tmpPos = nil
end

function IDRShip:onSearchPath(pathList, canReach)

end

function IDRShip:onMoving()
    if self.shadow then
        self.tmpPos = self.transform.position
        self.tmpPos.y = 0
        self.shadow.position = self.tmpPos
    end
    local pos = self.transform.position
    pos.y = self.flyHeigh
    self.transform.position = pos
end

function IDRShip:onArrived()
    if self.state == RoleState.walkAround then
        self.csSelf:invoke4Lua(self.dogoAround, NumEx.NextInt(20, 60) / 10)
    elseif self.state == RoleState.backDockyard then
        self.dockyard:onShipBack(self)
    end
end

---@public 显示拖尾
function IDRShip:showTrail()
    if self.trail == nil then
        CLThingsPool.borrowObjAsyn("shipTrail",
                function(name, obj, orgs)
                    if (not self.gameObject.activeInHierarchy) or self.trail then
                        CLThingsPool.returnObj(obj)
                        return
                    end
                    self.trail = obj:GetComponent(typeof(ShipTrail))
                    self.trail.transform.parent = self.transform
                    self.trail.transform.localPosition = Vector3(0, 0, 0.7)
                    self.trail.transform.localScale = Vector3.one
                    self.trail.transform.localEulerAngles = Vector3.zero
                    self.trail.speed = bio2number(self.attr.MoveSpeed) / 100
                    self.trail:resetPosition()
                    SetActive(self.trail.gameObject, true)
                end)
    end
end

function IDRShip:chgState(state)
    if self.state == RoleState.walkAround then
        self.csSelf:cancelInvoke4Lua(self.dogoAround)
        self.csSelf:cancelInvoke4Lua(self.backtoDockyard)
    end
    self.state = state
end

---@public 四处转转
function IDRShip:goAround(dockyard)
    self.dockyard = dockyard
    self:chgState(RoleState.walkAround)
    self:dogoAround()
    self.csSelf:invoke4Lua(self.backtoDockyard, NumEx.NextInt(600, 1800) / 10)
end

function IDRShip:backtoDockyard()
    self:chgState(RoleState.backDockyard)
    if self.dockyard then
        local toPos = self.dockyard.door.position
        self.seeker:seekAsyn(toPos)
    end
end

function IDRShip:dogoAround()
    local index = NumEx.NextInt(0, IDMainCity.grid.grid.NumberOfCells)
    local toPos = IDMainCity.grid.grid:GetCellCenter(index)
    self.seeker:seekAsyn(toPos)
end

function IDRShip:clean()
    self.csSelf:cancelInvoke4Lua()
    self.dockyard = nil
    self.seeker:stopMove()
    self:getBase(IDRShip).clean(self)
    if self.trail then
        CLThingsPool.returnObj(self.trail.gameObject)
        SetActive(self.trail.gameObject, false)
        self.trail = nil
    end
end
--------------------------------------------
return IDRShip
