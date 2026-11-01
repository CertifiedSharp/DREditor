//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public static class HandyEditor
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dropRect"></param>
        /// <param name="prop"></param>
        public static void HandleDragDrop<T>(Rect dropRect, SerializedProperty prop)
        {
            Event evt = Event.current;

            if (!dropRect.Contains(evt.mousePosition))
                return;

            switch (evt.type)
            {
                case EventType.DragUpdated:

                    DragAndDrop.visualMode = FirstValidDrag<T>() != null
                        ? DragAndDropVisualMode.Link
                        : DragAndDropVisualMode.Rejected;

                    evt.Use();

                    break;

                case EventType.DragPerform:

                    var valid = FirstValidDrag<T>();

                    if (valid != null)
                    {
                        DragAndDrop.AcceptDrag();

                        prop.objectReferenceValue = valid;

                        prop.serializedObject.ApplyModifiedProperties();
                    }

                    evt.Use();

                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ScriptableObject FirstValidDrag<T>()
        {
            foreach (var obj in DragAndDrop.objectReferences)
                if (obj is ScriptableObject so && so is T)
                    return so;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        /// <param name="rect"></param>
        /// <param name="callback"></param>
        public static void PingAndDoOnClick<T>(T asset, Rect rect, Action<T> callback) where T : UnityEngine.Object
        {
            PingOnClick(asset, rect);

            DoOnClick(asset, rect, callback);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        /// <param name="rect"></param>
        /// <param name="clickAmount"></param>
        public static void PingOnClick<T>(T asset, Rect rect, int clickAmount = 1) where T : UnityEngine.Object
        {
            if (ClickedOnRect(rect) && Event.current.clickCount == clickAmount && asset != null)
                EditorGUIUtility.PingObject(asset);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        /// <param name="rect"></param>
        /// <param name="callback"></param>
        /// <param name="clickAmount"></param>
        public static void DoOnClick<T>(T asset, Rect rect, Action<T> callback, int clickAmount = 2)
        {
            if (ClickedOnRect(rect))
            {
                if (Event.current.clickCount == clickAmount || asset == null)
                {
                    callback(asset);

                    Event.current.Use();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool ClickedOnRect(Rect rect) 
            => Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition);


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="serializedObject"></param>
        /// <param name="prop"></param>
        public static void DrawFilteredWindowButton<T, T2>(SerializedObject serializedObject, SerializedProperty prop)
            where T : SimpleFilteredSOWindow<T2>
        {
            var current = prop.objectReferenceValue as ScriptableObject;

            if (HandyFields.SkinnyButton("Select"))
            {
                var win = EditorWindow.GetWindow<T>(
                    utility: true,
                    title: $"Select {typeof(T2).Name} Asset");

                win.Show(serializedObject, prop.propertyPath, current);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="prop"></param>
        public static void DrawClearPropertyButton(SerializedObject serializedObject, SerializedProperty prop)
        {
            using (new EditorGUI.DisabledScope(prop.objectReferenceValue == null))
            {
                if (HandyFields.SkinnyButton("X"))
                {
                    prop.objectReferenceValue = null;

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static Rect Indent(Rect rect, float width = 4, float height = 0, float multiplier = 2) =>
            new(rect.x + width, rect.y + height,
                rect.width - width * multiplier, rect.height - height * multiplier);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="serializedObject"></param>
        /// <param name="prop"></param>
        public static void DrawFilteredOptionAssetField<T, T2>(SerializedObject serializedObject, SerializedProperty prop)
            where T : SimpleFilteredSOWindow<T2>
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PrefixLabel(new GUIContent("Option Asset",
                    $"ScriptableObject implementing {typeof(T2).Name}"));

                DrawSOAssetPill<T2>(prop);

                DrawFilteredWindowButton<T, T2>(serializedObject, prop);

                DrawClearPropertyButton(serializedObject, prop);
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <param name="instanceSpacing"></param>
        /// <param name="iconCenter"></param>
        public static void DrawSOAssetPill<T>(SerializedProperty prop, float instanceSpacing = 2, float iconCenter = 1)
        {
            ScriptableObject asset = prop.objectReferenceValue as ScriptableObject;

            Rect rect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));

            EditorGUI.DrawRect(rect, Color.black);

            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.gray);

            if (asset == null)
            {
                GUI.Label(Indent(rect), $"None ({typeof(T).Name})", EditorStyles.label);

                return;
            }

            if (EditorGUIUtility.ObjectContent(asset, asset.GetType()).image is Texture icon)
                GUI.DrawTexture(
                    new Rect(rect.x + instanceSpacing, rect.y + iconCenter, rect.height - instanceSpacing, rect.height - instanceSpacing),
                    icon, ScaleMode.ScaleToFit);

            GUI.Label(Indent(rect, rect.height),
                $"{asset.name}   ({asset.GetType().Name})", EditorStyles.label);

            PingOnClick(asset, rect);

            HandleDragDrop<T>(rect, prop);
        }
    }
}
#endif