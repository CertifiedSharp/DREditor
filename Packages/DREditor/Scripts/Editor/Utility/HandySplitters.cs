//Original Author: Jordi / Happy30
//Contributing Author: KHeartz
//Contributing Author: Benjamin "Sweden" Jillson
//Utility Functions for DREditor Editors

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class HandySplitters
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        /// <param name="horizontalpadding"></param>
        /// <param name="verticalpadding"></param>
        public static void UISplitterCustom(Color color, int thickness = 2, int horizontalpadding = 0, int verticalpadding = 5)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(verticalpadding + thickness));

            r.height = thickness;

            r.y += verticalpadding / 2;

            r.x += horizontalpadding;

            r.width -= horizontalpadding * 2;

            EditorGUI.DrawRect(r, color);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        /// <param name="width"></param>
        /// <param name="horizontalpadding"></param>
        /// <param name="verticalpadding"></param>
        public static void LineNumSplitter(Color color, int thickness = 2, float width = 5,
            int horizontalpadding = 0, int verticalpadding = 5)
        {
            Rect r = EditorGUILayout.GetControlRect(
                GUILayout.Height(verticalpadding + thickness),
                GUILayout.Width(width - horizontalpadding));

            r.height = thickness;

            r.y += verticalpadding / 2;

            r.x += horizontalpadding;

            r.width -= horizontalpadding * 2;

            EditorGUI.DrawRect(r, color);
        }

        /// <summary>
        ///
        /// </summary>
        public static void UISplitter() => UISplitter(Color.white);

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <param name="spacing"></param>
        public static void UISplitter(Color color, float spacing = 10)
        {
            GUILayout.Space(spacing);

            UISplitterCustom(color);

            GUILayout.Space(spacing);
        }
    }
}
#endif