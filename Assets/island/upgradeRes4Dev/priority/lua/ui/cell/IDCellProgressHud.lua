-- xx单元
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local mData = nil; --[[
    mData.target : gameobject
    mData.data:数据
    mData.offset:位置偏移
    --]]
    local uiobjs = {}


    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.table = csSelf:GetComponent("UITable")
        ---@type UIFollowTarget
        uiobjs.followTarget = csSelf:GetComponent("UIFollowTarget")
        uiobjs.followTarget:setCamera(MyCfg.self.mainCamera, MyCfg.self.uiCamera)

        uiobjs.spriteIcon = getCC(transform, "00SpriteIcon", "UISprite")
        uiobjs.Progress = getCC(transform, "01Progress Bar", "UISlider")
        uiobjs.Label = getCC(uiobjs.Progress.transform, "Label", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data;
        ---@type IDDBBuilding
        local serverData = mData.data
        mData.starttime = bio2number(serverData.starttime)
        mData.endtime = bio2number(serverData.endtime)
        mData.diff = mData.endtime - mData.starttime
        uiobjs.followTarget:setTarget(mData.target.transform, mData.offset or Vector3.zero)
        if bio2number(serverData.state) == IDConst.BuildingState.upgrade then
            CLUIUtl.setSpriteFit(uiobjs.spriteIcon, "icon_build", 80)
        elseif bio2number(serverData.state) == IDConst.BuildingState.working then
            if bio2number(serverData.attrid) == IDConst.dockyardBuildingID then
                local shipID = bio2number(serverData.val)
                CLUIUtl.setSpriteFit(uiobjs.spriteIcon, joinStr("roleIcon_", shipID), 80)
            end
        end

        if mData.diff > 0 then
            _cell.cooldown()
        else
            uiobjs.Label.text = ""
            uiobjs.Progress.value = 0
            InvokeEx.cancelInvokeByFixedUpdate(_cell.cooldown)
        end
        uiobjs.table.repositionNow = true
    end

    function _cell.cooldown()
        if not csSelf.gameObject.activeInHierarchy then
            return
        end
        local lefttime = mData.endtime - DateEx.nowMS
        if lefttime > 0 then
            uiobjs.Label.text = DateEx.toStrCn(lefttime)
            uiobjs.Progress.value = lefttime / mData.diff
            if lefttime > 10000 then
                InvokeEx.invokeByFixedUpdate(_cell.cooldown, 1)
            else
                InvokeEx.invokeByFixedUpdate(_cell.cooldown, 0.2)
            end
        end
    end

    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.refresh(paras)
        --[[
        if(paras == 1) then   -- 刷新血
          -- TODO:
        elseif(paras == 2) then -- 刷新状态
          -- TODO:
        end
        --]]
    end

    -- 取得数据
    function _cell.getData ()
        return mData;
    end

    --------------------------------------------
    return _cell;
end
