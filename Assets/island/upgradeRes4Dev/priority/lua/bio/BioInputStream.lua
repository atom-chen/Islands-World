do
    require("public.class")
    require("bio.BioType")
    BioInputStream = {}

    local B2Type = BioType.B2Type;
    local IntB2TypeValueMap = BioType.IntB2TypeValueMap;
    local StringB2TypeLenMap = BioType.StringB2TypeLenMap
    local MapB2TypeSizeMap = BioType.MapB2TypeSizeMap
    local ListB2TypeSizeMap = BioType.ListB2TypeSizeMap


    local type = type
    local sub = string.sub
    local strbyte = string.byte
    local strchar = string.char

    --===================================================
    --===================================================
    --===================================================
    -- 数据流
    LuaB2InputStream = class("LuaB2InputStream");
    function LuaB2InputStream:ctor(v)
        self.bytes = v;
        self.pos = 1;
    end

    function LuaB2InputStream:init(v)
        self.bytes = v;
        self.pos = 1;
    end

    function LuaB2InputStream:readByte()
        local ret = strbyte(self.bytes, self.pos);
        self.pos = self.pos + 1;
        return ret;
    end

    function LuaB2InputStream:readBytes(len)
        local ret = strbyte(self.bytes, self.pos, self.pos + len - 1);
        self.pos = self.pos + len;
        return ret;
    end

    function LuaB2InputStream:readString(len)
        local ret = sub(self.bytes, self.pos, self.pos + len - 1);
        self.pos = self.pos + len;
        return ret;
    end

    function LuaB2InputStream:release()
        self.bytes = nil;
        self.pos = 1;
    end
    --===================================================
    --===================================================
    --===================================================
    ---public Void readObject (LuaB2InputStream s)
    ---@param optional LuaB2InputStream s
    --[[
    local is = LuaB2InputStream.new(os:toBytes());
    local result = BioInputStream.readObject(is)
    --]]
    function BioInputStream.readObject (s)
        local tag = BioInputStream.ReadByteTag(s);
        if tag == B2Type.NULL then
            return nil;
        else
            -- decode map
            local mapSize = MapB2TypeSizeMap[tag]
            if mapSize then
                return BioInputStream.readMap(s, mapSize);
            elseif tag == B2Type.HASHTABLE then
                local len = BioInputStream.readInt(s);
                return BioInputStream.readMap(s, len);
            end
        end

        -- decode int
        local intVal = IntB2TypeValueMap[tag]
        if intVal then
            return intVal;
        elseif tag == B2Type.INT_N1 then
            return -1;
        elseif tag == B2Type.INT_8B then
            local int = 0;
            local int0 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的1字节是负数的补码,比如读出来是128， 128=1000 0000，对应signed char 是 -127，而127=0111 1111
                int = int0 - 256;
            else
                int = int0;
            end
            return int;
        elseif tag == B2Type.INT_16B then
            local int = 0;
            --INT_16B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的2字节是负数的补码
                int = (int0 * 256 + int1) - 65536;
            else
                int = (int0 * 256 + int1);
            end
            return int;
        elseif tag == B2Type.INT_32B then
            local int = 0;
            --INT_32B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            local int2 = BioInputStream.ReadByte(s)
            local int3 = BioInputStream.ReadByte(s)
            --字节是负数,里面存放的4字节是负数的补码
            if int0 >= 128 then
                int = (int0 * 16777216 + int1 * 65536 + int2 * 256 + int3) - 4294967296;
            else
                int = (int0 * 16777216 + int1 * 65536 + int2 * 256 + int3);
            end
            return int;
        end

        -- decode string
        local strLen = StringB2TypeLenMap[tag]
        if strLen then
            return s:readString(strLen);
        elseif tag == B2Type.STR then
            strLen = BioInputStream.readInt(s);
            return s:readString(strLen);
        end

        if tag == B2Type.BOOLEAN_TRUE then
            return true;
        elseif tag == B2Type.BOOLEAN_FALSE then
            return false;
        elseif tag == B2Type.BYTE_0 then
            return 0;
        elseif tag == B2Type.BYTE then
            return BioInputStream.ReadByte(s);
        elseif tag == B2Type.BYTES_0 then
            return 0;
        elseif tag == B2Type.BYTES then
            local len = BioInputStream.readInt(s);
            return BioInputStream.ReadBytes(s, len);
        end

        -- decode list
        local listLen = ListB2TypeSizeMap[tag]
        if listLen then
            return BioInputStream.readList(s, listLen);
        elseif tag == B2Type.VECTOR then
            local len = BioInputStream.readInt(s);
            return BioInputStream.readList(s, len);
        end

        if tag == B2Type.SHORT_0 then
            return 0;
        elseif tag == B2Type.SHORT_8B then
            local int = 0;
            local int0 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的1字节是负数的补码,比如读出来是128， 128=1000 0000，对应signed char 是 -127，而127=0111 1111
                int = int0 - 256;
            else
                int = int0;
            end
            return int;
        elseif tag == B2Type.SHORT_16B then
            local int = 0;
            --INT_16B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的2字节是负数的补码
                int = (int0 * 256 + int1) - 65536;
            else
                int = (int0 * 256 + int1);
            end
            return int;
        elseif tag == B2Type.LONG_0 then
            return 0;
        elseif tag == B2Type.LONG_8B then
            local int = 0;
            local int0 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的1字节是负数的补码,比如读出来是128， 128=1000 0000，对应signed char 是 -127，而127=0111 1111
                int = int0 - 256;
            else
                int = int0;
            end
            return int;
        elseif tag == B2Type.LONG_16B then
            local int = 0;
            --INT_16B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的2字节是负数的补码
                int = (int0 * 256 + int1) - 65536;
            else
                int = (int0 * 256 + int1);
            end
            return int;
        elseif tag == B2Type.LONG_32B then
            local int = 0;
            --INT_32B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            local int2 = BioInputStream.ReadByte(s)
            local int3 = BioInputStream.ReadByte(s)
            --字节是负数,里面存放的4字节是负数的补码
            if int0 >= 128 then
                int = (int0 * 16777216 + int1 * 65536 + int2 * 256 + int3) - 4294967296;
            else
                int = (int0 * 16777216 + int1 * 65536 + int2 * 256 + int3);
            end
            return int;
        elseif tag == B2Type.LONG_64B then
            local str = BioInputStream.readString(s)
            return tonumber(str);
        elseif tag == B2Type.DOUBLE_0 then
            return 0;
        elseif tag == B2Type.DOUBLE_64B then
            local str = BioInputStream.readString(s)
            return tonumber(str);
        elseif tag == B2Type.INT_B2 then
            return BioInputStream.readInt(s);
        else
            --//throw new IOException("unknow tag error then" + tag);
            print("bio2 unknon type then==" .. tostring(tag));
        end
        return 0;
    end

    function BioInputStream.ReadByte(s)
        if s == nil then
            return nil;
        end
        return s:readByte();
    end

    function BioInputStream.ReadByteTag(s)
        if s == nil then
            return nil;
        end
        local int0 = s:readByte()
        local int = 0;
        if int0 >= 128 then
            --字节是负数,里面存放的1字节是负数的补码,比如读出来是128， 128=1000 0000，对应signed char 是 -127，而127=0111 1111
            int = int0 - 256;
        else
            int = int0;
        end
        return int;
    end

    function BioInputStream.ReadBytes(s, len)
        return s:readBytes(len);
    end

    function BioInputStream.readMap(s, n)
        if n == nil or n <= 0 then
            return {}
        end

        local ret = {};
        for i = 1, n do
            local key = BioInputStream.readObject(s);
            local val = BioInputStream.readObject(s);
            ret [key] = val;
        end
        return ret;
    end

    function BioInputStream.readString(s)
        local tag = BioInputStream.ReadByteTag(s);
        local strLen = StringB2TypeLenMap[tag]
        if strLen then
            return s:readString(strLen)
        elseif tag == B2Type.STR then
            strLen = BioInputStream.readInt(s);
            return s:readString(strLen)
        end
        return "";
    end

    function BioInputStream.readInt(s)
        local tag = BioInputStream.ReadByteTag(s);
        local intVal = IntB2TypeValueMap[tag]
        if intVal then
            return intVal;
        elseif tag == B2Type.INT_N1 then
            return -1;
        elseif tag == B2Type.INT_8B then
            local int = 0;
            local int0 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的1字节是负数的补码,比如读出来是128， 128=1000 0000，对应signed char 是 -127，而127=0111 1111
                int = int0 - 256;
            else
                int = int0;
            end
            return int;
        elseif tag == B2Type.INT_16B then
            local int = 0;
            --INT_16B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            if int0 >= 128 then
                --字节是负数,里面存放的2字节是负数的补码
                int = (int0 * 256 + int1) - 65536;
            else
                int = (int0 * 256 + int1);
            end
            return int;
        elseif tag == B2Type.INT_32B then
            local int = 0;
            --INT_32B
            local int0 = BioInputStream.ReadByte(s)
            local int1 = BioInputStream.ReadByte(s)
            local int2 = BioInputStream.ReadByte(s)
            local int3 = BioInputStream.ReadByte(s)
            --字节是负数,里面存放的4字节是负数的补码
            if int0 >= 128 then
                int = (int0 * 16777216 + int1 * 65536 + int2 * 256 + int3) - 4294967296;
            else
                int = (int0 * 16777216 + int1 * 65536 + int2 * 256 + int3);
            end
            return int;
        end
        return 0;
    end

    function BioInputStream.readList(s, n)
        if n == nil or n <= 0 then
            return {}
        end

        local ret = {};
        for i = 1, n do
            local val = BioInputStream.readObject(s);
            table.insert(ret, val);
        end
        return ret;
    end
    --------------------------------------------
    return BioInputStream;
end
