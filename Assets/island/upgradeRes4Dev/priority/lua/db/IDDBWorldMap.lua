-- 世界地图数据

---@class WordlTileCfg
---@field public index number 网格index
---@field public id number 大地图块配置id

---@class IDLDBWorldPage
---@field public list table 里面存的是 NetProtoIsland.ST_mapCell
---@field public map table 里面存的是 NetProtoIsland.ST_mapCell

---@class IDLDBWorldPageCfg
---@field public list table 里面存的是 WordlTileCfg
---@field public map table 里面存的是 WordlTileCfg

-- require("public.class")
IDDBWorldMap = {}
--------------------------------------------
-- IDDBMapCell = class("IDDBMapCell")
-- function IDDBMapCell:ctor(d)
--     self._data = d
--     self.idx = d.idx -- 网格index
--     self.type = d.type --地块类型 1：玩家，2：npc
--     self.cidx = d.cidx--主城idx
--     self.pageIdx = d.pageIdx--所在屏的index
--     self.val1 = d.val1--值1
--     self.val2 = d.val2--值2
--     self.val3 = d.val3--值3
-- end
--------------------------------------------

local mapPageData = {} -- key:pageIdx, value = list
local mapPageCacheTime = {} -- key:pageIdx, value = timeOut
IDDBWorldMap.fleets = {}
IDDBWorldMap.ConstTimeOutSec = 60 -- 数据超时秒

--=========================================
-- 大地图配置数据取得
--=========================================
local mapPageCfgData = {} -- key:pageIdx, value = table(index:id)
-- 数据的路径
local upgradeRes = "/upgradeRes"
if (CLCfgBase.self.isEditMode) then
    upgradeRes = "/upgradeRes4Publish"
end
local priorityPath = joinStr(CLPathCfg.persistentDataPath, "/", CLPathCfg.self.basePath, upgradeRes, "/priority/")
local cfgWorldBasePath = joinStr(priorityPath, "cfg/worldmap/")
---@type Coolape.GridBase
local grid = GridBase()
grid:init(Vector3.zero, 1000, 1000, 1)
---@type Coolape.GridBase
local gridArea = GridBase()
gridArea:init(Vector3.zero, 10, 10, 100)
local scale = 100
local pageSize = 10
local mapAreaInfor

---@public 取得网格id对应的分区id
function IDDBWorldMap.getAreaId(index)
    if mapAreaInfor == nil then
        local cfgPath = joinStr(cfgWorldBasePath, "maparea.cfg")
        local bytes = FileEx.ReadAllBytes(cfgPath)
        mapAreaInfor = BioUtl.readObject(bytes)
    end
    local areaIdx = IDDBWorldMap.mapIndex2AreaIndex(index)
    return mapAreaInfor[areaIdx]
end

---@public 取得大图的index映射到分区网格的index
---@param index
function IDDBWorldMap.mapIndex2AreaIndex(index)
    local areaIndex = -1
    local col = grid:GetColumn(index)
    local row = grid:GetRow(index)
    col = NumEx.getIntPart(col / scale)
    row = NumEx.getIntPart(row / scale)

    areaIndex = gridArea:GetCellIndex(col, row)
    return areaIndex
end

---@public 分区网格的index转成大地图每屏的index
function IDDBWorldMap.areaIndex2MapPageIndexs(areaIndex)
    local ret = {}
    local col = gridArea:GetColumn(areaIndex)
    local row = gridArea:GetRow(areaIndex)
    col = NumEx.getIntPart(col * scale)
    row = NumEx.getIntPart(row * scale)
    for i = col + NumEx.getIntPart(pageSize / 2), col + scale - 1, pageSize do
        for j = row + NumEx.getIntPart(pageSize / 2), row + scale - 1, pageSize do
            table.insert(ret, grid:GetCellIndex(i, j))
        end
    end
    return ret
end

