require("public.CLLQueue")
---@public 预加载处理
local IDPreloadPrefab = {}
---@type CLLQueue
IDPreloadPrefab.roleQueue = CLLQueue.new()      -- 角色
IDPreloadPrefab.soundQueue = CLLQueue.new()     -- 音乐，音效
IDPreloadPrefab.effectQueue = CLLQueue.new()    -- 特效
IDPreloadPrefab.bulletQueue = CLLQueue.new()    -- 子弹
IDPreloadPrefab.thingQueue = CLLQueue.new()    -- 物件
IDPreloadPrefab.totalAssets = 0     -- 需要预加载资源总量

---@public 预加载角色
function IDPreloadPrefab.preloadRoles(roles, callback, progressCB)
    IDPreloadPrefab.roleQueue:clear()
    IDPreloadPrefab.soundQueue:clear()
    IDPreloadPrefab.effectQueue:clear()
    IDPreloadPrefab.bulletQueue:clear()
    IDPreloadPrefab.thingQueue:clear()
    IDPreloadPrefab.totalAssets = 0

    for k, v in pairs(roles) do
        local id = (v.id)
        IDPreloadPrefab.extractRole(id)
    end
end

---@param queue CLLQueue
function IDPreloadPrefab.enQueue(queue, prefabName)
    if queue:contain(prefabName) then
        return
    end
    queue:enQueue(prefabName)
    IDPreloadPrefab.totalAssets = IDPreloadPrefab.totalAssets + 1
end

---@public 提取了角色相关的需要预加载的资源
function IDPreloadPrefab.extractRole(id)
    local cfg = DBCfg.getRoleByID(id)
    IDPreloadPrefab.enQueue(IDPreloadPrefab.roleQueue, joinStr("ship", id))
    IDPreloadPrefab.enQueue(IDPreloadPrefab.soundQueue, cfg.AttackSound)
    IDPreloadPrefab.enQueue(IDPreloadPrefab.effectQueue, cfg.AttackEffect)
    IDPreloadPrefab.extractBullet(bio2Int(cfg.Bullets))
end

---@public 子弹
function IDPreloadPrefab.extractBullet(id)
    --//TODO:
end

return IDPreloadPrefab
