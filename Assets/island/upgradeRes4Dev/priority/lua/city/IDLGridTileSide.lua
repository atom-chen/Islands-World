---@class IDLGridTileSide
local IDLGridTileSide = {}
IDLGridTileSide.StateEnum = {
    none = 0,
    showing = 1,
    hidden = 2,
    procing = 3
}
IDLGridTileSide.state = IDLGridTileSide.StateEnum.none
local cache = {}
local tileSides = {}
--local tileWaves = {}
local tileAngleSides = {}
local selectedTileState = {}

local SidesName = {
    TileSideDown1 = "Tiles.TileSideDown1",
    TileSideDown2 = "Tiles.TileSideDown2",
    TileSideDown3 = "Tiles.TileSideDown3",
    TileSideDown4 = "Tiles.TileSideDown4",
    TileSideLeft1 = "Tiles.TileSideLeft1",
    TileSideLeft2 = "Tiles.TileSideLeft2",
    TileSideLeft3 = "Tiles.TileSideLeft3",
    TileSideLeft4 = "Tiles.TileSideLeft4",
    TileSideLeftDown = "Tiles.TileSideLeftDown",
    TileSideLeftDownAngle = "Tiles.TileSideLeftDownAngle",
    TileSideLeftUp = "Tiles.TileSideLeftUp",
    TileSideLeftUpAngle = "Tiles.TileSideLeftUpAngle",
    TileSideRight1 = "Tiles.TileSideRight1",
    TileSideRight2 = "Tiles.TileSideRight2",
    TileSideRight3 = "Tiles.TileSideRight3",
    TileSideRight4 = "Tiles.TileSideRight4",
    TileSideRightDown = "Tiles.TileSideRightDown",
    TileSideRightDownAngle = "Tiles.TileSideRightDownAngle",
    TileSideRightUp = "Tiles.TileSideRightUp",
    TileSideRightUpAngle = "Tiles.TileSideRightUpAngle",
    TileSideUp1 = "Tiles.TileSideUp1",
    TileSideUp2 = "Tiles.TileSideUp2",
    TileSideUp3 = "Tiles.TileSideUp3",
    TileSideUp4 = "Tiles.TileSideUp4",
    TileSideFour = "Tiles.TileSideFour",
    TileSideConnectLeft = "Tiles.TileSideConnectLeft",
    TileSideConnectRight = "Tiles.TileSideConnectRight"
}

local prefabSides = {}

-- 初始化，只会调用一次
function IDLGridTileSide.init(grid, waveUvAn)
    prefabSides = {}
    for k, v in pairs(SidesName) do
        table.insert(prefabSides, v)
    end
    IDLGridTileSide.state = IDLGridTileSide.StateEnum.none
    cache.grid = grid
    cache.waveUvAn = waveUvAn
    cache.waveUvAn.isStop = true
    cache.waveUvAn.speed = -0.5
    cache.waveUvAn.singleX = 1
    cache.waveUvAn.singleY = 1
end

---@public 隐藏边（只隐藏，不释放）
function IDLGridTileSide.hide()
    if IDLGridTileSide.state == IDLGridTileSide.StateEnum.showing then
        for k, v in pairs(tileSides) do
            SetActive(v, false)
        end
        cache.waveUvAn.isStop = true
        IDLGridTileSide.state = IDLGridTileSide.StateEnum.hidden
    end
end

---@public 显示边，如果还没有加载过则调用refreshAndShow方法把边加载了显示出来
function IDLGridTileSide.show()
    if IDLGridTileSide.state == IDLGridTileSide.StateEnum.none then
        IDLGridTileSide.refreshAndShow()
    else
        if IDLGridTileSide.state == IDLGridTileSide.StateEnum.hidden then
            for k, v in pairs(tileSides) do
                SetActive(v, true)
            end
            cache.waveUvAn.isStop = false
        end
        IDLGridTileSide.state = IDLGridTileSide.StateEnum.showing
    end
end

---@public 根据地块加载地块的四周的边缘
function IDLGridTileSide.refreshAndShow(callback, progressCB)
    IDLGridTileSide.clean()
    IDLGridTileSide.state = IDLGridTileSide.StateEnum.procing

    CLMaterialPool.borrowObjAsyn("Tiles.bolang", IDLGridTileSide.onSetMaterial, {callback, progressCB})
end

