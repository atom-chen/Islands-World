-- 世界地图
require("city.IDMainCity")
require("worldmap.IDWorldMapPage")
IDWorldMap = {}
IDWorldMap.popupEvent = {}
local csSelf = nil
local transform = nil
local gameObject
local cellSize = 10
local ConstCreenSize = 10
-- 完成进城时的回调
IDWorldMap.finishEnterCityCallbacks = {}
---@type CLGrid
IDWorldMap.grid = nil
IDWorldMap.mode = GameModeSub.none
---@type GridBase
local grid = nil
local isInited = false
local lookAtTarget = MyCfg.self.lookAtTarget
local lookAtTargetTween = lookAtTarget:GetComponent("MyTween")
local mapCells = {} -- 地块，key=在块网格idx， val=luatable
IDWorldMap.ocean = nil
local drag4World = CLUIDrag4World.self
---@type CLLQueue
local freePages = CLLQueue.new()
local pages = {}
---@type Coolape.CLSmoothFollow
local smoothFollow = IDLCameraMgr.smoothFollow
local cityGidx = 0
local centerPageIdx = -1
local dragSetting = CLUIDrag4World.self
IDWorldMap.offset4Ocean = -0.6 * Vector3.up
IDWorldMap.mapTileSize = nil -- 大地图地块
IDWorldMap.scaleCityHeighMin = 45
IDWorldMap.scaleCityHeighMax = 50
local isDragOcean = false
local popupMenus

function IDWorldMap.__init()
    if isInited then
        return
    end
    transform = getChild(MyMain.self.transform, "worldmap")
    IDWorldMap.transform = transform
    gameObject = transform.gameObject
    IDWorldMap.gameObject = gameObject
    csSelf = gameObject:GetComponent("CLBaseLua")
    IDWorldMap.csSelf = csSelf
    ---@type CLGrid
    IDWorldMap.grid = getCC(transform, "grid", "CLGrid")
    grid = IDWorldMap.grid.grid

    local worldsize = bio2number(DBCfg.getConstCfg().GridWorld)
    IDWorldMap.grid.numRows = worldsize
    IDWorldMap.grid.numCols = worldsize
    IDWorldMap.grid.numGroundRows = worldsize
    IDWorldMap.grid.numGroundCols = worldsize
    IDWorldMap.grid.cellSize = cellSize
    IDWorldMap.grid.transform.localPosition = Vector3(-worldsize * cellSize / 2, 0, -worldsize * cellSize / 2)
    IDWorldMap.grid:init(false)

    IDWorldMap.fogOfWarInfluence = GameObject("fogOfWarInfluence"):AddComponent(typeof(FogOfWarInfluence))
    IDWorldMap.fogOfWarInfluence.transform.parent = transform
    IDWorldMap.fogOfWarInfluence.transform.localPosition = Vector3.zero
    IDWorldMap.fogOfWarInfluence.transform.localScale = Vector3.one

    for i = 1, 9 do
        freePages:enQueue(IDWorldMapPage.new())
    end

    popupMenus = {
        enterCity = {
            --进城
            nameKey = "Enter",
            callback = IDWorldMap.popupEvent.enterCity,
            icon = "icon_detail",
            bg = "public_edit_circle_bt_management"
        },
        attack = {
            --攻击
            nameKey = "Attack",
            callback = IDWorldMap.popupEvent.attack,
            icon = "icon_detail",
            bg = "public_edit_circle_bt_management"
        },
        moveCity = {
            --迁城
            nameKey = "MoveCity",
            callback = IDWorldMap.popupEvent.moveCity,
            icon = "icon_detail",
            bg = "public_edit_circle_bt_management"
        }
    }
end

