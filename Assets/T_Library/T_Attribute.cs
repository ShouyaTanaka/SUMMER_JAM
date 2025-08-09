using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//=============================
#region コピペ用
//=============================

#if UNITY_EDITOR

#endif
#endregion

namespace T_Library.Attribute
{
    //=============================
    #region 変数名を指定文字.色にできる属性(Tooltipと連携済)
    // todo:色の名前やカラーコードで呼べるようにする("red"や"#FF0000")
    //=============================

    public class CustomLabelAttribute : PropertyAttribute
    {
        public string LabelText { get; }
        public Color? LabelColor { get; }

        public CustomLabelAttribute(string text)
        {
            LabelText = text;
            LabelColor = null;
        }

        public CustomLabelAttribute(string text, float r, float g, float b)
        {
            LabelText = text;
            LabelColor = new Color(r, g, b);
        }

        public CustomLabelAttribute(string text, float r, float g, float b, float a)
        {
            LabelText = text;
            LabelColor = new Color(r, g, b, a);
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(CustomLabelAttribute))]
    public class CustomLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (CustomLabelAttribute)attribute;

            // ラベル部分とフィールド部分に分割
            float labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = new Rect(position.x, position.y, labelWidth, position.height);
            var fieldRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);

            // --- Tooltipの合成 ---
            string tooltipText = "";
            // 元のTooltipが存在する場合
            if (!string.IsNullOrEmpty(label.tooltip))
            {
                tooltipText += label.tooltip + "\n";
            }
            // 変数名を追加（省略せず property.name）
            tooltipText += $"変数名: {property.name}";

            // ラベルのGUIContent
            GUIContent labelContent = new GUIContent(attr.LabelText ?? label.text, tooltipText);

            // カスタム色つきラベル描画
            if (attr.LabelColor.HasValue)
            {
                var style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = attr.LabelColor.Value;
                EditorGUI.LabelField(labelRect, labelContent, style);
            }
            else
            {
                EditorGUI.LabelField(labelRect, labelContent);
            }

            // 通常のプロパティ描画（フィールド）
            EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif
    #endregion

    //=============================
    #region 二次元配列をエディター上で確認可能にする属性
    //=============================
    public class Show2DArrayAttribute : PropertyAttribute { }
#if UNITY_EDITOR

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class Show2DArrayEditor : Editor
    {
        private static Dictionary<string, bool> foldoutStates = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetObject = target as MonoBehaviour;
            var type = targetObject.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<Show2DArrayAttribute>();
                if (attr != null && field.FieldType.IsArray && field.FieldType.GetArrayRank() == 2)
                {
                    string key = $"{type.FullName}.{field.Name}";
                    if (!foldoutStates.ContainsKey(key))
                        foldoutStates[key] = false;

                    EditorGUILayout.Space(5);
                    GUI.backgroundColor = new Color(0.95f, 0.95f, 1f);
                    EditorGUILayout.BeginVertical("box");
                    GUI.backgroundColor = Color.white;

                    foldoutStates[key] = EditorGUILayout.Foldout(foldoutStates[key], $"{field.Name} [{field.FieldType.GetElementType().Name}][,]", true);
                    if (foldoutStates[key])
                    {
                        Draw2DArray(field, targetObject);
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(5);
                }
            }
        }

        private void Draw2DArray(FieldInfo field, MonoBehaviour targetObject)
        {
            var value = field.GetValue(targetObject);
            if (value is Array array && array.Rank == 2)
            {
                Type elementType = field.FieldType.GetElementType();
                int rows = array.GetLength(0);
                int cols = array.GetLength(1);

                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"サイズ: {rows} x {cols}");

                GUIStyle cellStyle = new GUIStyle(GUI.skin.box)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 11,
                    fixedWidth = 60,
                    fixedHeight = 30,
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(1, 1, 1, 1)
                };

                for (int y = 0; y < rows; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < cols; x++)
                    {
                        object element = array.GetValue(y, x);
                        object newElement = element;

                        EditorGUILayout.BeginVertical("box", GUILayout.Width(64), GUILayout.Height(32));

                        if (elementType == typeof(int))
                        {
                            newElement = EditorGUILayout.IntField((int)element, GUILayout.Width(50));
                        }
                        else if (elementType == typeof(float))
                        {
                            newElement = EditorGUILayout.FloatField((float)element, GUILayout.Width(50));
                        }
                        else if (elementType == typeof(bool))
                        {
                            bool current = (bool)element;
                            newElement = GUILayout.Toggle(current, "", GUILayout.Width(20), GUILayout.Height(20));
                        }
                        else
                        {
                            EditorGUILayout.LabelField("N/A", cellStyle);
                        }

                        EditorGUILayout.EndVertical();

                        array.SetValue(newElement, y, x);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                field.SetValue(targetObject, array);
                EditorUtility.SetDirty(targetObject);
                EditorGUI.indentLevel--;
            }
        }
    }

#endif
    #endregion

}
