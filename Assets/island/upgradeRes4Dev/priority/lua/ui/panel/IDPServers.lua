-- xx界面
do
    local IDPServers = {}

    local csSelf = nil;
    local transform = nil;
    local uiobjs = {}
    local finishCallback
    local selectedServer

    -- 初始化，只会调用一次
    function IDPServers.init(csObj)
        csSelf = csObj;
        transform = csObj.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.grid = getCC(transform, "content/Grid", "CLUILoopGrid")
    end

    -- 设置数据
    function IDPServers.setData(paras)
        finishCallback = paras[1]
        selectedServer = paras[2]
    end

    function IDPServers.getServers()
        showHotWheel()
        CLLNet.httpPostUsermgr(NetProtoUsermgr.send.getServers(MyCfg.self.appUniqueID, getChlCode()))
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPServers.show()
        IDPServers.setList(IDPServers.getServers())
    end

    function IDPServers.setList(list)
        uiobjs.grid:setList(list or {}, IDPServers.initCell)
    end

    function IDPServers.initCell(cell, data)
        cell:init(data, IDPServers.onClickCell)
    end

    function IDPServers.onClickCell(cell)
        local data = cell.luaTable.getData()
        selectedServer = data
        if finishCallback then
            finishCallback(data)
        end
        hideTopPanel(csSelf)
    end

    -- 当加载好通用框的回调
    function IDPServers.onShowFrame(cs)
        csSelf.frameObj:init({ title = LGet("SelectServer"), panel = csSelf, hideClose = false, hideTitle = false })
    end
    -- 刷新
    function IDPServers.refresh()
    end

    -- 关闭页面
    function IDPServers.hide()
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPServers.procNetwork (cmd, succ, msg, paras)
        if (succ == NetSuccess) then
            if (cmd == NetProtoUsermgr.cmds.getServers) then
                IDPServers.setList(paras.servers)
                hideHotWheel()
            end
        end
    end

    -- 处理ui上的事件，例如点击等
    function IDPServers.uiEventDelegate( go )
        local goName = go.name;
        --[[
        if(goName == "xxx") then
        end
        --]]
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPServers.hideSelfOnKeyBack( )
        return true;
    end

    --------------------------------------------
    return IDPServers;
end