function IDWorldMap.init(gidx, onFinishCallback, onProgress)
    IDWorldMap.__init()
    lookAtTarget.transform.position = grid:GetCellCenter(gidx)

    local onLoadOcena = function()
        IDWorldMap.ocean.luaTable.playBGM()
        -- 屏幕拖动代理
        drag4World.onDragMoveDelegate = IDWorldMap.onDragMove
        drag4World.onDragScaleDelegate = IDWorldMap.onTouchScaleGround
        local pageIdx = IDWorldMap.getPageIdx(gidx)
        cityGidx = gidx
        centerPageIdx = pageIdx
        IDWorldMap.mode = GameModeSub.map
        IDWorldMap.loadPagesData()

        IDMainCity.init(
            nil,
            function()
                onFinishCallback()
                smoothFollow:tween(Vector2(20, 100), Vector2(10, 15), 2.5, nil, IDWorldMap.scaleGround)
            end,
            onProgress
        )
        IDWorldMap.showFogwar()
    end

    if IDWorldMap.mapTileSize == nil then
        CLThingsPool.borrowObjAsyn(
            "MapTileSize",
            function(name, obj, orgs)
                IDWorldMap.mapTileSize = obj
                IDWorldMap.mapTileSize.transform.parent = transform
                IDWorldMap.mapTileSize.transform.localScale = Vector3.one
                IDWorldMap.mapTileSize.transform.localEulerAngles = Vector3.zero
                SetActive(obj, false)
            end
        )
    end

    if IDWorldMap.ocean == nil then
        CLThingsPool.borrowObjAsyn(
            "OceanLow",
            function(name, obj, orgs)
                IDWorldMap.onLoadOcena(name, obj, orgs)
                onLoadOcena()
            end
        )
    else
        onLoadOcena()
    end
    IDWorldMap.grid:showRect()
end

-- 重新加载海
function IDWorldMap.onLoadOcena(name, obj, orgs)
    IDWorldMap.oceanObj = obj:GetComponent("MirrorReflection")
    IDWorldMap.oceanTransform = IDWorldMap.oceanObj.transform
    IDWorldMap.oceanTransform.parent = transform
    --IDWorldMap.oceanTransform.localPosition = IDMainCity.offset4Ocean
    IDWorldMap.oceanTransform.localScale = Vector3.one
    ---@type Coolape.CLBaseLua
    local oceanlua = IDWorldMap.oceanObj:GetComponent("CLBaseLua")
    if oceanlua.luaTable == nil then
        oceanlua:setLua()
        oceanlua.luaTable.init(oceanlua)
    end
    -- 先为false
    IDWorldMap.oceanObj.enableMirrorReflection = false
    IDWorldMap.ocean = oceanlua
    IDWorldMap.oceanTransform.position = lookAtTarget.position + IDWorldMap.offset4Ocean
    SetActive(obj, true)
end

function IDWorldMap.setGameMode()
    -- 判断当前中心点离自己的主城的距离来处理是否可以进入城里面
    if IDWorldMap.mode == GameModeSub.map then
        local lastHit =
            Utl.getRaycastHitInfor(
            MyCfg.self.mainCamera,
            Vector3(Screen.width / 2, Screen.height / 2, 0),
            Utl.getLayer("Water")
        )
        if lastHit then
            local currCenterPos = lastHit.point
            currCenterPos.y = 0
            local centerPos = grid:GetCellCenter(cityGidx)
            if Vector3.Distance(currCenterPos, centerPos) > 40 then
                dragSetting.scaleMini = 9
                dragSetting.scaleMax = 20
                dragSetting.scaleHeightMini = 50
                dragSetting.scaleHeightMax = 100
                return
            end
        end
    end
    if GameMode.map == MyCfg.mode then
        dragSetting.scaleMini = 7
        dragSetting.scaleMax = 20
        dragSetting.scaleHeightMini = 10
        dragSetting.scaleHeightMax = 100
    else
        dragSetting.scaleMini = 7
        dragSetting.scaleMax = 20
        dragSetting.scaleHeightMini = 10
        dragSetting.scaleHeightMax = 30
    end

    if smoothFollow.height > IDWorldMap.scaleCityHeighMin and smoothFollow.height < IDWorldMap.scaleCityHeighMax then
        if IDWorldMap.mode == GameModeSub.map then
            IDWorldMap.cleanPages()
        end
        if IDWorldMap.mode ~= GameModeSub.mapBtwncity and MyCfg.mode == GameMode.map then
            IDMainCity.onChgMode(IDWorldMap.mode, GameModeSub.mapBtwncity)
            IDWorldMap.grid:hideRect()
            IDWorldMap.mode = GameModeSub.mapBtwncity
            dragSetting.viewRadius = 15000
            dragSetting.viewCenter = Vector3.zero
            if IDPMain then
                -- 通知ui
                IDPMain.onChgMode()
            end
        end
    elseif smoothFollow.height > IDWorldMap.scaleCityHeighMax then
        if IDWorldMap.mode ~= GameModeSub.map and MyCfg.mode == GameMode.map then
            IDMainCity.onChgMode(IDWorldMap.mode, GameModeSub.map)
            IDWorldMap.grid:showRect()
            IDWorldMap.mode = GameModeSub.map
            dragSetting.viewRadius = 15000
            dragSetting.viewCenter = Vector3.zero
            if IDPMain then
                -- 通知ui
                IDPMain.onChgMode()
            end
        end
    else
        if IDWorldMap.mode ~= GameModeSub.city then
            IDMainCity.onChgMode(IDWorldMap.mode, GameModeSub.city)
            IDWorldMap.grid:hideRect()
            IDWorldMap.mode = GameModeSub.city
            dragSetting.viewRadius = 65
            dragSetting.viewCenter = grid:GetCellCenter(bio2number(IDDBCity.curCity.pos))
            if IDPMain then
                -- 通知ui
                IDPMain.onChgMode()
            end
        end
    end
