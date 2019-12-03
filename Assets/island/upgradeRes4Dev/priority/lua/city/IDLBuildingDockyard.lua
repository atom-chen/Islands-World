---@public 造船厂
require("city.IDLBuilding")

---@class IDLBuildingDockyard:IDLBuilding
IDLBuildingDockyard = class("IDLBuildingDockyard", IDLBuilding)

function IDLBuildingDockyard:__init(selfObj, other)
    if IDLBuildingDockyard.super.__init(self, selfObj, other) then
        self.shipsInPorts = {}
        self.shipsInOcean = {}
        self.ports = {}
        for i = 1, 8 do
            -- 取得停泊点，并设置为空闲
            self.ports[i] = {point = getChild(self.transform, joinStr("ports/", i)), isFree = true}
        end
        return true
    end
    return false
end

function IDLBuildingDockyard:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    IDLBuildingDockyard.super.init(self, selfObj, id, star, lev, _isOffense, other)
    self:showShipsInPort()
    if MyCfg.mode ~= GameMode.battle then
        self:buildShip()
        self.csSelf:invoke4Lua(self.showShipsInOcean, 1)
    end
end

---@public 造船
function IDLBuildingDockyard:buildShip()
    local serverData = self.serverData
    if serverData then
        if bio2number(serverData.state) == IDConst.BuildingState.working then
            self:loadProgressHud()
        else
            self:unLoadProgressHud()
        end
    else
        self:unLoadProgressHud()
    end
end
---@public 显示舰船停泊在港口
function IDLBuildingDockyard:showShipsInPort()
    if self.serverData == nil or MyCfg.mode == GameMode.battle then
        return
    end
    local shipsMap = IDDBCity.curCity:getShipsByDockyardId(bio2number(self.serverData.idx))
    if shipsMap then
        for k, v in pairs(shipsMap) do
            if self.shipsInPorts[k] == nil then
                CLRolePool.borrowObjAsyn(IDUtl.getRolePrefabName(k), self:wrapFunction4CS(self.onLoadShip), k)
            end
        end
    end
end

function IDLBuildingDockyard:onLoadShip(name, ship, shipAttrId)
    if not self.gameObject.activeInHierarchy then
        CLRolePool.returnObj(ship)
        SetActive(ship.gameObject, false)
        return
    end
    self.shipsInPorts[shipAttrId] = ship
    ship.transform.parent = self:getFreePort()
    ship.transform.localPosition = Vector3.zero
    ship.transform.localScale = Vector3.one * 0.6
    ship.transform.localEulerAngles = Vector3.zero

    if ship.luaTable == nil then
        ship.luaTable = IDUtl.newRoleLua(shipAttrId)
        ship:initGetLuaFunc()
    end
    ship:init(shipAttrId, 0, 1, true, {hideShadow = true})
    SetActive(ship.gameObject, true)
end

---@public 取得空闲的停泊点
function IDLBuildingDockyard:getFreePort()
    for i, v in ipairs(self.ports) do
        if v.isFree then
            v.isFree = false
            return v.point
        end
    end
end

---@public 让舰船在海航行
function IDLBuildingDockyard:showShipsInOcean()
    self.csSelf:cancelInvoke4Lua(self.showShipsInOcean)
    if self.serverData == nil or MyCfg.mode == GameMode.battle then
        return
    end
    -- 取得一个舰船id及数量
    local ships = IDDBCity.curCity:getShipsByDockyardId(bio2number(self.serverData.idx))
    if ships then
        local num = 0
        ---@param unit NetProtoIsland.ST_unitInfor
        for shipId, unit in pairs(ships) do
            num = bio2number(unit.num)
            if num > 0 then
                local list = self.shipsInOcean[shipId] or {}
                local hadNum = #list
                if hadNum < 3 and num > hadNum then
                    -- 最多5个
                    CLRolePool.borrowObjAsyn(
                        IDUtl.getRolePrefabName(shipId),
                        self:wrapFunction4CS(self.onLoadShipInOcean),
                        shipId
                    )
                    self.csSelf:invoke4Lua(self.showShipsInOcean, NumEx.NextInt(30, 70) / 10)
                    break
                end
            end
        end
    end
end

function IDLBuildingDockyard:onLoadShipInOcean(name, ship, shipAttrId)
    if not self.gameObject.activeInHierarchy then
        CLRolePool.returnObj(ship)
        SetActive(ship.gameObject, false)
        return
    end
    local list = self.shipsInOcean[shipAttrId] or {}
    table.insert(list, ship)
    self.shipsInOcean[shipAttrId] = list
    ship.transform.parent = self.transform.parent
    ship.transform.position = self.door.position
    ship.transform.localScale = Vector3.one * 0.8
    ship.transform.localEulerAngles = Vector3.zero
    if ship.luaTable == nil then
        ship.luaTable = IDUtl.newRoleLua(shipAttrId)
        ship:initGetLuaFunc()
    end
    SetActive(ship.gameObject, true)
    ship:init(shipAttrId, 0, 1, true, {})
    ship.luaTable:goAround(self)
end

function IDLBuildingDockyard:onShipBack(ship)
    local list = self.shipsInOcean[ship.csSelf.id] or {}
    for i, v in ipairs(list) do
        if ship.csSelf == v then
            CLRolePool.returnObj(ship.csSelf)
            ship.csSelf:clean()
            SetActive(ship.gameObject, false)
            table.remove(list, i)
            self.csSelf:invoke4Lua(self.showShipsInOcean, 5)
            break
        end
    end
end

function IDLBuildingDockyard:SetActive(active)
    IDLBuildingDockyard.super.SetActive(self, active)
    if active then
        self.csSelf:invoke4Lua(self.showShipsInOcean, 5)
    else
        self.csSelf:cancelInvoke4Lua()
        for k, list in pairs(self.shipsInOcean) do
            for i, ship in ipairs(list) do
                CLRolePool.returnObj(ship)
                ship:clean()
                SetActive(ship.gameObject, false)
            end
            self.shipsInOcean[k] = {}
        end
        self.shipsInOcean = {}
    end
end

function IDLBuildingDockyard:clean()
    IDLBuildingDockyard.super.clean(self)
    self.csSelf:cancelInvoke4Lua()
    for i, v in ipairs(self.ports) do
        v.isFree = true
    end
    for k, ship in pairs(self.shipsInPorts) do
        CLRolePool.returnObj(ship)
        ship:clean()
        SetActive(ship.gameObject, false)
    end
    self.shipsInPorts = {}

    for k, list in pairs(self.shipsInOcean) do
        for i, ship in ipairs(list) do
            CLRolePool.returnObj(ship)
            ship:clean()
            SetActive(ship.gameObject, false)
        end
        self.shipsInOcean[k] = {}
    end
    self.shipsInOcean = {}
end

--------------------------------------------
return IDLBuildingDockyard
