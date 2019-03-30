-- xx界面
do
    local CLLPLoginCoolape = {}

    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local onLoginCallback
    local onLoginCallbackParam
    local isAutoLogin

    -- 初始化，只会调用一次
    function CLLPLoginCoolape.init(csObj)
        csSelf = csObj
        transform = csObj.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.panelTw = csSelf:GetComponent("TweenPosition")
        local from = UIAnchor.top()
        from.y = from.y * 2
        uiobjs.panelTw.from = from

        uiobjs.gridContent = getCC(transform, "content/Grid", "TweenPosition")
        uiobjs.loginContent = getChild(uiobjs.gridContent.transform, "contentLogin/Grid")
        uiobjs.InputUser4Login = getCC(uiobjs.loginContent, "InputUser", "UIInput")
        uiobjs.InputPassword4Login = getCC(uiobjs.loginContent, "InputPassword", "UIInput")

        uiobjs.contentRegist = getChild(uiobjs.gridContent.transform, "contentRegist/Grid")
        uiobjs.InputUser4Regist = getCC(uiobjs.contentRegist, "InputUser", "UIInput")
        uiobjs.InputPassword4Regist = getCC(uiobjs.contentRegist, "InputPassword", "UIInput")
        uiobjs.InputPassword4Regist2 = getCC(uiobjs.contentRegist, "InputPassword2", "UIInput")
        uiobjs.InputEmail = getCC(uiobjs.contentRegist, "InputEmail", "UIInput")
    end

    -- 设置数据
    function CLLPLoginCoolape.setData(paras)
        onLoginCallback = paras[1]
        onLoginCallbackParam = paras[2]
        isAutoLogin = paras[3]
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function CLLPLoginCoolape.show()
        uiobjs.InputUser4Regist.value = ""
        uiobjs.InputPassword4Regist.value = ""
        uiobjs.InputPassword4Regist2.value = ""
        uiobjs.InputEmail.value = ""
        uiobjs.InputUser4Login.value = Prefs.getUserName()
        uiobjs.InputPassword4Login.value = Prefs.getUserPsd()

        uiobjs.gridContent:Play(false)
        if isAutoLogin and (not isNilOrEmpty(uiobjs.InputUser4Login.value)) and (not isNilOrEmpty(uiobjs.InputPassword4Login.value)) then
            -- 自动登陆
            csSelf:invoke4Lua(
                    function()
                        showHotWheel()
                        local user = trim(uiobjs.InputUser4Login.value)
                        local psd = trim(uiobjs.InputPassword4Login.value)
                        CLLPLoginCoolape.accountLogin(user, psd)
                    end, 0.2)
        end
    end

    -- 刷新
    function CLLPLoginCoolape.refresh()
    end

    -- 关闭页面
    function CLLPLoginCoolape.hide()
    end

    function CLLPLoginCoolape.accountLogin(user, psd)
        local deviceInfor = {}
        table.insert(deviceInfor, SystemInfo.deviceName)
        table.insert(deviceInfor, SystemInfo.deviceModel)
        table.insert(deviceInfor, SystemInfo.deviceType:ToString())
        table.insert(deviceInfor, SystemInfo.operatingSystem)
        table.insert(deviceInfor, SystemInfo.maxTextureSize)
        showHotWheel()
        CLLNet.httpPostUsermgr(NetProtoUsermgr.send.loginAccount(user, psd,
                CLCfgBase.self.appUniqueID,
                getChlCode()))
    end

    function CLLPLoginCoolape.accountRegist(user, psd, email)
        local deviceInfor = {}
        table.insert(deviceInfor, SystemInfo.deviceName)
        table.insert(deviceInfor, SystemInfo.deviceModel)
        table.insert(deviceInfor, SystemInfo.deviceType:ToString())
        table.insert(deviceInfor, SystemInfo.operatingSystem)
        table.insert(deviceInfor, SystemInfo.maxTextureSize)
        showHotWheel()
        CLLNet.httpPostUsermgr(NetProtoUsermgr.send.registAccount(user, psd, email,
                CLCfgBase.self.appUniqueID,
                getChlCode(),
                Utl.uuid,
                table.concat(deviceInfor, ",")))
    end

    function CLLPLoginCoolape.onAccountLogin(content)
        Utl.doCallback(onLoginCallback, content, onLoginCallbackParam)
        hideTopPanel(csSelf)
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function CLLPLoginCoolape.procNetwork (cmd, succ, msg, paras)
        if (succ == NetSuccess) then
            if (cmd == "loginAccount") then
                hideHotWheel()
                if not isNilOrEmpty(uiobjs.InputUser4Login.value) then
                    Prefs.setUserName(uiobjs.InputUser4Login.value)
                end

                if not isNilOrEmpty(uiobjs.InputPassword4Login.value) then
                    Prefs.setUserPsd(uiobjs.InputPassword4Login.value)
                end
                CLLPLoginCoolape.onAccountLogin(paras)
            elseif (cmd == "registAccount") then
                hideHotWheel()
                if not isNilOrEmpty(uiobjs.InputUser4Regist.value) then
                    Prefs.setUserName(uiobjs.InputUser4Regist.value)
                end

                if not isNilOrEmpty(uiobjs.InputPassword4Regist.value) then
                    Prefs.setUserPsd(uiobjs.InputPassword4Regist.value)
                end
                CLLPLoginCoolape.onAccountLogin(paras)
            end
        end
    end

    -- 处理ui上的事件，例如点击等
    function CLLPLoginCoolape.uiEventDelegate(go)
        local goName = go.name
        if (goName == "ButtonClose") then
            hideTopPanel(csSelf)
        elseif goName == "ButtonGotoRegist" then
            uiobjs.gridContent:Play(true)
        elseif goName == "ButtonBack" then
            uiobjs.gridContent:Play(false)
        elseif goName == "ButtonLogin" then
            local user = trim(uiobjs.InputUser4Login.value)
            local psd = trim(uiobjs.InputPassword4Login.value)
            if isNilOrEmpty(user) or isNilOrEmpty(psd) then
                CLAlert.add(LGet("UserIsEmpty"), Color.red, 1)
                return
            end
            uiobjs.InputUser4Login.value = user
            uiobjs.InputPassword4Login.value = psd
            if string.find(user, "%W") then
                -- havUtf8Char(user)
                CLAlert.add(LGet("UserIllegal"), Color.red, 1)
                return
            end
            if string.find(psd, "%W") then
                -- havUtf8Char(psd)
                CLAlert.add(LGet("PasswordIllegal"), Color.red, 1)
                return
            end

            showHotWheel()
            CLLPLoginCoolape.accountLogin(user, psd)
        elseif goName == "ButtonRegist" then
            local user = trim(uiobjs.InputUser4Regist.value)
            local psd = trim(uiobjs.InputPassword4Regist.value)
            local psd2 = trim(uiobjs.InputPassword4Regist2.value)
            if isNilOrEmpty(user) or isNilOrEmpty(psd) or isNilOrEmpty(psd2) then
                CLAlert.add(LGet("UserIsEmpty"), Color.red, 1)
                return
            end
            if psd ~= psd2 then
                CLAlert.add(LGet("PasswordConfirmError"), Color.red, 1)
                return
            end
            if not isNilOrEmpty(uiobjs.InputEmail.value) then
                if not string.find(uiobjs.InputEmail.value, "@") then
                    CLAlert.add(LGet("EmailIllegal"), Color.red, 1)
                    return
                end
            end
            uiobjs.InputUser4Regist.value = user
            uiobjs.InputPassword4Regist.value = psd

            if string.find(user, "%W") then
                -- havUtf8Char(user)
                CLAlert.add(LGet("UserIllegal"), Color.red, 1)
                return
            end
            if string.find(psd, "%W") then
                -- havUtf8Char(psd)
                CLAlert.add(LGet("PasswordIllegal"), Color.red, 1)
                return
            end

            showHotWheel()
            CLLPLoginCoolape.accountRegist(user, psd, uiobjs.InputEmail.value)
        end
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function CLLPLoginCoolape.hideSelfOnKeyBack()
        return true
    end

    --------------------------------------------
    return CLLPLoginCoolape
end
