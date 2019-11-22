require("public.class")
---@class IDWorldMapPage
IDWorldMapPage = class("IDWorldMapPage")

function IDWorldMapPage:init(pageIdx)
    self.pageIdx = pageIdx
    self.mapCells = {}

    self:refreshTiles()
    InvokeEx.invoke(self:wrapFunction4CS(self.checkDataTimeout), IDDBWorldMap.ConstTimeOutSec)
end

---@public 检测数据是否超时
function IDWorldMapPage:checkDataTimeout()
    IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    InvokeEx.invoke(self:wrapFunction4CS(self.checkDataTimeout), IDDBWorldMap.ConstTimeOutSec)
end

function IDWorldMapPage:refreshTiles()
    self:loadBaseData()
    self:loadServerData()
    self:releaseCellWithNoData()
end

-- 加载基础资源
function IDWorldMapPage:loadBaseData()
    --//TODO:加载地图配置的基础数据
end

function IDWorldMapPage:loadServerData()
    local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    if data and data.list then
        self:loadEachCell({i = 1, cells = data.list})
    end
end

--去掉已经被删掉的地块
function IDWorldMapPage:releaseCellWithNoData()
    local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    if data and data.map then
        for k, v in pairs(self.mapCells) do
            --//TODO:如果不是系统配置的地块，那如果在data.map里没有找到数据，说明该地块已经为空闲了，可以移除该地块上的对象
        end
    end
end

---@public 刷新一个单元格
function IDWorldMapPage:refreshOneCell(cellData)
    if bio2number(cellData.idx) == bio2number(IDDBCity.curCity.pos) then
        -- 说明是自己的城,直接跳过
    else
        if bio2number(cellData.cidx) <= 0 then
            -- 说明是移除
            ---@type IDWorldTile
            local cell = self.mapCells[bio2number(cellData.idx)]
            if cell then
                CLThings4LuaPool.returnObj(cell.gameObject)
                cell:clean()
                SetActive(cell.gameObject, false)
                self.mapCells[bio2number(cellData.idx)] = nil
            end
        else
            self:doLoadEachCell(cellData)
        end
    end
end

---@public 加载每一个单元
function IDWorldMapPage:loadEachCell(orgs)
    local i = orgs.i
    local cells = orgs.cells
    if i > #cells then
        return
    end
    ---@type NetProtoIsland.ST_mapCell
    local d = cells[i]
    local params = {i = i + 1, cells = cells}
    self:doLoadEachCell(d, self:wrapFunction4CS(self.loadEachCell), params)
end

---@param d NetProtoIsland.ST_mapCell
function IDWorldMapPage:doLoadEachCell(d, callback, orgs)
    local idx = bio2number(d.idx)
    if idx == bio2number(IDDBCity.curCity.pos) then
        -- 自己的主城， 直接跳过
        Utl.doCallback(callback, orgs)
        return
    end
    ---@type IDWorldTile
    local cellLua = self.mapCells[idx]
    local type = bio2number(d.type)
    if (cellLua and cellLua.type ~= type) then
        -- 不是同一个类型
        cellLua:clean()
        CLThings4LuaPool.returnObj(cellLua.csSelf)
        SetActive(cellLua.gameObject, false)
        self.mapCells[idx] = nil
        cellLua = nil
    end
    if cellLua then
        -- 说明该地块正好有对象
        cellLua:init(cellLua.csSelf, idx, type, d)
        Utl.doCallback(callback, orgs)
    else
        local prefabName = ""
        if type == 1 then
            -- 玩家城
            prefabName = "worldmap.mapcity"
        end
        local params = {data = d, callback = callback, orgs = orgs}
        CLThings4LuaPool.borrowObjAsyn(prefabName, self:wrapFunction4CS(self.onLoadOneMapTile), params)
    end
end

function IDWorldMapPage:onLoadOneMapTile(name, obj, params)
    local callback = params.callback
    local orgs = params.orgs
    ---@type NetProtoIsland.ST_mapCell
    local d = params.data
    local mapType = bio2number(d.type)
    local index = bio2number(d.idx)

    if GameMode.map ~= MyCfg.mode or IDWorldMap.mode ~= GameModeSub.map or self.mapCells[index] then
        CLThings4LuaPool.returnObj(obj)
        SetActive(obj.gameObject, false)
        return
    end
    local cell = obj
    ---@type IDWorldTile
    local cellLua = cell.luaTable
    if cellLua == nil then
        cellLua = IDUtl.newMapTileLua(mapType)
        cell.luaTable = cellLua
    end
    SetActive(obj.gameObject, true)
    obj.transform.parent = IDWorldMap.transform
    obj.transform.localScale = Vector3.one
    obj.transform.localEulerAngles = Vector3.zero
    obj.transform.position = IDWorldMap.grid.grid:GetCellCenter(index)
    cellLua:init(obj, index, mapType, d)
    self.mapCells[index] = cellLua
    Utl.doCallback(callback, orgs)
end

---@public 当缩放屏幕时
function IDWorldMapPage:onScaleScreen(delta, offset)
    ---@param v IDWorldTile
    for k, v in pairs(self.mapCells) do
        v:onScaleScreen(delta, offset)
    end
end

function IDWorldMapPage:clean()
    InvokeEx.cancelInvoke(self:wrapFunction4CS(self.loadEachCell))
    InvokeEx.cancelInvoke(self:wrapFunction4CS(self.checkDataTimeout))
    if self.mapCells then
        for k, v in pairs(self.mapCells) do
            v:clean()
            CLThings4LuaPool.returnObj(v.csSelf)
            SetActive(v.gameObject, false)
        end
    end
    self.mapCells = {}
end

return IDWorldMapPage
