using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EIslandMenus
{

    [MenuItem("Islands/Generate worldmap data", false, 1)]
    public static void showGenerateWorldmapDataWind()
    {
        EditorWindow.GetWindow<EWorldmapCfgDataGneratorWind>(false, "Generate worldmap data", true);
    }
}
