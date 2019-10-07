-- --[[
-- //                    ooOoo
-- //                   8888888
-- //                  88" . "88
-- //                  (| -_- |)
-- //                  O\  =  /O
-- //               ____/`---'\____
-- //             .'  \\|     |//  `.
-- //            /  \\|||  :  |||//  \
-- //           /  _||||| -:- |||||-  \
-- //           |   | \\\  -  /// |   |
-- //           | \_|  ''\---/''  |_/ |
-- //            \ .-\__  `-`  ___/-. /
-- //         ___`. .'  /--.--\  `. . ___
-- //      ."" '<  `.___\_<|>_/___.'  >' "".
-- //     | | : ` - \`.` \ _ / `.`/- ` : | |
-- //     \ \ `-.    \_ __\ /__ _/   .-` / /
-- //======`-.____`-.___\_____/___.-`____.-'======
-- //                   `=---='
-- //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
-- //           佛祖保佑       永无BUG
-- //           游戏大卖       公司腾飞
-- //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
-- --]]
do
    require("public.CLLInclude")

    local mApplicationPauseDelegate = {}
    CLLMainLua = {}

    -- 关掉gc
    if not MyCfg.self.isUnityEditor then
        CS.UnityEngine.Scripting.GarbageCollector.GCMode = CS.UnityEngine.Scripting.GarbageCollector.Mode.Disabled
    end

    MyCfg.mode = GameMode.none
    --UIAtlas.releaseSpriteTime = 5 -- 释放ui资源的时间（秒）

    -- 设置是否可以成多点触控
    -- CLCfgBase.self.uiCamera:GetComponent("UICamera").allowMultiTouch = false

    -- if (SystemInfo.systemMemorySize < 2048) then
    --     CLCfgBase.self.isFullEffect = false
    -- end

    --设置帧率
    Application.targetFrameRate = 30
    --QualitySettings.SetQualityLevel (1, false)
    Time.fixedDeltaTime = 0.08

    -- 日志开关
    --CS.Debug.logger.logEnabled = false

    -- 设置是否测试环境
    if (Prefs.getTestMode()) then
        local url = Prefs.getTestModeUrl()
        if (not isNilOrEmpty(url)) then
            CLAlert.add("Test...", Color.red, -1, 1, false)
            CLVerManager.self.baseUrl = url
        end
    end

    local fps = CLMainBase.self:GetComponent("CLFPS")
    fps.displayRect = Rect(10, 200, 640, 40)

    if Net.self.switchNetType == NetWorkType.publish then
        fps.enabled = false
    end

    -- 当离线调用
    function CLLMainLua.onOffline()
        local ok, result = pcall(procOffLine)
        if not ok then
            printe(result)
        end
    end

    -- 退出游戏确认
    function CLLMainLua.exitGmaeConfirm()
        if (CLCfgBase.self.isGuidMode) then
            return
        end
        -- 退出确认
        if (CLPanelManager.topPanel == nil or (not CLPanelManager.topPanel:hideSelfOnKeyBack())) then
            CLUIUtl.showConfirm(Localization.Get("MsgExitGame"), CLLMainLua.doExitGmae, nil)
        end
    end

    -- 退出游戏
    function CLLMainLua.doExitGmae(...)
        __ApplicationQuit__ = true
        Application.Quit()
    end

    -- 暂停游戏或恢复游戏
    function CLLMainLua.OnApplicationPause(isPause)
        if __ApplicationQuit__ then
            return
        end
        if (isPause) then
            --设置帧率
            Application.targetFrameRate = 1
            -- 内存释放
            GC.Collect()
        else
            -- 设置帧率
            Application.targetFrameRate = 30
        end
        for k, v in pairs(mApplicationPauseDelegate) do
            Utl.doCallback(v, isPause)
        end
    end

    ---@public 设置应用暂停代理
    function CLLMainLua.addApplicationPauseCallback(callback)
        mApplicationPauseDelegate[callback] = callback
    end

    ---@public 移除应用暂停代理
    function CLLMainLua.removeApplicationPauseCallback(callback)
        mApplicationPauseDelegate[callback] = nil
    end

    function CLLMainLua.OnApplicationQuit()
        __ApplicationQuit__ = true

        if CLCfgBase.self.isEditMode and CLCfgBase.self.isContBorrowSpriteTimes then
            onApplicationPauseCallback4CountAtlas()
        end
    end
    --=========================================
    function CLLMainLua.showPanelStart()
        if (CLPanelManager.topPanel ~= nil and CLPanelManager.topPanel.name == "PanelStart") then
            CLPanelManager.topPanel:show()
        else
            --异步方式打开页面
            CLPanelManager.getPanelAsy("PanelSplash", CLLMainLua.showSplash)
        end
    end

    function CLLMainLua.showSplash(p)
        CLPanelManager.showPanel(p)
    end

    --------------------------------------------
    ---------- 验证热更新器是否需要更新------------
    --------------------------------------------
    function CLLMainLua.onCheckUpgrader(isHaveUpdated)
        if (isHaveUpdated) then
            -- 说明热更新器有更新，需要重新加载lua
            CLMainBase.self:reStart()
        else
            if (CLCfgBase.self.isEditMode) then
                --主初始化完后，打开下一个页面
                CLMainBase.self:invoke4Lua(CLLMainLua.showPanelStart, 0.2)
            else
                -- 先执行一次热更新，注意isdoUpgrade=False,因为如果更新splash的atalse资源时，会用到
                CLLVerManager.init(
                    nil,
                    function()
                        --主初始化完后，打开下一个页面
                        CLMainBase.self:invoke4Lua(CLLMainLua.showPanelStart, 0.1)
                    end,
                    false,
                    ""
                )
            end
        end
    end

    function CLLMainLua.begain()
        -- 日志logveiw
        if ReporterMessageReceiver.self ~= nil then
            ReporterMessageReceiver.self.luaPath = "KOK/upgradeRes/priority/lua/toolkit/KKLogListener.lua"
            ReporterMessageReceiver.self:setLua()
        end

        -- 处理开始
        if (CLCfgBase.self.isEditMode) then
            CLLMainLua.onCheckUpgrader(false)
        else
            -- 更新热更新器
            -- CLLUpdateUpgrader.checkUpgrader(CLLMainLua.onCheckUpgrader)
            local ret, msg = pcall(CLLUpdateUpgrader.checkUpgrader, CLLMainLua.onCheckUpgrader)
            if not ret then
                printe(msg)
                CLLMainLua.onCheckUpgrader(false)
            end
        end
    end
    
    --------------------------------------------
    --------------------------------------------
    if CLCfgBase.self.isEditMode and CLCfgBase.self.isContBorrowSpriteTimes then
        UIAtlas.onBorrowSpriteCallback = onBorrowedSpriteCB
    end
    CLLMainLua.begain()
    --------------------------------------------
    --------------------------------------------
    return CLLMainLua
end
