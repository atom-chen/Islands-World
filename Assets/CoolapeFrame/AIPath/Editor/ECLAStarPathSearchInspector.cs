using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Coolape;

[CanEditMultipleObjects]
[CustomEditor(typeof(CLAStarPathSearch), true)]
public class ECLAStarPathSearchInspector : Editor
{
    CLAStarPathSearch instance;

    List<Vector3> path = null;
    public override void OnInspectorGUI()
    {
        instance = target as CLAStarPathSearch;
        //DrawDefaultInspector();
        ECLEditorUtl.BeginContents();
        {
            EditorGUILayout.HelpBox(
                "简单的A星寻路，目前只支持y轴朝上的情况，不支持有高度的寻路。\n" +
                "主要方法：\n" +
                "  CLAStarPathSearch.current. //当前A星的实例\n" +
                "  .init();//初始化网格\n" +
                "  .scan();//扫描网格，就是设置哪些格子是不可通过的\n" +
                "  .scanRange(center, r);//刷新坐标center，半径为r的网格的障碍状态\n" +
                "  .searchPath(from, to, out vectorList);//寻路"
                , MessageType.None, true);

        }
        ECLEditorUtl.EndContents();

        ECLEditorUtl.BeginContents();
        {
            GUI.color = Color.yellow;
            GUILayout.Label("*鼠标悬停在字段上有解释");
            GUI.color = Color.white;

            GUIContent uicontnt = null;
            uicontnt = new GUIContent("Row", "行（网格的长）");
            instance.numRows = EditorGUILayout.IntField(uicontnt, instance.numRows);
            uicontnt = new GUIContent("Column", "列（网格的宽）");
            instance.numCols = EditorGUILayout.IntField(uicontnt, instance.numCols);
            uicontnt = new GUIContent("Cell Size", "网格单元的大小");
            instance.cellSize = EditorGUILayout.FloatField(uicontnt, instance.cellSize);
            uicontnt = new GUIContent("Neighbours", "一个单元格的寻路方向数量。(4方向/8方向)");
            instance.numNeighbours = (CLAStarPathSearch.NumNeighbours)(EditorGUILayout.EnumPopup(uicontnt, instance.numNeighbours));
            uicontnt = new GUIContent("Scan Type", "扫描类型.ObstructNode：扫描障碍层设置不可通行的格子；PassableNode:扫描设置可通行的节点；");
            instance.scanType = (CLAStarPathSearch.ScanType)EditorGUILayout.EnumPopup(uicontnt, instance.scanType);
            //if (instance.scanType == CLAStarPathSearch.ScanType.PassableNode)
            //{
            //    GUI.enabled = false;
            //}
            uicontnt = new GUIContent("Obstruct Mask", "障碍物的layer");
            instance.obstructMask = ECLEditorUtl.drawMaskField(uicontnt, instance.obstructMask);

            GUI.enabled = true;
            if (instance.scanType == CLAStarPathSearch.ScanType.ObstructNode)
            {
                GUI.enabled = false;
            }
            uicontnt = new GUIContent("Passable Mask", "可通行节点的layer");
            instance.passableMask = ECLEditorUtl.drawMaskField(uicontnt, instance.passableMask);
            GUI.enabled = true;

            uicontnt = new GUIContent("Scan Ray Direction", "射线检测障碍的发射方向");
            instance.rayDirection = (CLAStarPathSearch.RayDirection)(EditorGUILayout.EnumPopup(uicontnt, instance.rayDirection));
            uicontnt = new GUIContent("Scan Ray Distance", "射线检测障碍的和射线长度");
            instance.rayDis4Scan = EditorGUILayout.FloatField(uicontnt, instance.rayDis4Scan);
            uicontnt = new GUIContent("Auto Scan", "自动扫描网格的障碍，其实就是在Start方法中自动调用init和Scan方法");
            instance.isAutoScan = EditorGUILayout.Toggle(uicontnt, instance.isAutoScan);
            uicontnt = new GUIContent("Need Cache Paths", "把寻路得到的数据缓存起来，可以重复使用");
            instance.needCachePaths = EditorGUILayout.Toggle(uicontnt, instance.needCachePaths);
            ECLEditorUtl.BeginContents();
            {
                uicontnt = new GUIContent("Filter Path By Ray", "通过射线检测障碍来过滤掉冗余的点，因此要注意障碍物的collider的高度及大小，以免射线检测不到");
                instance.isFilterPathByRay = EditorGUILayout.Toggle(uicontnt, instance.isFilterPathByRay);

                uicontnt = new GUIContent("Soften Path", "柔化路径");
                instance.isSoftenPath = EditorGUILayout.Toggle(uicontnt, instance.isSoftenPath);
                if (instance.isSoftenPath)
                {
                    uicontnt = new GUIContent("Soften Type", "柔化路径的方式，line:直接把路径分段的接点处分成两个点进行柔；sler:在路径分段的接点处加入曲面插值");
                    instance.softenPathType = (CLAIPathUtl.SoftenPathType)EditorGUILayout.EnumPopup(uicontnt, instance.softenPathType);
                    if (instance.softenPathType == CLAIPathUtl.SoftenPathType.Slerp)
                    {
                        uicontnt = new GUIContent("Soften Slerp Factor", "曲面插值的个数，【注意】不能太大");
                        instance.softenFactor = EditorGUILayout.IntField(uicontnt, instance.softenFactor);
                    }
                }
            }
            ECLEditorUtl.EndContents();
            uicontnt = new GUIContent("Debug Show Grid", "显示网格");
            instance.showGrid = EditorGUILayout.Toggle(uicontnt, instance.showGrid);
            uicontnt = new GUIContent("Debug Show Obstruct", "显示网格障碍");
            instance.showObstruct = EditorGUILayout.Toggle(uicontnt, instance.showObstruct);
        }
        ECLEditorUtl.EndContents();

        ECLEditorUtl.BeginContents();
        {
            if (GUILayout.Button("init"))
            {
                instance.init();
            }
            if (GUILayout.Button("Scan"))
            {
                instance.scan();
            }
        }
        ECLEditorUtl.EndContents();
        EditorUtility.SetDirty(target);
    }
}
