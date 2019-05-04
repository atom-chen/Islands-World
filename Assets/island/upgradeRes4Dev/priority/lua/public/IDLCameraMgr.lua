-- 界面
IDLCameraMgr = {}
IDLCameraMgr.smoothFollow = nil

---@type CameraMgr
local csSelf = CameraMgr.self

local profiles = {}

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
                    profile = assets.mainAsset
                    profiles[name] = assets
                    if callback then
                        callback(profile)
                    end
                end
            end,
            false
        )
    else
        if callback then
            callback(profile.mainAsset)
        end
    end
end

function IDLCameraMgr.clean()
    for k, v in pairs(profiles) do
        v:Unload(true)
    end
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
