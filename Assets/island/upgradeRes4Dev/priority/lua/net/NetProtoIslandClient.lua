do
    ---@class NetProtoIsland 网络协议
    NetProtoIsland = {}
    local table = table
    require("bio.BioUtl")

    NetProtoIsland.__sessionID = 0 -- 会话ID
    NetProtoIsland.dispatch = {}
    local __callbackInfor = {} -- 回调信息
    local __callTimes = 1
    ---@public 设计回调信息
    local setCallback = function (callback, orgs, ret)
       if callback then
           local callbackKey = os.time() + __callTimes
           __callTimes = __callTimes + 1
           __callbackInfor[callbackKey] = {callback, orgs}
           ret[3] = callbackKey
        end
    end
    ---@public 处理回调
    local doCallback = function(map, result)
        local callbackKey = map[3]
        if callbackKey then
            local cbinfor = __callbackInfor[callbackKey]
            if cbinfor then
                pcall(cbinfor[1], cbinfor[2], result)
            end
            __callbackInfor[callbackKey] = nil
        end
    end
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
    ---@field public msg string 返回消息
    ---@field public code number 返回值
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
    ---@field public cells table 地图数据 key=网络index, map
    ---@field public pageIdx number 一屏所在的网格index 
    NetProtoIsland.ST_mapPage = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = NetProtoIsland._toList(NetProtoIsland.ST_mapCell, m.cells)  -- 地图数据 key=网络index, map
            r[13] = m.pageIdx  -- 一屏所在的网格index  int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.cells = NetProtoIsland._parseList(NetProtoIsland.ST_mapCell, m[12])  -- 地图数据 key=网络index, map
            r.pageIdx = m[13] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_dockyardShips 造船厂的舰船信息
    ---@field public shipsMap table key=舰船的配置id, val=舰船数量 map
    ---@field public buildingIdx number 造船厂的idx
    NetProtoIsland.ST_dockyardShips = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[14] = m.shipsMap  -- key=舰船的配置id, val=舰船数量 map
            r[15] = m.buildingIdx  -- 造船厂的idx int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.shipsMap = m[14] --  table
            r.buildingIdx = m[15] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_tile 建筑信息对象
    ---@field public idx number 唯一标识 int
    ---@field public attrid number 属性配置id int
    ---@field public cidx number 主城idx int
    ---@field public pos number 位置，即在城的gird中的index int
    NetProtoIsland.ST_tile = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[16] = m.idx  -- 唯一标识 int int
            r[17] = m.attrid  -- 属性配置id int int
            r[18] = m.cidx  -- 主城idx int int
            r[19] = m.pos  -- 位置，即在城的gird中的index int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[16] --  int
            r.attrid = m[17] --  int
            r.cidx = m[18] --  int
            r.pos = m[19] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_building 建筑信息对象
    ---@field public idx number 唯一标识 int
    ---@field public val4 number 值4。如:产量，仓库的存储量等 int
    ---@field public val3 number 值3。如:产量，仓库的存储量等 int
    ---@field public val2 number 值2。如:产量，仓库的存储量等 int
    ---@field public endtime number 完成升级、恢复、采集等的时间点 long
    ---@field public lev number 等级 int
    ---@field public val number 值。如:产量，仓库的存储量等 int
    ---@field public cidx number 主城idx int
    ---@field public val5 number 值5。如:产量，仓库的存储量等 int
    ---@field public attrid number 属性配置id int
    ---@field public starttime number 开始升级、恢复、采集等的时间点 long
    ---@field public state number 状态. 0：正常；1：升级中；9：恢复中
    ---@field public pos number 位置，即在城的gird中的index int
    NetProtoIsland.ST_building = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[16] = m.idx  -- 唯一标识 int int
            r[20] = m.val4  -- 值4。如:产量，仓库的存储量等 int int
            r[21] = m.val3  -- 值3。如:产量，仓库的存储量等 int int
            r[22] = m.val2  -- 值2。如:产量，仓库的存储量等 int int
            r[23] = m.endtime  -- 完成升级、恢复、采集等的时间点 long int
            r[24] = m.lev  -- 等级 int int
            r[25] = m.val  -- 值。如:产量，仓库的存储量等 int int
            r[18] = m.cidx  -- 主城idx int int
            r[26] = m.val5  -- 值5。如:产量，仓库的存储量等 int int
            r[17] = m.attrid  -- 属性配置id int int
            r[27] = m.starttime  -- 开始升级、恢复、采集等的时间点 long int
            r[28] = m.state  -- 状态. 0：正常；1：升级中；9：恢复中 int
            r[19] = m.pos  -- 位置，即在城的gird中的index int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[16] --  int
            r.val4 = m[20] --  int
            r.val3 = m[21] --  int
            r.val2 = m[22] --  int
            r.endtime = m[23] --  int
            r.lev = m[24] --  int
            r.val = m[25] --  int
            r.cidx = m[18] --  int
            r.val5 = m[26] --  int
            r.attrid = m[17] --  int
            r.starttime = m[27] --  int
            r.state = m[28] --  int
            r.pos = m[19] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_mapCell 大地图地块数据
    ---@field public idx number 网格index
    ---@field public pageIdx number 所在屏的index
    ---@field public val2 number 值2
    ---@field public type number 地块类型 3：玩家，4：npc
    ---@field public lev number 等级
    ---@field public val1 number 值1
    ---@field public cidx number 主城idx
    ---@field public attrid number 配置id
    ---@field public val3 number 值3
    ---@field public state number 状态  1:正常; int
    ---@field public name string 名称
    NetProtoIsland.ST_mapCell = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[16] = m.idx  -- 网格index int
            r[13] = m.pageIdx  -- 所在屏的index int
            r[22] = m.val2  -- 值2 int
            r[30] = m.type  -- 地块类型 3：玩家，4：npc int
            r[24] = m.lev  -- 等级 int
            r[29] = m.val1  -- 值1 int
            r[18] = m.cidx  -- 主城idx int
            r[17] = m.attrid  -- 配置id int
            r[21] = m.val3  -- 值3 int
            r[28] = m.state  -- 状态  1:正常; int int
            r[35] = m.name  -- 名称 string
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[16] --  int
            r.pageIdx = m[13] --  int
            r.val2 = m[22] --  int
            r.type = m[30] --  int
            r.lev = m[24] --  int
            r.val1 = m[29] --  int
            r.cidx = m[18] --  int
            r.attrid = m[17] --  int
            r.val3 = m[21] --  int
            r.state = m[28] --  int
            r.name = m[35] --  string
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_resInfor 资源信息
    ---@field public oil number 油
    ---@field public gold number 金
    ---@field public food number 粮
    NetProtoIsland.ST_resInfor = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[31] = m.oil  -- 油 int
            r[32] = m.gold  -- 金 int
            r[33] = m.food  -- 粮 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.oil = m[31] --  int
            r.gold = m[32] --  int
            r.food = m[33] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_city 主城
    ---@field public idx number 唯一标识 int
    ---@field public tiles table 地块信息 key=idx, map
    ---@field public name string 名称
    ---@field public buildings table 建筑信息 key=idx, map
    ---@field public lev number 等级 int
    ---@field public status number 状态 1:正常; int
    ---@field public pos number 城所在世界grid的index int
    ---@field public pidx number 玩家idx int
    NetProtoIsland.ST_city = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[16] = m.idx  -- 唯一标识 int int
            r[34] = NetProtoIsland._toMap(NetProtoIsland.ST_tile, m.tiles)  -- 地块信息 key=idx, map
            r[35] = m.name  -- 名称 string
            r[36] = NetProtoIsland._toMap(NetProtoIsland.ST_building, m.buildings)  -- 建筑信息 key=idx, map
            r[24] = m.lev  -- 等级 int int
            r[37] = m.status  -- 状态 1:正常; int int
            r[19] = m.pos  -- 城所在世界grid的index int int
            r[38] = m.pidx  -- 玩家idx int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[16] --  int
            r.tiles = NetProtoIsland._parseMap(NetProtoIsland.ST_tile, m[34])  -- 地块信息 key=idx, map
            r.name = m[35] --  string
            r.buildings = NetProtoIsland._parseMap(NetProtoIsland.ST_building, m[36])  -- 建筑信息 key=idx, map
            r.lev = m[24] --  int
            r.status = m[37] --  int
            r.pos = m[19] --  int
            r.pidx = m[38] --  int
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_netCfg 网络协议解析配置
    ---@field public encryptType number 加密类别，1：只加密客户端，2：只加密服务器，3：前后端都加密，0及其它情况：不加密
    ---@field public checkTimeStamp useData 检测时间戳
    ---@field public secretKey string 密钥
    NetProtoIsland.ST_netCfg = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[83] = m.encryptType  -- 加密类别，1：只加密客户端，2：只加密服务器，3：前后端都加密，0及其它情况：不加密 int
            r[85] = m.checkTimeStamp  -- 检测时间戳 boolean
            r[84] = m.secretKey  -- 密钥 string
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.encryptType = m[83] --  int
            r.checkTimeStamp = m[85] --  boolean
            r.secretKey = m[84] --  string
            return r;
        end,
    }
    ---@class NetProtoIsland.ST_player 用户信息
    ---@field public idx number 唯一标识 int
    ---@field public diam number 钻石 long
    ---@field public name string 名字
    ---@field public unionidx number 联盟id int
    ---@field public cityidx number 城池id int
    ---@field public lev number 等级 long
    ---@field public status number 状态 1：正常 int
    NetProtoIsland.ST_player = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[16] = m.idx  -- 唯一标识 int int
            r[39] = m.diam  -- 钻石 long int
            r[35] = m.name  -- 名字 string
            r[41] = m.unionidx  -- 联盟id int int
            r[40] = m.cityidx  -- 城池id int int
            r[24] = m.lev  -- 等级 long int
            r[37] = m.status  -- 状态 1：正常 int int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[16] --  int
            r.diam = m[39] --  int
            r.name = m[35] --  string
            r.unionidx = m[41] --  int
            r.cityidx = m[40] --  int
            r.lev = m[24] --  int
            r.status = m[37] --  int
            return r;
        end,
    }
    --==============================
    NetProtoIsland.send = {
    -- 取得造船厂所有舰艇列表
    getShipsByBuildingIdx = function(buildingIdx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 42
        ret[1] = NetProtoIsland.__sessionID
        ret[15] = buildingIdx; -- 造船厂的idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 升级建筑
    upLevBuilding = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 44
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 建筑idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 移除建筑
    rmBuilding = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 46
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 地块idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 新建建筑
    newBuilding = function(attrid, pos, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 47
        ret[1] = NetProtoIsland.__sessionID
        ret[17] = attrid; -- 建筑配置id int
        ret[19] = pos; -- 位置 int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 登陆
    login = function(uidx, channel, deviceID, isEditMode, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 48
        ret[1] = NetProtoIsland.__sessionID
        ret[49] = uidx; -- 用户id
        ret[50] = channel; -- 渠道号
        ret[51] = deviceID; -- 机器码
        ret[52] = isEditMode; -- 编辑模式
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 自己的城变化时推送
    onMyselfCityChg = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 89
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 当完成建造部分舰艇的通知
    onFinishBuildOneShip = function(buildingIdx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 57
        ret[1] = NetProtoIsland.__sessionID
        ret[15] = buildingIdx; -- 造船厂的idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 网络协议配置
    sendNetCfg = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 81
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 取得建筑
    getBuilding = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 60
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 建筑idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 移除地块
    rmTile = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 61
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 地块idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 造船
    buildShip = function(buildingIdx, shipAttrID, num, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 66
        ret[1] = NetProtoIsland.__sessionID
        ret[15] = buildingIdx; -- 造船厂的idx int
        ret[58] = shipAttrID; -- 舰船配置id int
        ret[67] = num; -- 数量 int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 当地块发生变化时推送
    onMapCellChg = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 86
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 资源变化时推送
    onResChg = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 62
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 取得一屏的在地图数据
    getMapDataByPageIdx = function(pageIdx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 74
        ret[1] = NetProtoIsland.__sessionID
        ret[13] = pageIdx; -- 一屏所在的网格index
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 移动建筑
    moveBuilding = function(idx, pos, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 64
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 建筑idx int
        ret[19] = pos; -- 位置 int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 登出
    logout = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 65
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 搬迁
    moveCity = function(cidx, pos, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 88
        ret[1] = NetProtoIsland.__sessionID
        ret[18] = cidx; -- 城市idx
        ret[19] = pos; -- 新位置 int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 立即升级建筑
    upLevBuildingImm = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 68
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 建筑idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 新建地块
    newTile = function(pos, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 69
        ret[1] = NetProtoIsland.__sessionID
        ret[19] = pos; -- 位置 int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 建筑变化时推送
    onBuildingChg = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 71
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 玩家信息变化时推送
    onPlayerChg = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 72
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 心跳
    heart = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 73
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 收集资源
    collectRes = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 77
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 资源建筑的idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 移动地块
    moveTile = function(idx, pos, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 76
        ret[1] = NetProtoIsland.__sessionID
        ret[16] = idx; -- 地块idx int
        ret[19] = pos; -- 位置 int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 建筑升级完成
    onFinishBuildingUpgrade = function(__callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 80
        ret[1] = NetProtoIsland.__sessionID
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 攻击
    attack = function(pos, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 90
        ret[1] = NetProtoIsland.__sessionID
        ret[19] = pos; -- 世界地图坐标idx int
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    }
    --==============================
    NetProtoIsland.recive = {
    ---@class NetProtoIsland.RC_getShipsByBuildingIdx
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public dockyardShips NetProtoIsland.ST_dockyardShips 造船厂的idx int
    getShipsByBuildingIdx = function(map)
        local ret = {}
        ret.cmd = "getShipsByBuildingIdx"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.dockyardShips = NetProtoIsland.ST_dockyardShips.parse(map[43]) -- 造船厂的idx int
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_upLevBuilding
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息
    upLevBuilding = function(map)
        local ret = {}
        ret.cmd = "upLevBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_rmBuilding
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public idx  被移除建筑的idx int
    rmBuilding = function(map)
        local ret = {}
        ret.cmd = "rmBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.idx = map[16]-- 被移除建筑的idx int
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_newBuilding
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息对象
    newBuilding = function(map)
        local ret = {}
        ret.cmd = "newBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息对象
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_login
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public player NetProtoIsland.ST_player 玩家信息
    ---@field public city NetProtoIsland.ST_city 主城信息
    ---@field public systime  系统时间 long
    ---@field public session  会话id
    login = function(map)
        local ret = {}
        ret.cmd = "login"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.player = NetProtoIsland.ST_player.parse(map[53]) -- 玩家信息
        ret.city = NetProtoIsland.ST_city.parse(map[54]) -- 主城信息
        ret.systime = map[55]-- 系统时间 long
        ret.session = map[56]-- 会话id
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onMyselfCityChg
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public city NetProtoIsland.ST_city 主城信息
    onMyselfCityChg = function(map)
        local ret = {}
        ret.cmd = "onMyselfCityChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.city = NetProtoIsland.ST_city.parse(map[54]) -- 主城信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onFinishBuildOneShip
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public buildingIdx  造船厂的idx int
    ---@field public shipAttrID  舰船的配置id
    ---@field public shipNum  舰船的数量
    onFinishBuildOneShip = function(map)
        local ret = {}
        ret.cmd = "onFinishBuildOneShip"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.buildingIdx = map[15]-- 造船厂的idx int
        ret.shipAttrID = map[58]-- 舰船的配置id
        ret.shipNum = map[59]-- 舰船的数量
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_sendNetCfg
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public netCfg NetProtoIsland.ST_netCfg 网络协议解析配置
    ---@field public systime  系统时间 long
    sendNetCfg = function(map)
        local ret = {}
        ret.cmd = "sendNetCfg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.netCfg = NetProtoIsland.ST_netCfg.parse(map[82]) -- 网络协议解析配置
        ret.systime = map[55]-- 系统时间 long
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_getBuilding
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息对象
    getBuilding = function(map)
        local ret = {}
        ret.cmd = "getBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息对象
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_rmTile
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public idx  被移除地块的idx int
    rmTile = function(map)
        local ret = {}
        ret.cmd = "rmTile"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.idx = map[16]-- 被移除地块的idx int
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_buildShip
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 造船厂信息
    buildShip = function(map)
        local ret = {}
        ret.cmd = "buildShip"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 造船厂信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onMapCellChg
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public mapCell NetProtoIsland.ST_mapCell 地块
    ---@field public isRemove  是否是删除
    onMapCellChg = function(map)
        local ret = {}
        ret.cmd = "onMapCellChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.mapCell = NetProtoIsland.ST_mapCell.parse(map[87]) -- 地块
        ret.isRemove = map[98]-- 是否是删除
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onResChg
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public resInfor NetProtoIsland.ST_resInfor 资源信息
    onResChg = function(map)
        local ret = {}
        ret.cmd = "onResChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.resInfor = NetProtoIsland.ST_resInfor.parse(map[63]) -- 资源信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_getMapDataByPageIdx
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public mapPage NetProtoIsland.ST_mapPage 在地图一屏数据 map
    getMapDataByPageIdx = function(map)
        local ret = {}
        ret.cmd = "getMapDataByPageIdx"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.mapPage = NetProtoIsland.ST_mapPage.parse(map[75]) -- 在地图一屏数据 map
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_moveBuilding
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息
    moveBuilding = function(map)
        local ret = {}
        ret.cmd = "moveBuilding"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_logout
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    logout = function(map)
        local ret = {}
        ret.cmd = "logout"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_moveCity
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    moveCity = function(map)
        local ret = {}
        ret.cmd = "moveCity"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_upLevBuildingImm
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息
    upLevBuildingImm = function(map)
        local ret = {}
        ret.cmd = "upLevBuildingImm"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_newTile
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public tile NetProtoIsland.ST_tile 地块信息对象
    newTile = function(map)
        local ret = {}
        ret.cmd = "newTile"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.tile = NetProtoIsland.ST_tile.parse(map[70]) -- 地块信息对象
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onBuildingChg
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息
    onBuildingChg = function(map)
        local ret = {}
        ret.cmd = "onBuildingChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onPlayerChg
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public player NetProtoIsland.ST_player 玩家信息
    onPlayerChg = function(map)
        local ret = {}
        ret.cmd = "onPlayerChg"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.player = NetProtoIsland.ST_player.parse(map[53]) -- 玩家信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_heart
    heart = function(map)
        local ret = {}
        ret.cmd = "heart"
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_collectRes
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public resType  收集的资源类型 int
    ---@field public resVal  收集到的资源量 int
    ---@field public building NetProtoIsland.ST_building 建筑信息
    collectRes = function(map)
        local ret = {}
        ret.cmd = "collectRes"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.resType = map[78]-- 收集的资源类型 int
        ret.resVal = map[79]-- 收集到的资源量 int
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_moveTile
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public tile NetProtoIsland.ST_tile 地块信息
    moveTile = function(map)
        local ret = {}
        ret.cmd = "moveTile"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.tile = NetProtoIsland.ST_tile.parse(map[70]) -- 地块信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_onFinishBuildingUpgrade
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public building NetProtoIsland.ST_building 建筑信息
    onFinishBuildingUpgrade = function(map)
        local ret = {}
        ret.cmd = "onFinishBuildingUpgrade"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.building = NetProtoIsland.ST_building.parse(map[45]) -- 建筑信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoIsland.RC_attack
    ---@field public retInfor NetProtoIsland.ST_retInfor 返回信息
    ---@field public player NetProtoIsland.ST_player 被攻击玩家信息
    ---@field public city NetProtoIsland.ST_city 被攻击主城信息
    ---@field public dockyardShipss NetProtoIsland.ST_dockyardShips Array List 被攻击航船的数据
    ---@field public dockyardShipss2 NetProtoIsland.ST_dockyardShips Array List 进攻击方航船的数据
    attack = function(map)
        local ret = {}
        ret.cmd = "attack"
        ret.retInfor = NetProtoIsland.ST_retInfor.parse(map[2]) -- 返回信息
        ret.player = NetProtoIsland.ST_player.parse(map[53]) -- 被攻击玩家信息
        ret.city = NetProtoIsland.ST_city.parse(map[54]) -- 被攻击主城信息
        ret.dockyardShipss = NetProtoIsland._parseList(NetProtoIsland.ST_dockyardShips, map[91]) -- 被攻击航船的数据
        ret.dockyardShipss2 = NetProtoIsland._parseList(NetProtoIsland.ST_dockyardShips, map[97]) -- 进攻击方航船的数据
        doCallback(map, ret)
        return ret
    end,
    }
    --==============================
    NetProtoIsland.dispatch[42]={onReceive = NetProtoIsland.recive.getShipsByBuildingIdx, send = NetProtoIsland.send.getShipsByBuildingIdx}
    NetProtoIsland.dispatch[44]={onReceive = NetProtoIsland.recive.upLevBuilding, send = NetProtoIsland.send.upLevBuilding}
    NetProtoIsland.dispatch[46]={onReceive = NetProtoIsland.recive.rmBuilding, send = NetProtoIsland.send.rmBuilding}
    NetProtoIsland.dispatch[47]={onReceive = NetProtoIsland.recive.newBuilding, send = NetProtoIsland.send.newBuilding}
    NetProtoIsland.dispatch[48]={onReceive = NetProtoIsland.recive.login, send = NetProtoIsland.send.login}
    NetProtoIsland.dispatch[89]={onReceive = NetProtoIsland.recive.onMyselfCityChg, send = NetProtoIsland.send.onMyselfCityChg}
    NetProtoIsland.dispatch[57]={onReceive = NetProtoIsland.recive.onFinishBuildOneShip, send = NetProtoIsland.send.onFinishBuildOneShip}
    NetProtoIsland.dispatch[81]={onReceive = NetProtoIsland.recive.sendNetCfg, send = NetProtoIsland.send.sendNetCfg}
    NetProtoIsland.dispatch[60]={onReceive = NetProtoIsland.recive.getBuilding, send = NetProtoIsland.send.getBuilding}
    NetProtoIsland.dispatch[61]={onReceive = NetProtoIsland.recive.rmTile, send = NetProtoIsland.send.rmTile}
    NetProtoIsland.dispatch[66]={onReceive = NetProtoIsland.recive.buildShip, send = NetProtoIsland.send.buildShip}
    NetProtoIsland.dispatch[86]={onReceive = NetProtoIsland.recive.onMapCellChg, send = NetProtoIsland.send.onMapCellChg}
    NetProtoIsland.dispatch[62]={onReceive = NetProtoIsland.recive.onResChg, send = NetProtoIsland.send.onResChg}
    NetProtoIsland.dispatch[74]={onReceive = NetProtoIsland.recive.getMapDataByPageIdx, send = NetProtoIsland.send.getMapDataByPageIdx}
    NetProtoIsland.dispatch[64]={onReceive = NetProtoIsland.recive.moveBuilding, send = NetProtoIsland.send.moveBuilding}
    NetProtoIsland.dispatch[65]={onReceive = NetProtoIsland.recive.logout, send = NetProtoIsland.send.logout}
    NetProtoIsland.dispatch[88]={onReceive = NetProtoIsland.recive.moveCity, send = NetProtoIsland.send.moveCity}
    NetProtoIsland.dispatch[68]={onReceive = NetProtoIsland.recive.upLevBuildingImm, send = NetProtoIsland.send.upLevBuildingImm}
    NetProtoIsland.dispatch[69]={onReceive = NetProtoIsland.recive.newTile, send = NetProtoIsland.send.newTile}
    NetProtoIsland.dispatch[71]={onReceive = NetProtoIsland.recive.onBuildingChg, send = NetProtoIsland.send.onBuildingChg}
    NetProtoIsland.dispatch[72]={onReceive = NetProtoIsland.recive.onPlayerChg, send = NetProtoIsland.send.onPlayerChg}
    NetProtoIsland.dispatch[73]={onReceive = NetProtoIsland.recive.heart, send = NetProtoIsland.send.heart}
    NetProtoIsland.dispatch[77]={onReceive = NetProtoIsland.recive.collectRes, send = NetProtoIsland.send.collectRes}
    NetProtoIsland.dispatch[76]={onReceive = NetProtoIsland.recive.moveTile, send = NetProtoIsland.send.moveTile}
    NetProtoIsland.dispatch[80]={onReceive = NetProtoIsland.recive.onFinishBuildingUpgrade, send = NetProtoIsland.send.onFinishBuildingUpgrade}
    NetProtoIsland.dispatch[90]={onReceive = NetProtoIsland.recive.attack, send = NetProtoIsland.send.attack}
    --==============================
    NetProtoIsland.cmds = {
        getShipsByBuildingIdx = "getShipsByBuildingIdx", -- 取得造船厂所有舰艇列表,
        upLevBuilding = "upLevBuilding", -- 升级建筑,
        rmBuilding = "rmBuilding", -- 移除建筑,
        newBuilding = "newBuilding", -- 新建建筑,
        login = "login", -- 登陆,
        onMyselfCityChg = "onMyselfCityChg", -- 自己的城变化时推送,
        onFinishBuildOneShip = "onFinishBuildOneShip", -- 当完成建造部分舰艇的通知,
        sendNetCfg = "sendNetCfg", -- 网络协议配置,
        getBuilding = "getBuilding", -- 取得建筑,
        rmTile = "rmTile", -- 移除地块,
        buildShip = "buildShip", -- 造船,
        onMapCellChg = "onMapCellChg", -- 当地块发生变化时推送,
        onResChg = "onResChg", -- 资源变化时推送,
        getMapDataByPageIdx = "getMapDataByPageIdx", -- 取得一屏的在地图数据,
        moveBuilding = "moveBuilding", -- 移动建筑,
        logout = "logout", -- 登出,
        moveCity = "moveCity", -- 搬迁,
        upLevBuildingImm = "upLevBuildingImm", -- 立即升级建筑,
        newTile = "newTile", -- 新建地块,
        onBuildingChg = "onBuildingChg", -- 建筑变化时推送,
        onPlayerChg = "onPlayerChg", -- 玩家信息变化时推送,
        heart = "heart", -- 心跳,
        collectRes = "collectRes", -- 收集资源,
        moveTile = "moveTile", -- 移动地块,
        onFinishBuildingUpgrade = "onFinishBuildingUpgrade", -- 建筑升级完成,
        attack = "attack", -- 攻击
    }
    --==============================
    return NetProtoIsland
end