end

function IDWorldMap.onTouchScaleGround(delta, offset)
    if GameMode.map == MyCfg.mode then
        IDWorldMap.scaleGround(delta, offset)
    else
        drag4World:procScaler(offset)
    end
end

function IDWorldMap.scaleGround(delta, offset)
    drag4World:procScaler(offset)
    IDWorldMap.setGameMode()
    ---@param v IDWorldMapPage
    for k, v in pairs(pages) do
        v:onScaleScreen(delta, offset)
    end
    IDMainCity.onScaleScreen(delta, offset)
    if IDWorldMap.mapTileSize then
        SetActive(IDWorldMap.mapTileSize, false)
    end
    IDUtl.hidePopupMenus()
end

function IDWorldMap.onDragMove(delta)
    IDWorldMap.oceanTransform.position = lookAtTarget.position + IDWorldMap.offset4Ocean
    if MyCfg.mode == GameMode.map then
        -- 取得屏幕中心点下的地块
        local lastHit =
            Utl.getRaycastHitInfor(
            MyCfg.self.mainCamera,
            Vector3(Screen.width / 2, Screen.height / 2, 0),
            Utl.getLayer("Water")
        )
        if lastHit then
            local centerPos = lastHit.point
            local tmpPageIdx = IDWorldMap.getPageIdx(grid:GetCellIndex(centerPos))
            if tmpPageIdx ~= centerPageIdx then
                -- 说明已经切换屏了,重新加载数据
                centerPageIdx = tmpPageIdx
                IDWorldMap.refreshPagesData()
            end
        end
    end
end

---@public 刷新9屏
function IDWorldMap.refreshPagesData()
    local currPages = IDWorldMap.getAroundPage()
    for pageIdx, page in pairs(pages) do
        if currPages[pageIdx] == nil then
            page:clean()
            freePages:enQueue(page)
            pages[pageIdx] = nil
        end
    end
    for pageIdx, v in pairs(currPages) do
        if pages[pageIdx] == nil then
            IDWorldMap.loadMapPageData(pageIdx)
        end
    end
end

function IDWorldMap.showFogwar()
    local fogOfWar = MyCfg.self.fogOfWar
    local worldsize = bio2number(DBCfg.getConstCfg().GridWorld)
    local citysize = bio2number(DBCfg.getConstCfg().GridCity)
    worldsize = (worldsize + 200) * cellSize
    fogOfWar.color = Color.black -- ColorEx.getColor(34, 34, 34);
    fogOfWar.Size = worldsize
    fogOfWar.transform.position = Vector3(-worldsize / 2, 0, -worldsize / 2)
    SetActive(fogOfWar.gameObject, true)

    -- 设置可视范围
    IDWorldMap.fogOfWarInfluence.ViewDistance = (citysize * 2)
    local gridIndex = bio2number(IDDBCity.curCity.pos)
    IDWorldMap.fogOfWarInfluence.transform.position = grid:GetCellCenter(gridIndex)
    fogOfWar:ClearPersistenFog()
end

---@public 通过网格idx取得所以屏的index
function IDWorldMap.getPageIdx(gidx)
    local pos = grid:GetCellCenter(gidx)
    pos = pos - grid.Origin

    local col, row
    local screen = ConstCreenSize * IDWorldMap.grid.cellSize
    col = NumEx.getIntPart(pos.x / screen)
    row = NumEx.getIntPart(pos.z / screen)

    local x2 = col * screen + (screen / 2)
    local z2 = row * screen + (screen / 2)
    local cellPosition = grid.Origin + Vector3(x2, 0, z2)
    return grid:GetCellIndex(cellPosition)
end

