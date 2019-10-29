require("role.IDRoleBase")
-- 工人
---@class IDRWorker:IDRoleBase
IDRWorker = class("IDRWorker", IDRoleBase)

function IDRWorker:__init(selfObj, other)
    return self:getBase(IDRWorker).__init(self, selfObj, other)
end

-- 初始化，只调用一次
function IDRWorker:init (csObj, id, star, lev, isOffense, other)
    self:getBase(IDRWorker).init(self, csObj, id, star, lev, isOffense, other)
    self.grid = IDMainCity.grid.grid

    -- 设置寻路
    --seeker.mAStarPathSearch = IDMainCity.astar4Worker
    --seeker.mAStarPathSearch:addGridStateChgCallback(_cell.onAstarChgCallback)
    self.state = IDConst.RoleState.idel
    local _building = other.building
    if _building then
        ---@type IDDBBuilding
        local bdata = _building.serverData
        if bio2number(bdata.state) == IDConst.BuildingState.upgrade then
            if DateEx.nowMS - bio2number(bdata.starttime) > 5 * 1000 then
                local toPos = AngleEx.getCirclePointV3(_building.transform.position,
                        _building.size / 2 + 0.25,
                        NumEx.NextInt(0, 360))
                self.transform.position = toPos
            end
        end
    end
end

function IDRWorker:idel()
    self:playAction("idel")
    local val = NumEx.NextInt(0, 50) / 100
    self.action.animator:SetFloat("SubAction", val)
end

function IDRWorker:run()
    if NumEx.NextBool() then
        self:playAction("walk")
    else
        self:playAction("run")
    end
end

---@public 当A星网格刷新的回调
function IDRWorker:onAstarChgCallback()
    if (not self.gameObject.activeInHierarchy) then
        return
    end
    if self.state == IDConst.RoleState.working then
        self:gotoWork()
    elseif self.state == IDConst.RoleState.idel then
        self:goHome()
    end
end

---@public 当寻路完成后的回调
function IDRWorker:onSearchPath(pathList, canReach)
    self.currIndex = self.grid:GetCellIndex(self.transform.position)
    self.oldIndex = self.currIndex
    self:showShip(self.currIndex)
end

function IDRWorker:onMoving()
    self.currIndex = self.grid:GetCellIndex(self.transform.position)
    local v = self.transform.position
    v.y = IDMainCity.getPosOffset(self.currIndex).y
    self.transform.position = v
    if self.shadow then
        self.shadow.transform.position = v
    end
    if self.currIndex ~= self.oldIndex then
        self:showShip(self.currIndex)
        self.oldIndex = self.currIndex
    end
end

function IDRWorker:onArrived()
    self:idel()
    if self.state == IDConst.RoleState.working then
        if Vector3.Distance(self.transform.position, self.target.transform.position) > (self.target.size / 2 + 0.5) then
            -- 说明目标位置被移动
            self:gotoWork()
        else
            self:working()
        end
    elseif self.state == IDConst.RoleState.idel then
        -- 说明是回家，判断家是不是已经搬走了
        if Vector3.Distance(self.transform.position, IDMainCity.Headquarters.door.position) > 0.5 then
            self:goHome()
        else
            self:onArrivedHome()
        end
    end
end

---@public 显示船
function IDRWorker:showShip(index)
    if IDMainCity.isOnTheLandOrBeach(index) then
        -- 在陆地上
        self:run()
        self:releaseShip()
    else
        -- 说明是在海上
        self:loadShip()
        self:idel()
    end
end

function IDRWorker:loadShip()
    if self.ship == nil then
        CLThingsPool.borrowObjAsyn("portShip",
                function(name, obj, orgs)
                    local index = self.grid:GetCellIndex(self.transform.position)
                    if (not self.gameObject.activeInHierarchy) or
                            IDMainCity.isOnTheLand(index, 1) or
                            self.ship ~= nil
                    then
                        CLThingsPool.returnObj(obj)
                        SetActive(obj, false)
                        return
                    end
                    self.ship = obj
                    self.ship.transform.parent = self.transform
                    self.ship.transform.localScale = Vector3.one
                    self.ship.transform.localPosition = Vector3.zero
                    self.ship.transform.localEulerAngles = Vector3.zero
                    SetActive(self.ship, true)
                end)
    else
        SetActive(self.ship, true)
    end
end

function IDRWorker:releaseShip()
    if self.ship then
        CLThingsPool.returnObj(self.ship)
        SetActive(self.ship, false)
    end
    self.ship = nil
end

-- 去工作
function IDRWorker:gotoWork (building)
    if self.shadow then
        SetActive(self.shadow.gameObject, true)
    end
    self.target = building or self.target
    self.state = IDConst.RoleState.working
    local toPos = AngleEx.getCirclePointV3(self.target.transform.position,
            self.target.size / 2 + 0.25,
            NumEx.NextInt(0, 360))
    self.seeker:seek(toPos)
end

function IDRWorker:working()
    Utl.RotateTowards(self.transform, self.transform.position, self.target.transform.position)
    self:playAction("attack")
    self.csSelf:invoke4Lua(self.gotoWork, self.target, NumEx.NextInt(30, 80) / 10)
end

-- 结束工作
function IDRWorker:finishWork()
    self.state = IDConst.RoleState.idel
    self.csSelf:cancelInvoke4Lua(self.gotoWork)
    self.target = nil
    self:goHome()
end

function IDRWorker:goHome()
    self.seeker:seek(IDMainCity.Headquarters.door.position)
end

function IDRWorker:onArrivedHome()
    self:releaseShip()
    if self.shadow then
        SetActive(self.shadow.gameObject, false)
    end
    SetActive(self.gameObject, false)
end

function IDRWorker:clean()
    self:getBase(IDRWorker).clean(self)
    self:releaseShip()
    --self.seeker.mAStarPathSearch:removeGridStateChgCallback(self.onAstarChgCallback)
end
--------------------------------------------
return IDRWorker

