//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.IO;
using UnityEditor;

namespace DREditor.Editor.AssetTextEditing
{
    /// <summary>
    ///
    /// </summary>
    public static class ReplaceAssetTextUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="replaceProcess"></param>
        public static void ReplaceAssetText(UnityEngine.Object obj, Func<string, string> replaceProcess)
        {
            if (TryGetValidAssetPath(obj, out string assetPath))
                UpdateAssetText(assetPath, replaceProcess);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool TryGetValidAssetPath(UnityEngine.Object obj, out string assetPath)
        {
            assetPath = AssetDatabase.GetAssetPath(obj);

            bool result = !string.IsNullOrEmpty(assetPath) && File.Exists(assetPath);

            if (!result)
                UnityEngine.Debug.LogWarning($"ALERT: Could not find asset path for the selected object: {obj}.");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="replaceProcess"></param>
        private static void UpdateAssetText(string assetPath, Func<string, string> replaceProcess)
        {
            string content = File.ReadAllText(assetPath);

            string updated = replaceProcess(content);

            File.WriteAllText(assetPath, updated);
        }
    }
}