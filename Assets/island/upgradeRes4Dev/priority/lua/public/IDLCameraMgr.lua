-- 界面
IDLCameraMgr = {}
IDLCameraMgr.smoothFollow = nil

---@type CameraMgr
local csSelf = CameraMgr.self

local profiles = {}
local profileAssets = {}

-- 初始化，只会调用一次
function IDLCameraMgr.init()
    IDLCameraMgr.smoothFollow = MyCfg.self.mainCamera:GetComponent("CLSmoothFollow")
    csSelf.subpostprocessing.enabled = false

    CameraMgr.self:setLua()
    IDLCameraMgr.getProfile(
        "normal",
        function()
            IDLCameraMgr.setPostProcessingProfile("normal")
            IDLCameraMgr.setPostProcessingProfile("normal", csSelf.subcamera)
        end
    )
end

function IDLCameraMgr.setPostProcessingProfile(name, camera)
    IDLCameraMgr.getProfile(
        name,
        function(profile)
            if camera == nil or camera == csSelf.maincamera then
                csSelf.postProcessingProfile = profile
            else
                csSelf.postProcessingProfileSub = profile
            end
        end
    )
end

function IDLCameraMgr.getProfile(name, callback)
    local profile = profiles[name]
    if profile == nil then
        local path = IDLCameraMgr.wrapPath(name)
        CLVerManager.self:getNewestRes(
            path,
            CLAssetType.assetBundle,
            function(_name, assets, orgs)
                if assets then
                    -- profile = assets.mainAsset
                    profile = assets:LoadAsset(name, typeof(CS.UnityEngine.Rendering.PostProcessing.PostProcessProfile))
                    profileAssets[name] = assets
                    profiles[name] = profile
                    if callback then
                        callback(profile)
                    end
                end
            end,
            false
        )
    else
        if callback then
            callback(profile)
        end
    end
end

function IDLCameraMgr.clean()
    for k, v in pairs(profileAssets) do
        v:Unload(true)
    end
    profileAssets = {}
    profiles = {}
end

function IDLCameraMgr.wrapPath(name)
    local path = joinStr(CLPathCfg.self.basePath, "/", CLPathCfg.upgradeRes, "/other/things/postprocessing")
    return CLThingsPool.wrapPath(path, name)
end

function IDLCameraMgr.isInCameraView(bounds, camera)
    camera = camera or csSelf.maincamera
    return CameraMgr.isInCameraView(camera, bounds)
end

--------------------------------------------
return IDLCameraMgr
