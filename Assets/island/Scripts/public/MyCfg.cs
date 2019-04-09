using UnityEngine;
using System.Collections;
using Coolape;

public class MyCfg : CLCfgBase
{
	public static int mode = 0;
	public string default_UID = "";
	public Transform lookAtTarget;
	public Light directionalLight;
	public Camera mainCamera;
	public Camera uiCamera;
    public Transform hud3dRoot;
    public Transform shadowRoot;
    public SimpleFogOfWar.FogOfWarSystem fogOfWar;

    public bool _isEditScene = false;
    public bool isEditScene
    {
        get
        {
#if UNITY_EDITOR
            return _isEditScene;
#endif
            return false;
        }
    }
}
