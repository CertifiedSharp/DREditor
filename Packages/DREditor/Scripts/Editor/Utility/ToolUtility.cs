//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class ToolUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Rename Objects With Underscores")]
        public static void RenameObjectsWithUnderscores()
        {
            foreach (var selection in Selection.objects)
            {
                if (selection is GameObject gameObject)
                {
                    string originalName = gameObject.name;

                    string formattedName = Regex.Replace(originalName, @"([a-z0-9])([A-Z])", "$1_$2");

                    formattedName = Regex.Replace(formattedName, @"([a-z0-9]) ([A-Z])", "$1_$2");

                    gameObject.name = formattedName;

                    EditorUtility.SetDirty(gameObject);
                }
            }
        }
    }
}