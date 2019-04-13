-- 扩张地块
do
    require("public.CLLQueue")
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local tile = nil;
    local aroundTiles = {}
    local uiobjs = {}
    ---@type CLLQueue
    local addTiles = CLLQueue.new(8)

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.addTile = getCC(transform, "addTile", "CLCellLua")
        _cell.releaseAddtile(uiobjs.addTile)
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        tile = data
        local unit = tile.csSelf
        _cell.clean()
        transform.position = unit.transform.position
        csSelf.gameObject:SetActive(true)
        local index = tile.gridIndex
        local grid = IDMainCity.grid.grid
        local x = grid:GetColumn(index)
        local y = grid:GetRow(index)
        -- 左
        if IDMainCity.canPlaceTile(x - 2, y) then
            _cell.onAddAround(x - 2, y)
        end
        -- 左上
        if IDMainCity.canPlaceTile(x - 2, y + 2) then
            _cell.onAddAround(x - 2, y + 2)
        end
        -- 上
        if IDMainCity.canPlaceTile(x, y + 2) then
            _cell.onAddAround(x, y + 2)
        end
        -- 右上
        if IDMainCity.canPlaceTile(x + 2, y + 2) then
            _cell.onAddAround(x + 2, y + 2)
        end
        -- 右
        if IDMainCity.canPlaceTile(x + 2, y) then
            _cell.onAddAround(x + 2, y)
        end
        -- 右下
        if IDMainCity.canPlaceTile(x + 2, y - 2) then
            _cell.onAddAround(x + 2, y - 2)
        end
        -- 下
        if IDMainCity.canPlaceTile(x, y - 2) then
            _cell.onAddAround(x, y - 2)
        end
        -- 左下
        if IDMainCity.canPlaceTile(x - 2, y - 2) then
            _cell.onAddAround(x - 2, y - 2)
        end
    end

    function _cell.onAddAround(x, y)
        local addtile = _cell.getAddtile()
        SetActive(addtile.gameObject, true)
        addtile:init({ x = x, y = y }, _cell.onClickAddTile)
        table.insert(aroundTiles, addtile)
    end

    function _cell.onClickAddTile(cell)
        local d = cell.luaTable.getData()
        if not IDMainCity.canPlaceTile(d.x, d.y) then
            CLAlert.add(LGet("CannotPlaceTile"), Color.yellow, 1)
            return
        end
        showHotWheel()
        local grid = IDMainCity.grid.grid
        local index = grid:GetCellIndex(d.x, d.y)
        net:send(NetProtoIsland.send.newTile(index))
    end

    function _cell.getAddtile()
        if addTiles:size() > 0 then
            return addTiles:deQueue()
        else
            local obj = GameObject.Instantiate(uiobjs.addTile):GetComponent("CLCellLua")
            obj.transform.parent = transform
            SetActive(obj.gameObject, false)
            return obj
        end
    end

    function _cell.releaseAddtile(cell)
        addTiles:enQueue(cell)
        SetActive(cell.gameObject, false)
    end

    function _cell.OnDisable()
        _cell.clean()
    end

    -- 取得数据
    function _cell.getData ()
        return tile;
    end

    function _cell.clean()
        for k, v in pairs(aroundTiles) do
            _cell.releaseAddtile(v)
        end
        aroundTiles = {}
    end

    --------------------------------------------
    return _cell;
end
