LogLev = {
    error = 1,
    warning = 2,
    debug = 3,
}
local logLev = LogLev.debug
local logBackTraceLev = -1
local select = select
local table = table
local smatch = string.match
local sfind = string.find

---@public 设置日志等级，分别是，debug，warning，error
function setLogLev(val)
    logLev = val or LogLev.debug
end

---@public 设置日志的调用栈信息
function setLogBackTraceLev(lev)
    logBackTraceLev = lev

    if logBackTraceLev > 0 and logBackTraceLev < 3 then
        logBackTraceLev = 3
    end
end

local strSplit = function(inputstr, sep)
    if sep == nil then
        sep = "%s"
    end
    local t = {};
    local i = 1
    for str in string.gmatch(inputstr, "([^" .. sep .. "]+)") do
        t[i] = str
        i = i + 1
    end
    return t;
end

local trim = function(s)
    -- return (s:gsub("^%s*(.-)%s*$", "%1"))
    return smatch(s, '^()%s*$') and '' or smatch(s, '^%s*(.*%S)') -- 性能略优
end

local parseBackTrace = function(traceInfor, level)
    if traceInfor and level > 1 then
        local traces = strSplit(traceInfor, "\n")
        if #traces >= level then
            local str = trim(traces[level])
            local sList = strSplit(str, ":")
            local file = sList[1]
            local line = sList[2]
            local func = sList[3] or ""
            --file = string.match(file, "/%a+%.%a+") or ""
            func = string.match(func, "'%a+'") or ""
            return file .. ":" .. line .. ":" .. func
        end
    else
        return traceInfor or ""
    end
end

local wrapMsg = function (...)
    local tb = {}
    local v
    for i = 1, select("#", ...) do
        v = select(i, ...)
        if v then
            table.insert(tb, tostring(v))
        else
            table.insert(tb, "nil")
        end
    end
    return table.concat(tb, "|")
end

local luaprint = print

function print(...)
    if logLev < LogLev.debug then
        return
    end
    local trace = debug.traceback("")
    local msg = wrapMsg(...)
    msg = msg or ""
    luaprint("[debug]:" .. msg .. "\n" .. parseBackTrace(trace, logBackTraceLev))
end

function printw(...)
    if logLev < LogLev.warning then
        return
    end
    local trace = debug.traceback("")
    local msg = wrapMsg(...)
    msg = msg or ""
    Utl.printw("[warn]:" .. msg .. "\n" .. parseBackTrace(trace, logBackTraceLev))
end

function printe(...)
    if logLev < LogLev.error then
        return
    end
    local trace = debug.traceback("")
    local msg = wrapMsg(...)
    msg = msg or ""
    Utl.printe("[err]:" .. msg .. "\n" .. parseBackTrace(trace, logBackTraceLev))
end
