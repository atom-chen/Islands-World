---@class IDWorldMapPage._CellData 包装过的一个地块的数据
---@field public index number
---@field public attr DBCFMapTileData
---@field public serverData NetProtoIsland.ST_mapCell
---@field public pageIdx number
---@field public bounds UnityEngine.Bounds
---@field public position UnityEngine.Vector3

require("public.class")
---@class IDWorldMapPage
IDWorldMapPage = class("IDWorldMapPage")

function IDWorldMapPage:init(pageIdx)
    self.mapCells = {}
    ---@type Coolape.GridBase
    self.grid = IDWorldMap.grid.grid

    self.pageIdx = pageIdx
    self.baseData = IDDBWorldMap.getCfgByPageIdx(self.pageIdx)
    self.pageData = {} -- 一屏的数据,里面存的是IDWorldMapPage.CellData
    self:wrapPageData()
    self:refreshTiles()
    InvokeEx.invoke(self:wrapFunction4CS(self.checkDataTimeout), IDDBWorldMap.ConstTimeOutSec)
end

---@public 检测数据是否超时
function IDWorldMapPage:checkDataTimeout()
    IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    InvokeEx.cancelInvoke(self:wrapFunction4CS(self.checkDataTimeout))
    InvokeEx.invoke(self:wrapFunction4CS(self.checkDataTimeout), IDDBWorldMap.ConstTimeOutSec)
end

---@public 包装数据
function IDWorldMapPage:wrapPageData()
    ---@param d WordlTileCfg
    for i, d in ipairs(self.baseData.list) do
        self:addPageData(d.index, d.id, nil)
    end
end

function IDWorldMapPage:addPageData(index, id, serverData)
    ---@type IDWorldMapPage._CellData
    local pdata = {
        index = index,
        attr = DBCfg.getDataById(DBCfg.CfgPath.MapTile, id),
        pageIdx = self.pageIdx,
        serverData = serverData
    }
    local pos, size
    size = bio2number(pdata.attr.Size)
    if size % 2 == 0 then
        pos = self.grid:GetCellPosition(index)
    else
        pos = self.grid:GetCellCenter(index)
    end
    local s = Vector3.one
    s.y = 0.1
    pdata.position = pos
    pdata.bounds = MyBoundsPool.borrow(pos, s * size)
    self.pageData[index] = pdata
end

function IDWorldMapPage:rmPageDataByIndex(index)
    ---@type IDWorldMapPage._CellData
    local pData = self.pageData[index]
    if pData then
        MyBoundsPool.returnObj(pData.bounds)
        self.pageData[index] = nil
    end
end

function IDWorldMapPage:refreshTiles()
    -- 把服务器数据更新一下
    local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    if data and data.list then
        ---@type IDWorldMapPage._CellData
        local pdata
        ---@param d NetProtoIsland.ST_mapCell
        for index, d in ipairs(data.map) do
            pdata = self.pageData[index]
            if pdata then
                pdata.serverData = d
            else
                self:addPageData(index, bio2number(d.attrid), d)
            end
        end
    end

    local centerPos
    local lastHit =
        Utl.getRaycastHitInfor(
        MyCfg.self.mainCamera,
        Vector3(Screen.width / 2, Screen.height / 2, 0),
        Utl.getLayer("Water")
    )
    if lastHit then
        centerPos = lastHit.point
    end
    self:checkVisible(centerPos)
end

