do
    ---@class NetProtoIsland
    NetProtoIsland = {}
    local table = table
    require("bio.BioUtl")

    NetProtoIsland.__sessionID = 0; -- 会话ID
    NetProtoIsland.dispatch = {}
    --==============================
    -- public toMap
    NetProtoIsland._toMap = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for k,v in pairs(m) do
            ret[k] = stuctobj.toMap(v)
        end
        return ret
    end
    -- public toList
    NetProtoIsland._toList = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for i,v in ipairs(m) do
            table.insert(ret, stuctobj.toMap(v))
        end
        return ret
    end
    -- public parse
    NetProtoIsland._parseMap = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for k,v in pairs(m) do
            ret[k] = stuctobj.parse(v)
        end
        return ret
    end
    -- public parse
    NetProtoIsland._parseList = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for i,v in ipairs(m) do
            table.insert(ret, stuctobj.parse(v))
        end
        return ret
    end
  --==================================
  --==================================
    ---@class NetProtoIsland.ST_retInfor 返回信息
    NetProtoIsland.ST_retInfor = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[10] = m.msg  -- 返回消息 string
            r[11] = m.code  -- 返回值 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.msg = m[10] --  string
            r.code = m[11] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_mapPage 一屏大地图数据
    NetProtoIsland.ST_mapPage = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[82] = NetProtoIsland._toList(NetProtoIsland.ST_mapCell, m.cells)  -- 地图数据 key=网络index, map
            r[83] = m.pageIdx  -- 一屏所在的网格index  int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.cells = NetProtoIsland._parseList(NetProtoIsland.ST_mapCell, m[82])  -- 地图数据 key=网络index, map
            r.pageIdx = m[83] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_dockyardShips 造船厂的舰船信息
    NetProtoIsland.ST_dockyardShips = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[89] = m.shipsMap  -- key=舰船的配置id, val=舰船数量 map
            r[90] = m.buildingIdx  -- 造船厂的idx int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.shipsMap = m[89] --  table
            r.buildingIdx = m[90] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_tile 建筑信息对象
    NetProtoIsland.ST_tile = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- 唯一标识 int int
            r[46] = m.attrid  -- 属性配置id int int
            r[47] = m.cidx  -- 主城idx int int
            r[33] = m.pos  -- 位置，即在城的gird中的index int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] --  int
            r.attrid = m[46] --  int
            r.cidx = m[47] --  int
            r.pos = m[33] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_building 建筑信息对象
    NetProtoIsland.ST_building = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- 唯一标识 int int
            r[48] = m.val4  -- 值4。如:产量，仓库的存储量等 int int
            r[49] = m.val3  -- 值3。如:产量，仓库的存储量等 int int
            r[50] = m.val2  -- 值2。如:产量，仓库的存储量等 int int
            r[64] = m.endtime  -- 完成升级、恢复、采集等的时间点 long int
            r[30] = m.lev  -- 等级 int int
            r[51] = m.val  -- 值。如:产量，仓库的存储量等 int int
            r[47] = m.cidx  -- 主城idx int int
            r[61] = m.val5  -- 值5。如:产量，仓库的存储量等 int int
            r[46] = m.attrid  -- 属性配置id int int
            r[65] = m.starttime  -- 开始升级、恢复、采集等的时间点 long int
            r[63] = m.state  -- 状态. 0：正常；1：升级中；9：恢复中 int
            r[33] = m.pos  -- 位置，即在城的gird中的index int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] --  int
            r.val4 = m[48] --  int
            r.val3 = m[49] --  int
            r.val2 = m[50] --  int
            r.endtime = m[64] --  int
            r.lev = m[30] --  int
            r.val = m[51] --  int
            r.cidx = m[47] --  int
            r.val5 = m[61] --  int
            r.attrid = m[46] --  int
            r.starttime = m[65] --  int
            r.state = m[63] --  int
            r.pos = m[33] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_mapCell 大地图地块数据
    NetProtoIsland.ST_mapCell = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- 网格index int
            r[84] = m.val1  -- 值1 int
            r[47] = m.cidx  -- 主城idx int
            r[49] = m.val3  -- 值3 int
            r[83] = m.pageIdx  -- 所在屏的index int
            r[85] = m.type  -- 地块类型 1：玩家，2：npc int
            r[50] = m.val2  -- 值2 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] --  int
            r.val1 = m[84] --  int
            r.cidx = m[47] --  int
            r.val3 = m[49] --  int
            r.pageIdx = m[83] --  int
            r.type = m[85] --  int
            r.val2 = m[50] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_resInfor 资源信息
    NetProtoIsland.ST_resInfor = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[66] = m.oil  -- 油 int
            r[67] = m.gold  -- 金 int
            r[68] = m.food  -- 粮 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.oil = m[66] --  int
            r.gold = m[67] --  int
            r.food = m[68] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_city 主城
    NetProtoIsland.ST_city = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- 唯一标识 int int
            r[45] = NetProtoIsland._toMap(NetProtoIsland.ST_tile, m.tiles)  -- 地块信息 key=idx, map
            r[13] = m.name  -- 名称 string
            r[26] = m.status  -- 状态 1:正常; int int
            r[32] = NetProtoIsland._toMap(NetProtoIsland.ST_building, m.buildings)  -- 建筑信息 key=idx, map
            r[30] = m.lev  -- 等级 int int
            r[33] = m.pos  -- 城所在世界grid的index int int
            r[35] = m.pidx  -- 玩家idx int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] --  int
            r.tiles = NetProtoIsland._parseMap(NetProtoIsland.ST_tile, m[45])  -- 地块信息 key=idx, map
            r.name = m[13] --  string
            r.status = m[26] --  int
            r.buildings = NetProtoIsland._parseMap(NetProtoIsland.ST_building, m[32])  -- 建筑信息 key=idx, map
            r.lev = m[30] --  int
            r.pos = m[33] --  int
            r.pidx = m[35] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_player 用户信息
    NetProtoIsland.ST_player = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- 唯一标识 int int
            r[29] = m.diam  -- 钻石 long int
            r[13] = m.name  -- 名字 string
            r[26] = m.status  -- 状态 1：正常 int int
            r[28] = m.cityidx  -- 城池id int int
            r[27] = m.unionidx  -- 联盟id int int
            r[30] = m.lev  -- 等级 long int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] --  int
            r.diam = m[29] --  int
            r.name = m[13] --  string
            r.status = m[26] --  int
            r.cityidx = m[28] --  int
            r.unionidx = m[27] --  int
            r.lev = m[30] --  int
            return r;
        end,
    }
    --==============================
    NetProtoIsland.send = {
    -- 取得造船厂所有舰艇列表
    getShipsByBuildingIdx = function(buildingIdx)
        local ret = {}
        ret[0] = 91
        ret[1] = NetProtoIsland.__sessionID
        ret[90] = buildingIdx; -- 造船厂的idx int
        return ret
    end,
    -- 升级建筑
    upLevBuilding = function(idx)
        local ret = {}
        ret[0] = 54
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 建筑idx int
        return ret
    end,
    -- 移除建筑
    rmBuilding = function(idx)
        local ret = {}
        ret[0] = 76
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 地块idx int
        return ret
    end,
    -- 新建建筑
    newBuilding = function(attrid, pos)
        local ret = {}
        ret[0] = 52
        ret[1] = NetProtoIsland.__sessionID
        ret[46] = attrid; -- 建筑配置id int
        ret[33] = pos; -- 位置 int
        return ret
    end,
    -- 登陆
    login = function(uidx, channel, deviceID, isEditMode)
        local ret = {}
        ret[0] = 16
        ret[1] = NetProtoIsland.__sessionID
        ret[17] = uidx; -- 用户id
        ret[18] = channel; -- 渠道号
        ret[19] = deviceID; -- 机器码
        ret[78] = isEditMode; -- 编辑模式
        return ret
    end,
    -- 当完成建造部分舰艇的通知
    onFinishBuildOneShip = function(buildingIdx)
        local ret = {}
        ret[0] = 96
        ret[1] = NetProtoIsland.__sessionID
        ret[90] = buildingIdx; -- 造船厂的idx int
        return ret
    end,
    -- 取得建筑
    getBuilding = function(idx)
        local ret = {}
        ret[0] = 55
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 建筑idx int
        return ret
    end,
    -- 移除地块
    rmTile = function(idx)
        local ret = {}
        ret[0] = 75
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 地块idx int
        return ret
    end,
    -- 资源变化时推送
    onResChg = function()
        local ret = {}
        ret[0] = 69
        ret[1] = NetProtoIsland.__sessionID
        return ret
    end,
    -- 移动建筑
    moveBuilding = function(idx, pos)
        local ret = {}
        ret[0] = 56
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 建筑idx int
        ret[33] = pos; -- 位置 int
        return ret
    end,
    -- 登出
    logout = function()
        local ret = {}
        ret[0] = 15
        ret[1] = NetProtoIsland.__sessionID
        return ret
    end,
    -- 造船
    buildShip = function(buildingIdx, shipAttrID, num)
        local ret = {}
        ret[0] = 93
        ret[1] = NetProtoIsland.__sessionID
        ret[90] = buildingIdx; -- 造船厂的idx int
        ret[94] = shipAttrID; -- 舰船配置id int
        ret[95] = num; -- 数量 int
        return ret
    end,
    -- 立即升级建筑
    upLevBuildingImm = function(idx)
        local ret = {}
        ret[0] = 77
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 建筑idx int
        return ret
    end,
    -- 新建地块
    newTile = function(pos)
        local ret = {}
        ret[0] = 74
        ret[1] = NetProtoIsland.__sessionID
        ret[33] = pos; -- 位置 int
        return ret
    end,
    -- 建筑变化时推送
    onBuildingChg = function()
        local ret = {}
        ret[0] = 71
        ret[1] = NetProtoIsland.__sessionID
        return ret
    end,
    -- 玩家信息变化时推送
    onPlayerChg = function()
        local ret = {}
        ret[0] = 72
        ret[1] = NetProtoIsland.__sessionID
        return ret
    end,
    -- 心跳
    heart = function()
        local ret = {}
        ret[0] = 59
        ret[1] = NetProtoIsland.__sessionID
        return ret
    end,
    -- 取得一屏的在地图数据
    getMapDataByPageIdx = function(pageIdx)
        local ret = {}
        ret[0] = 88
        ret[1] = NetProtoIsland.__sessionID
        ret[83] = pageIdx; -- 一屏所在的网格index
        return ret
    end,
    -- 移动地块
    moveTile = function(idx, pos)
        local ret = {}
        ret[0] = 57
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 地块idx int
        ret[33] = pos; -- 位置 int
        return ret
    end,
    -- 收集资源
    collectRes = function(idx)
        local ret = {}
        ret[0] = 79
        ret[1] = NetProtoIsland.__sessionID
        ret[12] = idx; -- 资源建筑的idx int
        return ret
    end,
    -- 建筑升级完成
    onFinishBuildingUpgrade = function()
        local ret = {}
        ret[0] = 73
        ret[1] = NetProtoIsland.__sessionID
        return ret
    end,
    }
    --==============================
    NetProtoIsland.recive = {
    getShipsByBuildingIdx = function(map)
        local ret = {}
        ret.cmd = "getShipsByBuildingIdx"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.dockyardShips = NetProtoIsland.ST_dockyardShips.parse(map[92]) -- 造船厂的idx int
        return ret
    end,
    upLevBuilding = function(map)
        local ret = {}
        ret.cmd = "upLevBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息
        return ret
    end,
    rmBuilding = function(map)
        local ret = {}
        ret.cmd = "rmBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.idx = map[12]-- 被移除建筑的idx int
        return ret
    end,
    newBuilding = function(map)
        local ret = {}
        ret.cmd = "newBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息对象
        return ret
    end,
    login = function(map)
        local ret = {}
        ret.cmd = "login"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.player = NetProtoIsland.ST_player.parse(map[20]) -- 玩家信息
        ret.city = NetProtoIsland.ST_city.parse(map[44]) -- 主城信息
        ret.systime = map[21]-- 系统时间 long
        ret.session = map[22]-- 会话id
        return ret
    end,
    onFinishBuildOneShip = function(map)
        local ret = {}
        ret.cmd = "onFinishBuildOneShip"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.buildingIdx = map[90]-- 造船厂的idx int
        ret.shipAttrID = map[94]-- 航船的配置id
        ret.shipNum = map[97]-- 航船的数量
        return ret
    end,
    getBuilding = function(map)
        local ret = {}
        ret.cmd = "getBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息对象
        return ret
    end,
    rmTile = function(map)
        local ret = {}
        ret.cmd = "rmTile"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.idx = map[12]-- 被移除地块的idx int
        return ret
    end,
    onResChg = function(map)
        local ret = {}
        ret.cmd = "onResChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.resInfor = NetProtoIsland.ST_resInfor.parse(map[70]) -- 资源信息
        return ret
    end,
    moveBuilding = function(map)
        local ret = {}
        ret.cmd = "moveBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息
        return ret
    end,
    logout = function(map)
        local ret = {}
        ret.cmd = "logout"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        return ret
    end,
    buildShip = function(map)
        local ret = {}
        ret.cmd = "buildShip"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 造船厂信息
        return ret
    end,
    upLevBuildingImm = function(map)
        local ret = {}
        ret.cmd = "upLevBuildingImm"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息
        return ret
    end,
    newTile = function(map)
        local ret = {}
        ret.cmd = "newTile"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.tile = NetProtoIsland.ST_tile.parse(map[58]) -- 地块信息对象
        return ret
    end,
    onBuildingChg = function(map)
        local ret = {}
        ret.cmd = "onBuildingChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息
        return ret
    end,
    onPlayerChg = function(map)
        local ret = {}
        ret.cmd = "onPlayerChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.player = NetProtoIsland.ST_player.parse(map[20]) -- 玩家信息
        return ret
    end,
    heart = function(map)
        local ret = {}
        ret.cmd = "heart"
        return ret
    end,
    getMapDataByPageIdx = function(map)
        local ret = {}
        ret.cmd = "getMapDataByPageIdx"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.mapPage = NetProtoIsland.ST_mapPage.parse(map[87]) -- 在地图一屏数据 map
        return ret
    end,
    moveTile = function(map)
        local ret = {}
        ret.cmd = "moveTile"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.tile = NetProtoIsland.ST_tile.parse(map[58]) -- 地块信息
        return ret
    end,
    collectRes = function(map)
        local ret = {}
        ret.cmd = "collectRes"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.resType = map[80]-- 收集的资源类型 int
        ret.resVal = map[81]-- 收集到的资源量 int
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息
        return ret
    end,
    onFinishBuildingUpgrade = function(map)
        local ret = {}
        ret.cmd = "onFinishBuildingUpgrade"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[53]) -- 建筑信息
        return ret
    end,
    }
    --==============================
    NetProtoIsland.dispatch[91]={onReceive = NetProtoIsland.recive.getShipsByBuildingIdx, send = NetProtoIsland.send.getShipsByBuildingIdx}
    NetProtoIsland.dispatch[54]={onReceive = NetProtoIsland.recive.upLevBuilding, send = NetProtoIsland.send.upLevBuilding}
    NetProtoIsland.dispatch[76]={onReceive = NetProtoIsland.recive.rmBuilding, send = NetProtoIsland.send.rmBuilding}
    NetProtoIsland.dispatch[52]={onReceive = NetProtoIsland.recive.newBuilding, send = NetProtoIsland.send.newBuilding}
    NetProtoIsland.dispatch[16]={onReceive = NetProtoIsland.recive.login, send = NetProtoIsland.send.login}
    NetProtoIsland.dispatch[96]={onReceive = NetProtoIsland.recive.onFinishBuildOneShip, send = NetProtoIsland.send.onFinishBuildOneShip}
    NetProtoIsland.dispatch[55]={onReceive = NetProtoIsland.recive.getBuilding, send = NetProtoIsland.send.getBuilding}
    NetProtoIsland.dispatch[75]={onReceive = NetProtoIsland.recive.rmTile, send = NetProtoIsland.send.rmTile}
    NetProtoIsland.dispatch[69]={onReceive = NetProtoIsland.recive.onResChg, send = NetProtoIsland.send.onResChg}
    NetProtoIsland.dispatch[56]={onReceive = NetProtoIsland.recive.moveBuilding, send = NetProtoIsland.send.moveBuilding}
    NetProtoIsland.dispatch[15]={onReceive = NetProtoIsland.recive.logout, send = NetProtoIsland.send.logout}
    NetProtoIsland.dispatch[93]={onReceive = NetProtoIsland.recive.buildShip, send = NetProtoIsland.send.buildShip}
    NetProtoIsland.dispatch[77]={onReceive = NetProtoIsland.recive.upLevBuildingImm, send = NetProtoIsland.send.upLevBuildingImm}
    NetProtoIsland.dispatch[74]={onReceive = NetProtoIsland.recive.newTile, send = NetProtoIsland.send.newTile}
    NetProtoIsland.dispatch[71]={onReceive = NetProtoIsland.recive.onBuildingChg, send = NetProtoIsland.send.onBuildingChg}
    NetProtoIsland.dispatch[72]={onReceive = NetProtoIsland.recive.onPlayerChg, send = NetProtoIsland.send.onPlayerChg}
    NetProtoIsland.dispatch[59]={onReceive = NetProtoIsland.recive.heart, send = NetProtoIsland.send.heart}
    NetProtoIsland.dispatch[88]={onReceive = NetProtoIsland.recive.getMapDataByPageIdx, send = NetProtoIsland.send.getMapDataByPageIdx}
    NetProtoIsland.dispatch[57]={onReceive = NetProtoIsland.recive.moveTile, send = NetProtoIsland.send.moveTile}
    NetProtoIsland.dispatch[79]={onReceive = NetProtoIsland.recive.collectRes, send = NetProtoIsland.send.collectRes}
    NetProtoIsland.dispatch[73]={onReceive = NetProtoIsland.recive.onFinishBuildingUpgrade, send = NetProtoIsland.send.onFinishBuildingUpgrade}
    --==============================
    NetProtoIsland.cmds = {
        getShipsByBuildingIdx = "getShipsByBuildingIdx", -- 取得造船厂所有舰艇列表,
        upLevBuilding = "upLevBuilding", -- 升级建筑,
        rmBuilding = "rmBuilding", -- 移除建筑,
        newBuilding = "newBuilding", -- 新建建筑,
        login = "login", -- 登陆,
        onFinishBuildOneShip = "onFinishBuildOneShip", -- 当完成建造部分舰艇的通知,
        getBuilding = "getBuilding", -- 取得建筑,
        rmTile = "rmTile", -- 移除地块,
        onResChg = "onResChg", -- 资源变化时推送,
        moveBuilding = "moveBuilding", -- 移动建筑,
        logout = "logout", -- 登出,
        buildShip = "buildShip", -- 造船,
        upLevBuildingImm = "upLevBuildingImm", -- 立即升级建筑,
        newTile = "newTile", -- 新建地块,
        onBuildingChg = "onBuildingChg", -- 建筑变化时推送,
        onPlayerChg = "onPlayerChg", -- 玩家信息变化时推送,
        heart = "heart", -- 心跳,
        getMapDataByPageIdx = "getMapDataByPageIdx", -- 取得一屏的在地图数据,
        moveTile = "moveTile", -- 移动地块,
        collectRes = "collectRes", -- 收集资源,
        onFinishBuildingUpgrade = "onFinishBuildingUpgrade", -- 建筑升级完成
    }
    --==============================
    return NetProtoIsland
end
