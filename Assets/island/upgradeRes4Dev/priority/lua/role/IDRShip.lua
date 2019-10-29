require("role.IDRoleBase")
---@class IDRShip:IDRoleBase 航船
IDRShip = class("IDRShip", IDRoleBase)

function IDRShip:__init(selfObj, other)
    if self:getBase(IDRShip).__init(self, selfObj, other) then
        self.tmpPos = nil
        return true
    end
    return false
end

function IDRShip:init(selfObj, id, star, lev, _isOffense, other)
    self:getBase(IDRShip).init(self, selfObj, id, star, lev, _isOffense, other)
    self:chgState(RoleState.idel)
    if not self.attr.IsFlying then
        self:showTrail() --//TODO:拖尾还需要优化，因为有gc，为卡顿，后续再想办法
    end
end

function IDRShip:onArrived()
    self:getBase(IDRShip).onArrived(self)
    if self.state == RoleState.walkAround then
        self.csSelf:invoke4Lua(self.dogoAround, NumEx.NextInt(20, 60) / 10)
    elseif self.state == RoleState.backDockyard then
        self.dockyard:onShipBack(self)
    end
end

---@public 显示拖尾
function IDRShip:showTrail()
    if self.trail == nil then
        CLThingsPool.borrowObjAsyn(
            "shipTrail",
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
            end
        )
    end
end

function IDRShip:chgState(state)
    if self.state == RoleState.walkAround then
        self.csSelf:cancelInvoke4Lua(self.dogoAround)
        self.csSelf:cancelInvoke4Lua(self.backtoDockyard)
    end
    ---@type RoleState
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
        if self.attr.IsFlying then
            self.seeker:seek(toPos)
        else
            self.seeker:seekAsyn(toPos)
        end
    end
end

function IDRShip:dogoAround()
    local index = NumEx.NextInt(0, IDMainCity.grid.grid.NumberOfCells)
    local toPos = IDMainCity.grid.grid:GetCellCenter(index)
    if self.attr.IsFlying then
        self.seeker:seek(toPos)
    else
        self.seeker:seekAsyn(toPos)
    end
end

function IDRShip:clean()
    self.csSelf:cancelInvoke4Lua()
    self.dockyard = nil
    self:getBase(IDRShip).clean(self)
    if self.trail then
        CLThingsPool.returnObj(self.trail.gameObject)
        SetActive(self.trail.gameObject, false)
        self.trail = nil
    end
end
--------------------------------------------
return IDRShip
