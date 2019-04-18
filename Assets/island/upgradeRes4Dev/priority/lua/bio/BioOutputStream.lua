do
    require("public.class")
    require("bio.BioType")

    BioOutputStream = {}

    local B2Type = BioType.B2Type;
    local IntB2TypeList = BioType.IntB2TypeList;
    local StringB2TypeList = BioType.StringB2TypeList;
    local MapB2TypeList = BioType.MapB2TypeList;
    local ListB2TypeList = BioType.ListB2TypeList

    local type = type
    local sub = string.sub
    local strupper = string.upper;
    local strlen = string.len;
    local strbyte = string.byte
    local strchar = string.char
    local floorInt = math.floor
    --===================================================
    --===================================================
    -- 数据流
    LuaB2OutputStream = class("LuaB2OutputStream");
    function LuaB2OutputStream:ctor(v)
        self.content = {}
    end

    function LuaB2OutputStream:init(v)
        self.content = {}
    end

    function LuaB2OutputStream:writeByte(v)
        table.insert(self.content, strchar(v))
    end

    function LuaB2OutputStream:writeString(v)
        table.insert(self.content, v)
    end

    function LuaB2OutputStream:toBytes()
        --local ret = "";
        --for i, v in ipairs(self.content) do
        --    ret = ret .. v;
        --end
        return table.concat(self.content, "");
    end

    function LuaB2OutputStream:release()
        self.content = {};
    end
    --===================================================
    --===================================================
    --===================================================

    -- 原始数据类型
    BioOutputStream.DataType = {
        NIL = "nil",
        BOOLEAN = "boolean",
        STRING = "string",
        NUMBER = "number",
        LONG = "long",
        DOUBLE = "double",
        USERDATA = "userdata",
        FUNCTION = "function",
        THREAD = "thread",
        TABLE = "table",
        INT4B = "int4b",
        INT16B = "int16b",
        INT32B = "int32b",
    }

    ---@public 取得数据的类型，主要是对number做了处理
    function BioOutputStream.getDataType(obj)
        --nil, boolean, number, string, userdata, function, thread, table
        local val = nil;
        local t = type(obj);
        val = BioOutputStream.DataType[strupper(type(obj))];
        if val == nil then
            val = "undefined";
        end
        return val;
    end

    function BioOutputStream.getNumberType(obj)
        local val = nil;
        local t = type(obj);
        if t == "number" then
            local minInt = floorInt(obj);
            if minInt == obj then
                -- 说明是整数
                if (obj >= -128 and obj <= 127) then
                    val = BioOutputStream.DataType.INT4B
                elseif (obj >= -32768 and obj <= 32767) then
                    val = BioOutputStream.DataType.INT16B
                elseif (obj >= -2147483648 and obj <= 2147483647) then
                    val = BioOutputStream.DataType.INT32B
                else
                    val = BioOutputStream.DataType.LONG
                end
            else
                val = BioOutputStream.DataType.DOUBLE
            end
        end
        return val;
    end

    ---@public 返回table是否为一个array， 第二返回值：如查是array的时候是table的count
    function BioOutputStream.isArray(t)
        if t == nil then
            return false, 0;
        end
        local i = 0
        local ret = true;
        for _ in pairs(t) do
            i = i + 1
            if t[i] == nil then
                ret = false
            end
        end
        return ret, i
    end

    --===================================================
    ---public Void writeObject (LuaB2OutputStream os, obj)
    ---@param optional LuaB2OutputStream os
    ---@param optional object obj
    --[[
    local os = LuaB2OutputStream.new();
    BioOutputStream.writeObject(os, map)
    local bytes = os:toBytes()
    --]]
    function BioOutputStream.writeObject (os, obj)
        if os == nil then
            os = LuaB2OutputStream.new();
        end
        local objType = BioOutputStream.getDataType(obj)
        if (objType == BioOutputStream.DataType.NIL) then
            BioOutputStream.writeNil(os)
        elseif (objType == BioOutputStream.DataType.TABLE) then
            BioOutputStream.writeMap(os, obj);
        elseif (objType == BioOutputStream.DataType.NUMBER) then
            BioOutputStream.writeNumber(os, obj);
        elseif (objType == BioOutputStream.DataType.STRING) then
            BioOutputStream.writeString(os, obj);
        elseif (objType == BioOutputStream.DataType.BOOLEAN) then
            BioOutputStream.writeBoolean(os, obj);
        else
            --//throw new IOException("unsupported obj then" + obj);
            print("B2IO unsupported error then type=[" .. tostring(objType) .. "] val=[" .. obj .. "]");
        end
    end

    function BioOutputStream.writeNil(os)
        BioOutputStream.WriteByte(os, B2Type.NULL);
        return os;
    end

    function BioOutputStream.WriteByte(os, v)
        local v2 = v;
        if v < 0 then
            v2 = v + 256;
        end
        os:writeByte(v2)
        return os;
    end

    function BioOutputStream.writeNumber(os, v)
        local numType = BioOutputStream.getNumberType(v)
        if numType == BioOutputStream.DataType.INT4B or
        numType == BioOutputStream.DataType.INT16B or
        numType == BioOutputStream.DataType.INT32B then
            BioOutputStream.writeInt(os, v)
        elseif numType == BioOutputStream.DataType.LONG then
            BioOutputStream.writeLong(os, v);
        elseif numType == BioOutputStream.DataType.DOUBLE then
            BioOutputStream.writeDouble(os, v);
        end
        return os;
    end

    function BioOutputStream.writeInt(os, v)
        if v == -1 then
            BioOutputStream.WriteByte(os, B2Type.INT_N1);
        elseif v >= 0 and v <= 32 then
            local t = IntB2TypeList[v + 1]
            BioOutputStream.WriteByte(os, t);
        else
            if (v >= -128 and v <= 127) then
                BioOutputStream.WriteByte(os, B2Type.INT_8B);
                BioOutputStream.WriteByte(os, v);
            elseif (v >= -32768 and v <= 32767) then
                BioOutputStream.WriteByte(os, B2Type.INT_16B);
                local v2 = v;
                --if v < 0 then
                --    v2 = v + 65536
                --end
                BioOutputStream.WriteByte(os, math.floor(v2 / 256));
                BioOutputStream.WriteByte(os, v2 % 256);
            else
                BioOutputStream.WriteByte(os, B2Type.INT_32B);
                local v2 = v;
                --if v < 0 then
                --    v2 = v + 4294967296
                --end
                BioOutputStream.WriteByte(os, math.floor(v2 / 16777216));
                v2 = v2 % 16777216
                BioOutputStream.WriteByte(os, math.floor(v2 / 65536));
                v2 = v2 % 65536
                BioOutputStream.WriteByte(os, math.floor(v2 / 256));
                BioOutputStream.WriteByte(os, v2 % 256);
            end
        end
        return os;
    end

    function BioOutputStream.writeLong(os, v)
        BioOutputStream.WriteByte(os, B2Type.LONG_64B);
        local v2 = v;
        --if v < 0 then
        --    v2 = v + 4294967296
        --end
        BioOutputStream.WriteByte(os, math.floor(v2 / 72057594037927936));
        v2 = v2 % 72057594037927936
        BioOutputStream.WriteByte(os, math.floor(v2 / 281474976710656));
        v2 = v2 % 281474976710656
        BioOutputStream.WriteByte(os, math.floor(v2 / 1099511627776));
        v2 = v2 % 1099511627776
        BioOutputStream.WriteByte(os, math.floor(v2 / 4294967296));
        v2 = v2 % 4294967296
        BioOutputStream.WriteByte(os, math.floor(v2 / 16777216));
        v2 = v2 % 16777216
        BioOutputStream.WriteByte(os, math.floor(v2 / 65536));
        v2 = v2 % 65536
        BioOutputStream.WriteByte(os, math.floor(v2 / 256));
        BioOutputStream.WriteByte(os, v2 % 256);
        return os;
    end

    function BioOutputStream.writeDouble(os, v)
        BioOutputStream.WriteByte(os, B2Type.DOUBLE_64B);
        BioOutputStream.writeString(os, tostring(v));
        return os;
    end

    function BioOutputStream.writeString(os, v)
        if (v == nil) then
            BioOutputStream.writeNil(os);
            return os;
        end

        local len = strlen(v);
        local t = StringB2TypeList[len + 1]
        if t then
            BioOutputStream.WriteByte(os, t);
            os:writeString(v);
        else
            BioOutputStream.WriteByte(os, B2Type.STR);
            BioOutputStream.writeInt(os, len);
            os:writeString(v);
        end
    end

    function BioOutputStream.writeBoolean(os, v)
        if v then
            BioOutputStream.WriteByte(os, B2Type.BOOLEAN_TRUE);
        else
            BioOutputStream.WriteByte(os, B2Type.BOOLEAN_FALSE);
        end
        return os;
    end

    function BioOutputStream.writeMap(os, v)
        if (v == nil) then
            BioOutputStream.writeNil(os);
        end
        local isArray, len = BioOutputStream.isArray(v)
        if isArray then
            if len <= 24 then
                BioOutputStream.WriteByte(os, ListB2TypeList[len + 1]);
            else
                BioOutputStream.WriteByte(os, B2Type.VECTOR);
                BioOutputStream.writeInt(os, len);
            end

            for i, val in ipairs(v) do
                BioOutputStream.writeObject(os, val);
            end
        else
            if len <= 15 then
                BioOutputStream.WriteByte(os, MapB2TypeList[len + 1]);
            else
                BioOutputStream.WriteByte(os, B2Type.HASHTABLE);
                BioOutputStream.writeInt(os, len);
            end

            for key, val in pairs(v) do
                BioOutputStream.writeObject(os, key);
                BioOutputStream.writeObject(os, val);
            end
        end
    end

    --两个字节的and 操作
    function BioOutputStream.byte_xor(a, b)
        local bit_value_tb = {
            [8] = 2 ^ 7;
            [7] = 2 ^ 6;
            [6] = 2 ^ 5;
            [5] = 2 ^ 4;
            [4] = 2 ^ 3;
            [3] = 2 ^ 2;
            [2] = 2 ^ 1;
            [1] = 2 ^ 0; };
        local value = 0;
        for i = 8, 1, -1 do
            local bit_value = bit_value_tb[i];
            if a >= bit_value and b < bit_value then
                value = value + bit_value;
            elseif a < bit_value and b >= bit_value then
                value = value + bit_value;
            end
            if a >= bit_value then
                a = a - bit_value;
            end
            if b >= bit_value then
                b = b - bit_value;
            end
        end
        return value;
    end

    --------------------------------------------
    return BioOutputStream;
end
