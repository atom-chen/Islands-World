require("public.class")
---@class IDWorldMapPage
IDWorldMapPage = class("IDWorldMapPage")

function IDWorldMapPage:init(pageIdx)
    self.pageIdx = pageIdx
    self.mapCells = {}
    ---@type Coolape.GridBase
    self.grid = IDWorldMap.grid.grid
    self.baseData = IDDBWorldMap.getCfgByPageIdx(self.pageIdx)

    self:refreshTiles()
    InvokeEx.invoke(self:wrapFunction4CS(self.checkDataTimeout), IDDBWorldMap.ConstTimeOutSec)
end

---@public 检测数据是否超时
function IDWorldMapPage:checkDataTimeout()
    IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    InvokeEx.invoke(self:wrapFunction4CS(self.checkDataTimeout), IDDBWorldMap.ConstTimeOutSec)
end

function IDWorldMapPage:refreshTiles()
    --//TODO:还要考虑需要先加载好基础地块再加载
    self:loadBaseData()
    self:loadServerData()
    self:releaseCellWithNoData()
end

-- 加载基础资源
function IDWorldMapPage:loadBaseData()
    if self.baseData and self.baseData.list then
        self:loadEachCell4BaseData({i = 1, cells = self.baseData.list, pageIdx = self.pageIdx})
    end
end

function IDWorldMapPage:loadServerData()
    local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    if data and data.list then
        self:loadEachCell4ServerData({i = 1, cells = data.list, pageIdx = self.pageIdx})
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
---@param cellData NetProtoIsland.ST_mapCell
function IDWorldMapPage:refreshOneCell(cellData, isRemove)
    if bio2number(cellData.idx) == bio2number(IDDBCity.curCity.pos) then
        -- 说明是自己的城,直接跳过
    else
        if isRemove then
            -- 说明是移除
            ---@type IDWorldTile
            local cell = self.mapCells[bio2number(cellData.idx)]
            if cell then
                if self.baseData.map[bio2number(cellData.idx)] then
                    -- 说是在基础数据里
                    cell:init(cell.csSelf, cell.gidx, cell.type, nil, cell.attr)
                else
                    CLThings4LuaPool.returnObj(cell.gameObject)
                    cell:clean()
                    SetActive(cell.gameObject, false)
                    self.mapCells[bio2number(cellData.idx)] = nil
                end
            end
        else
            local index = bio2number(cellData.idx)
            local attr = DBCfg.getDataById(DBCfg.CfgPath.MapTile, bio2number(cellData.attrid))
            self:doLoadEachCell(index, cellData, attr)
        end
    end
end

---@public 加载每一个单元
function IDWorldMapPage:loadEachCell4BaseData(orgs)
    local i = orgs.i
    local cells = orgs.cells
    local d = cells[i]
    if i > #cells then
        return
    end

    local attr = DBCfg.getDataById(DBCfg.CfgPath.MapTile, d.id)
    orgs.i = i + 1
    local params = orgs
    self:doLoadEachCell(d.index, nil, attr, self:wrapFunction4CS(self.loadEachCell4BaseData), params)
end

---@public 加载每一个单元
function IDWorldMapPage:loadEachCell4ServerData(orgs)
    local i = orgs.i
    local cells = orgs.cells
    if i > #cells then
        return
    end
    ---@type NetProtoIsland.ST_mapCell
    local d = cells[i]
    local attr = DBCfg.getDataById(DBCfg.CfgPath.MapTile, bio2Int(d.attrid))
    orgs.i = i + 1
    local params = orgs
    self:doLoadEachCell(bio2number(d.idx), d, attr, self:wrapFunction4CS(self.loadEachCell4ServerData), params)
end

---@param serverData NetProtoIsland.ST_mapCell
---@param attr DBCFMapTileData
function IDWorldMapPage:doLoadEachCell(index, serverData, attr, callback, orgs)
    if serverData then
        local idx = bio2number(serverData.idx)
        if idx == bio2number(IDDBCity.curCity.pos) then
            -- 自己的主城， 直接跳过
            Utl.doCallback(callback, orgs)
            return
        end
    end
    ---@type IDWorldTile
    local cellLua = self.mapCells[index]
    local type = bio2number(attr.GID)
    if type == IDConst.WorldmapCellType.occupy then
        Utl.doCallback(callback, orgs)
        return
    end
    if (cellLua and cellLua.type ~= type) then
        -- 不是同一个类型
        cellLua:clean()
        CLThings4LuaPool.returnObj(cellLua.csSelf)
        SetActive(cellLua.gameObject, false)
        self.mapCells[index] = nil
        cellLua = nil
    end
    if cellLua then
        -- 说明该地块正好有对象
        cellLua:init(cellLua.csSelf, index, type, serverData, attr)
        Utl.doCallback(callback, orgs)
    else
        local prefabName = ""
        -- 玩家城
        prefabName = attr.PrefabName
        local params = {index = index, data = serverData, attr = attr, callback = callback, orgs = orgs}
        CLThings4LuaPool.borrowObjAsyn(prefabName, self:wrapFunction4CS(self.onLoadOneMapTile), params)
    end
end

function IDWorldMapPage:onLoadOneMapTile(name, obj, params)
    local callback = params.callback
    local orgs = params.orgs
    ---@type NetProtoIsland.ST_mapCell
    local serverData = params.data
    ---@type DBCFMapTileData
    local attr = params.attr
    local mapType = bio2number(attr.GID)
    local index = params.index
    if
        GameMode.map ~= MyCfg.mode or IDWorldMap.mode ~= GameModeSub.map or self.mapCells[index] or
            self.pageIdx ~= orgs.pageIdx
     then
        CLThings4LuaPool.returnObj(obj)
        SetActive(obj.gameObject, false)
        return
    end
    if obj == nil then
        printe("borrow obj is nil!==" .. name)
        Utl.doCallback(callback, orgs)
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
    local size = bio2number(attr.Size)
    if size % 2 == 0 then
        obj.transform.position = self.grid:GetCellPosition(index)
    else
        obj.transform.position = self.grid:GetCellCenter(index)
    end
    cellLua:init(obj, index, mapType, serverData, attr)
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
