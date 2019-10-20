//配置的详细介绍请看Doc下《XLua的配置.doc》
using System;
using UnityEngine;
using XLua;
using Coolape;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public static class XluaGenCodeConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static System.Collections.Generic.List<Type> LuaCallCSharp = new System.Collections.Generic.List<Type>() {
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Rect),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Ray2D),
        typeof(Bounds),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(MeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(System.Collections.Generic.List<int>),
        typeof(Action<string>),
        typeof(UnityEngine.Debug),
        typeof(Hashtable),
        typeof(ArrayList),
        typeof(Queue),
        typeof(Stack),
        typeof(GC),
        typeof(File),
        typeof(Directory),
        typeof(Application),
        typeof(SystemInfo),
        typeof(RaycastHit),
        typeof(System.IO.Path),
        typeof(System.IO.MemoryStream),
        typeof(Screen),
        typeof(PlayerPrefs),
        typeof(Shader),
        typeof(LayerMask),
        typeof(BoxCollider),
        typeof(QualitySettings),
        typeof(AudioSource),

		//NGUI
		typeof(UICamera),
        typeof(Localization),
        typeof(NGUITools),
        typeof(UIRect),
        typeof(UIWidget),
        typeof(UIWidgetContainer),
        typeof(UILabel),
        typeof(UIToggle),
        typeof(UIBasicSprite),
        typeof(UITexture),
        typeof(UISprite),
        typeof(UIProgressBar),
        typeof(UISlider),
        typeof(UIGrid),
        typeof(UITable),
        typeof(UIInput),
        typeof(UIScrollView),
        typeof(UITweener),
        typeof(TweenWidth),
        typeof(TweenRotation),
        typeof(TweenPosition),
        typeof(TweenScale),
        typeof(TweenAlpha),
        typeof(UICenterOnChild),
        typeof(UIAtlas),
        typeof(UILocalize),
        typeof(UIPlayTween),
        typeof(UIRect.AnchorPoint),
        typeof(UIFollowTarget),
        typeof(HUDRoot),
        typeof(UIRichText4Chat),
        typeof(UIAnchor),

		//Coolape
		typeof(CLAssetsManager),
        typeof(B2InputStream),
        typeof(B2OutputStream),
        typeof(CLBulletBase),
        typeof(CLBulletPool),
        typeof(CLEffect),
        typeof(CLEffectPool),
        typeof(CLMaterialPool),
        typeof(CLRolePool),
        typeof(CLSoundPool),
        typeof(CLSharedAssets),
        typeof(CLSharedAssets.CLMaterialInfor),
        typeof(CLTexturePool),
        typeof(CLThingsPool),
        typeof(CLThings4LuaPool),
        typeof(CLBaseLua),
        typeof(CLBehaviour4Lua),
        typeof(CLUtlLua),
        typeof(CLMainBase),
        typeof(Net),
        typeof(Net.NetWorkType),
        typeof(CLCfgBase),
        typeof(CLPathCfg),
        typeof(CLVerManager),
        typeof(CLAssetType),
        typeof(CLRoleAction),
        typeof(CLRoleAvata),
        typeof(CLUnit),

        typeof(ColorEx),
        typeof(BlockWordsTrie),
        typeof(DateEx),
        typeof(FileEx),
        typeof(JSON),
        typeof(ListEx),
        typeof(MapEx),
        typeof(MyMainCamera),
        typeof(MyTween),
        typeof(NewList),
        typeof(NewMap),
        typeof(SoundEx),
        typeof(NumEx),
        typeof(ObjPool),
        typeof(PStr),
        typeof(SScreenShakes),
        typeof(StrEx),
        typeof(Utl),
        typeof(WWWEx),
        typeof(ZipEx),
        typeof(XXTEA),

        typeof(CLButtonMsgLua),
        typeof(CLJoystick),
        typeof(CLUIDrag4World),
        typeof(CLAssetsPoolBase<UnityEngine.Object>),
        typeof(CLUILoopGrid),
        typeof(CLUILoopTable),
//		typeof(CLUILoopGrid2),
		typeof(CLUIPlaySound),
        typeof(TweenSpriteFill),
        typeof(UIDragPage4Lua),
        typeof(UIDragPageContents),
        typeof(UIGridPage),
        typeof(UIMoveToCell),
        typeof(UISlicedSprite),

        typeof(CLAlert),
        typeof(CLCellBase),
        typeof(CLCellLua),
        typeof(CLPanelBase),
        typeof(CLPanelLua),
        typeof(CLPanelManager),
        typeof(CLPanelMask4Panel),
        typeof(CLPBackplate),
        typeof(CLUIInit),
        typeof(CLUIOtherObjPool),
        typeof(CLUIRenderQueue),
        typeof(CLUIUtl),
        typeof(EffectNum),
        typeof(TweenProgress),
        typeof(B2Int),
        typeof(AngleEx),
        typeof(CLGridPoints),
        typeof(CLTweenColor),
        typeof(CLAStarPathSearch),
        typeof(CLSeeker),
        typeof(CLSeekerByRay),
        typeof(InvokeEx),
        typeof(CLEjector),

		//==========================
		typeof(MirrorReflection),
        typeof(MyMain),
        typeof(MyCfg),
        typeof(CLGrid),
        typeof(Coolape.GridBase),
        typeof(CLSmoothFollow),

        typeof(MyUnit),
        typeof(SFourWayArrow),
        typeof(CameraMgr),
        typeof(ScriptableObject),
        typeof(UnityEngine.Rendering.PostProcessing.PostProcessLayer),
        typeof(UnityEngine.Rendering.PostProcessing.PostProcessVolume),
        typeof(MyUIPanel),
        typeof(SimpleFogOfWar.FogOfWarSystem),
        typeof(SimpleFogOfWar.FogOfWarInfluence),
        typeof(MyUtl),
        typeof(HUDText),
    };

	//C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
	[CSharpCallLua]
	public static System.Collections.Generic.List<Type> CSharpCallLua = new System.Collections.Generic.List<Type>() {
		typeof(Action),
		typeof(Func<double, double, double>),
		typeof(Action<string>),
		typeof(Action<double>),
		typeof(UnityEngine.Events.UnityAction),
		typeof(Coolape.Callback),
	};

	//黑名单
	[BlackList]
	public static List<List<string>> BlackList = new List<List<string>>() {
		new List<string>(){ "UnityEngine.WWW", "movie" },
		new List<string>(){ "UnityEngine.Texture2D", "alphaIsTransparency" },
		new List<string>(){ "UnityEngine.Security", "GetChainOfTrustValue" },
		new List<string>(){ "UnityEngine.CanvasRenderer", "onRequestRebuild" },
		new List<string>(){ "UnityEngine.Light", "areaSize" },
		new List<string>(){ "UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup" },
    #if !UNITY_WEBPLAYER
		new List<string>(){ "UnityEngine.Application", "ExternalEval" },
    #endif
		new List<string>(){ "UnityEngine.GameObject", "networkView" }, //4.6.2 not support
		new List<string>(){ "UnityEngine.Component", "networkView" },  //4.6.2 not support
		new List<string>() {
			"System.IO.FileInfo",
			"GetAccessControl",
			"System.Security.AccessControl.AccessControlSections"
		},
		new List<string>() {
			"System.IO.FileInfo",
			"SetAccessControl",
			"System.Security.AccessControl.FileSecurity"
		},
		new List<string>() {
			"System.IO.DirectoryInfo",
			"GetAccessControl",
			"System.Security.AccessControl.AccessControlSections"
		},
		new List<string>() {
			"System.IO.DirectoryInfo",
			"SetAccessControl",
			"System.Security.AccessControl.DirectorySecurity"
		},
		new List<string>() {
			"System.IO.DirectoryInfo",
			"CreateSubdirectory",
			"System.String",
			"System.Security.AccessControl.DirectorySecurity"
		},
		new List<string>() {
			"System.IO.DirectoryInfo",
			"Create",
			"System.Security.AccessControl.DirectorySecurity"
		},
		new List<string>() {
			"System.IO.Directory",
			"CreateDirectory",
			"System.String",
			"System.Security.AccessControl.DirectorySecurity"
		},
		new List<string>() {
			"System.IO.Directory",
			"SetAccessControl",
			"System.String",
			"System.Security.AccessControl.DirectorySecurity"
		},
		new List<string>() {
			"System.IO.Directory",
			"GetAccessControl",
			"System.String"
		},
		new List<string>() {
			"System.IO.Directory",
			"GetAccessControl",
			"System.String",
			"System.Security.AccessControl.AccessControlSections"
		},
		new List<string>() {
			"System.IO.File",
			"Create",
			"System.String",
			"System.Int32",
			"System.IO.FileOptions"
		},
		new List<string>() {
			"System.IO.File",
			"Create",
			"System.String",
			"System.Int32",
			"System.IO.FileOptions",
			"System.Security.AccessControl.FileSecurity"
		},
		new List<string>() {
			"System.IO.File",
			"GetAccessControl",
			"System.String",
		},
		new List<string>() {
			"System.IO.File",
			"GetAccessControl",
			"System.String",
			"System.Security.AccessControl.AccessControlSections",
		},
		new List<string>() {
			"System.IO.File",
			"SetAccessControl",
			"System.String",
			"System.Security.AccessControl.FileSecurity",
		},
		new List<string>() {
			"Coolape.CLUnit",
			"OnDrawGizmos",
		},
		#if UNITY_ANDROID || UNITY_IOS
		new List<string>() {
			"UIInput",
			"ProcessEvent",
			"UnityEngine.Event",
		},
		#endif
		new List<string>() {
			"UIWidget",
			"showHandlesWithMoveTool",
		},
		new List<string>() {
			"UIWidget",
			"showHandles",
		},
		
		new List<string>() {
			"Coolape.PStr",
			"a",
			"System.Byte",
		},

		new List<string>() {
			"Coolape.PStr",
			"a",
			"System.Byte[]",
		},

		new List<string>() {
			"UnityEngine.MonoBehaviour",
			"runInEditMode",
		},

		new List<string>() {
			"Coolape.CLAssetsManager",
			"debugKey",
		},	
		new List<string>() {
			"MyCfg",
			"default_UID",
		},
		new List<string>() {
			"UnityEngine.PostProcessing.PostProcessingProfile",
			"monitors",
		},

		new List<string>() {
			"UnityEngine.WWW",
			"GetMovieTexture",
		},
        new List<string>() {
            "UnityEngine.QualitySettings",
            "streamingMipmapsRenderersPerFrame",
        },
    };
}
