require("public.class")

---@class IDDBCity 主城数据
IDDBCity = class("IDDBCity")
---@type IDDBCity 当前城
IDDBCity.curCity = nil

---@param d NetProtoIsland.ST_city
function IDDBCity:ctor(d)
    self:setBaseData(d)
    -- self.tiles = {} -- 地块信息 key=idx, map
    -- for k, v in pairs(d.tiles) do
    --     self.tiles[k] = NetProtoIsland.ST_tile.new(v)
    -- end
    ---@type NetProtoIsland.ST_building  主基地
    self.headquarters = nil
    self.buildings = {} -- 建筑信息 key=idx, map
    ---@param v NetProtoIsland.ST_building
    for k, v in pairs(d.buildings) do
        self.buildings[k] = v
        if self.headquarters == nil and bio2number(v.attrid) == IDConst.BuildingID.headquartersBuildingID then
            self.headquarters = self.buildings[k]
        end
    end
    ---@type table 舰队列表[NetProtoIsland.ST_fleetinfor]
    self.fleets = {}
end

---@param d NetProtoIsland.ST_city
function IDDBCity:setBaseData(d)
    self._data = d
    self.idx = d.idx -- 唯一标识 int int
    self.name = d.name -- 名称 string
    self.status = d.status -- 状态 1:正常; int int
    self.lev = d.lev -- 等级 int int
    self.pos = d.pos -- 城所在世界grid的index int int
    self.pidx = d.pidx -- 玩家idx int int
    self.dockyardShips = {} --造船厂里的已经有的舰船数据
    self.tiles = d.tiles
end

---@public 初始化造船厂数据
function IDDBCity:initDockyardShips()
    ---@param v NetProtoIsland.ST_building
    for k, v in pairs(self.buildings) do
        if bio2number(v.attrid) == IDConst.BuildingID.dockyardBuildingID then
            -- 取得造船厂的航船数据
            CLLNet.send(NetProtoIsland.send.getShipsByBuildingIdx(bio2number(v.idx)))
        end
    end
end

---@public 设置所有造船厂的舰船数据
function IDDBCity:setAllDockyardShips(list)
    ---@param v NetProtoIsland.ST_dockyardShips
    for i, v in ipairs(list) do
        self:onGetShips4Dockyard(v)
    end
end

function IDDBCity:toMap()
    return self._data
end

---@public 取得资源（food，gold，oil）
function IDDBCity:getRes()
    local ret = {}
    local food = 0
    local gold = 0
    local oil = 0
    local maxfood = 0
    local maxgold = 0
    local maxoil = 0

    local attrfood = nil
    local attrgold = nil
    local attroil = nil
    ---@type NetProtoIsland.ST_building
    local b
    local id
    for k, v in pairs(self.buildings) do
        b = v
        id = bio2number(b.attrid)
        if id == IDConst.BuildingID.foodStorageBuildingID then
            food = food + bio2number(b.val)
            if attrfood == nil then
                attrfood = DBCfg.getBuildingByID(id)
            end
            maxfood =
                maxfood +
                DBCfg.getGrowingVal(
                    bio2number(attrfood.ComVal1Min),
                    bio2number(attrfood.ComVal1Max),
                    bio2number(attrfood.ComVal1Curve),
                    bio2number(b.lev) / bio2number(attrfood.MaxLev)
                )
        elseif id == IDConst.BuildingID.goldStorageBuildingID then
            gold = gold + bio2number(b.val)
            if attrgold == nil then
                attrgold = DBCfg.getBuildingByID(id)
            end
            maxgold =
                maxgold +
                DBCfg.getGrowingVal(
                    bio2number(attrgold.ComVal1Min),
                    bio2number(attrgold.ComVal1Max),
                    bio2number(attrgold.ComVal1Curve),
                    bio2number(b.lev) / bio2number(attrgold.MaxLev)
                )
        elseif id == IDConst.BuildingID.oildStorageBuildingID then
            oil = oil + bio2number(b.val)
            if attroil == nil then
                attroil = DBCfg.getBuildingByID(id)
            end
            maxoil =
                maxoil +
                DBCfg.getGrowingVal(
                    bio2number(attroil.ComVal1Min),
                    bio2number(attroil.ComVal1Max),
                    bio2number(attroil.ComVal1Curve),
                    bio2number(b.lev) / bio2number(attroil.MaxLev)
                )
        elseif id == IDConst.BuildingID.headquartersBuildingID then
            -- 主基地
            food = food + bio2number(b.val)
            gold = gold + bio2number(b.val2)
            oil = oil + bio2number(b.val3)
        end
    end

    local baseRes = bio2number(IDConst.baseRes)
    ret.food = food
    ret.gold = gold
    ret.oil = oil
    ret.maxfood = maxfood + baseRes
    ret.maxgold = maxgold + baseRes
    ret.maxoil = maxoil + baseRes
    return ret
