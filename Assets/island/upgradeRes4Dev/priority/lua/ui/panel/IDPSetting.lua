-- xx界面
do
    local IDPSetting = {}

    local csSelf = nil
    local transform = nil
    local uiobjs = {}

    -- 初始化，只会调用一次
    function IDPSetting.init(csObj)
        csSelf = csObj
        transform = csObj.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.ToggleBGM = getCC(transform, "content/ToggleBGM", "UIToggle")
        uiobjs.ToggleEffect = getCC(transform, "content/ToggleEffect", "UIToggle")
    end

    -- 设置数据
    function IDPSetting.setData(paras)
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPSetting.show()
        uiobjs.ToggleBGM.value = SoundEx.musicBgSwitch
        uiobjs.ToggleEffect.value = SoundEx.soundEffectSwitch
    end

    -- 当加载好通用框的回调
    function IDPSetting.onShowFrame(cs)
        csSelf.frameObj:init({title = LGet("Setting"), panel = csSelf, hideClose = false, hideTitle = false})
    end

    -- 刷新
    function IDPSetting.refresh()
    end

    -- 关闭页面
    function IDPSetting.hide()
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPSetting.procNetwork(cmd, succ, msg, paras)
        --[[
        if(succ == NetSuccess) then
          if(cmd == "xxx") then
          end
        end
        --]]
    end

    -- 处理ui上的事件，例如点击等
    function IDPSetting.uiEventDelegate(go)
        local goName = go.name
        if goName ~= "hidden" then
            uiobjs.clickHiddenTimes = 0
        end
        if (goName == "ToggleBGM") then
            SoundEx.musicBgSwitch = uiobjs.ToggleBGM.value
            if not SoundEx.musicBgSwitch then
                SoundEx.stopMainMusic()
            else
                if MyCfg.mode == GameMode.city then
                    SoundEx.playMainMusic("MainScene_1")
                elseif MyCfg.mode == GameMode.battle then
                    SoundEx.playMainMusic("BattleSound1")
                elseif MyCfg.mode == GameMode.none then
                    SoundEx.playMainMusic("login")
                end
            end
        elseif goName == "ToggleEffect" then
            SoundEx.soundEffectSwitch = uiobjs.ToggleEffect.value
        elseif goName == "hidden" then
            uiobjs.clickHiddenTimes = uiobjs.clickHiddenTimes + 1
            if uiobjs.clickHiddenTimes == 5 then
                uiobjs.clickHiddenTimes = 0
                getPanelAsy("PanelDebugMgr", onLoadedPanelTT)
            end
        elseif goName == "ButtonQuit" then
            if CLLNet then
                CLLNet.stop()
            end
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPSetting.hideSelfOnKeyBack()
        return true
    end

    --------------------------------------------
    return IDPSetting
end
