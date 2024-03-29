---@class IDConst 常量
IDConst = {}
IDConst = {
    baseRes = number2bio(50000) -- 基础资源量
}

IDConst.BuildingID = {
    headquartersBuildingID = 1, --主基地
    dockyardBuildingID = 2, -- 造船厂
    AllianceID = 4, -- 联盟港口
    foodStorageBuildingID = 7,
    oildStorageBuildingID = 9,
    goldStorageBuildingID = 11,
    MortarDefenseID = 16, --烈焰式火箭炮
    ThunderboltID = 17, --电磁塔
    DestroyerRocketID = 18, -- 地狱之门
    AirBombID = 20, -- 放空气球
    FrozenMineID = 22, -- 冰冻地雷
    IceStormID = 23, -- 风暴控制器
    trapMonsterID = 24, -- 海怪陷阱
    trapSwirlID = 25, -- 漩涡陷阱
    activityCenterBuildingID = 38, -- 活动中心
    MailBoxBuildingID = 39 -- 邮箱
}

IDConst.BuildingState = {
    normal = 0, --正常
    upgrade = 1, --升级中
    working = 2, --工作中
    renew = 9 -- 恢复中
}

---@public 建筑类别
IDConst.BuildingGID = {
    spec = -1, -- 特殊建筑
    com = 1, -- 基础建筑
    resource = 2, -- 资源建筑
    defense = 3, -- 防御建筑
    trap = 4, --陷阱
    decorate = 5, -- 装饰
    tree = 6 -- 树
}

---@public 角色类别
IDConst.RoleGID = {
    worker = 100, -- 工人
    ship = 101, -- 舰船
    solider = 102, -- 陆战兵
    pet = 103 -- 宠物
}

IDConst.RoleID = {
    Barbarian = 3, -- 陆战兵
    LandCraft = 4 -- 登陆船
}

---@public 游戏中各种类型
IDConst.UnitType = {
    building = 1,
    ship = 2,
    tech = 3,
    pet = 4,
    skill = 5
}

---@public 资源各类
IDConst.ResType = {
    food = 1,
    gold = 2,
    oil = 3,
    diam = 9
}

---@public 角色的状态
IDConst.RoleState = {
    idel = 1,
    working = 2,
    dead = 3,
    frozen = 4 -- 冰冻
}

---@public 属性类型
IDConst.AttrType = {
    building = 1, -- 建筑
    buildingNextOpen = 2, -- 建筑下级开放
    ship = 3, -- 舰船
    ship4Build = 4 -- 舰船建造时
}

---@public 战斗类型
IDConst.BattleType = {
    pvp = 1, -- 攻击玩家
    pve = 2 -- 副本
}

---@public 换装的类型
IDConst.dressMode = {
    normal = 1,
    ice = 2
}
---@public 大地图地块类型
IDConst.WorldmapCellType = {
    port = 1, -- 港口
    decorate = 2, -- 装饰
    user = 3, -- 玩家
    empty = 4, -- 空地
    fleet = 5, -- 舰队停留
    occupy = 99 -- 占用
}

---@public 舰队状态
IDConst.FleetState = {
    none = 1, -- 无
    moving = 2, -- 航行中
    docked = 3, -- 停泊在港口
    stay = 4, -- 停留在海面
    fighting = 5 -- 正在战斗中
}
---@public 舰队任务
IDConst.FleetTask = {
    idel = 1, -- 待命状态
    voyage = 2, -- 出征
    back = 3, -- 返航
    attack = 4 -- 攻击
}
---@public 舰队状态
IDConst.FleetStateName = {
    [1] = "", -- 无
    [2] = "FleetStatemoving", -- 航行中
    [3] = "FleetStateDocked", -- 停泊在港口
    [4] = "FleetStateStay", -- 停留在海面
    [5] = "FleetStateFighting" -- 正在战斗中
}
---@public 舰队任务
IDConst.FleetTaskName = {
    [1] = "FleetTaskIdel", -- 待命状态
    [2] = "FleetTaskVoyage", -- 出征
    [3] = "FleetTaskBack", -- 返航
    [4] = "FleetTaskAttack" -- 攻击
}
return IDConst
