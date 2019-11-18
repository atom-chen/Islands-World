require("role.IDRoleBase")
---@class IDRShip:IDRoleBase 舰船
IDRShip = class("IDRShip", IDRoleBase)

function IDRShip:__init(selfObj, other)
    if self:getBase(IDRShip).__init(self, selfObj, other) then
        self.tmpPos = nil
        return true
    end
    return false
end

function IDRShip:init(selfObj, id, star, lev, _isOffense, other)
    self.searchBuildingWithBeachTimes = 0 -- 寻找离海岸最近建筑次数
    self.isLanded = false -- 是否已经登陆过了
    self:getBase(IDRShip).init(self, selfObj, id, star, lev, _isOffense, other)
    self:chgState(RoleState.idel)
    if not self.attr.IsFlying then
        self:showTrail() --//TODO:拖尾还需要优化，因为有gc，为卡顿，后续再想办法
    end
end

function IDRShip:onArrived()
    if self.state == RoleState.landing then
        self:landingSoldiers()
    else
        self:getBase(IDRShip).onArrived(self)
        if self.state == RoleState.walkAround then
            self.csSelf:invoke4Lua(self.dogoAround, NumEx.NextInt(20, 60) / 10)
        elseif self.state == RoleState.backDockyard then
            self.dockyard:onShipBack(self)
        end
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
    if MyCfg.mode == GameMode.battle then
        return
    end
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
    if MyCfg.mode == GameMode.battle then
        return
    end
    local index = NumEx.NextInt(0, IDMainCity.grid.grid.NumberOfCells)
    local toPos = IDMainCity.grid.grid:GetCellCenter(index)
    if self.attr.IsFlying then
        self.seeker:seek(toPos)
    else
        self.seeker:seekAsyn(toPos)
    end
end

---@public 当不能寻路到可攻击目标时
function IDRShip:onCannotReach4AttackTarget()
    if self:canLand() then
        self:gotoLandSoldiers()
    else
        self:searchCanReachTarget()
    end
end

---@public 能否登陆
function IDRShip:canLand()
    if bio2number(self.attr.SolderNum) > 0 and (not self.isLanded) and self.state ~= RoleState.landing then
        return true
    end
    return false
end

function IDRShip:gotoLandSoldiers()
    if self.target == nil then
        printe("目标为空，进入这里是不应该的，肯定又bug了，噫？为什么要说又呢？")
        return
    end
    self.state = RoleState.landing
    ---@type UnityEngine.Vector3
    local dir = self.target.transform.position - self.transform.position
    local ray = Ray(self.transform.position, dir)
    local isHit, hitInfor = Physics.Raycast(ray, 1000, LayerMask.NameToLayer("TileSide").value)
    if isHit then
        self.seeker:seekAsyn(hitInfor.point)
    else
        printw("居然没有找到海岸，运气真好！（不过确实存在这种情况）")
        self.seeker:seekAsyn(self.target.transform.position)
    end
end

---@public 登陆士兵
function IDRShip:landingSoldiers()
    self.isLanded = true
    self:chgState(RoleState.idel)
    self:doLandingSoldier({i = 1, max = bio2Int(self.attr.SolderNum)})
end

function IDRShip:doLandingSoldier(param)
    local i, max = param.i, param.max
    ---@type WrapBattleUnitData
    local data = {}
    data.id = 3
    data.lev = number2bio(1) -- //TODO:根据科技来得到等级
    data.num = 1
    data.type = IDConst.UnitType.ship

    CLRolePool.borrowObjAsyn(
        IDUtl.getRolePrefabName(3),
        function(name, ship, orgs)
            local serverData = orgs.serverData
            local pos = orgs.pos
            local isOffense = orgs.isOffense
            ship.transform.parent = IDLBattle.transform
            ship.transform.localScale = Vector3.one
            ship.transform.localEulerAngles = self.transform.localEulerAngles
            if ship.luaTable == nil then
                ---@type IDRSoldier
                ship.luaTable = IDUtl.newRoleLua(serverData.id)
                ship:initGetLuaFunc()
            end
            SetActive(ship.gameObject, true)
            ship:init(serverData.id, 0, 1, true, {serverData = serverData})
            ship.transform.position = pos

            local toPos = self.transform.forward * 1 + self.transform.position
            local hight = 0
            local offsetx = ship:fakeRandom(-10, 10) / 20
            local offsetz = ship:fakeRandom2(-10, 10) / 20
            toPos = Vector3(offsetx + toPos.x, hight, offsetz + toPos.z)
            ship.luaTable:jumpTo(
                toPos,
                function()
                    IDLBattle.someOneJoin(ship.luaTable)
                end
            )
        end,
        {serverData = data, pos = self.transform.position, isOffense = self.isOffense}
    )
    if i == max then
        self:onFinishLandSoldiers()
    else
        param.i = param.i + 1
        InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.doLandingSoldier), param, 0.5)
    end
end

---@public 当完成释放登陆士兵的回调
function IDRShip:onFinishLandSoldiers()
    self:searchCanReachTarget()
end

---@public 寻找可以到达的目标
function IDRShip:searchCanReachTarget()
    self:getBuildingWithBeach()
    if self.target then
        -- 寻路过去
        if self.attr.IsFlying then
            self.seeker:seek(self.target.transform.position)
        else
            self.seeker:seekAsyn(self.target.transform.position)
        end
    end
end

---@public 取得
function IDRShip:getBuildingWithBeach()
    self.searchBuildingWithBeachTimes = self.searchBuildingWithBeachTimes + 1
    local target = IDLBattle.searcher.getNearestBuildingWithBeach(self, self.searchBuildingWithBeach)

    -- 设置目标
    self:setTarget(target)
end

function IDRShip:clean()
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.doLandingSoldier))
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
