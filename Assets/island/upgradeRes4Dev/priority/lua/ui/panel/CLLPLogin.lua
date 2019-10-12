-- 用户登陆界面
do
    ---@type NetProtoUsermgr
    local NetProtoUsermgr = require("net.NetProtoUsermgrClient")
    local CLLPLogin = {}

    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local oldServerIdx
    local user
    local server
    local finishCallback
    local isAutoLogin

    -- 初始化，只会调用一次
    function CLLPLogin.init(csObj)
        csSelf = csObj
        transform = csObj.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.grid = getCC(transform, "PanelConent/Grid", "TweenPosition")
        uiobjs.ButtonEntry = getChild(uiobjs.grid.transform, "002EnterGame/ButtonEntry").gameObject
        uiobjs.ButtonServer = getChild(uiobjs.grid.transform, "002EnterGame/ButtonServer")
        uiobjs.LabelServerName = getCC(uiobjs.ButtonServer, "LabelName", "UILabel")
        uiobjs.LabelServerState = getCC(uiobjs.ButtonServer, "LabelState", "UILabel")
        uiobjs.ButtonServer = uiobjs.ButtonServer.gameObject
        uiobjs.ButtonRight = getChild(uiobjs.grid.transform, "001BtnLogins/ButtonRight").gameObject
        -- 网络
        Net.self:setLua()
    end

    -- 设置数据
    function CLLPLogin.setData(paras)
        finishCallback = paras[1]
        isAutoLogin = paras[2]
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function CLLPLogin.show()
        SetActive(uiobjs.ButtonRight, false)
        SetActive(uiobjs.ButtonEntry, false)
        SetActive(uiobjs.ButtonServer, false)
        uiobjs.grid:Play(false)
        if isAutoLogin then
            showHotWheel()
            csSelf:invoke4Lua(CLLPLogin.autoLogin, 1)
        end
    end

    function CLLPLogin.autoLogin()
        hideHotWheel()
        local lastLoginBtn = Prefs.getLastLoginBtn()
        if not isNilOrEmpty(lastLoginBtn) then
            CLLPLogin.doUiEventDelegate(lastLoginBtn)
        end
    end

    -- 当加载好通用框的回调
    function CLLPLogin.onShowFrame(cs)
        csSelf.frameObj:init({ title = "", panel = csSelf, hideClose = true, hideTitle = true })
    end

    -- 刷新
    function CLLPLogin.refresh()
    end

    -- 关闭页面
    function CLLPLogin.hide()
    end

    function CLLPLogin.setServer(data)
        server = data
        uiobjs.LabelServerName.text = server.name or LGet("None")
        local status = bio2number(server.status)
        local stateDesc
        if status == 2 then
            stateDesc = joinStr("[00ffff]", LGet("StateCrowded"), "[-]")
        elseif status == 3 then
            stateDesc = LGet("StateMaintain")
            stateDesc = joinStr("[ff0000]", LGet("StateMaintain"), "[-]")
        else
            stateDesc = joinStr("[00ff00]", LGet("StateNomal"), "[-]")
        end
        uiobjs.LabelServerState.text = stateDesc
        SetActive(uiobjs.ButtonServer, true)
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function CLLPLogin.procNetwork (cmd, succ, msg, paras)
        if succ == NetSuccess then
            if cmd == NetProtoUsermgr.cmds.loginAccountChannel
                    or cmd == NetProtoUsermgr.cmds.loginAccount
                    or cmd == NetProtoUsermgr.cmds.registAccount then
                hideHotWheel()
                CLLPLogin.onLogin4Coolape(paras)
            elseif cmd == NetProtoUsermgr.cmds.getServerInfor then
                CLLPLogin.setServer(paras.server)
                SetActive(uiobjs.ButtonEntry, true)
                hideHotWheel()
            end
        else
            isAutoLogin = false
            hideHotWheel()
            if cmd == NetProtoUsermgr.cmds.getServerInfor then
                CLLPLogin.setServer({})
                SetActive(uiobjs.ButtonEntry, true)
            end
        end
    end

    function CLLPLogin.login(uid, orgs)
        local btnName = orgs
        if not isNilOrEmpty(btnName) then
            Prefs.setLastLoginBtn(btnName)
        end

        local deviceInfor = {}
        table.insert(deviceInfor, SystemInfo.deviceName)
        table.insert(deviceInfor, SystemInfo.deviceModel)
        table.insert(deviceInfor, SystemInfo.deviceType:ToString())
        table.insert(deviceInfor, SystemInfo.operatingSystem)
        table.insert(deviceInfor, SystemInfo.maxTextureSize)
        showHotWheel()
        CLLNet.httpPostUsermgr(NetProtoUsermgr.send.loginAccountChannel(uid,
                CLCfgBase.self.appUniqueID,
                getChlCode(),
                Utl.uuid,
                table.concat(deviceInfor, ","),
                MyCfg.self.isEditScene or __EditorMode__
        ))
    end

    function CLLPLogin.onLogin4Coolape(d, orgs)
        isAutoLogin = false -- 登陆成功后设为false
        local btnName = orgs
        if not isNilOrEmpty(btnName) then
            Prefs.setLastLoginBtn(btnName)
        end
        user = d.userInfor
        NetProtoUsermgr.__sessionID = bio2Int(d.userInfor.idx)

        -- 取得服务器
        oldServerIdx = bio2number(d.serverid)
        if oldServerIdx > 0 then
            showHotWheel()
            CLLNet.httpPostUsermgr(NetProtoUsermgr.send.getServerInfor(oldServerIdx))
        else
            printw("get the server id == 0")
            CLLPLogin.setServer({})
            SetActive(uiobjs.ButtonEntry, true)
        end

        SetActive(uiobjs.ButtonRight, true)
        uiobjs.grid:Play(true)
    end

    -- 处理ui上的事件，例如点击等
    function CLLPLogin.uiEventDelegate(go)
        local goName = go.name
        CLLPLogin.doUiEventDelegate(goName)
    end

    function CLLPLogin.doUiEventDelegate(goName)
        if goName == "ButtonVisitor" then
            CLLPLogin.login(Utl.uuid, goName)
        elseif goName == "ButtonCoolape" then
            -- 登陆coolape的账号
            getPanelAsy("PanelLoginCoolape", onLoadedPanelTT, { CLLPLogin.onLogin4Coolape, goName, isAutoLogin })
        elseif goName == "ButtonFaceBook" then
            -- facebook登陆
            --showHotWheel()
            --KKChl.loginFaceBook(CLLPLogin.login, goName)
        elseif goName == "ButtonServer" then
            getPanelAsy("PanelServers", onLoadedPanelTT, { CLLPLogin.setServer, server })
        elseif goName == "ButtonEntry" then
            SetActive(uiobjs.ButtonEntry, false)
            CLLNet.httpPostUsermgr(NetProtoUsermgr.send.getServerInfor(bio2number(server.idx)),
                    function(content)
                        if content == nil then
                            SetActive(uiobjs.ButtonEntry, true)
                            return
                        end
                        local server = content.server
                        local state = bio2number(server.status)
                        if state == 3 then
                            -- 服务器停服了
                            CLUIUtl.showConfirm(joinStr("[B75605]", Localization.Get("MsgServerIsMaintain"), "[-]"), nil)
                            SetActive(uiobjs.ButtonEntry, true)
                            return
                        end
                        hideTopPanel(csSelf)
                        if oldServerIdx ~= bio2number(server.idx) then
                            -- 保存所选的服务器
                            CLLNet.httpPostUsermgr(NetProtoUsermgr.send.setEnterServer(bio2number(server.idx), bio2number(user.idx), CLCfgBase.self.appUniqueID))
                        end
                        Utl.doCallback(finishCallback, user, server)
                    end)
        elseif goName == "ButtonXieyi" then
            --CLPanelManager.getPanelAsy("PanelRobotTest", onLoadedPanelTT, { MapEx.getString(user, "idx"), selectedServer })
        elseif goName == "ButtonSetting" then
            getPanelAsy("PanelSetting", onLoadedPanelTT)
        elseif goName == "ButtonRight" then
            uiobjs.grid:Play(true)
        elseif goName == "ButtonLeft" then
            uiobjs.grid:Play(false)
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function CLLPLogin.hideSelfOnKeyBack()
        return false
    end

    --------------------------------------------
    return CLLPLogin
end
