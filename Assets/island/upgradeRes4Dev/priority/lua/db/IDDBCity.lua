require("public.class")
require("db.IDDBBuilding")
require("db.IDDBTile")

---@class IDDBCity 主城数据
IDDBCity = class("IDDBCity")
---@type IDDBCity 当前城
IDDBCity.curCity = nil

function IDDBCity:ctor(d)
    self:setBaseData(d)
    self.tiles = {} -- 地块信息 key=idx, map
    for k, v in pairs(d.tiles) do
        self.tiles[k] = IDDBTile.new(v)
    end
    self.buildings = {} -- 建筑信息 key=idx, map
    for k, v in pairs(d.buildings) do
        self.buildings[k] = IDDBBuilding.new(v)
        if bio2number(v.attrid) == IDConst.dockyardBuildingID then
            -- 取得造船厂的航船数据
            net:send(NetProtoIsland.send.getShipsByBuildingIdx(bio2number(v.idx)))
        end
    end
end

function IDDBCity:setBaseData(d)
    self._data = d
    self.idx = d.idx -- 唯一标识 int int
    self.name = d.name -- 名称 string
    self.stat = d.status -- 状态 1:正常; int int
    self.lev = d.lev -- 等级 int int
    self.pos = d.pos -- 城所在世界grid的index int int
    self.pidx = d.pidx -- 玩家idx int int
    self.dockyardShips = {} --造船厂里的已经有的舰船数据
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
    ---@type IDDBBuilding
    local b
    local id
    for k, v in pairs(self.buildings) do
        b = v
        id = bio2number(b.attrid)
        if id == IDConst.foodStorageBuildingID then
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
        elseif id == IDConst.goldStorageBuildingID then
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
        elseif id == IDConst.oildStorageBuildingID then
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
        elseif id == IDConst.headquartersBuildingID then
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
    ---@type IDDBBuilding
    local b = IDDBBuilding.new(data)
    self.buildings[bio2number(b.idx)] = b
    if bio2number(b.attrid) == IDConst.dockyardBuildingID then
        -- 取得造船厂的航船数据
        net:send(NetProtoIsland.send.getShipsByBuildingIdx(bio2number(b.idx)))
    end
end

function IDDBCity:onTileChg(tile)
    local t = IDDBTile.new(tile)
    self.tiles[bio2number(t.idx)] = t
end

---@public 取得造船厂的航船数据
---@param idx number 造船厂的idx
function IDDBCity:getShipsByDockyardId(idx)
    return self.dockyardShips[idx]
end

---@public 取得造船厂的已经使用了的
---@param idx number 造船厂的idx
function IDDBCity:getDockyardUsedSpace(idx)
    ---@type IDDBBuilding
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
        for roleAttrId, num in pairs(shipsMap) do
            attr = DBCfg.getRoleByID(roleAttrId)
            ret = ret + num * (bio2number(attr.SpaceSize))
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
function IDDBCity:onGetShips4Dockyard(data)
    local bidx = bio2number(data.buildingIdx)
    local shipsMap = {}
    for k, v in pairs(data.shipsMap or {}) do
        shipsMap[tonumber(k)] = v
    end
    self.dockyardShips[bidx] = shipsMap
end

---@public 当主城变化时
function IDDBCity:onMyselfCityChg(d)
    self:setBaseData(d)
    if MyCfg.mode == GameMode.map or MyCfg.mode == GameMode.city or MyCfg.mode == GameMode.mapBtwncity then
        if IDMainCity then
            IDMainCity.refreshData(self)
        end
    end
end
--------------------------------------------
return IDDBCity
