-- xx界面
do
    local IDPDebugMgr = {}

    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local fps = CLMainBase.self:GetComponent("CLFPS")

    -- 初始化，只会调用一次
    function IDPDebugMgr.init(csObj)
        csSelf = csObj
        transform = csObj.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        local grid = getChild(transform, "Panelinfor/Grid")
        uiobjs.ToggleEditorMode = getCC(grid, "ToggleEditorMode", "UIToggle")
        uiobjs.ToggleFps = getCC(grid, "ToggleFps", "UIToggle")
        uiobjs.ToggleLog = getCC(grid, "ToggleLog", "UIToggle")
        uiobjs.InputInfor = getCC(grid, "InputInfor", "UIInput")
        uiobjs.InputIP = getCC(grid, "InputIP", "UIInput")
    end

    -- 设置数据
    function IDPDebugMgr.setData(paras)
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPDebugMgr.show()
        uiobjs.ToggleFps.value = fps.enabled
        uiobjs.ToggleEditorMode.value = __EditorMode__
        if ReporterMessageReceiver and ReporterMessageReceiver.self then
            uiobjs.ToggleLog.value = ReporterMessageReceiver.self.gameObject.activeInHierarchy
        else
            uiobjs.ToggleLog.value = false
        end
        local deviceInfor = {}
        table.insert(deviceInfor, SystemInfo.deviceName)
        table.insert(deviceInfor, SystemInfo.deviceModel)
        table.insert(deviceInfor, SystemInfo.deviceType:ToString())
        table.insert(deviceInfor, SystemInfo.operatingSystem)
        table.insert(deviceInfor, SystemInfo.maxTextureSize)
        local deiveceInfor = table.concat(deviceInfor, ",")
        local uuid = Utl.uuid
        local idx = IDDBPlayer.myself and bio2number(IDDBPlayer.myself.idx) or 0
        local cityidx = IDDBPlayer.myself and bio2number(IDDBPlayer.myself.cityidx) or 0

        uiobjs.InputInfor.value = joinStr("idx=", idx, ";",
                "cityidx=", cityidx, ";",
                "uuid=", uuid, ";\n",
                "deviceinfor=", deiveceInfor, ";")
    end

    -- 刷新
    function IDPDebugMgr.refresh()
    end

    -- 关闭页面
    function IDPDebugMgr.hide()
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPDebugMgr.procNetwork (cmd, succ, msg, paras)
        --[[
        if(succ == NetSuccess) then
          if(cmd == "xxx") then
            -- TODO:
          end
        end
        --]]
    end

    -- 处理ui上的事件，例如点击等
    function IDPDebugMgr.uiEventDelegate(go)
        local goName = go.name
        if (goName == uiobjs.ToggleFps.name) then
            fps.enabled = uiobjs.ToggleFps.value
        elseif goName == uiobjs.ToggleEditorMode.name then
            __EditorMode__ = uiobjs.ToggleEditorMode.value
        elseif goName == uiobjs.ToggleLog.name then
            if ReporterMessageReceiver and ReporterMessageReceiver.self then
                ReporterMessageReceiver.self.gameObject:SetActive(uiobjs.ToggleLog.value)
            end
        elseif goName == "Spriteclose" then
            hideTopPanel(csSelf)
        elseif goName == "ButtonApplyIP" then
            CLLNet.refreshBaseUrl(uiobjs.InputIP.value)
            CLAlert.add("success")
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPDebugMgr.hideSelfOnKeyBack()
        return true
    end

    --------------------------------------------
    return IDPDebugMgr
end
