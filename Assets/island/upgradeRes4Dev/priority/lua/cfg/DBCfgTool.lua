--- - 管理数据配置
---@class DBCfgTool 管理数据配置
local DBCfgTool = {}
local mdb = {} -- 原始数据
local mMaps4ID = {}

---@public 把json数据转成对象
---@return list List 下标连续的列表
---@return map4ID Map 以ID为key的luatable
function DBCfgTool.getDatas(cfgPath, isParseWithID)
    local list = mdb[cfgPath]
    local map4ID = {}
    if (list == nil) then
        list = {}
        local _list = Utl.fileToObj(cfgPath)
        if (_list == nil or _list.Count < 2) then
            mdb[cfgPath] = list
            return list, map4ID
        end

        local count = _list.Count
        local n = 0
        local keys = _list[0]
        local cellList = nil
        local cell = nil
        local value = 0
        for i = 1, count - 1 do
            cellList = _list[i]
            n = cellList.Count
            cell = {}
            for j = 0, n - 1 do
                value = cellList[j]
                if (type(value) == "number") then
                    cell[keys[j]] = number2bio(value)
                else
                    cell[keys[j]] = value
                end
            end
            if (isParseWithID) then
                map4ID[bio2number(cell.ID)] = cell
            end
            table.insert(list, cell)
        end
        mdb[cfgPath] = list
        mMaps4ID[cfgPath] = map4ID
    else
        map4ID = mMaps4ID[cfgPath]
    end
    return list, map4ID
end

---@public （基础数据和等级数据是分开的情况）通用取得有base数据和lev数据的表,key＝GID_Lev的形式
function DBCfgTool.pubGetBaseAndLevData(baseDataPath, levDataPath)
    local tmp, baseData = DBCfgTool.getDatas(baseDataPath, true)
    local levData = DBCfgTool.getDatas(levDataPath)
    local key = ""
    local gid = 0
    local lev = 0

    local list = {}
    for i, v in pairs(levData) do
        gid = bio2number(v.GID)
        lev = bio2number(v.Lev)
        key = joinStr(gid, "_", lev)
        local m = {}
        m.base = baseData[gid]
        m.vals = v
        list[key] = m
    end
    return list
end

---@public （基础数据和等级数据在一起的情况）通用取得有base数据和lev数据的表,key＝GID_Lev的形式
function DBCfgTool.pubGet4GIDLev(dataPath)
    local datas = DBCfgTool.getDatas(dataPath)
    local gid
    local lev
    local key = ""
    local m = {}
    for i, v in pairs(datas) do
        gid = bio2number(v.GID)
        lev = bio2number(v.Lev)
        key = PStr.b():a(tostring(gid)):a("_"):a(tostring(lev)):e()
        m[key] = v
    end
    return m
end

---@public 取得数据gid的分类数据列表及原始的数据map
---@param dataPath string 数据路径
---@param gidKey string 分类的key，当为nil默认为GID
---@return gidListMap map key=gid, value = 以gid分类的数据列表
---@return map table 数据的map key=ID, value=数据内容
function DBCfgTool.pubGetList4GID(dataPath, gidKey)
    local datas, map = DBCfgTool.getDatas(dataPath, true)
    local gid
    local gidListMap = {}
    local list = {}
    gidKey = gidKey or "GID"
    for i, v in ipairs(datas) do
        gid = bio2number(v[gidKey])
        list = gidListMap[gid]
        if (list == nil) then
            list = {}
        end
        table.insert(list, v)
        gidListMap[gid] = list
    end
    return gidListMap, map
end

return DBCfgTool
