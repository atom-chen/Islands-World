﻿-- 世界地图
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
---@type SimpleFogOfWar.FogOfWarInfluence 离屏中心最近的Influence
IDWorldMap.nearestInfluence = nil
---@type GridBase
local grid = nil
local isInited = false
local lookAtTarget = MyCfg.self.lookAtTarget
---@type Coolape.MyTween
local lookAtTargetTween = lookAtTarget:GetComponent("MyTween")
---@type Coolape.CLSmoothFollow
local lookAtTargetFollow = lookAtTarget:GetComponent("CLSmoothFollow")
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
local aroundPageCache = {}
-- 舰队对象
local fleets = {}

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

    ---@type SimpleFogOfWar.FogOfWarInfluence
    -- IDWorldMap.fogOfWarInfluence = GameObject("fogOfWarInfluence"):AddComponent(typeof(FogOfWarInfluence))
    -- IDWorldMap.fogOfWarInfluence.transform.parent = transform
    -- IDWorldMap.fogOfWarInfluence.transform.localPosition = Vector3.zero
    -- IDWorldMap.fogOfWarInfluence.transform.localScale = Vector3.one

    for i = 1, 9 do
        freePages:enQueue(IDWorldMapPage.new())
    end

    popupMenus = {
        enterCity = {
            --进城
            nameKey = "Enter",
            callback = IDWorldMap.popupEvent.enterCity,
            icon = "icon_detail", --//TODO:图标
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
        },
        docked = {
            -- 停泊
            nameKey = "Docked",
            callback = IDWorldMap.popupEvent.docked,
            icon = "icon_detail",
            bg = "public_edit_circle_bt_management"
        },
        SetBeacon = {
            -- 设置为灯标
            nameKey = "SetBeacon",
            callback = IDWorldMap.popupEvent.setBeacon,
            icon = "icon_detail",
            bg = "public_edit_circle_bt_management"
        },
        RmBeacon = {
            -- 移除灯标
            nameKey = "RmBeacon",
            callback = IDWorldMap.popupEvent.rmBeacon,
            icon = "icon_detail",
            bg = "public_edit_circle_bt_management"
        },
        MoveTo = {
            -- 移动到指定位置
            nameKey = "MoveTo",
            callback = IDWorldMap.popupEvent.moveTo,
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

        -- 先设置fog，后面加载地图地块时会用到判断是否可见
        IDWorldMap.showFogwar()
        IDWorldMap.mode = GameModeSub.map
        IDWorldMap.loadPagesData()

        IDMainCity.init(
            nil,
            function()
                CLLNet.send(NetProtoIsland.send.setPlayerCurrLook4WorldPage(centerPageIdx))
                onFinishCallback()
                smoothFollow:tween(Vector2(20, 100), Vector2(10, 15), 2.5, nil, IDWorldMap.scaleGround)
            end,
            onProgress
        )
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
    if IDWorldMap.mode == GameModeSub.fleet then
        return
    end
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
        if IDWorldMap.mode ~= GameModeSub.mapBtwncity and MyCfg.mode == GameMode.map then
            if IDWorldMap.mode == GameModeSub.map then
                IDWorldMap.cleanPages()
            end
            IDWorldMap.mode = GameModeSub.mapBtwncity
            IDMainCity.onChgMode(IDWorldMap.mode, GameModeSub.mapBtwncity)
            IDWorldMap.grid:hideRect()
            dragSetting.viewRadius = 15000
            dragSetting.viewCenter = Vector3.zero
            if IDPMain then
                -- 通知ui
                IDPMain.onChgMode()
            end
        end
    elseif smoothFollow.height > IDWorldMap.scaleCityHeighMax then
        if IDWorldMap.mode ~= GameModeSub.map and MyCfg.mode == GameMode.map then
            IDWorldMap.mode = GameModeSub.map
            IDMainCity.onChgMode(IDWorldMap.mode, GameModeSub.map)
            IDWorldMap.refreshPagesData()
            IDWorldMap.grid:showRect()
            dragSetting.viewRadius = 15000
            dragSetting.viewCenter = Vector3.zero
            if IDPMain then
                -- 通知ui
                IDPMain.onChgMode()
            end
        end
    else
        if IDWorldMap.mode ~= GameModeSub.city then
            if IDWorldMap.mode == GameModeSub.map then
                -- 可能出现直接从map跳到city的情况
                IDWorldMap.cleanPages()
            end
            IDWorldMap.mode = GameModeSub.city
            IDMainCity.onChgMode(IDWorldMap.mode, GameModeSub.city)
            IDWorldMap.grid:hideRect()
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
    IDWorldMap.recheckCellsVisible()
    IDMainCity.onScaleScreen(delta, offset)
    if IDWorldMap.mapTileSize then
        SetActive(IDWorldMap.mapTileSize, false)
    end
    IDUtl.hidePopupMenus()
end

function IDWorldMap.onDragMove(delta)
    IDWorldMap.oceanTransform.position = lookAtTarget.position + IDWorldMap.offset4Ocean

    if MyCfg.mode == GameMode.map and IDWorldMap.mode ~= GameModeSub.city then
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
                -- 通知服务器，我当前所在屏改变了
                CLLNet.send(NetProtoIsland.send.setPlayerCurrLook4WorldPage(centerPageIdx))
                IDWorldMap.refreshPagesData()
            end
        end
    end
    IDWorldMap.recheckCellsVisible()
end

function IDWorldMap.recheckCellsVisible()
    if IDWorldMap.isRecheckingCellsVisible then
        return
    end
    IDWorldMap.isRecheckingCellsVisible = true
    InvokeEx.invokeByUpdate(IDWorldMap.doRecheckCellsVisible, 0.5)
end

function IDWorldMap.doRecheckCellsVisible()
    local centerPos
    local lastHit =
        Utl.getRaycastHitInfor(
        MyCfg.self.mainCamera,
        Vector3(Screen.width / 2, Screen.height / 2, 0),
        Utl.getLayer("Water")
    )
    if lastHit then
        centerPos = lastHit.point

        -- 取得最近的influence
        local influences = MyCfg.self.fogOfWar:getInfluences()
        local nearestDis = -1
        local dis
        IDWorldMap.nearestInfluence = nil
        for i = 0, influences.Count - 1 do
            dis = Vector3.Distance(influences[i].transform.position, centerPos)
            if nearestDis < 0 or dis < nearestDis then
                IDWorldMap.nearestInfluence = influences[i]
            end
        end
    end

    ---@param v IDWorldMapPage
    for k, v in pairs(pages) do
        v:checkVisible(centerPos)
    end
    IDWorldMap.isRecheckingCellsVisible = false
end

---@public 刷新9屏
function IDWorldMap.refreshPagesData()
    IDWorldMap.loadFleets()
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
    fogOfWar:ClearPersistenFog()

    -- 设置可视范围
    -- IDWorldMap.fogOfWarInfluence.ViewDistance = (citysize * 2)
    -- local gridIndex = bio2number(IDDBCity.curCity.pos)
    -- IDWorldMap.fogOfWarInfluence.transform.position = grid:GetCellCenter(gridIndex)
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
    if aroundPageCache[centerPageIdx] then
        return aroundPageCache[centerPageIdx]
    end
    local screenSize = ConstCreenSize * IDWorldMap.grid.cellSize
    local centerPos = grid:GetCellCenter(centerPageIdx)
    -- center
    local index = grid:GetCellIndex(centerPos)
    ret[index] = index
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
    aroundPageCache[centerPageIdx] = ret
    return ret
end

---@public 加载9屏
function IDWorldMap.loadPagesData()
    local pageIndexs = IDWorldMap.getAroundPage()
    for i, v in pairs(pageIndexs) do
        IDWorldMap.loadMapPageData(v)
    end
    IDWorldMap.loadFleets()
end

---@public 加载一屏
function IDWorldMap.loadMapPageData(pageIdx)
    if pageIdx < 0 then
        return
    end
    ---@type IDWorldMapPage
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
        if isDragOcean then
            IDWorldMap.doRecheckCellsVisible()
        end
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

---@param tile IDWorldTile
function IDWorldMap.onClickTile(tile)
    isDragOcean = false
    local index = tile.gidx
    -- local cellPos = grid:GetCellCenter(index)
    if MyCfg.mode == GameMode.map then
        if IDWorldMap.mapTileSize then
            IDWorldMap.mapTileSize.transform.position = tile.transform.position
            IDWorldMap.mapTileSize.transform.localScale = Vector3.one * tile.size
            SetActive(IDWorldMap.mapTileSize, true)
        end
        if IDWorldMap.isVisibile(tile.transform.position) then
            -- 当可见时，才弹出菜单
            local label = joinStr("Pos:", index)
            local buttons = {}
            if tile.type == IDConst.WorldmapCellType.user then
                table.insert(buttons, popupMenus.attack)
            elseif tile.type == IDConst.WorldmapCellType.port then
                table.insert(buttons, popupMenus.docked)
            elseif tile.type == IDConst.WorldmapCellType.decorate then
                label = joinStr("[ff0000]", LGet("CannotProcTile"), "[-]")
            end

            IDUtl.showPopupMenus(tile, tile.transform.position, buttons, label, index)
        end
    end
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
        if IDWorldMap.isVisibile(cellPos) then
            if IDWorldMap.mode == GameModeSub.fleet then
                if IDWorldMap.selectedFleet then
                    CLLNet.send(NetProtoIsland.send.fleetDepart(bio2number(IDWorldMap.selectedFleet.data.idx), index))
                end
            else
                if IDWorldMap.mapTileSize then
                    IDWorldMap.mapTileSize.transform.position = cellPos
                    IDWorldMap.mapTileSize.transform.localScale = Vector3.one
                    SetActive(IDWorldMap.mapTileSize, true)
                end

                -- 当可见时，才弹出菜单
                local label = joinStr("Pos:", index)
                local buttons = {}
                table.insert(buttons, popupMenus.moveCity)
                table.insert(buttons, popupMenus.SetBeacon)
                table.insert(buttons, popupMenus.MoveTo)
                IDUtl.showPopupMenus(nil, cellPos, buttons, label, index)
            end
        end
    end
end

---@public 点击了自己的城
function IDWorldMap.onClickSelfCity()
    -- IDWorldMap.onClickOcean()
    local clickPos = MyMainCamera.lastHit.point
    local index = grid:GetCellIndex(clickPos)
    local cellPos = grid:GetCellCenter(index)

    if IDWorldMap.mapTileSize then
        IDWorldMap.mapTileSize.transform.position = cellPos
        IDWorldMap.mapTileSize.transform.localScale = Vector3.one
        SetActive(IDWorldMap.mapTileSize, true)
    end

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
    for k, func in pairs(IDWorldMap.finishEnterCityCallbacks) do
        if func then
            func()
        end
    end
end

IDWorldMap.popupEvent = {
    ---@public 进入的城
    enterCity = function(cellIndex)
        IDUtl.hidePopupMenus()
        IDWorldMap.moveToView(cellIndex, GameModeSub.city)
    end,
    ---@public 攻击
    attack = function(cellIndex)
        IDUtl.hidePopupMenus()
        showHotWheel()
        CLLNet.send(NetProtoIsland.send.attack(cellIndex, IDWorldMap.doAttack, cellIndex))
    end,
    ---@public 搬迁
    moveCity = function(cellIndex)
        IDUtl.hidePopupMenus()
        CLLNet.send(
            NetProtoIsland.send.moveCity(bio2number(IDDBCity.curCity.idx), cellIndex, IDWorldMap.doMoveCity, cellIndex)
        )
    end,
    ---@public 停靠
    docked = function(cellIndex)
        IDUtl.hidePopupMenus()
        if IDWorldMap.mode == GameModeSub.fleet then
            if IDWorldMap.selectedFleet then
                CLLNet.send(NetProtoIsland.send.fleetDepart(bio2number(IDWorldMap.selectedFleet.data.idx), cellIndex))
            end
        else
            getPanelAsy("PanelFleets", onLoadedPanelTT, {toPos = cellIndex})
        end
    end,
    ---@public 移动到
    moveTo = function(cellIndex)
        getPanelAsy("PanelFleets", onLoadedPanelTT, {toPos = cellIndex})
    end
}

---@public 迁城服务器接口回调
function IDWorldMap.doMoveCity(cellIndex, retData)
    if bio2number(retData.retInfor.code) == NetSuccess then
        cityGidx = cellIndex
        -- IDWorldMap.showFogwar()
        -- IDWorldMap.fogOfWarInfluence.transform.position = grid:GetCellCenter(cellIndex)
        if IDMainCity then
            IDMainCity.onMoveCity()
        end
        csSelf:invoke4Lua(IDWorldMap.recheckCellsVisible, 0.5)
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
            ---@param unit NetProtoIsland.ST_unitInfor
            for i, unit in pairs(v.ships or {}) do
                if bio2number(unit.num) > 0 then
                    _offShips[bio2number(unit.id)] = (_offShips[bio2number(unit.id)] or 0) + bio2number(unit.num)
                end
            end
        end
        for k, v in pairs(_offShips) do
            -- 转成bio存储，避免被修改
            offShips[k] = {id = k, num = number2bio(v)}
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
function IDWorldMap.onMapCellChg(mapCell, isRemove)
    if IDWorldMap.mode ~= GameModeSub.map and IDWorldMap.mode ~= GameModeSub.fleet then
        return
    end
    local pageIdx = bio2number(mapCell.pageIdx)
    ---@type IDWorldMapPage
    local page = pages[pageIdx]
    if page then
        page:refreshOneCell(mapCell, isRemove)
    end
end

---@public 清除所有页的元素
function IDWorldMap.cleanPages()
    for pageIdx, page in pairs(pages) do
        page:clean()
        freePages:enQueue(page)
        pages[pageIdx] = nil
    end

    ---@param v IDWorldFleet
    for k, v in pairs(fleets) do
        IDWorldMap.releaseFleet(bio2number(v.data.idx))
    end
    fleets = {}
end

---@public 离开世界后的清理
function IDWorldMap.clean()
    IDWorldMap.selectedFleet = nil
    IDWorldMap.isRecheckingCellsVisible = false
    IDWorldMap.finishEnterCityCallbacks = {}
    IDWorldMap.cleanPages()
    if IDWorldMap.mapTileSize then
        CLThingsPool.returnObj(IDWorldMap.mapTileSize)
        SetActive(IDWorldMap.mapTileSize, false)
        IDWorldMap.mapTileSize = nil
    end

    IDWorldMap.grid:clean()
    IDWorldMap.unselectFleet()
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

    -- GameObject.DestroyImmediate(IDWorldMap.fogOfWarInfluence.gameObject)
    -- IDWorldMap.fogOfWarInfluence = nil
end

---@public 是否可见
function IDWorldMap.isVisibile(position, bounds)
    if
        position and MyCfg.self.fogOfWar:GetVisibility(position) ~= FogOfWarSystem.FogVisibility.Visible and
            MyCfg.self.fogOfWar:GetVisibility(position) ~= FogOfWarSystem.FogVisibility.Undetermined
     then
        return false
    end
    if bounds and (not IDLCameraMgr.isInCameraView(bounds)) then
        return false
    end
    return true
end
---@public 是否通过9屏
---@param fleet NetProtoIsland.ST_fleetinfor
function IDWorldMap.isPassThe9Screens(fleet)
    if fleet == nil then
        return false
    end
    local pos1 = grid:GetCellCenter(bio2number(fleet.frompos))
    local pos2 = grid:GetCellCenter(bio2number(fleet.topos))
    local dir = pos2 - pos1
    local ray = Ray(pos1, dir)
    local maxDis = Vector3.Distance(pos1, pos2)

    local pages = IDWorldMap.getAroundPage()
    local bounds
    local bSize = Vector3.one * ConstCreenSize * IDWorldMap.grid.cellSize
    for k, index in pairs(pages) do
        if index >= 0 then
            bounds = MyBoundsPool.borrow(grid:GetCellCenter(index), bSize)
            if Utl.IntersectRay(bounds, ray, 0, maxDis) then
                MyBoundsPool.returnObj(bounds)
                return true
            end
            MyBoundsPool.returnObj(bounds)
        end
    end
    return false
end

---@public 加载舰队
function IDWorldMap.loadFleets()
    local fleets = IDDBWorldMap.fleets
    ---@param v NetProtoIsland.ST_fleetinfor
    for k, v in pairs(fleets) do
        IDWorldMap.refreshFleet(v, false)
    end
end

---@param fleet NetProtoIsland.ST_fleetinfor
function IDWorldMap.refreshFleet(fleet, isRemove)
    if fleet == nil then
        return
    end
    if IDWorldMap.mode ~= GameModeSub.map and IDWorldMap.mode ~= GameModeSub.fleet then
        return
    end
    if isRemove then
        IDWorldMap.releaseFleet(bio2number(fleet.idx))
        return
    end
    local task = bio2number(fleet.task)
    if task == IDConst.FleetTask.idel then
        IDWorldMap.releaseFleet(bio2number(fleet.idx))
        return
    end
    if IDWorldMap.isPassThe9Screens(fleet) then
        ---@type IDWorldFleet
        local fleetObj = fleets[bio2number(fleet.idx)]
        if fleetObj then
            fleetObj.refreshData(fleet)
        else
            CLThingsPool.borrowObjAsyn("worldmap.fleet", IDWorldMap.onLoadFleet, fleet)
        end
    else
        IDWorldMap.releaseFleet(bio2number(fleet.idx))
    end
end

---@param go UnityEngine.GameObject
function IDWorldMap.onLoadFleet(name, go, orgs)
    ---@type NetProtoIsland.ST_fleetinfor
    local fleet = orgs
    local fidx = bio2number(fleet.idx)
    if (IDWorldMap.mode ~= GameModeSub.map and IDWorldMap.mode ~= GameModeSub.fleet) or fleets[fidx] then
        CLThingsPool.returnObj(go)
        SetActive(go, false)
        return
    end
    go.transform.parent = transform
    go.transform.localScale = Vector3.one
    ---@type Coolape.CLCellLua
    local fleetObj = go:GetComponent("CLCellLua")
    SetActive(go, true)
    fleetObj:init(fleet, nil)
    fleets[fidx] = fleetObj.luaTable
    if bio2number(fleet.cidx) == bio2number(IDDBCity.curCity.idx) then
        IDWorldMap.recheckCellsVisible()
    end
end

---@public 释放舰队
function IDWorldMap.releaseFleet(fidx)
    ---@type IDWorldFleet
    local fleetObj = fleets[fidx]
    if fleetObj then
        if fleetObj == IDWorldMap.selectedFleet then
            IDWorldMap.unselectFleet(fidx)
        end
        fleetObj.clean()
        CLThingsPool.returnObj(fleetObj.gameObject)
        SetActive(fleetObj.gameObject, false)
        fleets[fidx] = nil
    end
end

---@param fleetObj IDWorldFleet
function IDWorldMap.onSomeFleetArrived(fleetObj)
end

---@public 选中舰队
---@param fidx number 舰队的idx
function IDWorldMap.selectFleet(fidx)
    ---@type IDWorldFleet
    local fleetObj = fleets[fidx]
    if fleetObj then
        hideHotWheel()
        IDWorldMap.selectFleetMode(fidx)
    else
        local fleet = IDDBCity.curCity:getFleet(fidx)
        if fleet then
            CLThingsPool.borrowObjAsyn(
                "worldmap.fleet",
                function(name, go, orgs)
                    hideHotWheel()
                    IDWorldMap.onLoadFleet(name, go, orgs)
                    IDWorldMap.selectFleetMode(fidx)
                end,
                fleet
            )
        end
    end
end

function IDWorldMap.selectFleetMode(fidx)
    local fleetObj = fleets[fidx]
    if fleetObj == nil then
        return
    end
    ---@type IDWorldFleet
    IDWorldMap.selectedFleet = fleetObj
    IDWorldMap.mode = GameModeSub.fleet
    dragSetting.scaleMini = 7
    dragSetting.scaleMax = 20
    dragSetting.scaleHeightMini = 51
    dragSetting.scaleHeightMax = 100
    IDPMain.onChgMode()
    drag4World.canMove = false
    csSelf:invoke4Lua(
        function()
            lookAtTargetFollow.target = IDWorldMap.selectedFleet.transform
            lookAtTargetFollow.enabled = true
        end,
        0.1
    )
    --//TODO:给舰队加一个选中的效果
end

function IDWorldMap.unselectFleet()
    if IDWorldMap.mode ~= GameModeSub.fleet then
        return
    end
    IDWorldMap.selectedFleet = nil
    IDWorldMap.mode = GameModeSub.map
    dragSetting.scaleMini = 7
    dragSetting.scaleMax = 20
    dragSetting.scaleHeightMini = 10
    dragSetting.scaleHeightMax = 100
    IDPMain.onChgMode()
    lookAtTargetFollow.enabled = false
    lookAtTargetFollow.target = nil
    drag4World.canMove = true
    --//TODO:去掉舰队选中的效果
end

---@public 跳转到舰队所在位置
function IDWorldMap.gotoFleet(fidx)
    showHotWheel()
    local fleetData = IDDBWorldMap.getFleet(fidx)
    if fleetData == nil then
        fleetData = IDDBCity.curCity:getFleet(fidx)
        if fleetData == nil then
            CLAlert.add(LGet("MsgFleetIsNil"), Color.red, 1)
            return
        end
    end
    local curindex, curPos = IDDBWorldMap.getFleetRealCurPos(fleetData)

    IDUtl.hidePopupMenus()
    IDWorldMap.moveToView(
        curindex,
        GameModeSub.map,
        function()
            IDWorldMap.selectFleet(fidx)
        end
    )
end

---@public 移动到指定位置
---@param newCenter number 新的中心点坐标
---@param mode GameModeSub
function IDWorldMap.moveToView(newCenter, mode, callback)
    local lastHit =
        Utl.getRaycastHitInfor(
        MyCfg.self.mainCamera,
        Vector3(Screen.width / 2, Screen.height / 2, 0),
        Utl.getLayer("Water")
    )
    local curCenterPos = Vector3.zero
    if lastHit then
        curCenterPos = lastHit.point
    end

    local doTargetTween = function()
        local newCenterPos = grid:GetCellCenter(newCenter)

        local outofView = false
        local toPos
        if Vector3.Distance(newCenterPos, curCenterPos) > ConstCreenSize * cellSize then
            ---@type UnityEngine.Vector3
            local dir = newCenterPos - curCenterPos
            toPos = curCenterPos + dir.normalized * ConstCreenSize * cellSize
            outofView = true
        else
            toPos = newCenterPos
        end
        lookAtTargetTween:flyout(
            toPos,
            3,
            0,
            0,
            IDWorldMap.onDragMove,
            function()
                if outofView and (GameModeSub.map == mode or GameModeSub.fleet == mode) then
                    -- 重新加载地图数据
                    lookAtTarget.position = newCenterPos
                    csSelf:invoke4Lua(IDWorldMap.onDragMove, 0.2)
                end
                if callback then
                    callback()
                end
            end,
            nil,
            true
        )
    end

    if IDWorldMap.mode ~= mode then
        local toView = nil
        local finishCallback = nil
        if mode == GameModeSub.city then
            finishCallback = IDWorldMap.finisEnterCity
            toView = Vector2(10, 15)
        elseif mode == GameModeSub.map or mode == GameModeSub.fleet then
            toView = Vector2(20, 80)
        elseif mode == GameModeSub.mapBtwncity then
            toView = Vector2(9, 50)
        end
        smoothFollow:tween(
            Vector2(smoothFollow.distance, smoothFollow.height),
            toView,
            5,
            function()
                Utl.doCallback(finishCallback)
                doTargetTween()
            end,
            IDWorldMap.scaleGround
        )
    else
        doTargetTween()
    end
end
--------------------------------------------
return IDWorldMap