---@public 加载海浪的material
function IDLGridTileSide.onSetMaterial(name, material, orgs)
    cache.waveUvAn.material = material
    cache.waveUvAn.isStop = false

    selectedTileState = {}
    if IDMainCity.selectedUnit and IDMainCity.selectedUnit.isTile then
        -- 选中状态的地块是没有记录地块状态的，所以临时记录一下
        local tile = IDMainCity.selectedUnit
        local center = tile.getPosIndex()
        if (IDMainCity.isSizeInFreeCell(center, tile.size, false, true)) then
            local list = IDMainCity.grid:getOwnGrids(center, tile.size)
            local count = list.Count
            for i = 0, count - 1 do
                selectedTileState[NumEx.getIntPart(list[i])] = true
            end
        end
    end

    cache.gridState4Tile = IDMainCity.getState4Tile()
    local callback = orgs[1]
    local progressCB = orgs[2]
    cache.onFinishCallback = callback
    cache.onProgressCB = progressCB

    -- 准备数据
    cache.tileList = {}
    local tiles = IDMainCity.getTiles()
    for k, tile in pairs(tiles) do
        table.insert(cache.tileList, tile)
    end
    IDLGridTileSide.set4Sides(0)
end

function IDLGridTileSide.getLeftSide(index)
    --local x = cache.grid:GetColumn(index)
    local y = cache.grid:GetRow(index)
    local offset = NumEx.getIntPart(y % 4) + 1
    return SidesName[joinStr("TileSideLeft", offset)]
end

function IDLGridTileSide.getRightSide(index)
    --local x = cache.grid:GetColumn(index)
    local y = cache.grid:GetRow(index)
    local offset = NumEx.getIntPart(y % 4) + 1
    return SidesName[joinStr("TileSideRight", offset)]
end

function IDLGridTileSide.getUpSide(index)
    local offset = NumEx.getIntPart(index % 4) + 1
    return SidesName[joinStr("TileSideUp", offset)]
end

function IDLGridTileSide.getDownSide(index)
    local offset = NumEx.getIntPart(index % 4) + 1
    return SidesName[joinStr("TileSideDown", offset)]
end

function IDLGridTileSide.set4Sides(i)
    local count = #cache.tileList
    if i >= count then
        -- finish
        IDLGridTileSide.set4SidesAngle(0)
    else
        if cache.onProgressCB then
            cache.onProgressCB(count * 2, i)
        end
        i = i + 1
        local tile = cache.tileList[i]
        IDLGridTileSide.procOneCellSilde(tile, IDLGridTileSide.set4Sides, i)
    end
end

---@public 处理一个地块的四边，left,right,up,down
function IDLGridTileSide.procOneCellSilde(tile, callback, orgs)
    if tile == IDMainCity.selectedUnit then
        if (not IDMainCity.isSizeInFreeCell(tile.getPosIndex(), tile.size, false, true)) then
            if callback then
                callback(orgs)
            end
            return
        end
    end
    local left1, left2, right1, right2, up1, up2, down1, down2, leftUp, leftDown, rightUp, rightDown =
        tile.getSidesIndex()
    IDLGridTileSide.setLeftSilde(
        left1,
        function()
            IDLGridTileSide.setLeftSilde(
                left2,
                function()
                    IDLGridTileSide.setRightSilde(
                        right1,
                        function()
                            IDLGridTileSide.setRightSilde(
                                right2,
                                function()
                                    IDLGridTileSide.setUpSilde(
                                        up1,
                                        function()
                                            IDLGridTileSide.setUpSilde(
                                                up2,
                                                function()
                                                    IDLGridTileSide.setDownSilde(
                                                        down1,
                                                        function()
                                                            IDLGridTileSide.setDownSilde(down2, callback, orgs)
                                                        end
                                                    )
                                                end
                                            )
                                        end
                                    )
                                end
                            )
                        end
                    )
                end
            )
        end
    )
end