---@public 地块的显示与隐藏
function IDWorldMapPage:checkVisible(centerPos)
    local pos
    ---@type UnityEngine.Vector3
    local diff
    ---@param d IDWorldMapPage._CellData
    for index, d in pairs(self.pageData) do
        if centerPos then
            -- pos = d.position +
            -- 把bounds向中心靠拢一点，让看起来更流畅一些
            centerPos.y = 0
            diff = (centerPos - d.position)
            diff = diff.normalized * 4 * self.grid.CellSize
            d.bounds.center = d.position + diff
        else
            d.bounds.center = d.position
        end

        -- 把位置向最近的influence靠拢一点
        if IDWorldMap.nearestInfluence then
            diff = (IDWorldMap.nearestInfluence.transform.position - d.position)
            pos = d.position + diff.normalized * 5 * self.grid.CellSize
        else
            pos = nil
        end

        if IDWorldMap.isVisibile(pos, d.bounds) then
            self:doLoadEachCell(index, d.serverData, d.attr, d.pageIdx, nil, nil)
        else
            local cell = self.mapCells[index]
            if cell then
                CLThings4LuaPool.returnObj(cell.csSelf)
                cell:clean()
                SetActive(cell.gameObject, false)
                self.mapCells[index] = nil
            end
        end
    end
end

---@public 刷新一个单元格
---@param cellData NetProtoIsland.ST_mapCell
function IDWorldMapPage:refreshOneCell(cellData, isRemove)
    local index = bio2number(cellData.idx)
    if index == bio2number(IDDBCity.curCity.pos) then
        -- 说明是自己的城,直接跳过
    else
        if isRemove then
            -- 说明是移除
            if self.baseData.map[index] then
                -- 说是在基础数据里
                ---@type IDWorldMapPage._CellData
                local pData = self.pageData[index]
                if pData then
                    pData.serverData = nil
                end
            else
                local cell = self.mapCells[index]
                if cell then
                    CLThings4LuaPool.returnObj(cell.csSelf)
                    cell:clean()
                    SetActive(cell.gameObject, false)
                    self.mapCells[index] = nil
                end
                self:rmPageDataByIndex(index)
                return
            end
        end
        ---@type IDWorldTile
        local cell = self.mapCells[index]
        if cell then
            ---@type IDWorldMapPage._CellData
            local pdata = self.pageData[index]
            cell:init(cell.csSelf, cell.gidx, cell.type, pdata.serverData, cell.attr)
        else
            -- 这种情况说明cell还未加载出来，不过没关系，会刷新出来的
        end
    end
end

---@param serverData NetProtoIsland.ST_mapCell
---@param attr DBCFMapTileData
function IDWorldMapPage:doLoadEachCell(index, serverData, attr, pageIdx, callback, orgs)
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
        local prefabName = attr.PrefabName
        local params = {
            index = index,
            data = serverData,
            attr = attr,
            pageIdx = pageIdx,
            callback = callback,
            orgs = orgs
        }
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
    ---@type IDWorldMapPage._CellData
    local pData = self.pageData[index]

    -- 判断可能已经不需显示了
    if
        GameMode.map ~= MyCfg.mode or IDWorldMap.mode ~= GameModeSub.map or self.mapCells[index] or
            self.pageIdx ~= params.pageIdx or
            (pData and (not IDWorldMap.isVisibile(nil, pData.bounds)))
     then
        CLThings4LuaPool.returnObj(obj)
        SetActive(obj.gameObject, false)
        return
    end

    if serverData == nil then
        -- 因为是异步加载的，可能这时数据已经重新取得了，再取一次数据
        ---@type IDLDBWorldPage
        local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
        if data then
            serverData = data.map[index]
        end
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
-- function IDWorldMapPage:onScaleScreen(delta, offset)
--     self:checkVisible()
-- end

function IDWorldMapPage:clean()
    InvokeEx.cancelInvoke(self:wrapFunction4CS(self.checkDataTimeout))
    if self.mapCells then
        for k, v in pairs(self.mapCells) do
            v:clean()
            CLThings4LuaPool.returnObj(v.csSelf)
            SetActive(v.gameObject, false)
        end
    end
    self.mapCells = {}

    ---@param d IDWorldMapPage._CellData
    for k, d in pairs(self.pageData) do
        MyBoundsPool.returnObj(d.bounds)
    end
    self.pageData = {}
end

return IDWorldMapPage
