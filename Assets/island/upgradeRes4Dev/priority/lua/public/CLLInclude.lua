-- 需要先加载的部分
do
    -------------------------------------------------------
    -- 重写require
    local localReq = require
    function require(path)
        local ret, result = pcall(localReq, path)
        --("toolkit.KKWhiteList")
        if not ret then
            print("err:" .. result)
            return nil
        end
        return result
    end
    -------------------------------------------------------
    -- 重新命名
    ---@type UnityEngine.Vector2
    Vector2 = CS.UnityEngine.Vector2
    ---@type UnityEngine.Vector3
    Vector3 = CS.UnityEngine.Vector3
    ---@type UnityEngine.Vector4
    Vector4 = CS.UnityEngine.Vector4
    ---@type UnityEngine.Rect
    Rect = CS.UnityEngine.Rect
    ---@type UnityEngine.Color
    Color = CS.UnityEngine.Color
    ---@type UnityEngine.Ray
    Ray = CS.UnityEngine.Ray
    ---@type UnityEngine.Ray2D
    Ray2D = CS.UnityEngine.Ray2D
    ---@type UnityEngine.Bounds
    Bounds = CS.UnityEngine.Bounds
    ---@type UnityEngine.Time
    Time = CS.UnityEngine.Time
    ---@type UnityEngine.GameObject
    GameObject = CS.UnityEngine.GameObject
    ---@type UnityEngine.Component
    Component = CS.UnityEngine.Component
    ---@type UnityEngine.Behaviour
    Behaviour = CS.UnityEngine.Behaviour
    ---@type UnityEngine.Transform
    Transform = CS.UnityEngine.Transform
    ---@type UnityEngine.Resources
    Resources = CS.UnityEngine.Resources
    ---@type UnityEngine.TextAsset
    TextAsset = CS.UnityEngine.TextAsset
    ---@type UnityEngine.AnimationCurve
    AnimationCurve = CS.UnityEngine.AnimationCurve
    ---@type UnityEngine.AnimationClip
    AnimationClip = CS.UnityEngine.AnimationClip
    ---@type UnityEngine.MonoBehaviour
    MonoBehaviour = CS.UnityEngine.MonoBehaviour
    ---@type UnityEngine.ParticleSystem
    ParticleSystem = CS.UnityEngine.ParticleSystem
    ---@type UnityEngine.SkinnedMeshRenderer
    SkinnedMeshRenderer = CS.UnityEngine.SkinnedMeshRenderer
    ---@type UnityEngine.MeshRenderer
    MeshRenderer = CS.UnityEngine.MeshRenderer
    ---@type UnityEngine.Renderer
    Renderer = CS.UnityEngine.Renderer
    ---@type UnityEngine.WWW
    WWW = CS.UnityEngine.WWW
    ---@type UnityEngine.Screen
    Screen = CS.UnityEngine.Screen
    ---@type System.Collections.Hashtable
    Hashtable = CS.System.Collections.Hashtable
    ---@type System.Collections.ArrayList
    ArrayList = CS.System.Collections.ArrayList
    ---@type System.Collections.Queue
    Queue = CS.System.Collections.Queue
    ---@type System.Collections.Stack
    Stack = CS.System.Collections.Stack
    ---@type System.GC
    GC = CS.System.GC
    ---@type System.IO.File
    File = CS.System.IO.File
    ---@type System.IO.Directory
    Directory = CS.System.IO.Directory
    ---@type UnityEngine.Application
    Application = CS.UnityEngine.Application
    ---@type UnityEngine.RaycastHit
    RaycastHit = CS.UnityEngine.RaycastHit
    ---@type UnityEngine.PlayerPrefs
    PlayerPrefs = CS.UnityEngine.PlayerPrefs
    ---@type UnityEngine.SystemInfo
    SystemInfo = CS.UnityEngine.SystemInfo
    ---@type UnityEngine.Shader
    Shader = CS.UnityEngine.Shader
    ---@type System.IO.Path
    Path = CS.System.IO.Path
    ---@type System.IO.MemoryStream
    MemoryStream = CS.System.IO.MemoryStream
    ---@type UnityEngine.LayerMask
    LayerMask = CS.UnityEngine.LayerMask
    ---@type UnityEngine.BoxCollider
    BoxCollider = CS.UnityEngine.LayerMask
    ---@type UnityEngine.QualitySettings
    QualitySettings = CS.UnityEngine.QualitySettings
    ---@type UnityEngine.AudioSource
    AudioSource = CS.UnityEngine.AudioSource
    ---@type UnityEngine.Physics
    Physics = CS.UnityEngine.Physics

    ---@type UICamera
    UICamera = CS.UICamera
    ---@type Localization
    Localization = CS.Localization
    ---@type NGUITools
    NGUITools = CS.NGUITools
    ---@type UIRect
    UIRect = CS.UIRect
    ---@type UIWidget
    UIWidget = CS.UIWidget
    ---@type UIWidgetContainer
    UIWidgetContainer = CS.UIWidgetContainer
    ---@type UILabel
    UILabel = CS.UILabel
    ---@type UIToggle
    UIToggle = CS.UIToggle
    ---@type UIBasicSprite
    UIBasicSprite = CS.UIBasicSprite
    ---@type UITexture
    UITexture = CS.UITexture
    ---@type UISprite
    UISprite = CS.UISprite
    ---@type UIProgressBar
    UIProgressBar = CS.UIProgressBar
    ---@type UISlider
    UISlider = CS.UISlider
    ---@type UIGrid
    UIGrid = CS.UIGrid
    ---@type UITable
    UITable = CS.UITable
    ---@type UIInput
    UIInput = CS.UIInput
    ---@type UIScrollView
    UIScrollView = CS.UIScrollView
    ---@type UITweener
    UITweener = CS.UITweener
    ---@type TweenWidth
    TweenWidth = CS.TweenWidth
    ---@type TweenRotation
    TweenRotation = CS.TweenRotation
    ---@type TweenPosition
    TweenPosition = CS.TweenPosition
    ---@type TweenScale
    TweenScale = CS.TweenScale
    ---@type TweenAlpha
    TweenAlpha = CS.TweenAlpha
    ---@type UICenterOnChild
    UICenterOnChild = CS.UICenterOnChild
    ---@type UIAtlas
    UIAtlas = CS.UIAtlas
    ---@type UILocalize
    UILocalize = CS.UILocalize
    ---@type UIPlayTween
    UIPlayTween = CS.UIPlayTween
    ---@type UIFollowTarget
    UIFollowTarget = CS.UIFollowTarget
    ---@type UIFollowTarget
    HUDRoot = CS.HUDRoot
    ---@type UIRichText4Chat
    UIRichText4Chat = CS.UIRichText4Chat
    ---@type UIAnchor
    UIAnchor = CS.UIAnchor
    ---@type UIPanel
    UIPanel = CS.UIPanel

    ---@type Coolape.CLAssetsManager
    CLAssetsManager = CS.Coolape.CLAssetsManager
    ---@type Coolape.B2InputStream
    B2InputStream = CS.Coolape.B2InputStream
    ---@type Coolape.B2OutputStream
    B2OutputStream = CS.Coolape.B2OutputStream
    ---@type Coolape.SoundEx
    SoundEx = CS.Coolape.SoundEx
    ---@type Coolape.InvokeEx
    InvokeEx = CS.Coolape.InvokeEx
    ---@type Coolape.CLAssetsPoolBase
    CLAssetsPoolBase = CS.Coolape.CLAssetsPoolBase
    ---@type Coolape.CLBulletBase
    CLBulletBase = CS.Coolape.CLBulletBase
    ---@type Coolape.CLBulletPool
    CLBulletPool = CS.Coolape.CLBulletPool
    ---@type Coolape.CLEffect
    CLEffect = CS.Coolape.CLEffect
    ---@type Coolape.CLEffectPool
    CLEffectPool = CS.Coolape.CLEffectPool
    ---@type Coolape.CLMaterialPool
    CLMaterialPool = CS.Coolape.CLMaterialPool
    ---@type Coolape.CLRolePool
    CLRolePool = CS.Coolape.CLRolePool
    ---@type Coolape.CLSoundPool
    CLSoundPool = CS.Coolape.CLSoundPool
    ---@type Coolape.CLSharedAssets
    CLSharedAssets = CS.Coolape.CLSharedAssets
    ---@type Coolape.CLSharedAssets.CLMaterialInfor
    CLMaterialInfor = CS.Coolape.CLSharedAssets.CLMaterialInfor
    ---@type Coolape.CLTexturePool
    CLTexturePool = CS.Coolape.CLTexturePool
    ---@type Coolape.CLThingsPool
    CLThingsPool = CS.Coolape.CLThingsPool
    ---@type Coolape.CLThings4LuaPool
    CLThings4LuaPool = CS.Coolape.CLThings4LuaPool
    ---@type Coolape.CLBaseLua
    CLBaseLua = CS.Coolape.CLBaseLua
    ---@type Coolape.CLBehaviour4Lua
    CLBehaviour4Lua = CS.Coolape.CLBehaviour4Lua
    ---@type Coolape.CLUtlLua
    CLUtlLua = CS.Coolape.CLUtlLua
    ---@type Coolape.CLMainBase
    CLMainBase = CS.Coolape.CLMainBase
    ---@type Coolape.Net
    Net = CS.Coolape.Net
    ---@type Coolape.Net.NetWorkType
    NetWorkType = CS.Coolape.Net.NetWorkType
    ---@type Coolape.CLCfgBase
    CLCfgBase = CS.Coolape.CLCfgBase
    ---@type Coolape.CLPathCfg
    CLPathCfg = CS.Coolape.CLPathCfg
    ---@type Coolape.CLVerManager
    CLVerManager = CS.Coolape.CLVerManager
    ---@type Coolape.CLAssetType
    CLAssetType = CS.Coolape.CLAssetType
    ---@type Coolape.CLRoleAction
    CLRoleAction = CS.Coolape.CLRoleAction
    ---@type Coolape.CLRoleAvata
    CLRoleAvata = CS.Coolape.CLRoleAvata
    ---@type Coolape.CLUnit
    CLUnit = CS.Coolape.CLUnit
    ---@type Coolape.BlockWordsTrie
    BlockWordsTrie = CS.Coolape.BlockWordsTrie
    ---@type Coolape.ColorEx
    ColorEx = CS.Coolape.ColorEx
    ---@type Coolape.DateEx
    DateEx = CS.Coolape.DateEx
    ---@type Coolape.FileEx
    FileEx = CS.Coolape.FileEx
    ---@type Coolape.JSON
    JSON = CS.Coolape.JSON
    ---@type Coolape.ListEx
    ListEx = CS.Coolape.ListEx
    ---@type Coolape.MapEx
    MapEx = CS.Coolape.MapEx
    ---@type Coolape.MyMainCamera
    MyMainCamera = CS.Coolape.MyMainCamera
    ---@type Coolape.MyTween
    MyTween = CS.Coolape.MyTween
    ---@type Coolape.NewList
    NewList = CS.Coolape.NewList
    ---@type Coolape.NewMap
    NewMap = CS.Coolape.NewMap
    ---@type Coolape.SoundEx
    SoundEx = CS.Coolape.SoundEx
    ---@type Coolape.NumEx
    NumEx = CS.Coolape.NumEx
    ---@type Coolape.ObjPool
    ObjPool = CS.Coolape.ObjPool
    ---@type Coolape.PStr
    PStr = CS.Coolape.PStr
    ---@type Coolape.SScreenShakes
    SScreenShakes = CS.Coolape.SScreenShakes
    ---@type Coolape.StrEx
    StrEx = CS.Coolape.StrEx
    ---@type Coolape.Utl
    Utl = CS.Coolape.Utl
    ---@type Coolape.WWWEx
    WWWEx = CS.Coolape.WWWEx
    ---@type Coolape.ZipEx
    ZipEx = CS.Coolape.ZipEx
    ---@type Coolape.XXTEA
    XXTEA = CS.Coolape.XXTEA
    ---@type Coolape.CLButtonMsgLua
    CLButtonMsgLua = CS.Coolape.CLButtonMsgLua
    ---@type Coolape.CLJoystick
    CLJoystick = CS.Coolape.CLJoystick
    ---@type Coolape.CLUIDrag4World
    CLUIDrag4World = CS.Coolape.CLUIDrag4World
    ---@type Coolape.CLUILoopGrid
    CLUILoopGrid = CS.Coolape.CLUILoopGrid
    ---@type Coolape.CLUILoopTable
    CLUILoopTable = CS.Coolape.CLUILoopTable
    ---@type Coolape.TweenSpriteFill
    TweenSpriteFill = CS.Coolape.TweenSpriteFill
    ---@type Coolape.UIDragPage4Lua
    UIDragPage4Lua = CS.Coolape.UIDragPage4Lua
    ---@type Coolape.UIDragPageContents
    UIDragPageContents = CS.Coolape.UIDragPageContents
    ---@type Coolape.UIGridPage
    UIGridPage = CS.Coolape.UIGridPage
    ---@type Coolape.UIMoveToCell
    UIMoveToCell = CS.Coolape.UIMoveToCell
    ---@type Coolape.UISlicedSprite
    UISlicedSprite = CS.Coolape.UISlicedSprite
    ---@type Coolape.CLAlert
    CLAlert = CS.Coolape.CLAlert
    ---@type Coolape.CLCellBase
    CLCellBase = CS.Coolape.CLCellBase
    ---@type Coolape.CLCellLua
    CLCellLua = CS.Coolape.CLCellLua
    ---@type Coolape.CLPanelBase
    CLPanelBase = CS.Coolape.CLPanelBase
    ---@type Coolape.CLPanelLua
    CLPanelLua = CS.Coolape.CLPanelLua
    ---@type Coolape.CLPanelManager
    CLPanelManager = CS.Coolape.CLPanelManager
    ---@type Coolape.CLUIInit
    CLUIInit = CS.Coolape.CLUIInit
    ---@type Coolape.CLUIOtherObjPool
    CLUIOtherObjPool = CS.Coolape.CLUIOtherObjPool
    ---@type Coolape.CLUIRenderQueue
    CLUIRenderQueue = CS.Coolape.CLUIRenderQueue
    ---@type Coolape.CLUIUtl
    CLUIUtl = CS.Coolape.CLUIUtl
    ---@type Coolape.EffectNum
    EffectNum = CS.Coolape.EffectNum
    ---@type Coolape.TweenProgress
    TweenProgress = CS.Coolape.TweenProgress
    ---@type Coolape.B2Int
    B2Int = CS.Coolape.B2Int
    ---@type Coolape.AngleEx
    AngleEx = CS.Coolape.AngleEx
    ---@type Coolape.CLGridPoints
    CLGridPoints = CS.Coolape.CLGridPoints
    ---@type Coolape.ReporterMessageReceiver
    ReporterMessageReceiver = CS.ReporterMessageReceiver
    ---@type Coolape.CLTweenColor
    CLTweenColor = CS.Coolape.CLTweenColor
    ---@type Coolape.CLAStarPathSearch
    CLAStarPathSearch = CS.Coolape.CLAStarPathSearch
    ---@type Coolape.CLSeeker
    CLSeeker = CS.Coolape.CLSeeker
    ---@type Coolape.CLSeekerByRay
    CLSeekerByRay = CS.Coolape.CLSeekerByRay
    ---@type Coolape.CLSmoothFollow
    CLSmoothFollow = CS.Coolape.CLSmoothFollow
    ---@type Coolape.uvAn
    uvAn = CS.Coolape.uvAn
    ---@type Coolape.CLEjector
    CLEjector = CS.Coolape.CLEjector
    -------------------------------------------------------
    --
    ---@type MyMain
    MyMain = CS.MyMain
    ---@type MyCfg
    MyCfg = CS.MyCfg
    ---@type MirrorReflection
    MirrorReflection = CS.MirrorReflection

    ---@type CLGrid
    CLGrid = CS.CLGrid
    ---@type Coolape.GridBase
    GridBase = CS.Coolape.GridBase

    ---@type MyUnit
    MyUnit = CS.MyUnit
    ---@type SFourWayArrow
    SFourWayArrow = CS.SFourWayArrow
    ---@type MyUtl
    MyUtl = CS.MyUtl

    ---@type CameraMgr
    CameraMgr = CS.CameraMgr
    ScriptableObject = CS.UnityEngine.ScriptableObject
    --PostProcessingBehaviour = CS.UnityEngine.PostProcessing.PostProcessingBehaviour
    PostProcessVolume = CS.UnityEngine.Rendering.PostProcessing.PostProcessingProfile

    ---@type MyUIPanel
    MyUIPanel = CS.MyUIPanel

    ---@type FogOfWarSystem
    FogOfWarSystem = CS.SimpleFogOfWar.FogOfWarSystem
    ---@type SimpleFogOfWar.FogOfWarInfluence
    FogOfWarInfluence = CS.SimpleFogOfWar.FogOfWarInfluence

    ---@type ShipTrail
    ShipTrail = CS.ShipTrail
    ---@type HUDText
    HUDText = CS.HUDText
    ---@type MyBoundsPool
    MyBoundsPool = CS.MyBoundsPool
    -------------------------------------------------------
    -- require
    require("toolkit.CLLPrintEx")
    require("toolkit.LuaUtl")
    require("public.class")
    require("bio.BioUtl")
    require("public.CLLPrefs")
    require("toolkit.CLLUpdateUpgrader")
    require("toolkit.CLLVerManager")
    require("toolkit.BitUtl")
    ---@type CLQuickSort
    CLQuickSort = require("toolkit.CLQuickSort")
    -------------------------------------------------------
    -- 全局变量
    __version__ = Application.version -- "1.0"
    __UUID__ = ""
    -------------------------------------------------------
    --bio2Int = NumEx.bio2Int
    --int2Bio = NumEx.int2Bio
    --bio2Long = NumEx.bio2Long
    --long2Bio = NumEx.Long2Bio
    bio2Int = BioUtl.bio2int
    int2Bio = BioUtl.int2bio
    bio2Long = BioUtl.bio2long
    long2Bio = BioUtl.long2bio
    bio2number = BioUtl.bio2number
    number2bio = BioUtl.number2bio
    LGet = Localization.Get
    hideTopPanel = CLPanelManager.hideTopPanel
    getPanelAsy = CLPanelManager.getPanelAsy
    NetSuccess = Net.SuccessCode
    -------------------------------------------------------

    -- 模式
    GameMode = {
        none = 0,
        city = 1,
        map = 2,
        battle = 3
    }
    -- 子模式
    GameModeSub = {
        none = 0,
        map = 1,
        city = 2,
        mapBtwncity = 4 -- 地图与主城之前切换
    }

    ---@class RoleState 角色的状态
    RoleState = {
        walkAround = 1,
        idel = 2,
        beakBack = 3,
        searchTarget = 4,
        attack = 5,
        waitAttack = 6,
        dizzy = 7,
        backDockyard = 8, -- 返回造船厂
        landing = 9 -- 正在登陆
    }

    -- 重载pcall，以便可以自动print error msg
    local _pcall = pcall
    function pcall(func, ...)
        local ret, result = _pcall(func, ...)
        if not ret then
            printe(result)
        end
        return ret, result
    end
    -------------------------------------------------------
    local chnCfg  -- 安装包配置
    function getChlCfg()
        if chnCfg ~= nil then
            return chnCfg
        end
        if not CLCfgBase.self.isEditMode then
            local fpath = "chnCfg.json" -- 该文在打包时会自动放在streamingAssetsPath目录下，详细参见打包工具
            local content = FileEx.readNewAllText(fpath)
            if (content ~= nil) then
                chnCfg = JSON.DecodeMap(content)
            end
        end
        return chnCfg
    end

    -- 取得渠道代码
    function getChlCode()
        if chnCfg ~= nil then
            return MapEx.getString(chnCfg, "SubChannel")
        end
        local chlCode = "0000"
        chnCfg = getChlCfg()
        if (chnCfg ~= nil) then
            chlCode = MapEx.getString(chnCfg, "SubChannel")
        end
        return chlCode
    end
    -------------------------------------------------------
end
