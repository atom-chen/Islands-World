require("public.class")

---@class IDDBTile 建筑
IDDBTile = class("IDDBTile")

function IDDBTile:ctor(d)
    self._data = d
    self.idx = d.idx  -- 唯一标识 int int
    self.attrid = d.attrid  -- 属性配置id int int
    self.cidx = d.cidx  -- 主城idx int int
    self.pos = d.pos  -- 位置，即在城的gird中的index int int
end

function IDDBTile:toMap()
    return self._data
end

--------------------------------------------
return IDDBTile