function IDLGridTileSide.setLeftSilde(index, callback, orgs)
    -- logic of Left Side
    if IDLGridTileSide.isNeedSet(index) then
        local grid = cache.grid
        local _index = index
        local sideName = ""
        local pos = cache.grid:GetCellCenter(_index)
        local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
        local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
        local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
        if upIsempty and downIsempty and leftIsempty then
            sideName = IDLGridTileSide.getLeftSide(_index)
        elseif (not upIsempty) and downIsempty and leftIsempty then
            sideName = SidesName.TileSideLeftDown
        elseif upIsempty and (not downIsempty) and leftIsempty then
            sideName = SidesName.TileSideLeftUp
        elseif upIsempty and downIsempty and (not leftIsempty) then
            sideName = SidesName.TileSideFour
        elseif (not upIsempty) and (not downIsempty) and leftIsempty then
            sideName = SidesName.TileSideFour
        elseif upIsempty and (not downIsempty) and (not leftIsempty) then
            sideName = SidesName.TileSideFour
        elseif (not upIsempty) and downIsempty and (not leftIsempty) then
            sideName = SidesName.TileSideFour
        else
            -- (not upIsempty) and (not downIsempty) and (not leftIsempty)
            sideName = SidesName.TileSideFour
        end
        if not isNilOrEmpty(sideName) then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

function IDLGridTileSide.setRightSilde(index, callback, orgs)
    -- logic of Right Side
    if IDLGridTileSide.isNeedSet(index) then
        local grid = cache.grid
        local _index = index
        local sideName = ""
        local pos = cache.grid:GetCellCenter(_index)
        local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
        local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
        local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))
        if upIsempty and downIsempty and rightIsempty then
            sideName = IDLGridTileSide.getRightSide(_index)
        elseif (not upIsempty) and downIsempty and rightIsempty then
            sideName = SidesName.TileSideRightDown
        elseif upIsempty and (not downIsempty) and rightIsempty then
            sideName = SidesName.TileSideRightUp
        elseif upIsempty and downIsempty and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        elseif (not upIsempty) and (not downIsempty) and rightIsempty then
            sideName = SidesName.TileSideFour
        elseif upIsempty and (not downIsempty) and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        elseif (not upIsempty) and downIsempty and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        else
            -- (not upIsempty) and (not downIsempty) and (not leftIsempty)
            sideName = SidesName.TileSideFour
        end
        if not isNilOrEmpty(sideName) then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

function IDLGridTileSide.setUpSilde(index, callback, orgs)
    -- logic of Up Side
    if IDLGridTileSide.isNeedSet(index) then
        local grid = cache.grid
        local _index = index
        local sideName = ""
        local pos = cache.grid:GetCellCenter(_index)
        local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
        local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
        local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))
        if upIsempty and leftIsempty and rightIsempty then
            --SidesName.TileSideUp
            sideName = IDLGridTileSide.getUpSide(_index)
        elseif (not upIsempty) and leftIsempty and rightIsempty then
            sideName = SidesName.TileSideFour
        elseif upIsempty and (not leftIsempty) and rightIsempty then
            sideName = SidesName.TileSideRightUp
        elseif upIsempty and leftIsempty and (not rightIsempty) then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsempty) and (not leftIsempty) and rightIsempty then
            sideName = SidesName.TileSideFour
        elseif upIsempty and (not leftIsempty) and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        elseif (not upIsempty) and leftIsempty and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        else
            sideName = SidesName.TileSideFour
        end
        if not isNilOrEmpty(sideName) then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

function IDLGridTileSide.setDownSilde(index, callback, orgs)
    -- logic of down Side
    if IDLGridTileSide.isNeedSet(index) then
        local grid = cache.grid
        local _index = index
        local sideName = ""
        local pos = cache.grid:GetCellCenter(_index)
        local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
        local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
        local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))
        if downIsempty and leftIsempty and rightIsempty then
            --SidesName.TileSideDown
            sideName = IDLGridTileSide.getDownSide(_index)
        elseif (not downIsempty) and leftIsempty and rightIsempty then
            sideName = SidesName.TileSideFour
        elseif downIsempty and (not leftIsempty) and rightIsempty then
            sideName = SidesName.TileSideRightDown
        elseif downIsempty and leftIsempty and (not rightIsempty) then
            sideName = SidesName.TileSideLeftDown
        elseif (not downIsempty) and (not leftIsempty) and rightIsempty then
            sideName = SidesName.TileSideFour
        elseif downIsempty and (not leftIsempty) and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        elseif (not downIsempty) and leftIsempty and (not rightIsempty) then
            sideName = SidesName.TileSideFour
        else
            sideName = SidesName.TileSideFour
        end
        if not isNilOrEmpty(sideName) then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

