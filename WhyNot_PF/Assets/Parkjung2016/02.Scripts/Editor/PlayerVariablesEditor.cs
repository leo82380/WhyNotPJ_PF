using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlayerController), true)]
public class PlayerVariablesEditor : Editor
{
    GUISkin skin;

    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("vSkin") as GUISkin;
        GUI.skin = skin;

        GUILayout.BeginVertical("<플레이어 변수 설정>", "window");

        GUILayout.Space(30);

        EditorGUILayout.BeginVertical();

        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        GUILayout.Space(2);
    }
}