end

---@public 当建筑数据有变化时
function IDDBCity:onBuildingChg(data)
    ---@type NetProtoIsland.ST_building
    local b = data
    self.buildings[bio2number(b.idx)] = b
    if bio2number(b.attrid) == IDConst.BuildingID.dockyardBuildingID then
        -- 取得造船厂的航船数据
        CLLNet.send(NetProtoIsland.send.getShipsByBuildingIdx(bio2number(b.idx)))
    end
end

---@type NetProtoIsland.ST_tile
function IDDBCity:onTileChg(tile)
    self.tiles[bio2number(tile.idx)] = tile
end

---@public 取得造船厂的航船数据
---@param idx number 造船厂的idx
function IDDBCity:getShipsByDockyardId(idx)
    return self.dockyardShips[idx]
end

---@public 取得所有的舰船数据
---@return table key:id, val:num
function IDDBCity:getAllShips()
    local shipMap = {}
    for bidx, map in pairs(self.dockyardShips) do
        ---@param unit NetProtoIsland.ST_unitInfor
        for id, unit in pairs(map) do
            if shipMap[id] then
                shipMap[id] = bio2number(unit.num) + shipMap[id]
            else
                shipMap[id] = bio2number(unit.num)
            end
        end
    end
    return shipMap
end

---@public 取得造船厂的已经使用了的
---@param idx number 造船厂的idx
function IDDBCity:getDockyardUsedSpace(idx)
    ---@type NetProtoIsland.ST_building
    local b = self.buildings[idx]
    if b == nil then
        printe("get building is nil")
        return 0
    end

    --已经造好的
    local shipsMap = self:getShipsByDockyardId(idx)
    local ret = 0
    local attr
    if shipsMap then
        ---@param unit NetProtoIsland.ST_unitInfor
        for roleAttrId, unit in pairs(shipsMap) do
            attr = DBCfg.getRoleByID(roleAttrId)
            ret = ret + bio2number(unit.num) * (bio2number(attr.SpaceSize))
        end
    end
    -- 正在造的
    if bio2number(b.val) > 0 then
        local shipAttrId = bio2number(b.val)
        local needBuildNum = bio2number(b.val2)
        attr = DBCfg.getRoleByID(shipAttrId)
        ret = ret + needBuildNum * (bio2number(attr.SpaceSize))
    end

    return ret
end

---@public 当取得造船厂的舰船数据
---@param data NetProtoIsland.ST_dockyardShips
function IDDBCity:onGetShips4Dockyard(data)
    local bidx = bio2number(data.buildingIdx)
    local shipsMap = {}
    ---@param v NetProtoIsland.ST_unitInfor
    for i, v in ipairs(data.ships or {}) do
        shipsMap[bio2number(v.id)] = v
    end
    self.dockyardShips[bidx] = shipsMap
end

---@public 当主城变化时
function IDDBCity:onMyselfCityChg(d)
    self:setBaseData(d)
    if MyCfg.mode == GameMode.map or MyCfg.mode == GameMode.city then
        if IDMainCity then
            IDMainCity.refreshData(self)
        end
    end
end

function IDDBCity:setFleets(list)
    ---@param v NetProtoIsland.ST_fleetinfor
    for i, v in ipairs(list) do
        self.fleets[bio2number(v.idx)] = v
    end
end

---@param fleet NetProtoIsland.ST_fleetinfor
function IDDBCity:onFleetChg(fleet, isRemove)
    if isRemove then
        self.fleets[bio2number(fleet.idx)] = nil
    end
    -- 有可能收到其它玩家的舰队信息，所以需要判断下是否自己的
    if bio2number(fleet.cidx) == bio2number(self.idx) then
        self.fleets[bio2number(fleet.idx)] = fleet
    end
end
--------------------------------------------
return IDDBCity