---@public 取得中心屏四周8屏的index
function IDWorldMap.getAroundPage()
    local ret = {}
    if centerPageIdx < 0 then
        return ret
    end
    local screenSize = ConstCreenSize * IDWorldMap.grid.cellSize
    local centerPos = grid:GetCellCenter(centerPageIdx)
    -- left
    local pos = centerPos + Vector3(-1, 0, 0) * screenSize
    local index = grid:GetCellIndex(pos)
    ret[index] = index
    -- right
    pos = centerPos + Vector3(1, 0, 0) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    -- up
    pos = centerPos + Vector3(0, 0, 1) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    -- down
    pos = centerPos + Vector3(0, 0, -1) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    -- leftup
    pos = centerPos + Vector3(-1, 0, 1) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    -- leftdown
    pos = centerPos + Vector3(-1, 0, -1) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    -- rightup
    pos = centerPos + Vector3(1, 0, 1) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    -- rightdown
    pos = centerPos + Vector3(1, 0, -1) * screenSize
    index = grid:GetCellIndex(pos)
    ret[index] = index
    return ret
end

---@public 加载9屏
function IDWorldMap.loadPagesData()
    IDWorldMap.loadMapPageData(centerPageIdx)
    local pageIndexs = IDWorldMap.getAroundPage()
    for i, v in pairs(pageIndexs) do
        IDWorldMap.loadMapPageData(v)
    end
end

---@public 加载一屏
function IDWorldMap.loadMapPageData(pageIdx)
    if pageIdx < 0 then
        return
    end
    local mapPage = pages[pageIdx] or freePages:deQueue()
    mapPage:init(pageIdx)
    pages[pageIdx] = mapPage
end

---@public 服务器数据刷新时
function IDWorldMap.onGetMapPageData(pageIdx, cells)
    ---@type IDWorldMapPage
    local mapPage = pages[pageIdx]
    if mapPage then
        mapPage:refreshTiles()
    end
end

function IDWorldMap.onPress(isPressed)
    if IDWorldMap.mode == GameModeSub.city then
        IDMainCity.onPress(isPressed)
        return
    end
    if isPressed then
    else
        csSelf:invoke4Lua(
            function()
                isDragOcean = false
            end,
            0.1
        )
        IDUtl.hidePopupMenus()
    end
end

function IDWorldMap.onDragOcean()
    if IDWorldMap.mode == GameModeSub.city then
        IDMainCity.onDragOcean()
        return
    end
    isDragOcean = true
    if IDWorldMap.mapTileSize and IDWorldMap.mapTileSize.activeInHierarchy then
        csSelf:invoke4Lua(IDWorldMap.hideOnClickShow, 0.3)
    end
end

---@public 隐藏点击后显示的相关显示物
function IDWorldMap.hideOnClickShow()
    if isDragOcean then
        SetActive(IDWorldMap.mapTileSize, false)
    end
    --//TODO:隐藏主UI，当视野更广
end

---@public 点击了海面
function IDWorldMap.onClickOcean()
    if IDWorldMap.mode == GameModeSub.city then
        IDMainCity.onClickOcean()
        return
    end
    isDragOcean = false
    local clickPos = MyMainCamera.lastHit.point
    local index = grid:GetCellIndex(clickPos)
    local cellPos = grid:GetCellCenter(index)
    if MyCfg.mode == GameMode.map then
        if IDWorldMap.mapTileSize then
            IDWorldMap.mapTileSize.transform.position = cellPos
            SetActive(IDWorldMap.mapTileSize, true)
        end
        if MyCfg.self.fogOfWar:GetVisibility(cellPos) == FogOfWarSystem.FogVisibility.Visible then
            -- 当可见时，才弹出菜单
            local label = joinStr("Pos:", index)
            IDUtl.showPopupMenus(nil, cellPos, {popupMenus.moveCity}, label, index)
        end
    end
end

---@public 点击了自己的城
function IDWorldMap.onClickSelfCity()
    IDWorldMap.onClickOcean()
    local clickPos = MyMainCamera.lastHit.point
    local index = grid:GetCellIndex(clickPos)
    local cellPos = grid:GetCellCenter(index)
    local buttons = {}
    table.insert(buttons, popupMenus.enterCity)
    table.insert(buttons, popupMenus.attack)
    local label = joinStr("Pos:", index)
    IDUtl.showPopupMenus(nil, cellPos, buttons, label, index)
end

---@public 添加进城的回调
function IDWorldMap.addFinishEnterCityCallback(func)
    IDWorldMap.finishEnterCityCallbacks[func] = func
end
---@public remove进城的回调
function IDWorldMap.rmFinishEnterCityCallback(func)
    IDWorldMap.finishEnterCityCallbacks[func] = nil
end

---@public 处理当进城后的回调
function IDWorldMap.finisEnterCity()
    for k,func in pairs(IDWorldMap.finishEnterCityCallbacks) do
        if func then
            func()
        end
    end
end

