using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;

[CanEditMultipleObjects]
[CustomEditor(typeof(CLSeeker), true)]
public class ECLSeekerInspector : Editor
{
    CLSeeker instance;
    public override void OnInspectorGUI()
    {
        instance = target as CLSeeker;
        //DrawDefaultInspector();
        ECLEditorUtl.BeginContents();
        {
            EditorGUILayout.HelpBox(
                "配合A星使用的seeker。\n" +
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
            uicontnt = new GUIContent("AStar Instance", "A星寻路实例,如果不设值，在运行时自动使用CLAStarPathSearch.current");
            instance.mAStarPathSearch = (CLAStarPathSearch)EditorGUILayout.ObjectField(uicontnt, instance.mAStarPathSearch, typeof(CLAStarPathSearch));
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
            uicontnt = new GUIContent("is Moving", "正在移动");
            EditorGUILayout.Toggle(uicontnt, instance.canMove);
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
