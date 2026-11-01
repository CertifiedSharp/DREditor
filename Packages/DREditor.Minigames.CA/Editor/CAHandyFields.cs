// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.CA.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class CAHandyFields
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="setData"></param>
        /// <param name="data"></param>
        public static void DisplayStockVectorField(GUIContent content, System.Action<Vector2> setData, Vector2 data)
        {
            GUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.Label(content);

                setData(EditorGUILayout.Vector2Field("", data, GUILayout.Width(100)));
            }

            GUILayout.FlexibleSpace();
        }
    }
}