IDWorldMap.popupEvent = {
    ---@public 进入的城
    enterCity = function(cellIndex)
        local cellPos = grid:GetCellCenter(cellIndex)
        IDUtl.hidePopupMenus()
        smoothFollow:tween(
            Vector2(smoothFollow.distance, smoothFollow.height),
            Vector2(10, 15),
            3,
            IDWorldMap.finisEnterCity,
            IDWorldMap.scaleGround
        )
        lookAtTargetTween:flyout(cellPos, 2, 0, 0, nil, nil, nil, true)
    end,
    ---@public 攻击
    attack = function(cellIndex)
        IDUtl.hidePopupMenus()
        showHotWheel()
        net:send(NetProtoIsland.send.attack(cellIndex, IDWorldMap.doAttack, cellIndex))
    end,
    ---@public 搬迁
    moveCity = function(cellIndex)
        IDUtl.hidePopupMenus()
        net:send(NetProtoIsland.send.moveCity(cellIndex, IDWorldMap.doMoveCity, cellIndex))
    end
}

---@public 迁城服务器接口回调
function IDWorldMap.doMoveCity(cellIndex, retData)
    if bio2number(retData.retInfor.code) == NetSuccess then
        cityGidx = cellIndex
        -- IDWorldMap.showFogwar()
        IDWorldMap.fogOfWarInfluence.transform.position = grid:GetCellCenter(cellIndex)
        if IDMainCity then
            IDMainCity.onMoveCity()
        end
    end
end

---@public 战斗服务器接口回调
---@param retData NetProtoIsland.RC_attack
function IDWorldMap.doAttack(cellIndex, retData)
    hideHotWheel()
    local code = BioUtl.bio2int(retData.retInfor.code)
    if code == NetSuccess then
        ---@type IDDBPlayer
        local player = IDDBPlayer.new(retData.player)
        ---@type IDDBCity
        local city = IDDBCity.new(retData.city)
        city:setAllDockyardShips(retData.dockyardShipss)
        ---进攻方舰船数据
        local atkShips = retData.dockyardShipss2
        local offShips = {}
        local _offShips = {}
        ---@param v NetProtoIsland.ST_dockyardShips
        for k, v in pairs(atkShips) do
            for shipId, num in pairs(v.shipsMap or {}) do
                if num > 0 then
                    _offShips[shipId] = (_offShips[shipId] or 0) + num
                end
            end
        end
        for k, v in pairs(_offShips) do
            -- 转成bio存储，避免被修改
            offShips[tonumber(k)] = {id = tonumber(k), num = number2bio(v)}
        end

        ---@type BattleData
        local battleData = {}
        battleData.type = IDConst.BattleType.pvp
        battleData.targetCity = city
        battleData.offShips = offShips

        IDUtl.chgScene(
            GameMode.battle,
            battleData,
            function()
                IDWorldMap.popupEvent.enterCity(cellIndex)
            end
        )
    else
        CLAlert.add(LGet("Error_" .. code))
    end
end

---@public 当地图块有变化时的推送
function IDWorldMap.onMapCellChg(mapCell)
    local pageIdx = bio2number(mapCell.pageIdx)
    ---@type IDWorldMapPage
    local page = pages[pageIdx]
    if page then
        page:refreshOneCell(mapCell)
    end
end

---@public 清除所有页的元素
function IDWorldMap.cleanPages()
    for pageIdx, page in pairs(pages) do
        page:clean()
        freePages:enQueue(page)
        pages[pageIdx] = nil
    end
end

---@public 离开世界后的清理
function IDWorldMap.clean()
    IDWorldMap.finishEnterCityCallbacks = {}
    IDWorldMap.cleanPages()
    if IDWorldMap.mapTileSize then
        CLThingsPool.returnObj(IDWorldMap.mapTileSize)
        SetActive(IDWorldMap.mapTileSize, false)
        IDWorldMap.mapTileSize = nil
    end
    IDWorldMap.grid:clean()
end

---@public 需要销毁处理
function IDWorldMap.destory()
    IDWorldMap.clean()
    if IDWorldMap.ocean then
        if IDWorldMap.ocean.luaTable then
            IDWorldMap.ocean.luaTable.clean()
        end
        CLThingsPool.returnObj(IDWorldMap.ocean.gameObject)
        SetActive(IDWorldMap.ocean.gameObject, false)
        IDWorldMap.ocean = nil
    end

    GameObject.DestroyImmediate(IDWorldMap.fogOfWarInfluence.gameObject)
    IDWorldMap.fogOfWarInfluence = nil
end
--------------------------------------------
return IDWorldMap
