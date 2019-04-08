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
    --todo:
end

function IDWorldMapPage:loadServerData()
    local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    if data and data.list then
        self:loadEachCell({ i = 1, cells = data.list })
    end
end

--去掉已经被删掉的地块
function IDWorldMapPage:releaseCellWithNoData()
    local data = IDDBWorldMap.getDataByPageIdx(self.pageIdx)
    if data and data.map then
        for k,v in pairs(self.mapCells) do
            -- todo:如果不是系统配置的地块，那如果在data.map里没有找到数据，说明该地块已经为空闲了，可以移除该地块上的对象
        end
    end
end

function IDWorldMapPage:loadEachCell(orgs)
    local i = orgs.i
    local cells = orgs.cells
    if i > #cells then
        return
    end
    ---@type IDDBMapCell
    local d = cells[i]
    local idx = bio2number(d.idx)
    if idx == bio2number(IDDBCity.curCity.pos) then
        -- 自己的主城， 直接跳过
        InvokeEx.invoke(self:wrapFunction4CS(self.loadEachCell), { i = i + 1, cells = cells }, 0)
        return
    end
    ---@type IDWorldTile
    local cellLua = self.mapCells[idx]
    local type = bio2number(d.type)
    if cellLua and cellLua.type ~= type then
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
        InvokeEx.invoke(self:wrapFunction4CS(self.loadEachCell), { i = i + 1, cells = cells }, 0)
    else
        local prefabName = ""
        if type == 1 then
            -- 玩家城
            prefabName = "worldmap.mapcity"
        end
        CLThings4LuaPool.borrowObjAsyn(prefabName, self:wrapFunction4CS(self.onLoadOneMapTile), orgs)
    end
end

function IDWorldMapPage:onLoadOneMapTile(name, obj, orgs)
    if GameMode.map ~= MyCfg.mode then
        CLThings4LuaPool.returnObj(obj)
        SetActive(obj.gameObject, false)
        return
    end
    local i = orgs.i
    local cells = orgs.cells
    ---@type IDDBMapCell
    local d = cells[i]
    local mapType = bio2number(d.type)
    local cell = obj
    ---@type IDWorldTile
    local cellLua = cell.luaTable
    if cellLua == nil then
        cellLua = IDUtl.newMapTileLua(mapType)
        cell.luaTable = cellLua
    end
    local index = bio2number(d.idx)
    SetActive(obj.gameObject, true)
    cellLua:init(obj, index, mapType, d)
    cellLua.transform.parent = IDWorldMap.transform
    cellLua.transform.localScale = Vector3.one
    cellLua.transform.localEulerAngles = Vector3.zero
    cellLua.transform.position = IDWorldMap.grid.grid:GetCellCenter(index)
    self.mapCells[index] = cellLua
    InvokeEx.invoke(self:wrapFunction4CS(self.loadEachCell), { i = i + 1, cells = cells }, 0)
end

---@public 当缩放屏幕时
function IDWorldMapPage:onScaleScreen(delta, offset)
    for k, v in pairs(self.mapCells) do
        v:onScaleScreen(delta, offset)
    end
end

function IDWorldMapPage:clean()
    InvokeEx.cancelInvoke(self:wrapFunction4CS(self.loadEachCell))
    InvokeEx.cancelInvoke(self:wrapFunction4CS(self.checkDataTimeout))
    for k, v in pairs(self.mapCells) do
        v:clean()
        CLThings4LuaPool.returnObj(v.csSelf)
        SetActive(v.gameObject, false)
    end
    self.mapCells = {}
end

return IDWorldMapPage
