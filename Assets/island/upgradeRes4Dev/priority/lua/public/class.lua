---@class ClassBase
---@field public super ClassBase
local m = {}
function m.new()
end
function m:ctor(...)
end
---@public 取得父类的实例（在多重继承情况下，需要取得父类的实例时调用）
---@param selfClass table 注意不能传self.class，因为self可能是子类
function m:getBase(selfClass)
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
        self:getBase(B).func1(self)
        print("I'm B call func1")
    end
    function B:func3()
        print("I'm B call func3")
    end

    ------------------------------
    C = class("C", B) -- 创建类C,继承B
    function C:func1() -- 重载func1，并且调用父类的func1，注意使用方式
        self:getBase(C).func1(self)
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
            cls.__lev = super.__lev + 1
        else
            cls = {
                ctor = function()
                end
            }
            cls.__lev = 1
        end

        cls.__cname = classname
        cls.__ctype = 2 -- lua
        cls.__index = cls

        ---@public 取得父类的实例（在多重继承情况下，需要取得父类的实例时调用）
        ---@param selfClass table 注意不能传self.class，因为self可能是子类
        function cls:getBase(selfClass)
            local obj = self

            while (obj) do
                if obj.__lev == selfClass.__lev then
                    return obj.super
                else
                    obj = obj.super
                end
            end
        end

        ---@public 包装函数给c#用
        function cls:wrapFunc(func)
            return self:wrapFunction4CS(func)
        end
        ---@public 包装函数给c#用
        function cls:wrapFunction4CS(func)
            return {instance = self, func = func}
        end

        function cls.new(...)
            local instance = setmetatable({}, cls)
            instance.class = cls
            instance:ctor(...)
            return instance
        end
    end
    return cls
end
