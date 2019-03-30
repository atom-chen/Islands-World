require("public.class")
require("public.CLLQueue")

CLLPool = class("CLLPool")
--local queue;
--local cloneClass;

function CLLPool:ctor(classObj)
    self.queue = CLLQueue.new(100)
    self.cloneClass = classObj
end

function CLLPool:createObj()
    return self.cloneClass.new()
end

function CLLPool:borrow()
    if self.queue:isEmpty() then
        return self:createObj()
    end
    return self.queue:deQueue();
end

function CLLPool:retObj(obj)
    self.queue:enQueue(obj);
end

return CLLPool;
