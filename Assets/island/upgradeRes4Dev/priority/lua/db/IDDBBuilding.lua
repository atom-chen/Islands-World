require("public.class")

---@class IDDBBuilding 建筑
IDDBBuilding = class("IDDBBuilding")

function IDDBBuilding:ctor(d)
    self._data = d
    self.idx = d.idx  -- 唯一标识 int int
    self.lev = d.lev  -- 等级 int int
    self.cidx = d.cidx  -- 主城idx int int
    self.attrid = d.attrid  -- 属性配置id int int
    self.pos = d.pos  -- 位置，即在城的gird中的index int int
    self.val = d.val  -- 值。如:产量，仓库的存储量等 int int
    self.val2 = d.val2  -- 值2。如:产量，仓库的存储量等 int int
    self.val3 = d.val3  -- 值3。如:产量，仓库的存储量等 int int
    self.val4 = d.val4  -- 值4。如:产量，仓库的存储量等 int int
    self.val5 = d.val5  -- 值4。如:产量，仓库的存储量等 int int
    self.state = d.state -- 状态. 0：正常；1：升级中；9：恢复中 int
    self.starttime = d.starttime -- 开始升级、恢复、采集等的时间点 long
    self.endtime = d.endtime -- 结束升级、恢复、采集等的时间点 long
end

function IDDBBuilding:toMap()
    return self._data
end
--------------------------------------------
return IDDBBuilding
