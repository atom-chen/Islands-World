do
    ---@class NetProtoUsermgr
    NetProtoUsermgr = {}
    local table = table
    require("bio.BioUtl")

    NetProtoUsermgr.__sessionID = 0; -- 会话ID
    NetProtoUsermgr.dispatch = {}
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
            r.msg = m[10] --  string
            r.code = m[11] --  int
            return r;
        end,
    }
    ---@class NetProtoUsermgr.ST_server 服务器
    NetProtoUsermgr.ST_server = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[13] = m.idx  -- id int
            r[41] = m.port  -- 端口 int
            r[14] = m.name  -- 名称 string
            r[42] = m.host  -- ip地址 string
            r[38] = m.iosVer  -- 客户端ios版本 string
            r[15] = m.status  -- 状态 1:正常; 2:爆满; 3:维护 int
            r[39] = m.androidVer  -- 客户端android版本 string
            r[34] = m.isnew  -- 新服 boolean
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[13] --  int
            r.port = m[41] --  int
            r.name = m[14] --  string
            r.host = m[42] --  string
            r.iosVer = m[38] --  string
            r.status = m[15] --  int
            r.androidVer = m[39] --  string
            r.isnew = m[34] --  boolean
            return r;
        end,
    }
    ---@class NetProtoUsermgr.ST_userInfor 用户信息
    NetProtoUsermgr.ST_userInfor = {
        toMap = function(m)
            local r = {}
            if m == nil then return r end
            r[13] = m.idx  -- 唯一标识 int
            return r;
        end,
        parse = function(m)
            local r = {}
            if m == nil then return r end
            r.idx = m[13] --  int
            return r;
        end,
    }
    --==============================
    NetProtoUsermgr.send = {
    -- 注册
    registAccount = function(userId, password, email, appid, channel, deviceID, deviceInfor)
        local ret = {}
        ret[0] = 36
        ret[1] = NetProtoUsermgr.__sessionID
        ret[21] = userId; -- 用户名
        ret[22] = password; -- 密码
        ret[43] = email; -- 邮箱
        ret[17] = appid; -- 应用id
        ret[25] = channel; -- 渠道号
        ret[26] = deviceID; -- 机器码
        ret[27] = deviceInfor; -- 机器信息
        return ret
    end,
    -- 取得服务器列表
    getServers = function(appid, channel)
        local ret = {}
        ret[0] = 16
        ret[1] = NetProtoUsermgr.__sessionID
        ret[17] = appid; -- 应用id
        ret[25] = channel; -- 渠道号
        return ret
    end,
    -- 取得服务器信息
    getServerInfor = function(idx)
        local ret = {}
        ret[0] = 32
        ret[1] = NetProtoUsermgr.__sessionID
        ret[13] = idx; -- 服务器id
        return ret
    end,
    -- 保存所选服务器
    setEnterServer = function(sidx, uidx, appid)
        local ret = {}
        ret[0] = 29
        ret[1] = NetProtoUsermgr.__sessionID
        ret[30] = sidx; -- 服务器id
        ret[31] = uidx; -- 用户id
        ret[17] = appid; -- 应用id
        return ret
    end,
    -- 登陆
    loginAccount = function(userId, password, appid, channel)
        local ret = {}
        ret[0] = 37
        ret[1] = NetProtoUsermgr.__sessionID
        ret[21] = userId; -- 用户名
        ret[22] = password; -- 密码
        ret[17] = appid; -- 应用id int
        ret[25] = channel; -- 渠道号 string
        return ret
    end,
    -- 渠道登陆
    loginAccountChannel = function(userId, appid, channel, deviceID, deviceInfor)
        local ret = {}
        ret[0] = 40
        ret[1] = NetProtoUsermgr.__sessionID
        ret[21] = userId; -- 用户名
        ret[17] = appid; -- 应用id int
        ret[25] = channel; -- 渠道号 string
        ret[26] = deviceID; -- 
        ret[27] = deviceInfor; -- 
        return ret
    end,
    }
    --==============================
    NetProtoUsermgr.recive = {
    registAccount = function(map)
        local ret = {}
        ret.cmd = "registAccount"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.userInfor = NetProtoUsermgr.ST_userInfor.parse(map[23]) -- 用户信息
        ret.serverid = map[28]-- 服务器id int
        ret.systime = map[35]-- 系统时间 long
        return ret
    end,
    getServers = function(map)
        local ret = {}
        ret.cmd = "getServers"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.servers = NetProtoUsermgr._parseList(NetProtoUsermgr.ST_server, map[19]) -- 服务器列表
        return ret
    end,
    getServerInfor = function(map)
        local ret = {}
        ret.cmd = "getServerInfor"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.server = NetProtoUsermgr.ST_server.parse(map[33]) -- 服务器信息
        return ret
    end,
    setEnterServer = function(map)
        local ret = {}
        ret.cmd = "setEnterServer"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        return ret
    end,
    loginAccount = function(map)
        local ret = {}
        ret.cmd = "loginAccount"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.userInfor = NetProtoUsermgr.ST_userInfor.parse(map[23]) -- 用户信息
        ret.serverid = map[28]-- 服务器id int
        ret.systime = map[35]-- 系统时间 long
        return ret
    end,
    loginAccountChannel = function(map)
        local ret = {}
        ret.cmd = "loginAccountChannel"
        ret.retInfor = NetProtoUsermgr.ST_retInfor.parse(map[2]) -- 返回信息
        ret.userInfor = NetProtoUsermgr.ST_userInfor.parse(map[23]) -- 用户信息
        ret.serverid = map[28]-- 服务器id int
        ret.systime = map[35]-- 系统时间 long
        return ret
    end,
    }
    --==============================
    NetProtoUsermgr.dispatch[36]={onReceive = NetProtoUsermgr.recive.registAccount, send = NetProtoUsermgr.send.registAccount}
    NetProtoUsermgr.dispatch[16]={onReceive = NetProtoUsermgr.recive.getServers, send = NetProtoUsermgr.send.getServers}
    NetProtoUsermgr.dispatch[32]={onReceive = NetProtoUsermgr.recive.getServerInfor, send = NetProtoUsermgr.send.getServerInfor}
    NetProtoUsermgr.dispatch[29]={onReceive = NetProtoUsermgr.recive.setEnterServer, send = NetProtoUsermgr.send.setEnterServer}
    NetProtoUsermgr.dispatch[37]={onReceive = NetProtoUsermgr.recive.loginAccount, send = NetProtoUsermgr.send.loginAccount}
    NetProtoUsermgr.dispatch[40]={onReceive = NetProtoUsermgr.recive.loginAccountChannel, send = NetProtoUsermgr.send.loginAccountChannel}
    --==============================
    NetProtoUsermgr.cmds = {
        registAccount = "registAccount",
        getServers = "getServers",
        getServerInfor = "getServerInfor",
        setEnterServer = "setEnterServer",
        loginAccount = "loginAccount",
        loginAccountChannel = "loginAccountChannel"
    }
    --==============================
    return NetProtoUsermgr
end