---@public 取得一屏的配置数据
function IDDBWorldMap.getCfgByPageIdx(pageIdx)
    if pageIdx < 0 then
        return
    end
    local cfgMap = mapPageCfgData[pageIdx]
    if cfgMap == nil then
        -- 未取得数据，从配置文件读取
        local areaIndex = IDDBWorldMap.mapIndex2AreaIndex(pageIdx)
        local cfgPath = joinStr(cfgWorldBasePath, areaIndex, ".cfg")
        local bytes = FileEx.ReadAllBytes(cfgPath)
        local map = BioUtl.readObject(bytes)
        if map then
            for pIndex, d in pairs(map) do
                local list = {}
                for idx, id in pairs(d) do
                    table.insert(list, {index = idx, id = id})
                end
                mapPageCfgData[pIndex] = {map = d, list = list}
            end
        else
            -- 取得数据失败
            printe("取得地图配置数据失败，cfgPath =" .. cfgPath)
        end

        -- 重新设置了数据，再取一次
        cfgMap = mapPageCfgData[pageIdx]
        if cfgMap == nil then
            printe("重新设置了数据，再取一次 is nil pageidx==" .. pageIdx)
        end
    end
    return cfgMap
end
--=========================================
--=========================================
---@public 取得一屏的数据
function IDDBWorldMap.getDataByPageIdx(pageIdx)
    if pageIdx == nil then
        return nil
    end
    if mapPageData[pageIdx] == nil or mapPageCacheTime[pageIdx] == nil then
        -- 没有数据，发送服务器
        CLLNet.send(NetProtoIsland.send.getMapDataByPageIdx(pageIdx))
        return
    end
    local timeOut = mapPageCacheTime[pageIdx]
    if DateEx.nowMS - timeOut > 0 then
        --说明已经超时了
        CLLNet.send(NetProtoIsland.send.getMapDataByPageIdx(pageIdx))
        return
    end

    return mapPageData[pageIdx]
end

---@public 取得一屏数据
---@param data NetProtoIsland.ST_mapPage
function IDDBWorldMap.onGetMapPageData(data)
    local pageIdx = bio2number(data.pageIdx)
    local cells = data.cells

    local cellmap = {}
    ---@type NetProtoIsland.ST_mapCell
    for i, v in ipairs(cells) do
        cellmap[bio2number(v.idx)] = v
    end
    mapPageData[pageIdx] = {list = cells, map = cellmap}
    mapPageCacheTime[pageIdx] = DateEx.nowMS + IDDBWorldMap.ConstTimeOutSec * 1000
    if MyCfg.mode == GameMode.map then
        IDWorldMap.onGetMapPageData(pageIdx, mapPageData[pageIdx])
    end
end

---@param mapCell NetProtoIsland.ST_mapCell
function IDDBWorldMap.onMapCellChg(mapCell, isRemove)
    local pageIdx = bio2number(mapCell.pageIdx)
    local pageData = mapPageData[pageIdx]
    if pageData then
        local list = pageData.list
        local idx = bio2number(mapCell.idx)
        ---@param v NetProtoIsland.ST_mapCell
        for i, v in ipairs(list) do
            if bio2number(v.idx) == idx then
                table.remove(list, i)
                pageData.map[idx] = nil
                break
            end
        end
        if not isRemove then
            table.insert(list, mapCell)
            pageData.map[idx] = mapCell
        end

        if MyCfg.mode == GameMode.map then
            if IDWorldMap then
                IDWorldMap.onMapCellChg(mapCell, isRemove)
            end
        end
    end
end

function IDDBWorldMap.onGetFleets4Page(fleets)
    ---@param fleet ST_fleetinfor
    for i, fleet in ipairs(fleets) do
        IDDBWorldMap.onGetFleet(fleet, false)
    end
end

---@param fleet NetProtoIsland.ST_fleetinfor
function IDDBWorldMap.onGetFleet(fleet, isRemove)
    if isRemove then
        IDDBWorldMap.fleets[bio2number(fleet.idx)] = nil
    else
        IDDBWorldMap.fleets[bio2number(fleet.idx)] = fleet
    end
    if IDWorldMap then
        IDWorldMap.refreshFleet(fleet, isRemove)
    end
end

function IDDBWorldMap.clean()
    mapPageData = {} -- key:pageIdx, value = list
    mapPageCacheTime = {} -- key:pageIdx, value = timeOut
    IDDBWorldMap.fleets = {}
end
--------------------------------------------
return IDDBWorldMap
