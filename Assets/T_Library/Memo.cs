using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class Memo : MonoBehaviour
{
    [TextArea(3, 10)]
    public string memo;

    [SerializeField]
    public Color textColor = Color.yellow;

    [SerializeField]
    public Color backgroundColor = new Color(0f, 0f, 0f, 0.2f);
}

#if UNITY_EDITOR

[CustomEditor(typeof(Memo))]
public class MemoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var memo = (Memo)target;

        EditorGUILayout.BeginVertical("box");
        Color originColor = GUI.backgroundColor;
        GUI.backgroundColor = memo.backgroundColor;

        EditorGUILayout.LabelField("メモ", EditorStyles.boldLabel);

        GUIStyle style = new GUIStyle(EditorStyles.textArea)
        {
            wordWrap = true,
            fontSize = 12,
            normal = { textColor = memo.textColor }
        };

        memo.memo = EditorGUILayout.TextArea(memo.memo, style, GUILayout.Height(100));

        EditorGUILayout.Space();
        memo.textColor = EditorGUILayout.ColorField("文字色", memo.textColor);
        memo.backgroundColor = EditorGUILayout.ColorField("背景色", memo.backgroundColor);

        GUI.backgroundColor = originColor;
        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(memo);
        }

    }
}

#endif
