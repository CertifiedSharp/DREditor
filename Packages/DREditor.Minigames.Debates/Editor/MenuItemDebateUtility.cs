//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class MenuItemDebateUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/Debates", false, 4)]
        public static void Scan()
        {
            var builders = BuilderUtility.FindAssetsByInterface<DialogueCore, IDebateScanner>();

            UnityEngine.Debug.Log("Found: " + builders.Count);

            foreach (var builder in builders)
            {
                builder.Scan(out string log);

                if (log != "")
                    UnityEngine.Debug.Log(log);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Convert/Convert Debates")]
        public static void Convert()
        {
            var builders = BuilderUtility.FindAssetsByInterface<DialogueCore, IDebateConverter>();

            UnityEngine.Debug.Log("Found: " + builders.Count);

            foreach (var builder in builders)
            {
                builder.Convert();

                EditorUtility.SetDirty((ScriptableObject)builder);
            }
        }
    }
}
#endif