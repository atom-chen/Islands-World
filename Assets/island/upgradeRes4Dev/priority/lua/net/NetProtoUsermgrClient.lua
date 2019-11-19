do
    ---@class NetProtoUsermgr 网络协议
    NetProtoUsermgr = {}
    local table = table
    require("bio.BioUtl")

    NetProtoUsermgr.__sessionID = 0 -- 会话ID
    NetProtoUsermgr.dispatch = {}
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
    NetProtoUsermgr._toMap = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for k,v in pairs(m) do
            ret[k] = stuctobj.toMap(v)
        end
        return ret
    end
    -- public toList
    NetProtoUsermgr._toList = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for i,v in ipairs(m) do
            table.insert(ret, stuctobj.toMap(v))
        end
        return ret
    end
    -- public parse
    NetProtoUsermgr._parseMap = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for k,v in pairs(m) do
            ret[k] = stuctobj.parse(v)
        end
        return ret
    end
    -- public parse
    NetProtoUsermgr._parseList = function(stuctobj, m)
        local ret = {}
        if m == nil then return ret end
        for i,v in ipairs(m) do
            table.insert(ret, stuctobj.parse(v))
        end
        return ret
    end
  --==================================
  --==================================
    ---@class NetProtoUsermgr.ST_retInfor 返回信息
    ---@field public msg string 返回消息
    ---@field public code number 返回值
    NetProtoUsermgr.ST_retInfor = {
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
            r.msg = m[10] or m["10"] --  string
            r.code = m[11] or m["11"] --  int
            return r;
        end,
    }
    ---@class NetProtoUsermgr.ST_server 服务器
    ---@field public idx number id
    ---@field public port number 端口
    ---@field public name string 名称
    ---@field public host string ip地址
    ---@field public iosVer string 客户端ios版本
    ---@field public androidVer string 客户端android版本
    ---@field public isnew useData 新服
    ---@field public status number 状态 1:正常; 2:爆满; 3:维护
    NetProtoUsermgr.ST_server = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- id int
            r[15] = m.port  -- 端口 int
            r[14] = m.name  -- 名称 string
            r[16] = m.host  -- ip地址 string
            r[17] = m.iosVer  -- 客户端ios版本 string
            r[18] = m.androidVer  -- 客户端android版本 string
            r[19] = m.isnew  -- 新服 boolean
            r[13] = m.status  -- 状态 1:正常; 2:爆满; 3:维护 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] or m["12"] --  int
            r.port = m[15] or m["15"] --  int
            r.name = m[14] or m["14"] --  string
            r.host = m[16] or m["16"] --  string
            r.iosVer = m[17] or m["17"] --  string
            r.androidVer = m[18] or m["18"] --  string
            r.isnew = m[19] or m["19"] --  boolean
            r.status = m[13] or m["13"] --  int
            return r;
        end,
    }
    ---@class NetProtoUsermgr.ST_userInfor 用户信息
    ---@field public idx number 唯一标识
    NetProtoUsermgr.ST_userInfor = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[12] = m.idx  -- 唯一标识 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[12] or m["12"] --  int
            return r;
        end,
    }
    --==============================
    NetProtoUsermgr.send = {
    -- 注册
    registAccount = function(userId, password, email, appid, channel, deviceID, deviceInfor, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 20
        ret[1] = NetProtoUsermgr.__sessionID
        ret[21] = userId; -- 用户名
        ret[22] = password; -- 密码
        ret[23] = email; -- 邮箱
        ret[24] = appid; -- 应用id
        ret[25] = channel; -- 渠道号
        ret[26] = deviceID; -- 机器码
        ret[27] = deviceInfor; -- 机器信息
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 取得服务器列表
    getServers = function(appid, channel, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 31
        ret[1] = NetProtoUsermgr.__sessionID
        ret[24] = appid; -- 应用id
        ret[25] = channel; -- 渠道号
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 取得服务器信息
    getServerInfor = function(idx, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 33
        ret[1] = NetProtoUsermgr.__sessionID
        ret[12] = idx; -- 服务器id
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 保存所选服务器
    setEnterServer = function(sidx, uidx, appid, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 35
        ret[1] = NetProtoUsermgr.__sessionID
        ret[36] = sidx; -- 服务器id
        ret[37] = uidx; -- 用户id
        ret[24] = appid; -- 应用id
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 登陆
    loginAccount = function(userId, password, appid, channel, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 38
        ret[1] = NetProtoUsermgr.__sessionID
        ret[21] = userId; -- 用户名
        ret[22] = password; -- 密码
        ret[24] = appid; -- 应用id int
        ret[25] = channel; -- 渠道号 string
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    -- 渠道登陆
    loginAccountChannel = function(userId, appid, channel, deviceID, deviceInfor, __callback, __orgs) -- __callback:接口回调, __orgs:回调参数
        local ret = {}
        ret[0] = 39
        ret[1] = NetProtoUsermgr.__sessionID
        ret[21] = userId; -- 用户名
        ret[24] = appid; -- 应用id int
        ret[25] = channel; -- 渠道号 string
        ret[26] = deviceID; -- 
        ret[27] = deviceInfor; -- 
        setCallback(__callback, __orgs, ret)
        return ret
    end,
    }
    --==============================
    NetProtoUsermgr.recive = {
    ---@class NetProtoUsermgr.RC_registAccount
    ---@field public retInfor NetProtoUsermgr.ST_retInfor 返回信息
    ---@field public userInfor NetProtoUsermgr.ST_userInfor 用户信息
    ---@field public serverid  服务器id int
    ---@field public systime  系统时间 long
    registAccount = function(map)
        local ret = {}
        ret.cmd = "registAccount"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.userInfor = NetProtoUsermgr.ST_userInfor.parse(map[28]) -- 用户信息
        ret.serverid = map[29]-- 服务器id int
        ret.systime = map[30]-- 系统时间 long
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoUsermgr.RC_getServers
    ---@field public retInfor NetProtoUsermgr.ST_retInfor 返回信息
    ---@field public servers NetProtoUsermgr.ST_server Array List 服务器列表
    getServers = function(map)
        local ret = {}
        ret.cmd = "getServers"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.servers = NetProtoUsermgr._parseList(NetProtoUsermgr.ST_server, map[32]) -- 服务器列表
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoUsermgr.RC_getServerInfor
    ---@field public retInfor NetProtoUsermgr.ST_retInfor 返回信息
    ---@field public server NetProtoUsermgr.ST_server 服务器信息
    getServerInfor = function(map)
        local ret = {}
        ret.cmd = "getServerInfor"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.server = NetProtoUsermgr.ST_server.parse(map[34]) -- 服务器信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoUsermgr.RC_setEnterServer
    ---@field public retInfor NetProtoUsermgr.ST_retInfor 返回信息
    setEnterServer = function(map)
        local ret = {}
        ret.cmd = "setEnterServer"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoUsermgr.RC_loginAccount
    ---@field public retInfor NetProtoUsermgr.ST_retInfor 返回信息
    ---@field public userInfor NetProtoUsermgr.ST_userInfor 用户信息
    ---@field public serverid  服务器id int
    ---@field public systime  系统时间 long
    loginAccount = function(map)
        local ret = {}
        ret.cmd = "loginAccount"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.userInfor = NetProtoUsermgr.ST_userInfor.parse(map[28]) -- 用户信息
        ret.serverid = map[29]-- 服务器id int
        ret.systime = map[30]-- 系统时间 long
        doCallback(map, ret)
        return ret
    end,
    ---@class NetProtoUsermgr.RC_loginAccountChannel
    ---@field public retInfor NetProtoUsermgr.ST_retInfor 返回信息
    ---@field public userInfor NetProtoUsermgr.ST_userInfor 用户信息
    ---@field public serverid  服务器id int
    ---@field public systime  系统时间 long
    loginAccountChannel = function(map)
        local ret = {}
        ret.cmd = "loginAccountChannel"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.userInfor = NetProtoUsermgr.ST_userInfor.parse(map[28]) -- 用户信息
        ret.serverid = map[29]-- 服务器id int
        ret.systime = map[30]-- 系统时间 long
        doCallback(map, ret)
        return ret
    end,
    }
    --==============================
    NetProtoUsermgr.dispatch[20]={onReceive = NetProtoUsermgr.recive.registAccount, send = NetProtoUsermgr.send.registAccount}
    NetProtoUsermgr.dispatch[31]={onReceive = NetProtoUsermgr.recive.getServers, send = NetProtoUsermgr.send.getServers}
    NetProtoUsermgr.dispatch[33]={onReceive = NetProtoUsermgr.recive.getServerInfor, send = NetProtoUsermgr.send.getServerInfor}
    NetProtoUsermgr.dispatch[35]={onReceive = NetProtoUsermgr.recive.setEnterServer, send = NetProtoUsermgr.send.setEnterServer}
    NetProtoUsermgr.dispatch[38]={onReceive = NetProtoUsermgr.recive.loginAccount, send = NetProtoUsermgr.send.loginAccount}
    NetProtoUsermgr.dispatch[39]={onReceive = NetProtoUsermgr.recive.loginAccountChannel, send = NetProtoUsermgr.send.loginAccountChannel}
    --==============================
    NetProtoUsermgr.cmds = {
        registAccount = "registAccount", -- 注册,
        getServers = "getServers", -- 取得服务器列表,
        getServerInfor = "getServerInfor", -- 取得服务器信息,
        setEnterServer = "setEnterServer", -- 保存所选服务器,
        loginAccount = "loginAccount", -- 登陆,
        loginAccountChannel = "loginAccountChannel", -- 渠道登陆
    }
    --==============================
    return NetProtoUsermgr
end
