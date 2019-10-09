---@public 常量
IDConst = {
    headquartersBuildingID = 1, --主基地
    dockyardBuildingID = 2, -- 造船厂
    foodStorageBuildingID = 7,
    goldStorageBuildingID = 11,
    oildStorageBuildingID = 9,
    activityCenterBuildingID = 38, -- 活动中心
    MailBoxBuildingID = 39, -- 邮箱
    baseRes = number2bio(50000), -- 基础资源量
}
IDConst.BuildingState = {
    normal = 0, --正常
    upgrade = 1, --升级中
    working = 2, --工作中
    renew = 9, -- 恢复中
}

---@public 建筑类别
IDConst.BuildingGID = {
    spec = 0, -- 特殊建筑
    com = 1, -- 基础建筑
    resource = 2, -- 资源建筑
    defense = 3, -- 防御建筑
    trap = 4, --陷阱
    decorate = 5, -- 装饰
    tree = 6, -- 树

}

---@public 角色类别
IDConst.RoleGID = {
    worker = 100, -- 工人
    ship = 101, -- 舰船
    solider = 102, -- 陆战兵
    pet = 103, -- 宠物
}

---@public 游戏中各种类型
IDConst.UnitType = {
    building = 1,
    ship = 2,
    tech = 3,
    pet = 4,
    skill = 5,
}

---@public 资源各类
IDConst.ResType = {
    food = 1,
    gold = 2,
    oil = 3,
    diam = 9,
}

---@public 角色的状态
IDConst.RoleState = {
    idel = 1,
    working = 2,
    dead = 3,
}

---@public 属性类型
IDConst.AttrType = {
    building = 1, -- 建筑
    buildingNextOpen = 2, -- 建筑下级开放
    ship = 3, -- 舰船
    ship4Build = 4, -- 舰船建造时
}

return IDConst
