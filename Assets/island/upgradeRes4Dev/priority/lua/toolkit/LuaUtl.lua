-- require 'CLLFunctions'
--- lua工具方法
local table = table
local smatch = string.match
local sfind = string.find

local pnlShade = nil

function showHotWheel(...)
    if pnlShade == nil then
        pnlShade = CLPanelManager.getPanel("PanelHotWheel")
    end
    local paras = {...}
    if (#(paras) > 1) then
        local msg = paras[1]
        pnlShade:setData(msg)
    else
        pnlShade:setData("")
    end

    -- CLPanelManager.showPanel(pnlShade)
    pnlShade:show()
end

function hideHotWheel(...)
    if pnlShade == nil then
        pnlShade = CLPanelManager.getPanel("PanelHotWheel")
    end
    -- CLPanelManager.hidePanel(pnlShade)
    local func = pnlShade:getLuaFunction("hideSelf")
    if (func ~= nil) then
        func()
    end
end

function mapToColor(map)
    local color = Color.white
    if (map == nil) then
        return color
    end
    color =
        Color(
        MapEx.getNumber(map, "r"),
        MapEx.getNumber(map, "g"),
        MapEx.getNumber(map, "b"),
        MapEx.getNumber(map, "a")
    )
    return color
end

function mapToVector2(map)
    local v = Vector2.zero
    if (map == nil) then
        return v
    end
    v = Vector2(MapEx.getNumber(map, "x"), MapEx.getNumber(map, "y"))
    return v
end

function mapToVector3(map)
    local v = Vector3.zero
    if (map == nil) then
        return v
    end
    v = Vector3(MapEx.getNumber(map, "x"), MapEx.getNumber(map, "y"), MapEx.getNumber(map, "z"))
    return v
end

function mapToVector4(map)
    local v = Vector4.zero
    if (map == nil) then
        return v
    end
    v =
        Vector4(
        MapEx.getNumber(map, "x"),
        MapEx.getNumber(map, "y"),
        MapEx.getNumber(map, "z"),
        MapEx.getNumber(map, "w")
    )
    return v
end

function getChild(root, ...)
    local args = {...}
    local path = ""
    if (#args > 1) then
        local str = PStr.b()
        for i, v in ipairs(args) do
            str:a(v):a("/")
        end
        path = str:e()
    else
        path = args[1]
    end
    return root:Find(path)
end

--获取路径
function stripfilename(filename)
    return smatch(filename, "(.+)/[^/]*%.%w+$") --*nix system
    --return smatch(filename, “(.+)\\[^\\]*%.%w+$”) — windows
end

--获取文件名
function strippath(filename)
    return smatch(filename, ".+/([^/]*%.%w+)$") -- *nix system
    --return smatch(filename, “.+\\([^\\]*%.%w+)$”) — *nix system
end

--去除扩展名
function stripextension(filename)
    local idx = filename:match(".+()%.%w+$")
    if (idx) then
        return filename:sub(1, idx - 1)
    else
        return filename
    end
end

--获取扩展名
function getextension(filename)
    return filename:match(".+%.(%w+)$")
end

function trim(s)
    -- return (s:gsub("^%s*(.-)%s*$", "%1"))
    return smatch(s, "^()%s*$") and "" or smatch(s, "^%s*(.*%S)") -- 性能略优
end

function startswith(str, substr)
    if str == nil or substr == nil then
        return nil, "the string or the sub-stirng parameter is nil"
    end
    if sfind(str, substr) ~= 1 then
        return false
    else
        return true
    end
end

function getAction(act)
    local actionValue = 0
    if (act == "idel") then
        --,       //0 空闲
        return 0
    elseif (act == "idel2") then
        --,      //1 空闲
        actionValue = 1
    elseif (act == "walk") then
        --,     //2 走
        actionValue = 2
    elseif (act == "run") then
        --,        //3 跑
        actionValue = 3
    elseif (act == "jump") then
        --,     //4 跳
        actionValue = 4
    elseif (act == "slide") then
        --,      //5 滑行，滚动，闪避
        actionValue = 5
    elseif (act == "drop") then
        --,     //6 下落
        actionValue = 6
    elseif (act == "attack") then
        --,     //7 攻击
        actionValue = 7
    elseif (act == "attack2") then
        --,    //8 攻击2
        actionValue = 8
    elseif (act == "skill") then
        --,        //9 技能
        actionValue = 9
    elseif (act == "skill2") then
        --,     //10 技能2
        actionValue = 10
    elseif (act == "skill3") then
        --,     //11 技能3
        actionValue = 11
    elseif (act == "skill4") then
        --,     //12 技能4
        actionValue = 12
    elseif (act == "hit") then
        --,        //13 受击
        actionValue = 13
    elseif (act == "dead") then
        --,     //14 死亡
        actionValue = 14
    elseif (act == "happy") then
        --,      //15 高兴
        actionValue = 15
    elseif (act == "sad") then
        --,        //16 悲伤
        actionValue = 16
    elseif (act == "up") then
        --,        //17 起立
        actionValue = 17
    elseif (act == "down") then
        --,        //18 倒下
        actionValue = 18
    elseif (act == "biggestAK") then
        --,        //19 最大的大招
        actionValue = 19
    elseif (act == "dizzy") then
        --,        //20 晕
        actionValue = 20
    elseif (act == "stiff") then
        --,        //21 僵硬
        actionValue = 21
    elseif (act == "idel3") then
        --,        //21 空闲
        actionValue = 22
    else
        actionValue = 0
    end
    return actionValue
end

-- 动作回调toMap
function ActCBtoList(...)
    local paras = {...}
    if (#(paras) > 0) then
        local callbackInfor = ArrayList()
        for i, v in ipairs(paras) do
            callbackInfor:Add(v)
        end
        return callbackInfor
    end
    return nil
end

function strSplit(inputstr, sep)
    if sep == nil then
        sep = "%s"
    end
    local t = {}
    local i = 1
    for str in string.gmatch(inputstr, "([^" .. sep .. "]+)") do
        t[i] = str
        i = i + 1
    end
    return t
end

-- 取得方法体
function getLuaFunc(trace)
    if (trace == nil) then
        return nil
    end
    local list = strSplit(trace, ".")
    local func = nil
    if (list ~= nil) then
        func = _G
        for i, v in ipairs(list) do
            func = func[v]
        end
    end
    return func
end

---@param p coolape.Coolape.CLPanelLua
function onLoadedPanel(p, paras)
    p:setData(paras)
    CLPanelManager.showTopPanel(p)
end

---@param p coolape.Coolape.CLPanelLua
function onLoadedPanelTT(p, paras)
    p:setData(paras)
    CLPanelManager.showTopPanel(p, true, true)
end

---@param p coolape.Coolape.CLPanelLua
function onLoadedPanelTF(p, paras)
    p:setData(paras)
    CLPanelManager.showTopPanel(p, true, false)
end

function SetActive(go, isActive)
    if (go == nil) then
        return
    end
    NGUITools.SetActive(go, isActive)
end

function getCC(transform, path, com)
    if not transform then
        return
    end
    local tf = getChild(transform, path)
    if not tf then
        return
    end
    return tf:GetComponent(com)
end

function isNilOrEmpty(s)
    if s == nil or s == "" then
        return true
    end
    return false
end

-- function joinStr(...)
--     local paras = {...}
--     if paras == nil or #paras == 0 then
--         return ""
--     end
--     return table.concat(paras)
-- end

---@public 拼接字符串
function joinStr(...)
    -- local paras = {...}
    local tb = {}
    local v
    for i = 1, select("#", ...) do
        v = select(i, ...)
        if v then
            table.insert(tb, tostring(v))
        end
    end
    return table.concat(tb)
end

---@public 保留小数位位数
function getPreciseDecimal(nNum, n)
    if type(nNum) ~= "number" then
        return nNum
    end
    n = n or 0
    n = math.floor(n)
    if n < 0 then
        n = 0
    end
    local nDecimal = 10 ^ n
    local nTemp = math.floor(nNum * nDecimal)
    local nRet = nTemp / nDecimal
    return nRet
end

---@public 离线处理
function procOffLine()
    if MyCfg.mode == GameMode.none or (IDPSceneManager and IDPSceneManager.isLoadingScene()) then
        return
    end

    -- CLAlert.add(LGet("MsgOffline"), Color.yellow, 1)
    printe(LGet("MsgOffline"))
    InvokeEx.cancelInvoke() -- 放在前面，后面马上就要用到invoke
    local ok, result = pcall(doReleaseAllRes)
    if not ok then
        printe(result)
    end

    local panel = CLPanelManager.getPanel(CLMainBase.self.firstPanel)
    CLPanelManager.showPanel(panel)
    -- 重新进入游戏
    InvokeEx.invoke(
        function()
            pcall(releaseRes4GC, false)
            CLLMainLua.begain()
        end,
        0.5
    )
end

function doReleaseAllRes()
    Time.timeScale = 1

    SetActive(CLPanelManager.self.mask, false)

    if NetProtoIsland then
        NetProtoIsland.__sessionID = 0
    end

    MyCfg.mode = GameMode.none

    if CLLNet then
        CLLNet.cancelHeart()
        CLLNet.stop()
    end

    if Net.self.netGameDataQueue then
        Net.self.netGameDataQueue:Clear()
    end

    -- 页面处理
    CLPanelManager.hideAllPanel()
    local panel = CLPanelManager.getPanel(CLMainBase.self.firstPanel)
    CLPanelManager.showPanel(panel)
    UIRichText4Chat.pool:clean()
    if IDUtl then
        IDUtl.clean()
    end
    if IDMainCity then
        IDMainCity.clean()
    end
    if IDLBattle then
        IDLBattle.clean()
    end

    if IDWorldMap then
        IDWorldMap.clean()
    end

    hideHotWheel()
end

---@public 重新启动前的处理
function doSomethingBeforeRestart()
    local ok, result = pcall(doReleaseAllRes)
    if not ok then
        printe(result)
    end
    Net.self.luaTable = nil

    if IDLCameraMgr then
        IDLCameraMgr.clean()
    end
    if IDMainCity then
        IDMainCity.destory()
    end
    if IDLBattle then
        IDLBattle.destory()
    end

    if IDWorldMap then
        IDWorldMap.destory()
    end
    -- 取消音效开关的回调
    SoundEx.clean()
end

---@public gc
function releaseRes4GC(releaseRes)
    if releaseRes then
        CLUIInit.self.emptAtlas:releaseAllTexturesImm()
        CLAssetsManager.self:releaseAsset(true)
        Resources.UnloadUnusedAssets()
    end
    if not MyCfg.self.isUnityEditor then
        CS.UnityEngine.Scripting.GarbageCollector.GCMode = CS.UnityEngine.Scripting.GarbageCollector.Mode.Enabled
        MyMain.self.lua:GC()
        GC.Collect() -- 内存释放
        CS.UnityEngine.Scripting.GarbageCollector.GCMode = CS.UnityEngine.Scripting.GarbageCollector.Mode.Disabled
    end
end

function dump(obj)
    local getIndent, quoteStr, wrapKey, wrapVal, dumpObj
    getIndent = function(level)
        return string.rep("\t", level)
    end
    quoteStr = function(str)
        return '"' .. string.gsub(str, '"', '\\"') .. '"'
    end
    wrapKey = function(val)
        if type(val) == "number" then
            return "[" .. val .. "]"
        elseif type(val) == "string" then
            return "[" .. quoteStr(val) .. "]"
        else
            return "[" .. tostring(val) .. "]"
        end
    end
    wrapVal = function(val, level)
        if type(val) == "table" then
            return dumpObj(val, level)
        elseif type(val) == "number" then
            return val
        elseif type(val) == "string" then
            return quoteStr(val)
        else
            return tostring(val)
        end
    end
    dumpObj = function(obj, level)
        if type(obj) ~= "table" then
            return wrapVal(obj)
        end
        level = level + 1
        local tokens = {}
        tokens[#tokens + 1] = "{"
        for k, v in pairs(obj) do
            tokens[#tokens + 1] = getIndent(level) .. wrapKey(k) .. " = " .. wrapVal(v, level) .. ","
        end
        tokens[#tokens + 1] = getIndent(level - 1) .. "}"
        return table.concat(tokens, "\n")
    end
    return dumpObj(obj, 0)
end
--*******************************************************************
--*******************************************************************
local borrowedSpList = {}
-- 当向atlas借一个sprite时
function onBorrowedSpriteCB(atlas, spData)
    --print(atlas.name, spData.path)
    local m = borrowedSpList[spData.name]
    m = m or {}
    m.atlas = m.atlas or {}
    m.atlas[atlas.name] = atlas.name
    if m.data == nil then
        m.data = {}
        m.data.name = spData.name
        m.data.path = spData.path

        m.data.x = spData.x
        m.data.y = spData.y
        m.data.width = spData.width
        m.data.height = spData.height

        m.data.borderLeft = spData.borderLeft
        m.data.borderRight = spData.borderRight
        m.data.borderTop = spData.borderTop
        m.data.borderBottom = spData.borderBottom

        m.data.paddingLeft = spData.paddingLeft
        m.data.paddingRight = spData.paddingRight
        m.data.paddingTop = spData.paddingTop
        m.data.paddingBottom = spData.paddingBottom
    end
    m.times = m.times or 0
    m.times = m.times + 1
    borrowedSpList[spData.name] = m
end
---@public
function onApplicationPauseCallback4CountAtlas()
    local jstr = json.encode(borrowedSpList)
    local path =
        joinStr(CLPathCfg.persistentDataPath, "/", CLPathCfg.self.basePath, "/xRes/spriteBorrow/spriteBorrowInfo.json")
    Directory.CreateDirectory(Path.GetDirectoryName(path))
    File.WriteAllText(path, jstr)
end
--*******************************************************************
--*******************************************************************
