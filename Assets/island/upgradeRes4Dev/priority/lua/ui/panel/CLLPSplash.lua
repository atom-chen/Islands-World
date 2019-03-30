--开始loading页面，处理资源更新、及相关初始化
do
    ---@type json
    local json = require("json.json")

    ---@type Coolape.CLPanelLua
    local csSelf = nil
    local transform = nil
    local gameObject = nil
    local progressBar = nil
    local progressBarTotal = nil
    local lbprogressBarTotal = nil
    local LabelTip = nil
    local LabelVer = nil
    local lbCustServer
    local loadedPanelCount = 0
    local server
    local user
    local bottom

    local www4UpgradeCell = nil -- 更新时单个单元的www

    -- 预先加载的页面(在热更新完成后，先把必要的公共页面先加载了，后面的处理可能会用到)
    local beforeLoadPanels = {
        "PanelHotWheel", -- 菊花
        "PanelBackplate", -- 背板遮罩
        "PanelConfirm", -- 确认提示页面
        "PanelMask4Panel", -- 遮挡
        "PanelWWWProgress", -- 显示网络请求资源的进度
    }

    local CLLPSplash = {}

    function CLLPSplash.init(go)
        csSelf = go
        transform = csSelf.transform
        gameObject = csSelf.gameObject
        bottom = getChild(transform, "Bottom")
        progressBar = getCC(bottom, "Progress Bar", "UISlider")
        NGUITools.SetActive(progressBar.gameObject, false)

        progressBarTotal = getChild(transform, "Bottom", "Progress BarTotal")
        lbprogressBarTotal = getChild(progressBarTotal, "Thumb", "Label"):GetComponent("UILabel")
        progressBarTotal = progressBarTotal:GetComponent("UISlider")
        NGUITools.SetActive(progressBarTotal.gameObject, false)

        LabelTip = getChild(bottom, "LabelTip")
        LabelTip = LabelTip:GetComponent("UILabel")
        NGUITools.SetActive(LabelTip.gameObject, false)
        LabelVer = getChild(transform, "TopLeft", "LabelVer")
        LabelVer = LabelVer:GetComponent("UILabel")

        lbCustServer = getCC(transform, "TopLeft/LabelCustomerServer", "UILabel")
    end

    function CLLPSplash.show()
        SoundEx.playMainMusic("login")
        csSelf.panel.depth = 200
        loadedPanelCount = 0
        SetActive(progressBar.gameObject, false)
        SetActive(progressBarTotal.gameObject, false)

        -- load alert
        CLLPSplash.addAlertHud()

        -- 初始化需要提前加载的页面
        loadedPanelCount = 0
        for i, v in ipairs(beforeLoadPanels) do
            CLPanelManager.getPanelAsy(v, CLLPSplash.onLoadPanelBefore)
        end

        -- Hide company panel
        csSelf:invoke4Lua(CLLPSplash.hideFirstPanel, 1.5)
    end

    -- 关闭页面
    function CLLPSplash.hide()
        csSelf:cancelInvoke4Lua()
        CLLPSplash.hideFirstPanel()
    end

    -- 刷新页面
    function CLLPSplash.refresh()
        LabelVer.text = joinStr(Localization.Get("Version"), __version__)
        lbCustServer.text = ""
        LabelTip.text = ""
    end

    -- 加载hud alert
    function CLLPSplash.addAlertHud()
        local onGetObj = function(name, AlertRoot, orgs)
            AlertRoot.transform.parent = CLUIInit.self.uiPublicRoot
            AlertRoot.transform.localPosition = Vector3.zero
            AlertRoot.transform.localScale = Vector3.one
            NGUITools.SetActive(AlertRoot, true)
        end
        CLUIOtherObjPool.borrowObjAsyn("AlertRoot", onGetObj)
    end

    function CLLPSplash.hideFirstPanel()
        ---@type Coolape.CLPanelLua
        local p = CLPanelManager.getPanel(MyMain.self.firstPanel)
        if (p ~= nil and p.gameObject.activeInHierarchy) then
            CLPanelManager.hidePanel(p)
        end
    end

    function CLLPSplash.onLoadPanelBefore(p)
        p:init()
        loadedPanelCount = loadedPanelCount + 1
        if (p.name == "PanelConfirm" or
                p.name == "PanelHotWheel" or
                p.name == "PanelMask4Panel" or
                p.name == "PanelWWWProgress") then
            p.transform.parent = CLUIInit.self.uiPublicRoot
            p.transform.localScale = Vector3.one
        end

        if (p.name == "PanelWWWProgress") then
            CLPanelManager.showPanel(p)
        end
        CLLPSplash.onProgress(#(beforeLoadPanels), loadedPanelCount)
        if (loadedPanelCount >= #(beforeLoadPanels)) then
            -- 页面已经加载完成，处理热更新
            if (not Application.isEditor) then
                CLLPSplash.checkNewVersion()
            else
                csSelf:invoke4Lua(CLLPSplash.updateRes, 0.2)
            end
        end
    end

    --[[
    -- 更新版本
    --{"ver":"1.0","force":true,"url":"http://"}
    --]]
    function CLLPSplash.checkNewVersion()
        local oldVer = __version__
        local onGetVer = function(content, orgs)
            local map = JSON.DecodeMap(content)
            local newVer = MapEx.getString(map, "ver")
            if (tonumber(newVer) > tonumber(oldVer)) then
                local doUpgradeApp = function()
                    CLLPSplash.upgradeGame(MapEx.getString(map, "url"))
                end
                if MapEx.getBool(map, "force") then
                    CLUIUtl.showConfirm(LGet("MsgHadNewVerApp"), true, LGet("Update"), doUpgradeApp, "", nil)
                else
                    CLUIUtl.showConfirm(LGet("MsgHadNewVerApp"), false, LGet("Update"), doUpgradeApp, LGet("UpdateLater"), CLLPSplash.updateRes)
                end
            else
                CLLPSplash.updateRes()
            end
        end

        local onGetVerError = function(msg, orgs)
            CLAlert.add(LGet("MsgCheckAppUpgradeFail"), Color.white, 1)
            CLLPSplash.updateRes()
        end

        local chlCode = getChlCode()
        local url = Utl.urlAddTimes(joinStr(CLVerManager.self.baseUrl, "/appVer.", chlCode, ".json"))
        WWWEx.get(url, CLAssetType.text, onGetVer, onGetVerError, nil, true)
    end

    -- 更新安装游戏
    function CLLPSplash.upgradeGame(url)
        if not isNilOrEmpty(url ) then
            Application.OpenURL(url)
        end
    end

    -- 处理热更新
    function CLLPSplash.updateRes()
        if CLCfgBase.self.isDirectEntry then
            -- 取得缓存的数据
            user = json.decode(Prefs.getUserInfor())
            server = json.decode(Prefs.getCurrServer())
            CLLPSplash.checkHotUpgrade()
        else
            if not CLCfgBase.self.hotUpgrade4EachServer then
                -- 更新资源
                CLLVerManager.init(CLLPSplash.onProgress, CLLPSplash.onFinishResUpgrade, true, "")
            else
                --
                CLLPSplash.accountLogin()
            end
        end
    end

    function CLLPSplash.accountLogin()
        getPanelAsy("PanelLogin", onLoadedPanelTT, {CLLPSplash.onAccountLogin, true})
    end

    function CLLPSplash.onAccountLogin(_user, _server)
        Prefs.setUserInfor(json.encode(_user))
        Prefs.setCurrServer(json.encode(_server))
        user = _user
        server = _server
        CLLPSplash.checkHotUpgrade()
    end

    function CLLPSplash.checkHotUpgrade()
        if CLCfgBase.self.hotUpgrade4EachServer then
            local resMd5 = ""
            if CLPathCfg.self.platform == "IOS" then
                resMd5 = server.iosVer
            else
                resMd5 = server.androidVer
            end
            -- 更新资源
            CLLVerManager.init(CLLPSplash.onProgress, CLLPSplash.onFinishResUpgrade, true, resMd5)
        else
            CLLPSplash.prepareStartGame()
        end
    end

    --设置进度条
    function CLLPSplash.onProgress(...)
        local args = { ... }
        local all = args[1] -- 总量
        local v = args[2] -- 当前值
        if (#(args) >= 3) then
            www4UpgradeCell = args[3]
        else
            www4UpgradeCell = nil
        end

        if (progressBarTotal ~= nil) then
            NGUITools.SetActive(progressBarTotal.gameObject, true)
            NGUITools.SetActive(LabelTip.gameObject, true)
            if (type(all) == "number") then
                if (all > 0) then
                    local value = v / all
                    progressBarTotal.value = value
                    if (www4UpgradeCell ~= nil) then
                        -- 说明有单个资源
                        lbprogressBarTotal.text = joinStr(v, "/", all)
                    end
                    -- 单个资源的进度
                    CLLPSplash.onProgressCell()

                    -- 表明已经更新完成
                    if (value == 1) then
                        csSelf:cancelInvoke4Lua(CLLPSplash.onProgressCell)
                        NGUITools.SetActive(progressBarTotal.gameObject, false)
                        NGUITools.SetActive(LabelTip.gameObject, false)
                        NGUITools.SetActive(progressBar.gameObject, false)
                    end
                else
                    csSelf:cancelInvoke4Lua(CLLPSplash.onProgressCell)
                    progressBarTotal.value = 0
                    NGUITools.SetActive(progressBarTotal.gameObject, false)
                    NGUITools.SetActive(LabelTip.gameObject, false)
                    NGUITools.SetActive(progressBar.gameObject, false)
                end
            else
                print(joinStr("all====", all))
            end
        end
    end

    -- 单个文件更新进度
    function CLLPSplash.onProgressCell(...)
        if (www4UpgradeCell ~= nil) then
            NGUITools.SetActive(progressBar.gameObject, true)
            progressBar.value = www4UpgradeCell.progress
            csSelf:cancelInvoke4Lua(CLLPSplash.onProgressCell)
            csSelf:invoke4Lua(CLLPSplash.onProgressCell, 0.1)
        else
            NGUITools.SetActive(progressBar.gameObject, false)
            csSelf:cancelInvoke4Lua(CLLPSplash.onProgressCell)
        end
    end

    -- 资源更新完成
    function CLLPSplash.onFinishResUpgrade(upgradeProcSuccess)
        if (not upgradeProcSuccess) then
            print("UpgradeResFailed")
        else
            if (CLLVerManager.isHaveUpgrade()) then
                -- 说明有更新，重新启动
                if CLCfgBase.self.hotUpgrade4EachServer then
                    CLCfgBase.self.isDirectEntry = true
                end
                csSelf:cancelInvoke4Lua()
                csSelf:invoke4Lua(CLLPSplash.reLoadGame, 0.1)
                return
            end
        end

        if CLCfgBase.self.hotUpgrade4EachServer then
            -- 准备开始游戏
            CLLPSplash.prepareStartGame()
        else
            --SetActive(ButtonEntry, true)
            CLLPSplash.accountLogin()
        end
    end

    -- 重新启动lua
    function CLLPSplash.reLoadGame()
        --- 释放资源开始-------------------------------
        local cleanRes = function()
            if CLAlert ~= nil and CLAlert.csSelf ~= nil then
                GameObject.DestroyImmediate(CLAlert.csSelf.gameObject, true)
            end
            pcall(doSomethingBeforeRestart)
            pcall(releaseRes4GC, true)
        end
        --- 释放资源结束-------------------------------
        pcall(cleanRes)
        local panel = CLPanelManager.getPanel(CLMainBase.self.firstPanel)
        if panel then
            CLPanelManager.showPanel(panel)
        end
        CLMainBase.self:reStart()
    end

    -- 准备开始游戏
    function CLLPSplash.prepareStartGame()
        CLLPSplash.checkSignCode()

        if (progressBar ~= nil) then
            csSelf:cancelInvoke4Lua(CLLPSplash.onProgressCell)
            NGUITools.SetActive(progressBar.gameObject, false)
            NGUITools.SetActive(progressBarTotal.gameObject, false)
            NGUITools.SetActive(LabelTip.gameObject, false)
        end

        -- 播放背景音乐---------------
        -- SoundEx.playMainMusic()
        ----------------------------
    end

    function CLLPSplash.checkSignCode()
        -- 把热更新及加载ui完了后，再做验证签名
        if (not CLLPSplash.isSignCodeValid()) then
            CLUIUtl.showConfirm(Localization.Get("MsgTheVerIsNotCorrect"), nil)
            -- CLUIUtl.showConfirm("亲爱的玩家你所下载的版本可能是非官方版本，请到xxx去下载。非常感谢！", nil)
            return
        end

        CLLPSplash.goNext()
    end

    -- 签名是否有效(Only 4 android)
    function CLLPSplash.isSignCodeValid(...)
        if isNilOrEmpty(CLCfgBase.self.singinMd5Code) then
            return true
        end
        -- 取得签名串
        local md5Code = Utl.getSingInCodeAndroid()

        if (isNilOrEmpty(md5Code)) then
            if (string.lower(md5Code) ~= string.lower(CLCfgBase.self.singinMd5Code)) then
                return false
            end
        end
        return true
    end

    function CLLPSplash.uiEventDelegate(go)
    end

    function CLLPSplash.goNext()
        if CLCfgBase.self.isDirectEntry then
            CLCfgBase.self.isDirectEntry = false
        end
        CLPanelManager.getPanelAsy("PanelStart", onLoadedPanel, { user, server })
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function CLLPSplash.hideSelfOnKeyBack( )
        return false
    end
    ----------------------------------------------
    return CLLPSplash
end