function IDLGridTileSide.set4SidesAngle(i)
    local count = #cache.tileList
    if i >= count then
        -- finish
        IDLGridTileSide.state = IDLGridTileSide.StateEnum.showing
        if cache.onFinishCallback then
            cache.onFinishCallback()
        end
    else
        if cache.onProgressCB then
            cache.onProgressCB(count * 2, count + i)
        end
        i = i + 1
        local tile = cache.tileList[i]
        IDLGridTileSide.procOneCellSildeAngle(tile, IDLGridTileSide.set4SidesAngle, i)
    end
end

---@public 处理一个地块的四个角，leftUp, leftDown, rightUp, rightDown
function IDLGridTileSide.procOneCellSildeAngle(tile, callback, orgs)
    if tile == IDMainCity.selectedUnit then
        if (not IDMainCity.isSizeInFreeCell(tile.getPosIndex(), tile.size, false, true)) then
            if callback then
                callback(orgs)
            end
            return
        end
    end
    local leftUp, leftDown, rightUp, rightDown = tile.getSidesAngleIndex()
    IDLGridTileSide.setLeftUpAngle(
        leftUp,
        function()
            IDLGridTileSide.setLeftDownAngle(
                leftDown,
                function()
                    IDLGridTileSide.setRightUpAngle(
                        rightUp,
                        function()
                            IDLGridTileSide.setRightDownAngle(rightDown, callback, orgs)
                        end
                    )
                end
            )
        end
    )
end

