---@public 预加载处理
---@class IDPreloadPrefab
local IDPreloadPrefab = {}
require("public.CLLQueue")
IDPreloadPrefab.roleQueue = CLLQueue.new() -- 角色
IDPreloadPrefab.soundQueue = CLLQueue.new() -- 音乐，音效
IDPreloadPrefab.effectQueue = CLLQueue.new() -- 特效
IDPreloadPrefab.bulletQueue = CLLQueue.new() -- 子弹
IDPreloadPrefab.thingQueue = CLLQueue.new() -- 物件
IDPreloadPrefab.uiThingQueue = CLLQueue.new() -- ui相关物件
IDPreloadPrefab.totalAssets = 0 -- 需要预加载资源总量

---@public 重置
function IDPreloadPrefab.reset()
    IDPreloadPrefab.roleQueue:clear()
    IDPreloadPrefab.soundQueue:clear()
    IDPreloadPrefab.effectQueue:clear()
    IDPreloadPrefab.bulletQueue:clear()
    IDPreloadPrefab.thingQueue:clear()
    IDPreloadPrefab.uiThingQueue:clear()
    IDPreloadPrefab.totalAssets = 0
    IDPreloadPrefab.currCount = 0
end

---@public 预加载角色
function IDPreloadPrefab.preloadRoles(roles, callback, progressCB)
    IDPreloadPrefab.reset()
    IDPreloadPrefab.onFinishCallback = callback
    IDPreloadPrefab.onProgressCB = progressCB
    for k, v in pairs(roles) do
        local id = (v.id) or k
        IDPreloadPrefab.extractRole(id)
    end
    IDPreloadPrefab.startPreload()
end

---@public 预加载建筑
function IDPreloadPrefab.preloadBuildings(buildings, callback, progressCB)
    IDPreloadPrefab.reset()
    IDPreloadPrefab.onFinishCallback = callback
    IDPreloadPrefab.onProgressCB = progressCB
    for k, v in pairs(buildings) do
        local id = (v.id) or k
        IDPreloadPrefab.extractBuilding(id)
    end
    IDPreloadPrefab.startPreload()
end

---@param queue CLLQueue
function IDPreloadPrefab.enQueue(queue, prefabName)
    if isNilOrEmpty(prefabName) or queue:contains(prefabName) then
        return
    end
    queue:enQueue(prefabName)
    IDPreloadPrefab.totalAssets = IDPreloadPrefab.totalAssets + 1
end

---@public 提取了角色相关的需要预加载的资源
function IDPreloadPrefab.extractRole(id)
    local cfg = DBCfg.getRoleByID(id)
    if cfg then
        IDPreloadPrefab.enQueue(IDPreloadPrefab.roleQueue, cfg.PrefabName)
        IDPreloadPrefab.enQueue(IDPreloadPrefab.soundQueue, cfg.AttackSound)
        IDPreloadPrefab.enQueue(IDPreloadPrefab.effectQueue, cfg.AttackEffect)
        IDPreloadPrefab.extractBullet(bio2Int(cfg.Bullets))
        if bio2Int(cfg.SolderNum) > 0 then
            -- 说明是可以下来士兵的
            IDPreloadPrefab.extractRole(3)
        end
    else
        printe("get cfg is nil.id=" .. id .. ",type=" .. type(id))
        if IDPreloadPrefab.onFinishCallback then
            IDPreloadPrefab.onFinishCallback()
        end
    end
end

---@public 提取了建筑相关的需要预加载的资源
function IDPreloadPrefab.extractBuilding(id)
    local cfg = DBCfg.getBuildingByID(id)
    if cfg then
        IDPreloadPrefab.enQueue(IDPreloadPrefab.soundQueue, cfg.AttackSound)
        IDPreloadPrefab.enQueue(IDPreloadPrefab.effectQueue, cfg.AttackEffect)
        IDPreloadPrefab.extractBullet(bio2Int(cfg.Bullets))
    else
        printe("get cfg is nil.id=" .. id .. ",type=" .. type(id))
        if IDPreloadPrefab.onFinishCallback then
            IDPreloadPrefab.onFinishCallback()
        end
    end
end

---@public 子弹
function IDPreloadPrefab.extractBullet(id)
    if id > 0 then
        local attr = DBCfg.getBulletByID(id)
        IDPreloadPrefab.enQueue(IDPreloadPrefab.bulletQueue, attr.PrefabName)
        IDPreloadPrefab.enQueue(IDPreloadPrefab.effectQueue, attr.OnHitEffect)
        IDPreloadPrefab.enQueue(IDPreloadPrefab.soundQueue, attr.OnHitSoundEff)
    end
end

--//////////////////////////////////////////////////////////////////
function IDPreloadPrefab.startPreload()
    --加载顺序 角色->特效->音效->子弹->物件->ui物件
    IDPreloadPrefab.loadRole()
end

function IDPreloadPrefab.loadRole(obj)
    if obj then
        IDPreloadPrefab.onFinishOne(obj)
    end
    if IDPreloadPrefab.roleQueue:size() > 0 then
        CLRolePool.setPrefab(IDPreloadPrefab.roleQueue:deQueue(), IDPreloadPrefab.loadRole)
    else
        IDPreloadPrefab.loadEffect()
    end
end

function IDPreloadPrefab.loadEffect(obj)
    if obj then
        IDPreloadPrefab.onFinishOne(obj)
    end
    if IDPreloadPrefab.effectQueue:size() > 0 then
        CLEffectPool.setPrefab(IDPreloadPrefab.effectQueue:deQueue(), IDPreloadPrefab.loadEffect)
    else
        IDPreloadPrefab.loadSound()
    end
end

function IDPreloadPrefab.loadSound(obj)
    if obj then
        IDPreloadPrefab.onFinishOne(obj)
    end
    if IDPreloadPrefab.soundQueue:size() > 0 then
        CLSoundPool.setPrefab(IDPreloadPrefab.soundQueue:deQueue(), IDPreloadPrefab.loadSound)
    else
        IDPreloadPrefab.loadThing()
    end
end

function IDPreloadPrefab.loadThing(obj)
    if obj then
        IDPreloadPrefab.onFinishOne(obj)
    end
    if IDPreloadPrefab.thingQueue:size() > 0 then
        CLThingsPool.setPrefab(IDPreloadPrefab.thingQueue:deQueue(), IDPreloadPrefab.loadThing)
    else
        IDPreloadPrefab.loadUIThing()
    end
end

function IDPreloadPrefab.loadUIThing(obj)
    if obj then
        IDPreloadPrefab.onFinishOne(obj)
    end
    if IDPreloadPrefab.uiThingQueue:size() > 0 then
        CLUIOtherObjPool.setPrefab(IDPreloadPrefab.uiThingQueue:deQueue(), IDPreloadPrefab.loadUIThing)
    else
        -- finish
        if IDPreloadPrefab.onFinishCallback then
            IDPreloadPrefab.onFinishCallback()
        end
    end
end

function IDPreloadPrefab.onFinishOne(obj)
    IDPreloadPrefab.currCount = IDPreloadPrefab.currCount + 1
    if (IDPreloadPrefab.onProgressCB ~= nil) then
        IDPreloadPrefab.onProgressCB(IDPreloadPrefab.totalAssets, IDPreloadPrefab.currCount)
    end
end

return IDPreloadPrefab
