-- xx界面
do
    local IDPBuildings = {}

    local csSelf = nil;
    local transform = nil;
    local uiobjs = {}
    local defaultGid = 1
    local tabsMap = {}

    -- 初始化，只会调用一次
    function IDPBuildings.init(csObj)
        csSelf = csObj;
        transform = csObj.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        local AnchorBottom = getChild(transform, "AnchorBottom/offset")
        uiobjs.tabs = getCC(AnchorBottom, "tabs", "UIGrid")
        uiobjs.tabprefab = getChild(uiobjs.tabs.transform, "00000").gameObject
        uiobjs.scrollView = getCC(AnchorBottom, "scrollView", "UIScrollView")
        uiobjs.Grid = getCC(uiobjs.scrollView.transform, "Grid", "CLUILoopGrid")
    end

    -- 设置数据
    function IDPBuildings.setData(paras)
        defaultGid = paras or 1
        tabsMap = {}
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPBuildings.show()
        local titles = {
            { gid = 1, name = LGet("Basic") },
            { gid = 2, name = LGet("Resource") },
            { gid = 3, name = LGet("Defense") },
            { gid = 4, name = LGet("Trap") },
            { gid = 5, name = LGet("Decoration") },
        }
        CLUIUtl.resetList4Lua(uiobjs.tabs, uiobjs.tabprefab, titles, IDPBuildings.initTabCell)
    end

    function IDPBuildings.initTabCell(cell, data)
        cell:init(data, IDPBuildings.onClickTab)
        tabsMap[data.gid] = cell
        if data.gid == defaultGid then
            cell:GetComponent("UIToggle").value = true
            IDPBuildings.onClickTab(cell)
        end
    end

    function IDPBuildings.onClickTab(cell)
        local d = cell.luaTable.getData()
        local list = DBCfg.getBuildingsByGID(d.gid)
        uiobjs.Grid:setList(list, IDPBuildings.initCellBuilding)
    end

    function IDPBuildings.initCellBuilding(cell, data)
        cell:init(data, IDPBuildings.onClickBuilding)
    end

    function IDPBuildings.onClickBuilding(cell)
        local d, canClick = cell.luaTable.getData()
        if not canClick then
            return
        end
        hideTopPanel(csSelf)
        IDMainCity.createBuilding(d)
    end

    -- 刷新
    function IDPBuildings.refresh()
    end

    -- 关闭页面
    function IDPBuildings.hide()
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPBuildings.procNetwork (cmd, succ, msg, paras)
        --[[
        if(succ == NetSuccess) then
          if(cmd == "xxx") then
          end
        end
        --]]
    end

    -- 处理ui上的事件，例如点击等
    function IDPBuildings.uiEventDelegate( go )
        local goName = go.name;
        if (goName == "SpriteBg") then
            hideTopPanel(csSelf)
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPBuildings.hideSelfOnKeyBack( )
        return true;
    end

    --------------------------------------------
    return IDPBuildings;
end
