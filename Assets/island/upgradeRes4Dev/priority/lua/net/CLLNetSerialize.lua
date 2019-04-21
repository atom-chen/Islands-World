-- 组包
require("public.CLLInclude")
require("bio.BioUtl")
require("toolkit.CLLPrintEx")
require("toolkit.BitUtl")

local CLLNetSerialize = {}

local strLen = string.len
local strSub = string.sub
local strPack = string.pack
local strbyte = string.byte
local strchar = string.char
local insert = table.insert
local maxPackSize = 64 * 1024 - 1
local subPackSize = 64 * 1024 - 1 - 50
local __maxLen = 1024 * 1024

local currPack = {}
--============================================================
function CLLNetSerialize.packMsg(data, tcp)
    local bytes = BioUtl.writeObject(data)
    if bytes == nil or tcp == nil or tcp.socket == nil then
        return nil
    end
    local len = strLen(bytes)
    if len > maxPackSize then
        -- 处理分包
        --local packList = ArrayList()
        local subPackgeCount = math.floor(len / subPackSize)
        local left = len % subPackSize
        local count = subPackgeCount
        if left > 0 then
            count = subPackgeCount + 1
        end
        for i = 1, subPackgeCount do
            local subPackg = {}
            subPackg.__isSubPack = true
            subPackg.count = count
            subPackg.i = i
            subPackg.content = strSub(bytes, ((i - 1) * subPackSize) + 1, i * subPackSize)
            local package = strPack(">s2", BioUtl.writeObject(subPackg))
            tcp.socket:SendAsync(package)
        end
        if left > 0 then
            local subPackg = {}
            subPackg.__isSubPack = true
            subPackg.count = count
            subPackg.i = count
            subPackg.content = strSub(bytes, len - left + 1, len)
            local package = strPack(">s2", BioUtl.writeObject(subPackg))
            tcp.socket:SendAsync(package)
        end
    else
        print(bytes)
        local package = strPack(">s2", bytes)
        tcp.socket:SendAsync(package)
    end
end

--============================================================
-- 完整的接口都是table，当有分包的时候会收到list。list[1]=共有几个分包，list[2]＝第几个分包，list[3]＝ 内容
local function isSubPackage(m)
    if m.__isSubPack then
        --判断有没有cmd
        return true
    end
    return false
end

local function unPackSubMsg(m)
    -- 是分包
    local count = m.count
    local index = m.i
    if m.content == nil then
        printe("the m.content is nil")
        return
    end
    currPack[index] = m.content
    if (#currPack == count) then
        -- 说明分包已经取完整
        local map = BioUtl.readObject(table.concat(currPack, ""))
        currPack = {}
        return map
    end
    return nil
end

--============================================================
---@public 解包
function CLLNetSerialize.unpackMsg(buffer, tcp)
    local ret = nil
    local oldPos = buffer.Position
    buffer.Position = 0
    local totalLen = buffer.Length
    local needLen = buffer:ReadByte() * 256 + buffer:ReadByte()
    if (needLen <= 0 and needLen > __maxLen) then
        --// 网络Number据错误。断isOpen网络
        tcp.socket:close()
        return nil
    end
    local usedLen = buffer.Position
    if (usedLen + needLen <= totalLen) then
        local lessBuff = Utl.read4MemoryStream(buffer, 0, needLen)
        ret = BioUtl.readObject(lessBuff)
    else
        --说明长度不够
        buffer.Position = oldPos
    end

    if ret and isSubPackage(ret) then
        return unPackSubMsg(ret)
    else
        return ret
    end
end

--============================================================
local secretKey = "coolape99"
---@public 加密
function CLLNetSerialize.encrypt(bytes, key)
    return CLLNetSerialize.xor(bytes, key)
end

---@public 解密
function CLLNetSerialize.decrypt(bytes, key)
    return CLLNetSerialize.xor(bytes, key)
end

function CLLNetSerialize.xor(bytes, key)
    key = key or secretKey
    local len = #bytes
    local keyLen = #key
    local byte, byte2
    local keyIdx = 0
    local result = {}
    for i = 1, len do
        byte = strbyte(bytes, i)
        keyIdx = i % keyLen + 1
        byte2 = BitUtl.xorOp(byte, strbyte(key, keyIdx))
        insert(result, strchar(byte2))
    end
    return table.concat(result)
end
--------------------------------------------
return CLLNetSerialize
