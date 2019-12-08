-- 建筑属性通用处理
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local mData = nil
    local uiobjs = {}
    local attr
    ---@type NetProtoIsland.ST_building
    local serverData

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.table = csSelf:GetComponent("UITable")
        uiobjs.cellPrefab = getChild(transform, "00000").gameObject
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data
        attr = mData.attr
        serverData = mData.serverData

        local list
        if mData.type == IDConst.AttrType.buildingNextOpen then
            list = _cell.wrapData4BuildingNextLev()
        elseif mData.type == IDConst.AttrType.building then
            list = _cell.wrapData4Building()
        elseif mData.type == IDConst.AttrType.ship4Build then
            list = _cell.wrapData4BuildShip()
        end
        if mData.maxRow and mData.maxRow > 0 and #list > mData.maxRow then
            uiobjs.table.columns = 2
        else
            uiobjs.table.columns = 1
        end
        CLUIUtl.resetList4Lua(uiobjs.table, uiobjs.cellPrefab, list, _cell.initCell)
    end

    function _cell.wrapData4Building()
        local list = {}
        local d = {}
        -- 能否放在地面
        d.name = LGet("CanPlaceOnGround")
        d.icon = "attrIcon_com"
        d.value = attr.PlaceGround and LGet("Yes") or LGet("No")
        table.insert(list, d)
        -- 能否放在海面
        d = {}
        d.name = LGet("CanPlaceOnOcean")
        d.icon = "attrIcon_com"
        d.value = attr.PlaceSea and LGet("Yes") or LGet("No")
        table.insert(list, d)
        -- 生命值
        d = {}
        d.name = LGet("HP")
        d.icon = "attrIcon_hp"
        d.value = DBCfg.getGrowingVal(bio2number(attr.HPMin), bio2number(attr.HPMax), bio2number(attr.HPCurve), bio2number(serverData.lev) / bio2number(attr.MaxLev))
        table.insert(list, d)

        local attrid = bio2number(attr.ID)
        local gid = bio2number(attr.GID)
        if attrid == IDConst.BuildingID.headquartersBuildingID then
            -- 主基地
            local headOpenAtrr = DBCfg.getHeadquartersLevsDataByLev(bio2number(serverData.lev))
            -- 地块数量
            d = {}
            d.name = LGet("Tile")
            d.icon = "attrIcon_com"
            d.value = bio2number(headOpenAtrr.Tiles)
            table.insert(list, d)
            -- 工人数量
            d = {}
            d.name = LGet("Worker")
            d.icon = "attrIcon_build"
            d.value = bio2number(headOpenAtrr.Workers)
            table.insert(list, d)
            -- 出征队列
        elseif attrid == 6 or attrid == 8 or attrid == 10 then
            -- 资源建筑
            --产量
            local resType = IDUtl.getResTypeByBuildingID(attrid)
            d = {}
            d.name = IDUtl.getResNameByType(resType)
            d.icon = IDUtl.getResIcon(resType)
            d.value = DBCfg.getGrowingVal(bio2number(attr.ComVal1Min), bio2number(attr.ComVal1Max), bio2number(attr.ComVal1Curve), bio2number(serverData.lev) / bio2number(attr.MaxLev))
            d.value = joinStr(d.value, "/", LGet("UIMinute"))
            table.insert(list, d)
        elseif attrid == 7 or attrid == 9 or attrid == 11 then
            -- 仓库
            local resType = IDUtl.getResTypeByBuildingID(attrid)
            d = {}
            d.name = IDUtl.getResNameByType(resType)
            d.icon = IDUtl.getResIcon(resType)
            d.value = bio2number(serverData.val)
            table.insert(list, d)
        elseif gid == IDConst.BuildingGID.defense then
            -- 防御建筑
            -- 攻击半径
            d = {}
            d.name = LGet("AttackDistance")
            d.icon = "attrIcon_gongjili"
            d.value = DBCfg.getGrowingVal(bio2number(attr.AttackRangeMin), bio2number(attr.AttackRangeMax), bio2number(attr.AttackRangeCurve), bio2number(serverData.lev) / bio2number(attr.MaxLev))
            d.value = d.value / 100
            table.insert(list, d)

            -- 攻击半径
            d = {}
            d.name = LGet("AttackSpeed")
            d.icon = "attrIcon_gongjisudu"
            d.value = 1 / (bio2number(attr.AttackSpeedMS) / 1000)
            table.insert(list, d)
            -- 伤害
            d = {}
            d.name = LGet("AttackVal")
            d.icon = "attrIcon_gongjili"
            d.value = DBCfg.getGrowingVal(bio2number(attr.DamageMin), bio2number(attr.DamageMax), bio2number(attr.DamageCurve), bio2number(serverData.lev) / bio2number(attr.MaxLev))
            table.insert(list, d)
            --是否群伤
            if bio2number(attr.DamageRadius) > 0 then
                d = {}
                d.name = LGet("MulAttack")
                d.icon = "attrIcon_gongjili"
                d.value = LGet("Yes")
                table.insert(list, d)
            end
        end

        return list
    end

    -- 下级属性变化
    function _cell.wrapData4BuildingNextLev()
        local isMaxLev = false
        if bio2number(serverData.lev) >= bio2number(attr.MaxLev) then
            isMaxLev = true
        end
        local list = {}
        local d
        -- 生命值
        d = {}
        d.name = LGet("HP")
        d.icon = "attrIcon_fangyuli"
        d.value = DBCfg.getGrowingVal(bio2number(attr.HPMin), bio2number(attr.HPMax), bio2number(attr.HPCurve), bio2number(serverData.lev) / bio2number(attr.MaxLev))
        if not isMaxLev then
            local nextVal = DBCfg.getGrowingVal(bio2number(attr.HPMin), bio2number(attr.HPMax), bio2number(attr.HPCurve), (bio2number(serverData.lev) + 1) / bio2number(attr.MaxLev))
            d.addValue = nextVal - d.value
        else
            d.addValue = 0
        end
        table.insert(list, d)

        local attrid = bio2number(attr.ID)
        local gid = bio2number(attr.GID)
        if bio2number(attr.ID) == IDConst.BuildingID.headquartersBuildingID then
            -- 主基地
            -- 地块数量
            local headOpenAtrr = DBCfg.getHeadquartersLevsDataByLev(bio2number(serverData.lev))
            local nextheadOpenAtrr = DBCfg.getHeadquartersLevsDataByLev(bio2number(serverData.lev) + 1)
            -- 地块数量
            d = {}
            d.name = LGet("Tile")
            d.icon = "attrIcon_com"
            d.value = bio2number(headOpenAtrr.Tiles)
            if not isMaxLev then
                d.addValue = bio2number(nextheadOpenAtrr.Tiles) - bio2number(headOpenAtrr.Tiles)
            else
                d.addValue = 0
            end
            table.insert(list, d)
            -- 工人数量
            d = {}
            d.name = LGet("Worker")
            d.icon = "attrIcon_build"
            d.value = bio2number(headOpenAtrr.Workers)
            if isMaxLev then
                d.addValue = 0
            else
                d.addValue = bio2number(nextheadOpenAtrr.Workers) - bio2number(headOpenAtrr.Workers)
            end
            table.insert(list, d)
        elseif attrid == 6 or attrid == 8 or attrid == 10 then
            -- 资源建筑
            --产量
            local resType = IDUtl.getResTypeByBuildingID(attrid)
            d = {}
            d.name = IDUtl.getResNameByType(resType)
            d.icon = IDUtl.getResIcon(resType)
            d.value = DBCfg.getGrowingVal(bio2number(attr.ComVal1Min), bio2number(attr.ComVal1Max), bio2number(attr.ComVal1Curve), bio2number(serverData.lev) / bio2number(attr.MaxLev))

            if isMaxLev then
                d.addValue = 0
            else
                local nextVal = DBCfg.getGrowingVal(bio2number(attr.ComVal1Min), bio2number(attr.ComVal1Max), bio2number(attr.ComVal1Curve), (bio2number(serverData.lev) + 1) / bio2number(attr.MaxLev))
                d.addValue = nextVal - d.value
            end
            d.value = joinStr(d.value, "/", LGet("UIMinute"))
            table.insert(list, d)
        elseif attrid == 7 or attrid == 9 or attrid == 11 then
            -- 仓库
            local resType = IDUtl.getResTypeByBuildingID(attrid)
            d = {}
            d.name = IDUtl.getResNameByType(resType)
            d.icon = IDUtl.getResIcon(resType)
            d.value = DBCfg.getGrowingVal(bio2number(attr.ComVal1Min), bio2number(attr.ComVal1Max), bio2number(attr.ComVal1Curve), bio2number(serverData.lev) / bio2number(attr.MaxLev))
            if isMaxLev then
                d.addValue = 0
            else
                local nextVal = DBCfg.getGrowingVal(bio2number(attr.ComVal1Min), bio2number(attr.ComVal1Max), bio2number(attr.ComVal1Curve), (bio2number(serverData.lev) + 1) / bio2number(attr.MaxLev))
                d.addValue = nextVal - d.value
            end
            table.insert(list, d)
        end

        return list
    end

    function _cell.wrapData4BuildShip()
        local list = {}
        local d = {}
        -- 空间
        d.name = LGet("ShipSpace")
        d.icon = "icon_store"
        d.value = bio2number(attr.SpaceSize)
        table.insert(list, d)
        -- 建造时间
        d = {}
        d.name = LGet("BuildTime")
        d.icon = "attrIcon_time"
        d.value = bio2number(attr.BuildTimeS)/10
        table.insert(list, d)
        --花费
        d = {}
        d.name = LGet("Cost")
        d.icon = IDUtl.getResIcon(bio2number(attr.BuildRscType))
        d.value = bio2number(attr.BuildCost)
        table.insert(list, d)

        return list
    end

    function _cell.initCell(cell, data)
        cell:init(data, nil)
    end

    -- 取得数据
    function _cell.getData ()
        return mData;
    end

    --------------------------------------------
    return _cell;
end
