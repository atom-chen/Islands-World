-- xx界面
require("city.IDMainCity")
IDPSceneManager = {}

---@type Coolape.CLPanelLua
local csSelf = nil
local transform = nil
---@type UnityEngine.Transform
local lookAtTarget = MyCfg.self.lookAtTarget
local progressBar
local LabelTip
local mData
local _isLoadingScene = false
local dragSetting = CLUIDrag4World.self
local smoothFollow = IDLCameraMgr.smoothFollow

-- 初始化，只会调用一次
function IDPSceneManager.init(csObj)
    csSelf = csObj
    transform = csObj.transform
    --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
    local bottom = getChild(transform, "Bottom")
    progressBar = getCC(bottom, "Progress Bar", "UISlider")
    LabelTip = getCC(bottom, "LabelTip", "UILabel")
end

-- 设置数据
function IDPSceneManager.setData(paras)
    mData = paras
end

-- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
function IDPSceneManager.show()
    _isLoadingScene = true
    progressBar.value = 0
    csSelf:invoke4Lua(IDPSceneManager.loadScene, 0.1)
end

function IDPSceneManager.isLoadingScene()
    return _isLoadingScene
end

function IDPSceneManager.beforeLoadScene()
    local oldMode = MyCfg.mode
    if oldMode == GameMode.city then
        if IDMainCity then
            IDMainCity.clean()
        end
    elseif oldMode == GameMode.battle then
        if IDLBattle then
            IDLBattle.clean()
        end
    elseif oldMode == GameMode.map then
        if IDMainCity then
            IDMainCity.clean()
        end
        if IDWorldMap then
            IDWorldMap.clean()
        end
    end
    releaseRes4GC()
end

function IDPSceneManager.loadScene()
    IDPSceneManager.beforeLoadScene()
    csSelf:invoke4Lua(IDPSceneManager.doLoadScene, 0.5)
end

function IDPSceneManager.doLoadScene()
    MyCfg.mode = mData.mode
    local currMode = MyCfg.mode
    if currMode == GameMode.city then
        IDPSceneManager.loadCity()
    elseif currMode == GameMode.map then
        IDPSceneManager.loadWorldMap()
    elseif currMode == GameMode.battle then
        IDPSceneManager.loadBattle()
    end
end

-- 刷新
function IDPSceneManager.refresh()
end

-- 关闭页面
function IDPSceneManager.hide()
    csSelf:cancelInvoke4Lua()
    _isLoadingScene = false
end

-- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
function IDPSceneManager.procNetwork(cmd, succ, msg, paras)
    --[[
        if(succ == NetSuccess) then
          if(cmd == "xxx") then
          end
        end
        --]]
end

-- 处理ui上的事件，例如点击等
function IDPSceneManager.uiEventDelegate(go)
    local goName = go.name
    --[[
        if(goName == "xxx") then
        end
        --]]
end

-- 当按了返回键时，关闭自己（返值为true时关闭）
function IDPSceneManager.hideSelfOnKeyBack()
    return false
end

-- 加载主基地
function IDPSceneManager.loadCity()
    Time.fixedDeltaTime = 0.04
    -- Turn off v-sync
    QualitySettings.vSyncCount = 0
    Application.targetFrameRate = 30

    if dragSetting then
        dragSetting.isLimitCheckStrict = false
        dragSetting.canMove = true
        dragSetting.canRotation = true
        dragSetting.canScale = true
        dragSetting.scaleMini = 7
        dragSetting.scaleMax = 20
        dragSetting.scaleHeightMini = 10
        dragSetting.scaleHeightMax = 100
        dragSetting.viewRadius = 65
        dragSetting.dragMovement = Vector3.one
         -- * 0.4
        dragSetting.scaleSpeed = 1
    end

    smoothFollow.distance = 5
    smoothFollow.height = 5

    IDMainCity.init(nil, IDPSceneManager.onLoadCity, IDPSceneManager.onProgress)
end

function IDPSceneManager.onLoadCity()
    lookAtTarget.localEulerAngles = Vector3(0, 45, 0)
    SoundEx.playMainMusic("MainScene_1")
    getPanelAsy("PanelMain", onLoadedPanel)
end

function IDPSceneManager.onProgress(totalAssets, currCount)
    SetActive(progressBar.gameObject, true)
    progressBar.value = currCount / totalAssets
end

function IDPSceneManager.loadWorldMap()
    Time.fixedDeltaTime = 0.08
    -- Turn off v-sync
    QualitySettings.vSyncCount = 0
    Application.targetFrameRate = 30

    if dragSetting then
        dragSetting.isLimitCheckStrict = false
        dragSetting.canMove = true
        dragSetting.canRotation = true
        dragSetting.canScale = true
        dragSetting.scaleMini = 7
        dragSetting.scaleMax = 20
        dragSetting.scaleHeightMini = 10
        dragSetting.scaleHeightMax = 100
        dragSetting.viewRadius = 15000
        dragSetting.dragMovement = Vector3.one
         -- * 0.5
        dragSetting.scaleSpeed = 1
    end

    smoothFollow.distance = 20
    smoothFollow.height = 100
    lookAtTarget.localEulerAngles = Vector3(0, 45, 0)
    IDWorldMap.init(
        bio2number(IDDBCity.curCity.pos),
        function()
            --//TODO:播放音乐
            -- SoundEx.playMainMusic("MainScene_1")
            getPanelAsy("PanelMain", onLoadedPanel)
        end,
        IDPSceneManager.onProgress
    )
end

-- 加载战场
function IDPSceneManager.loadBattle()
    Time.fixedDeltaTime = 0.02
    -- Turn off v-sync
    QualitySettings.vSyncCount = 0
    Application.targetFrameRate = 30

    if dragSetting then
        dragSetting.isLimitCheckStrict = false
        dragSetting.canMove = true
        dragSetting.canRotation = true
        dragSetting.canScale = true
        dragSetting.scaleMini = 7
        dragSetting.scaleMax = 20
        dragSetting.scaleHeightMini = 10
        dragSetting.scaleHeightMax = 100
        dragSetting.viewRadius = 65
        dragSetting.dragMovement = Vector3.one
         -- * 0.4
        dragSetting.scaleSpeed = 1
    end

    smoothFollow.distance = 5
    smoothFollow.height = 5

    IDLBattle.init(mData.defData, mData.offData, IDPSceneManager.onLoadCity, IDPSceneManager.onProgress)
end

--------------------------------------------
return IDPSceneManager
