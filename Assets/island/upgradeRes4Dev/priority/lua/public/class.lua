---@class ClassBase
---@field public super ClassBase
local m = {}
function m.new()
end
function m:ctor(...)
end

---@public 包装函数给c#用
function m:wrapFunc(func)
end
function m:wrapFunction4CS(func)
end

---@public 创建类
---使用例：
--[[
    A = class("A")  -- 创建类A
    function A:func1()
        print("I'm A call func1")
    end

    function A:func2()
        print("I'm A call func2")
    end
    ------------------------------
    B = class("B", A) -- 创建类B,继承A
    function B:func1()   -- 重载func1，并且调用父类的func1，注意使用方式
        B.super.func1(self)
        print("I'm B call func1")
    end
    function B:func3()
        print("I'm B call func3")
    end

    ------------------------------
    C = class("C", B) -- 创建类C,继承B
    function C:func1() -- 重载func1，并且调用父类的func1，注意使用方式
        C.super.func1(self)
        print("I'm C call func1")
    end
    ------------------------------
    -- 测试
    local obj = C.new()
    obj:func1()
    obj:func2()
    obj:func3()

    -- 运行结果
    LUA: [debug]:I'm A call func1
    LUA: [debug]:I'm B call func1
    LUA: [debug]:I'm C call func1

    LUA: [debug]:I'm A call func2

    LUA: [debug]:I'm B call func3

--]]
---@param classname string 类名
---@param super table 基类（也是用class方法创建的类，可以不传）
function class(classname, super)
    local superType = type(super)
    local cls

    if superType ~= "function" and superType ~= "table" then
        superType = nil
        super = nil
    end

    if superType == "function" or (super and super.__ctype == 1) then
        -- inherited from native C++ Object
        cls = {}

        if superType == "table" then
            -- copy fields from super
            for k, v in pairs(super) do
                cls[k] = v
            end
            cls.__create = super.__create
            cls.super = super
        else
            cls.__create = super
        end

        cls.ctor = function()
        end
        cls.__cname = classname
        cls.__ctype = 1

        function cls.new(...)
            local instance = cls.__create(...)
            -- copy fields from class to native object
            for k, v in pairs(cls) do
                instance[k] = v
            end
            instance.class = cls
            instance:ctor(...)
            return instance
        end
    else
        -- inherited from Lua Object
        if super then
            --cls = clone(super)
            cls = {}
            setmetatable(cls, {__index = super})

            cls.super = super
        else
            cls = {
                ctor = function()
                end
            }
        end

        cls.__cname = classname
        cls.__ctype = 2 -- lua
        cls.__index = cls

        ---@public 包装函数给c#用
        function cls:wrapFunc(func)
            return self:wrapFunction4CS(func)
        end
        ---@public 包装函数给c#用
        function cls:wrapFunction4CS(func)
            if func == nil then
                return nil
            end
            if self.__wrapFuncMap == nil then
                self.__wrapFuncMap = {}
            end
            local infor = self.__wrapFuncMap[func]
            if infor == nil then
                infor = {instance = self, func = func}
                self.__wrapFuncMap[func] = infor
            end
            return infor
        end

        function cls.new(...)
            local instance = setmetatable({}, cls)
            instance.class = cls
            instance.__wrapFuncMap = {}, -- 包装函数缓存
            instance:ctor(...)
            return instance
        end
    end
    return cls
end
