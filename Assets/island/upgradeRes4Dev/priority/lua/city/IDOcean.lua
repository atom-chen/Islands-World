-- 海水
do
    local IDOcean = {}

    local csSelf = nil
    local transform = nil
    local audioSource

    -- 初始化，只会调用一次
    function IDOcean.init(csObj)
        csSelf = csObj
        transform = csObj.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        audioSource = csSelf:GetComponent("AudioSource")
        SoundEx.addCallbackOnMusicBgSwitch(IDOcean.onBgmSwitchChg)

        if SoundEx.musicBgSwitch then
            IDOcean.playBGM()
        end
    end

    -- 当背景开着变化时
    function IDOcean.onBgmSwitchChg(val)
        if val then
            IDOcean.playBGM()
        else
            audioSource:Pause()
        end
    end

    function IDOcean.playBGM()
        if audioSource.clip == nil then
            CLSoundPool.borrowObjAsyn("Sea",
                    function(name, sound, orgs)
                        if audioSource.clip ~= nil then
                            CLSoundPool.returnObj(name)
                            return
                        end
                        audioSource.clip = sound
                        if SoundEx.musicBgSwitch then
                            audioSource:Play()
                        end
                    end)
        else
            audioSource:Play()
        end
    end

    -- 处理ui上的事件，例如点击等
    function IDOcean.onNotifyLua( go )
        local goName = go.name
    end

    function IDOcean.onPress()
        if MyCfg.mode == GameMode.city then
            IDMainCity.onPress(true)
        elseif MyCfg.mode == GameMode.map or MyCfg.mode == GameMode.mapBtwncity then
            IDWorldMap.onPress(true)
        end
    end
    function IDOcean.onRelease()
        if MyCfg.mode == GameMode.city then
            IDMainCity.onPress(false)
        elseif MyCfg.mode == GameMode.map or MyCfg.mode == GameMode.mapBtwncity then
            IDWorldMap.onPress(false)
        end
    end

    function IDOcean.onClick()
        -- 点击了海面
        if MyCfg.mode == GameMode.city then
            IDMainCity.onClickOcean()
        elseif MyCfg.mode == GameMode.map or MyCfg.mode == GameMode.mapBtwncity then
            IDWorldMap.onClickOcean()
        end
    end

    function IDOcean.onDrag()
        if MyCfg.mode == GameMode.city then
            IDMainCity.onDragOcean()
        elseif MyCfg.mode == GameMode.map or MyCfg.mode == GameMode.mapBtwncity then
            IDWorldMap.onDragOcean()
        end
    end

    function IDOcean.clean()
        if audioSource.audioClip then
            audioSource:Pause()
            CLSoundPool.returnObj("Sea")
            audioSource.audioClip = nil
        end
    end

    --------------------------------------------
    return IDOcean
end