---@public 处理左上角
function IDLGridTileSide.setLeftUpAngle(index, callback, orgs)
    if not IDLGridTileSide.isNeedSetAngel(index) then
        if callback then
            callback(orgs)
        end
        return
    end
    local grid = cache.grid
    local _index = index
    local sideName = ""
    local pos = cache.grid:GetCellCenter(_index)

    local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
    local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
    local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
    local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))

    local upIsFourSide = IDLGridTileSide.isFourSide(grid:UpIndex(_index))
    local downIsFourSide = IDLGridTileSide.isFourSide(grid:DownIndex(_index))
    local leftIsFourSide = IDLGridTileSide.isFourSide(grid:LeftIndex(_index))
    local rightIsFourSide = IDLGridTileSide.isFourSide(grid:RightIndex(_index))

    if upIsempty and downIsempty and leftIsempty and rightIsempty then
        -- 当四周围都是空的时间，不需要修改处理，所以加上isNeedSet判断
        if upIsFourSide and downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and downIsFourSide and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif upIsFourSide and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getUpSide(_index)
        elseif (not upIsFourSide) and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            --SidesName.TileSideUp
            sideName = IDLGridTileSide.getLeftSide(_index)
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            local _leftUpIsEmpty = IDLGridTileSide.isEmpty(grid:LeftUpIndex(_index))
            if _leftUpIsEmpty then
                sideName = SidesName.TileSideLeftUpAngle
            else
                sideName = SidesName.TileSideConnectLeft
            end
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and rightIsempty then
        if downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            --todo: LeftDown->横边有草，坚边无草。暂时先用空上代替
            sideName = SidesName.TileSideLeftDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and rightIsempty then
        if upIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif (not upIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            -- SidesName.TileSideUp
            sideName = IDLGridTileSide.getUpSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and rightIsempty then
        if upIsFourSide and downIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif (not upIsFourSide) and (not downIsFourSide) and (not rightIsFourSide) then
            --todo:RightUp->横边无草，坚边有草。暂时先用空上代替
            sideName = SidesName.TileSideRightUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and leftIsempty and (not rightIsempty) then
        if upIsFourSide and downIsFourSide and leftIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftUp
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) then
            sideName = IDLGridTileSide.getLeftSide(_index) -- SidesName.TileSideLeft
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and (not downIsempty) and leftIsempty and rightIsempty then
        sideName = SidesName.TileSideFour
    elseif (not upIsempty) and downIsempty and (not leftIsempty) and rightIsempty then
        sideName = SidesName.TileSideFour
    elseif (not upIsempty) and downIsempty and leftIsempty and (not rightIsempty) then
        if (not downIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and (not leftIsempty) and rightIsempty then
        if (not upIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and (not rightIsempty) then
        if (not upIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and (not rightIsempty) then
        sideName = SidesName.TileSideFour
    else
        sideName = SidesName.TileSideFour
    end

    if not isNilOrEmpty(sideName) then
        local oldSide = tileSides[_index]
        if oldSide and oldSide.name ~= sideName then
            CLThingsPool.returnObj(oldSide)
            SetActive(oldSide, false)
            tileSides[_index] = nil
        end

        tileAngleSides[_index] = true
        if tileSides[_index] == nil then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

---@public 处理左下角
function IDLGridTileSide.setLeftDownAngle(index, callback, orgs)
    if not IDLGridTileSide.isNeedSetAngel(index) then
        if callback then
            callback(orgs)
        end
        return
    end
    local grid = cache.grid
    local _index = index
    local sideName = ""
    local pos = cache.grid:GetCellCenter(_index)

    local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
    local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
    local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
    local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))

    local upIsFourSide = IDLGridTileSide.isFourSide(grid:UpIndex(_index))
    local downIsFourSide = IDLGridTileSide.isFourSide(grid:DownIndex(_index))
    local leftIsFourSide = IDLGridTileSide.isFourSide(grid:LeftIndex(_index))
    local rightIsFourSide = IDLGridTileSide.isFourSide(grid:RightIndex(_index))

    if upIsempty and downIsempty and leftIsempty and rightIsempty then
        -- 当四周围都是空的时间，不需要修改处理，所以加上isNeedSet判断
        if upIsFourSide and downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and downIsFourSide and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif upIsFourSide and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getDownSide(_index)
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif (not upIsFourSide) and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = IDLGridTileSide.getLeftSide(_index)
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            local _leftDownIsempty = IDLGridTileSide.isEmpty(grid:LeftDownIndex(_index))
            if _leftDownIsempty then
                sideName = SidesName.TileSideLeftDownAngle
            else
                sideName = SidesName.TileSideConnectRight
            end
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and rightIsempty then
        if downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getDownSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and rightIsempty then
        if upIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            --todo:LeftUp->横边有草，坚边无草。暂时先用空上代替
            sideName = SidesName.TileSideLeftUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and rightIsempty then
        if upIsFourSide and downIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif upIsFourSide and (not downIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not rightIsFourSide) then
            --todo:RightDown->横边无草，坚边有草。暂时先用空上代替
            sideName = SidesName.TileSideRightDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and leftIsempty and (not rightIsempty) then
        if upIsFourSide and downIsFourSide and leftIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftUp
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) then
            sideName = IDLGridTileSide.getLeftSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and (not downIsempty) and leftIsempty and rightIsempty then
        sideName = SidesName.TileSideFour
    elseif (not upIsempty) and downIsempty and (not leftIsempty) and rightIsempty then
        if (not downIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and (not rightIsempty) then
        if (not downIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and (not leftIsempty) and rightIsempty then
        sideName = SidesName.TileSideFour
    elseif upIsempty and (not downIsempty) and leftIsempty and (not rightIsempty) then
        if (not upIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and (not rightIsempty) then
        sideName = SidesName.TileSideFour
    else
        sideName = SidesName.TileSideFour
    end

    if not isNilOrEmpty(sideName) then
        local oldSide = tileSides[_index]
        if oldSide and oldSide.name ~= sideName then
            CLThingsPool.returnObj(oldSide)
            SetActive(oldSide, false)
            tileSides[_index] = nil
        end

        tileAngleSides[_index] = true
        if tileSides[_index] == nil then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

---@public 处理右上角
function IDLGridTileSide.setRightUpAngle(index, callback, orgs)
    if not IDLGridTileSide.isNeedSetAngel(index) then
        if callback then
            callback(orgs)
        end
        return
    end
    local grid = cache.grid
    local _index = index
    local sideName = ""
    local pos = cache.grid:GetCellCenter(_index)

    local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
    local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
    local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
    local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))

    local upIsFourSide = IDLGridTileSide.isFourSide(grid:UpIndex(_index))
    local downIsFourSide = IDLGridTileSide.isFourSide(grid:DownIndex(_index))
    local leftIsFourSide = IDLGridTileSide.isFourSide(grid:LeftIndex(_index))
    local rightIsFourSide = IDLGridTileSide.isFourSide(grid:RightIndex(_index))

    if upIsempty and downIsempty and leftIsempty and rightIsempty then
        -- 当四周围都是空的时间，不需要修改处理，所以加上isNeedSet判断
        if upIsFourSide and downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and downIsFourSide and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif upIsFourSide and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and (not rightIsFourSide) then
            --SidesName.TileSideUp
            sideName = IDLGridTileSide.getUpSide(_index)
        elseif (not upIsFourSide) and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getRightSide(_index)
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            local _rightUpIsempty = IDLGridTileSide.isEmpty(grid:RightUpIndex(_index))
            if _rightUpIsempty then
                sideName = SidesName.TileSideRightUpAngle
            else
                sideName = SidesName.TileSideConnectRight
            end
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and rightIsempty then
        if downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            --todo:RightDown->横边有草，坚边无草。暂时先用空上代替
            sideName = SidesName.TileSideRightDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and rightIsempty then
        if upIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif (not upIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            -- SidesName.TileSideUp
            sideName = IDLGridTileSide.getUpSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and rightIsempty then
        if upIsFourSide and downIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getRightSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and leftIsempty and (not rightIsempty) then
        if upIsFourSide and downIsFourSide and leftIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftUp
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) then
            --todo:LeftUp->横边无草，坚边有草。暂时先用空上代替
            sideName = SidesName.TileSideLeftUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and (not downIsempty) and leftIsempty and rightIsempty then
        sideName = SidesName.TileSideFour
    elseif (not upIsempty) and downIsempty and (not leftIsempty) and rightIsempty then
        if (not downIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and (not rightIsempty) then
        sideName = SidesName.TileSideFour
    elseif upIsempty and (not downIsempty) and (not leftIsempty) and rightIsempty then
        if (not upIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and (not rightIsempty) then
        if (not upIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and (not rightIsempty) then
        sideName = SidesName.TileSideFour
    else
        sideName = SidesName.TileSideFour
    end

    if not isNilOrEmpty(sideName) then
        local oldSide = tileSides[_index]
        if oldSide and oldSide.name ~= sideName then
            CLThingsPool.returnObj(oldSide)
            SetActive(oldSide, false)
            tileSides[_index] = nil
        end

        tileAngleSides[_index] = true
        if tileSides[_index] == nil then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

---@public 处理右下角
function IDLGridTileSide.setRightDownAngle(index, callback, orgs)
    if not IDLGridTileSide.isNeedSetAngel(index) then
        if callback then
            callback(orgs)
        end
        return
    end
    local grid = cache.grid
    local _index = index
    local sideName = ""
    local pos = cache.grid:GetCellCenter(_index)

    local upIsempty = IDLGridTileSide.isEmpty(grid:UpIndex(_index))
    local downIsempty = IDLGridTileSide.isEmpty(grid:DownIndex(_index))
    local leftIsempty = IDLGridTileSide.isEmpty(grid:LeftIndex(_index))
    local rightIsempty = IDLGridTileSide.isEmpty(grid:RightIndex(_index))

    local upIsFourSide = IDLGridTileSide.isFourSide(grid:UpIndex(_index))
    local downIsFourSide = IDLGridTileSide.isFourSide(grid:DownIndex(_index))
    local leftIsFourSide = IDLGridTileSide.isFourSide(grid:LeftIndex(_index))
    local rightIsFourSide = IDLGridTileSide.isFourSide(grid:RightIndex(_index))

    if upIsempty and downIsempty and leftIsempty and rightIsempty then
        -- 当四周围都是空的时间，不需要修改处理，所以加上isNeedSet判断
        if upIsFourSide and downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getDownSide(_index)
        elseif (not upIsFourSide) and downIsFourSide and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif (not upIsFourSide) and (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getRightSide(_index)
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            local _rightDownIsempty = IDLGridTileSide.isEmpty(grid:RightDownIndex(_index))
            if _rightDownIsempty then
                sideName = SidesName.TileSideRightDownAngle
            else
                sideName = SidesName.TileSideConnectLeft
            end
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and rightIsempty then
        if downIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not downIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not downIsFourSide) and (not leftIsFourSide) and rightIsFourSide then
            sideName = SidesName.TileSideLeftDown
        elseif (not downIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getDownSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and rightIsempty then
        if upIsFourSide and leftIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and leftIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif (not upIsFourSide) and (not leftIsFourSide) and (not rightIsFourSide) then
            --todo:RightUp->横边有草，坚边无草。暂时先用空上代替
            sideName = SidesName.TileSideRightUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and (not leftIsempty) and rightIsempty then
        if upIsFourSide and downIsFourSide and rightIsFourSide then
            sideName = SidesName.TileSideFour
        elseif (not upIsFourSide) and downIsFourSide and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        elseif upIsFourSide and (not downIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not rightIsFourSide) then
            sideName = IDLGridTileSide.getRightSide(_index)
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and downIsempty and leftIsempty and (not rightIsempty) then
        if upIsFourSide and downIsFourSide and leftIsFourSide then
            sideName = SidesName.TileSideFour
        elseif upIsFourSide and (not downIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        elseif (not upIsFourSide) and (not downIsFourSide) and (not leftIsFourSide) then
            --todo:LeftDown->横边无草，坚边有草。暂时先用空上代替
            sideName = SidesName.TileSideLeftDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and (not downIsempty) and leftIsempty and rightIsempty then
        sideName = SidesName.TileSideFour
    elseif (not upIsempty) and downIsempty and (not leftIsempty) and rightIsempty then
        if (not downIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif (not upIsempty) and downIsempty and leftIsempty and (not rightIsempty) then
        if (not downIsFourSide) and (not leftIsFourSide) then
            sideName = SidesName.TileSideLeftDown
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and (not leftIsempty) and rightIsempty then
        if (not upIsFourSide) and (not rightIsFourSide) then
            sideName = SidesName.TileSideRightUp
        else
            sideName = SidesName.TileSideFour
        end
    elseif upIsempty and (not downIsempty) and leftIsempty and (not rightIsempty) then
        sideName = SidesName.TileSideFour
    elseif upIsempty and downIsempty and (not leftIsempty) and (not rightIsempty) then
        sideName = SidesName.TileSideFour
    else
        sideName = SidesName.TileSideFour
    end

    if not isNilOrEmpty(sideName) then
        local oldSide = tileSides[_index]
        if oldSide and oldSide.name ~= sideName then
            CLThingsPool.returnObj(oldSide)
            SetActive(oldSide, false)
            tileSides[_index] = nil
        end

        tileAngleSides[_index] = true
        if tileSides[_index] == nil then
            IDLGridTileSide.loadSide2Show(sideName, _index, pos, callback, orgs)
        else
            if callback then
                callback(orgs)
            end
        end
    else
        if callback then
            callback(orgs)
        end
    end
end

function IDLGridTileSide.onLoadSide(name, obj, orgs)
    local index = orgs[1]
    local pos = orgs[2]
    local callback = orgs[3]
    local param = orgs[4]
    if obj == nil then
        if callback then
            callback(orgs)
        end
        printe("get tile side is nil. ==" .. name)
        return
    end
    obj.transform.parent = IDMainCity.transform
    obj.transform.localScale = Vector3.one
    obj.transform.localEulerAngles = Vector3.zero
    if name == SidesName.TileSideFour then
        pos = pos + IDMainCity.offset4Tile
    end
    obj.transform.position = pos
    SetActive(obj, true)
    tileSides[index] = obj
    if callback then
        callback(param)
    end
end

function IDLGridTileSide.loadSide2Show(name, index, pos, callback, rogs)
    CLThingsPool.borrowObjAsyn(name, IDLGridTileSide.onLoadSide, {index, pos, callback, rogs})
end

function IDLGridTileSide.isFourSide(index)
    local side = tileSides[index]
    if side and side.name == SidesName.TileSideFour then
        return true
    end
    return false
end

function IDLGridTileSide.isNeedSet(index)
    if (not cache.grid:IsInBounds(index)) or cache.gridState4Tile[index] or selectedTileState[index] or tileSides[index] then
        return false
    end
    return true
end

function IDLGridTileSide.isNeedSetAngel(index)
    if
        (not cache.grid:IsInBounds(index)) or cache.gridState4Tile[index] or selectedTileState[index] or
            tileAngleSides[index]
     then
        return false
    end
    return true
end

function IDLGridTileSide.isEmpty(index)
    if cache.grid:IsInBounds(index) and (not cache.gridState4Tile[index]) and (not selectedTileState[index]) then
        return true
    end
    return false
end

---@public index是否在沙滩上
function IDLGridTileSide.isOnTheBeach(index)
    if tileSides[index] then
        return true
    end
    return false
end

function IDLGridTileSide.clean()
    for k, v in pairs(tileSides) do
        CLThingsPool.returnObj(v)
        SetActive(v, false)
    end
    tileSides = {}

    if cache.waveUvAn and cache.waveUvAn.material then
        cache.waveUvAn.isStop = true
        CLMaterialPool.returnObj(cache.waveUvAn.material.name)
        cache.waveUvAn.material = nil
    end

    selectedTileState = {}
    tileAngleSides = {}
    IDLGridTileSide.state = IDLGridTileSide.StateEnum.none
end
--------------------------------------------
return IDLGridTileSide
