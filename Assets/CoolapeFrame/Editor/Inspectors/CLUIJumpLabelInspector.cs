using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Coolape;

[CanEditMultipleObjects]
[CustomEditor(typeof(CLUIJumpLabel), true)]
public class CLUIJumpLabelInspector : UILabelInspector
{
    protected override bool ShouldDrawProperties()
    {
        NGUIEditorTools.DrawProperty("speed", serializedObject, "speed", GUILayout.MinWidth(40f));
        return base.ShouldDrawProperties();
    }
}
