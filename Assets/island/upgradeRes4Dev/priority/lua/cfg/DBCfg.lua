---@public 管理数据配置
---@class DBCfg
DBCfg = {}
require("cfg.DBCfgTool")
local curve = require("toolkit.curve")
local math = math
local db = {} -- 经过处理后的数据
-- 数据的路径
local upgradeRes = "/upgradeRes"
if (CLCfgBase.self.isEditMode) then
    upgradeRes = "/upgradeRes4Publish"
end
local priorityPath = joinStr(CLPathCfg.persistentDataPath, "/", CLPathCfg.self.basePath, upgradeRes, "/priority/")
local cfgBasePath = joinStr(priorityPath, "cfg/")
local cfgWorldBasePath = joinStr(priorityPath, "worldMap/")

-- 大地图
local cfgMapPath = joinStr(cfgWorldBasePath)
-- 全局变量定义
local cfgCfgPath = joinStr(cfgBasePath, "DBCFCfgData.cfg")
local cfgBuildingPath = joinStr(cfgBasePath, "DBCFBuildingData.cfg")
local cfgHeadquartersLevsDataPath = joinStr(cfgBasePath, "DBCFHeadquartersLevsData.cfg")
local cfgRolePath = joinStr(cfgBasePath, "DBCFRoleData.cfg")
local cfgBulletPath = joinStr(cfgBasePath, "DBCFBulletData.cfg")

---@public 取得数据列表
function DBCfg.getData(path)
    local dbMap = db[path]
    if (dbMap == nil) then
        if path == cfgBuildingPath or path == cfgRolePath then
            local gidList
            gidList, dbMap = DBCfgTool.pubGetList4GID(path)
            dbMap.list = gidList
        else
            -- 其它没有特殊处理的都以ID为key（dbList:下标连续的列表, dbMap：以ID为key的luatable）
            local dbList = nil
            dbList, dbMap = DBCfgTool.getDatas(path, true)
            -- ====================================
            dbMap.list = dbList
        end
        db[path] = dbMap
    end
    return dbMap
end

---@public 取得常量配置
function DBCfg.getConstCfg(...)
    local datas = DBCfg.getData(cfgCfgPath)
    if (datas == nil) then
        return nil
    end
    return datas[1]
end

---@public 取得曲线
function DBCfg.getCurve(id)
    if db.curves == nil then
        db.curves = {}
        local curveIns = curve.new(1, 0, 1, curve.easing.linear)
        table.insert(db.curves, curveIns)
        --curveIns = curve.new(1,0,1, curve.easing.inQuad)
        --table.insert(db.curves, curveIns)
        --curveIns = curve.new(1,0,1, curve.easing.outQuad)
        --table.insert(db.curves, curveIns)
        --curveIns = curve.new(1,0,1, curve.easing.inOutQuad)
        --table.insert(db.curves, curveIns)
        --curveIns = curve.new(1,0,1, curve.easing.outInQuad)
        --table.insert(db.curves, curveIns)

        curveIns = curve.new(1, 0, 1, curve.easing.inCirc)
        table.insert(db.curves, curveIns)
        curveIns = curve.new(1, 0, 1, curve.easing.outCirc)
        table.insert(db.curves, curveIns)
        curveIns = curve.new(1, 0, 1, curve.easing.inOutCirc)
        table.insert(db.curves, curveIns)
        curveIns = curve.new(1, 0, 1, curve.easing.outInCirc)
        table.insert(db.curves, curveIns)

        curveIns = curve.new(1, 0, 1, curve.easing.inExpo)
        table.insert(db.curves, curveIns)
        curveIns = curve.new(1, 0, 1, curve.easing.outExpo)
        table.insert(db.curves, curveIns)
        curveIns = curve.new(1, 0, 1, curve.easing.inOutExpo)
        table.insert(db.curves, curveIns)
        curveIns = curve.new(1, 0, 1, curve.easing.outInExpo)
        table.insert(db.curves, curveIns)
    end
    return db.curves[id]
end

---@public 常量配置
GConstCfg = DBCfg.getConstCfg()

---@public 取得成长值
---@param base 基础值
---@param max 最大值
---@param curveID 曲线id
---@param persent 百分比
---@param precision 小数点后几位
function DBCfg.getGrowingVal(base, max, curveID, persent, precision)
    ---@type curve
    local curveins = DBCfg.getCurve(curveID)
    if curveins == nil then
        return 0
    end
    local persentVal = curveins:evaluate(persent)
    local val = base + (max - base) * persentVal

    if precision == nil or precision == 0 then
        return math.ceil(val)
    else
        return getPreciseDecimal(val, precision)
    end
end

---@public 取得建筑配置
function DBCfg.getBuildingByID(id)
    local datas = DBCfg.getData(cfgBuildingPath)
    if (datas == nil) then
        return nil
    end
    return datas[id]
end

---@public 取得建筑列表
function DBCfg.getBuildingsByGID(gid)
    local datas = DBCfg.getData(cfgBuildingPath)
    if (datas == nil) then
        return nil
    end
    return datas.list[gid]
end

---@public 取得数据主基地各等级开放配置
function DBCfg.getHeadquartersLevsDataByLev(lev)
    local datas = DBCfg.getData(cfgHeadquartersLevsDataPath)
    if (datas == nil) then
        return nil
    end
    return datas[lev]
end

---@public 取得兵种列表
function DBCfg.getRolesByGID(gid)
    local datas = DBCfg.getData(cfgRolePath)
    if (datas == nil) then
        return nil
    end
    return datas.list[gid]
end

---@public 取得兵种
function DBCfg.getRoleByID(id)
    local datas = DBCfg.getData(cfgRolePath)
    if (datas == nil) then
        return nil
    end
    return datas[id]
end

---@public 取得子弹
function DBCfg.getBulletByID(id)
    local datas = DBCfg.getData(cfgBulletPath)
    if (datas == nil) then
        return nil
    end
    return datas[id]
end
--------------------------------------------------
return DBCfg
