//Original Author: Jordi / Happy30
//Contributing Author: KHeartz
//Contributing Author: Benjamin "Sweden" Jillson
//Utility Functions for DREditor Editors

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class HandyFields
    {
        public static bool Foldout(bool showFoldout, string labelText, GUIStyle labelStyle = null)
        {
            labelStyle ??= new(EditorStyles.label)
            {
                wordWrap = true
            };

            float height = EditorStyles.label.CalcHeight(new GUIContent(labelText),
                EditorGUIUtility.currentViewWidth - 20);

            Rect labelRect = EditorGUILayout.GetControlRect(GUILayout.Height(height));

            Rect foldoutRect = new(labelRect.x, labelRect.y, 15, EditorGUIUtility.singleLineHeight);

            showFoldout = EditorGUI.Foldout(foldoutRect, showFoldout, GUIContent.none, true);

            EditorGUI.LabelField(labelRect, labelText, labelStyle);

            return showFoldout;
        }

        public static bool Foldout(bool showFoldout, string labelText, System.Action displayFoldout, GUIStyle labelStyle = null)
        {
            showFoldout = Foldout(showFoldout, labelText, labelStyle);

            if (showFoldout)
                displayFoldout?.Invoke();

            return showFoldout;
        }

        public static string GetPreviewText(string labelText, string scopedText = "==")
        {
            return string.IsNullOrWhiteSpace(labelText) ?
                "ALERT: Text is empty!" : scopedText + labelText + scopedText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wordArray"></param>
        /// <returns></returns>
        public static string[] CreateUniqueDropdownLabels(string[] wordArray)
        {
            var counts = new Dictionary<string, int>();

            var result = new string[wordArray.Length];

            for (int i = 0; i < wordArray.Length; i++)
            {
                string word = wordArray[i];

                if (!counts.TryGetValue(word, out int count))
                {
                    counts[word] = 1;

                    result[i] = word;
                }
                else
                {
                    counts[word] = count + 1;

                    result[i] = $"{word}{count + 1}";
                }
            }

            return result;
        }

        public static void Label(string text, params GUILayoutOption[] options) => Label(text, null, options);
        
        public static void Label(string text, GUIStyle style = null, params GUILayoutOption[] options)
        {
            var optionList = options.ToList();

            optionList.Add(CalculateWidthOption(text));

            style ??= EditorStyles.label;

            GUILayout.Label(text, style, optionList.ToArray());
        }

        public static bool ToolbarButton(string text, params GUILayoutOption[] options)
        {
            var optionList = options.ToList();

            optionList.Add(CalculateWidthOption(text));

            return GUILayout.Button(text, EditorStyles.toolbarButton, optionList.ToArray());
        }

        public static bool SkinnyButton(string text, params GUILayoutOption[] options)
        {
            var optionList = options.ToList();

            optionList.Add(CalculateSkinnyWidth(text));

            optionList.Add(GUILayout.Height(EditorGUIUtility.singleLineHeight));

            return GUILayout.Button(text, optionList.ToArray());
        }

        public static GUILayoutOption CalculateSkinnyWidth(string label)
            => GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(label)).x + 10);

        public static void Button(string text, System.Action onClick)
        {
            if (Button(text))
                onClick?.Invoke();
        }

        public static bool Button(string text, string toolTip = "", params GUILayoutOption[] options)
        {
            var optionList = options.ToList();

            optionList.Add(CalculateWidthOption(text, 20));

            return GUILayout.Button(new GUIContent(text, toolTip), optionList.ToArray());
        }

        public static GUILayoutOption CalculateWidthOption(string label, float offset = 0) 
            => GUILayout.Width(CalculateWidth(label, offset));

        public static float CalculateWidth(string label, float offset = 0) => GUI.skin.label.CalcSize(new GUIContent(label)).x + offset;


        public static void SystemMessage(string key, string title, string message, string ok, System.Action onClick = null)
        {
            if (EditorPrefs.GetBool(key))
                return;

            EditorUtility.DisplayDialog(title, message, ok);

            EditorPrefs.SetBool(key, true);

            onClick?.Invoke();
        }


        public static void DisplayIcon(string iconPath, Color color, float width = 16, float height = 16)
        {
            Color originalColor = GUI.color;

            GUI.color = color;

            Texture2D icon = EditorGUIUtility.IconContent(iconPath).image as Texture2D;

            GUILayout.Label(icon, GUILayout.Width(width), GUILayout.Height(height));

            GUI.color = originalColor;
        }

        public static Texture2D TextureField(Texture2D texture, int width = 120, int height = 120)
        {
            Texture2D result;
            using (new EditorGUILayout.VerticalScope())
            {
                var style = new GUIStyle(GUI.skin.label)
                {
                    fixedWidth = 70
                };
                result = UnityField(texture, width, height);
            }
            return result;
        }

        public static int IntDropdown(string label, int index, string[] names, int currentNameLength)
        {
            if (names.Length > 0)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(label, GUILayout.MaxWidth(100));

                    int width = currentNameLength * 7 + 20;

                    return EditorGUILayout.IntPopup(index, names,
                    ContainerUtil.Iota(names.Length),
                    GUILayout.MaxWidth(width));
                }
            }
            else
                return IntField(new GUIContent(label), index);
        }

        public static int IntField(GUIContent content, int value, int valueWidth = 50, int labelBoxMulti = 7, bool useFlexableSpace = true, 
            params GUILayoutOption[] options)
        {
            GUI.backgroundColor = Color.white;
            
            int result;

            int originalLength = options.Length;

            System.Array.Resize(ref options, originalLength + 1);

            options[originalLength] = GUILayout.Width(valueWidth);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(content, GUILayout.Width(content.text.Length * labelBoxMulti));

                result = EditorGUILayout.IntField(value, options);
            }

            if (useFlexableSpace)
                GUILayout.FlexibleSpace();

            return result;
        }

        public static int IntField(string name, int value, int valueWidth = 200, int labelBoxMulti = 6, bool useFlexableSpace = true) =>
            IntField(new GUIContent(name), value, valueWidth, labelBoxMulti, useFlexableSpace);

        public static float FloatField(string label, float value, int textBoxWidth = 200) =>
            DisplayField(label, value, EditorGUILayout.FloatField, textBoxWidth);

        public static Vector3 Vector3Field(string label, Vector3 value, int textBoxWidth = 200) =>
            DisplayField(label, value, EditorGUILayout.Vector3Field, textBoxWidth);

        public static Vector4 Vector4Field(string label, Vector4 value, int textBoxWidth = 200) =>
            DisplayField(label, value, EditorGUILayout.Vector4Field, textBoxWidth);

        public static int Popup(string label, int value, string[] arr, int textBoxWidth = 150) =>
            DisplayField(label, value, 
                (label, value, options) => EditorGUILayout.Popup(value, arr, options), 
                textBoxWidth);

        

        public static T DisplayField<T>(string label, T value, LabeledEditorFieldDrawer<T> display, int textBoxWidth = 200)
        {
            GUI.backgroundColor = Color.white;

            T result;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(label, CalculateWidthOption(label));

                result = display("", value, GUILayout.Width(textBoxWidth));
            }

            GUILayout.FlexibleSpace();

            return result;
        }

        public static Sprite SpriteField(string name, Sprite sprite, int scale = 5)
        {
            Sprite result;

            using (new EditorGUILayout.VerticalScope())
            {
                int width = sprite != null ? (int)sprite.rect.width / scale : 140;

                int height = sprite != null ? (int)sprite.rect.height / scale / scale : 140;

                result = UnityField(new GUIContent(name), sprite, 7, width, height);
            }

            return result;
        }

        public static T UnityField<T>(GUIContent content, T data, int labelBoxMulti = 7, int width = 200, int height = 20) where T : Object
        {
            T result;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(content, GUILayout.Width(content.text.Length * labelBoxMulti));

                result = UnityField(data, width, height);
            }

            return result;
        }

        public static T UnityField<T>(T data, int width = 170, int height = 20) where T : Object
        {
            T result;

            using (new EditorGUILayout.VerticalScope())
                result = (T)EditorGUILayout.ObjectField(data, typeof(T), false, GUILayout.Width(width), GUILayout.Height(height));

            return result;
        }

        public static string StringField(string label, string value) => StringField(label, value, 200);

        public static string StringField(string label, string value, int textWidth) =>
            DisplayField(label, value, EditorGUILayout.TextField, textWidth);

        public static void HardPropertyField(string label, SerializedProperty prop, float width = 200, params GUILayoutOption[] options)
        {
            GUI.backgroundColor = Color.white;

            options.Append(GUILayout.Width(width));

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUIUtility.labelWidth = 80;

                EditorGUILayout.PropertyField(prop, new GUIContent(label), options);

                EditorGUIUtility.labelWidth = 0;
            }

            GUILayout.FlexibleSpace();
        }

        public static void FMODField(string label, SerializedProperty prop)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                Label(label);

                EditorGUILayout.PropertyField(prop, new GUIContent(""));
            }
        }


        public static void LabeledPropertyFieldGenericBG<T>(string name, SerializedProperty data, int width = 120, int height = 120) where T : Object
        {
            GUILayout.Label(name, GUILayout.Width(80));
            using (new EditorGUILayout.VerticalScope())
            {
                var result = (T)EditorGUILayout.ObjectField(data.objectReferenceValue as T, typeof(T), 
                    false, GUILayout.Width(width), GUILayout.Height(height));
            }
        }

        public static Color ColorField(string label, Color color) =>
            DisplayField(label, color, EditorGUILayout.ColorField, 40);

        public static string StringArea(string name, string value, float width = 200, float height = 60)
        {
            GUI.backgroundColor = Color.white;

            string result;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(name, GUILayout.Width(80));

                result = EditorGUILayout.TextArea(value, GUILayout.Width(width), GUILayout.Height(height));
            }

            GUILayout.FlexibleSpace();

            return result;
        }

        public static void PropertyTextArea(SerializedProperty property, params GUILayoutOption[] options) =>
            property.stringValue = EditorGUILayout.TextArea(property.stringValue, options);

        public static Texture2D GetMaterialTexture(Material material)
        {
            return material.GetTexture(
#if DR_URP
                "_BaseMap"
#else
                "_MainTex"
#endif
                ) as Texture2D;
        }

        public static bool Option(string label, bool option) => DisplayField(label, option, EditorGUILayout.Toggle, 15);

        public static bool Option(GUIContent label, bool option) => Option(label.text, option);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="nameOfItem"></param>
        /// <param name="addedItem"></param>
        /// <param name="options"></param>
        public static void AddItemButton<T>(List<T> items, string nameOfItem, T addedItem = default, params GUILayoutOption[] options)
        {
            if (GUILayout.Button("Add " + nameOfItem, options))
                items.Add(addedItem);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="nameOfItem"></param>
        /// <param name="index"></param>
        /// <param name="options"></param>
        public static void RemoveItemButton<T>(List<T> items, string nameOfItem, int index, params GUILayoutOption[] options)
        {
            if (GUILayout.Button("Remove " + nameOfItem, options))
                items.Remove(items[index]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="label"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public delegate T LabeledEditorFieldDrawer<T>(string label, T value, params GUILayoutOption[] options);
}
#endif