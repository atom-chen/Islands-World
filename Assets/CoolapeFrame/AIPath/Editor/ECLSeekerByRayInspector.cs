using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;

[CanEditMultipleObjects]
[CustomEditor(typeof(CLSeekerByRay), true)]
public class ECLSeekerByRayInspector : Editor
{
    CLSeekerByRay instance;
    public override void OnInspectorGUI()
    {
        instance = target as CLSeekerByRay;
        //DrawDefaultInspector();
        ECLEditorUtl.BeginContents();
        {
            EditorGUILayout.HelpBox(
                "通过射线寻路\n" +
                "【注意注意注意】\n" +
                "  1.不需要A星；\n" +
                "  2.只能处理简单的寻路，最好是每个目标点都是可到达的；\n" +
                "  3.如果某个点不能到达，则肯定会寻设置的最大的寻路次数；\n" +
                "主要方法：\n" +
                "  .init();//初始化回调：寻路完成的回调，移动过程中的回调，到达目的地的回调\n" +
                "  .seek();//寻路，完成后会调用在init里设置的回调函数，第一个参数是路径，第二个参数为能否到达目的地\n" +
                "  .seekTarget();//寻路，会定时重新寻目标对象的路径\n" +
                "  .cancelSeekTarget();//取消对目标对象的定时寻路\n" +
                "  .startMove();//开始移动，只能通过此方法，不能直接改变canMove变量\n" +
                "  .stopMove();//停止移动\n"
                , MessageType.None, true);
        }
        ECLEditorUtl.EndContents();

        ECLEditorUtl.BeginContents();
        {
            GUI.color = Color.yellow;
            GUILayout.Label("*鼠标悬停在字段上有解释");
            GUI.color = Color.white;

            GUIContent uicontnt = null;

            ECLEditorUtl.BeginContents();
            {
                uicontnt = new GUIContent("Obstruct Mask", "障碍物的layer");
                instance.obstructMask = ECLEditorUtl.drawMaskField(uicontnt, instance.obstructMask);
                uicontnt = new GUIContent("Ray Distance", "每次寻路发射的射线长度用于检测障碍物");
                instance.rayDistance = EditorGUILayout.FloatField(uicontnt, instance.rayDistance);
                uicontnt = new GUIContent("Ray Height", "每次寻路发射的射线高度用于检测障碍物");
                instance.rayHeight = EditorGUILayout.FloatField(uicontnt, instance.rayHeight);
                uicontnt = new GUIContent("Ray Dirs", "4方向、8方向、16方向发射线寻路");
                instance.rayDirs = (CLSeekerByRay.SearchDirs)EditorGUILayout.EnumPopup(uicontnt, instance.rayDirs);
                uicontnt = new GUIContent("Max Search Times", "最大寻路次数，防止死循环");
                instance.maxSearchTimes = EditorGUILayout.IntField(uicontnt, instance.maxSearchTimes);
            }
            ECLEditorUtl.EndContents();

            uicontnt = new GUIContent("Target", "目标对象");
            instance.target = (Transform)EditorGUILayout.ObjectField(uicontnt, instance.target, typeof(Transform));
            uicontnt = new GUIContent("Move Speed", "移动速度");
            instance.speed = EditorGUILayout.FloatField(uicontnt, instance.speed);
            uicontnt = new GUIContent("Turning Speed", "转动速度，【注意】当为负数时表示立即转到目标方向，大于0时则会慢慢转向目标方向");
            instance.turningSpeed = EditorGUILayout.FloatField(uicontnt, instance.turningSpeed);
            uicontnt = new GUIContent("EndReached Distance", "离目标到一定距离后结束移动");
            instance.endReachedDistance = EditorGUILayout.FloatField(uicontnt, instance.endReachedDistance);
            uicontnt = new GUIContent("Auto Move After Seek", "当寻路完成后就移动过去");
            instance.autoMoveOnFinishSeek = EditorGUILayout.Toggle(uicontnt, instance.autoMoveOnFinishSeek);
            uicontnt = new GUIContent("Moving By", "能哪种update移动");
            instance.movingBy = (CLSeeker.MovingBy)EditorGUILayout.EnumPopup(uicontnt, instance.movingBy);
            uicontnt = new GUIContent("Moving Unscaled Time", "移动时忽略时间的缩放");
            instance.unscaledTime = EditorGUILayout.Toggle(uicontnt, instance.unscaledTime);
            ECLEditorUtl.BeginContents();
            {
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

            uicontnt = new GUIContent("Debug Show Path", "显示路径");
            instance.showPath = EditorGUILayout.Toggle(uicontnt, instance.showPath);
        }
        ECLEditorUtl.EndContents();

        ECLEditorUtl.BeginContents();
        {
            if (GUILayout.Button("Search"))
            {
                instance.seek(instance.target.position);
            }

            if (GUILayout.Button("SearchTarget"))
            {
                instance.seekTarget(instance.target);
            }
            if (GUILayout.Button("Cancel SearchTarget"))
            {
                instance.cancelSeekTarget();
            }

            if (GUILayout.Button("Begain Move"))
            {
                instance.startMove();
            }

            if (GUILayout.Button("Stop Move"))
            {
                instance.stopMove();
            }
        }
        ECLEditorUtl.EndContents();
        EditorUtility.SetDirty(target);
    }
}
