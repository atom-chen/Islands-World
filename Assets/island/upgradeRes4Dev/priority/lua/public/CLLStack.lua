--
-- Date: 2014-11-19 15:29:02
--

require("public.class")
CLLStack = class("CLLStack")

local remove = table.remove
function CLLStack:ctor()
    self.stack_table = {}
end

function CLLStack:push(element)
    local size = self:size()
    self.stack_table[size + 1] = element
end

function CLLStack:pop()
    local size = self:size()
    if self:isEmpty() then
        print("Error: CLLStack is empty!")
        return
    end
    return remove(self.stack_table,size)
end

function CLLStack:top()
    local size = self:size()
    if self:isEmpty() then
        print("Error: CLLStack is empty!")
        return
    end
    return self.stack_table[size]
end

function CLLStack:isEmpty()
    local size = self:size()
    if size == 0 then
        return true
    end
    return false
end

function CLLStack:size()
    return #(self.stack_table) or 0
end

function CLLStack:clear()
    -- body
    self.stack_table = nil
    self.stack_table = {}
end

function CLLStack:printElement()
    local size = self:size()

    if self:isEmpty() then
        print("Error: CLLStack is empty!")
        return
    end

    local str = "{"..self.stack_table[size]
    size = size - 1
    while size > 0 do
        str = str..", "..self.stack_table[size]
        size = size - 1
    end
    str = str.."}"
    print(str)
end


return CLLStack
