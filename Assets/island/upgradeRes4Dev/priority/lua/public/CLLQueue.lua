--
-- Date: 2014-11-19 16:51:19
--

require("public.class")
---@class CLLQueue
CLLQueue = class("CLLQueue")

local insert = table.insert;
local remove = table.remove
function CLLQueue:ctor()
    --self.capacity = capacity
    self.queue = {}
    --self.size_ = 0
    --self.head = -1
    --self.rear = -1
end

function CLLQueue:enQueue(element)
    insert(self.queue, 1, element)
    --if self.size_ == 0 then
    --    self.head = 0
    --    self.rear = 1
    --    self.size_ = 1
    --    self.queue[self.rear] = element
    --else
    --    local temp = (self.rear + 1) % self.capacity
    --    if temp == self.head then
    --        print("Error: capacity is full.")
    --        return
    --    else
    --        self.rear = temp
    --    end
    --
    --    self.queue[self.rear] = element
    --    self.size_ = self.size_ + 1
    --end

end

function CLLQueue:deQueue()
    if self:isEmpty() then
        return nil;
    end
    return remove(self.queue, #(self.queue))
    --if self:isEmpty() then
    --    print("Error: The CLLQueue is empty.")
    --    return
    --end
    --self.size_ = self.size_ - 1
    --self.head = (self.head + 1) % self.capacity
    --local value = self.queue[self.head]
    --return value
end

function CLLQueue:contains(obj)
    if self:isEmpty() then
        return false
    end
    for i, v in ipairs(self.queue) do
        if v == obj then
            return true
        end
    end
    return false
end

function CLLQueue:clear()
    self.queue = nil
    self.queue = {}
    --self.size_ = 0
    --self.head = -1
    --self.rear = -1
end

function CLLQueue:isEmpty()
    if #(self.queue) == 0 then
        return true
    end
    return false
    --if self:size() == 0 then
    --    return true
    --end
    --return false
end

function CLLQueue:size()
    return #(self.queue)
end

function CLLQueue:printElement()
    local str = ""
    for i = self.size(), 1 do
        str = str .. self.queue[i] .. ","
    end
    print(str)
    --local h = self.head
    --local r = self.rear
    --local str = nil
    --local first_flag = true
    --while h ~= r do
    --    if first_flag == true then
    --        str = "{"..self.queue[h]
    --        h = (h + 1) % self.capacity
    --        first_flag = false
    --    else
    --        str = str..","..self.queue[h]
    --        h = (h + 1) % self.capacity
    --    end
    --end
    --str = str..","..self.queue[r].."}"
    --print(str)
end

return CLLQueue
