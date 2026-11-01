//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class ReplaceDebateAssetTextEditor : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] string lookFor = "ns: DREditor.Minigames.NSD";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] string replaceWith = "ns: DREditor.Minigames.NSD";


        [MenuItem("DREditor/Convert/Replace Debate Asset Text Editor", false, 4)]
        public static void ShowWindow()
            => DisplayWizard<ReplaceDebateAssetTextEditor>("Replace Debate Asset Text Editor", "Convert");


        protected virtual void OnWizardCreate() => ReplaceTextInAllAssets();


        /// <summary>
        ///
        /// </summary>
        protected virtual void ReplaceTextInAllAssets()
        {
            var assets = BuilderUtility.FindAssetsByInterface<ScriptableObject, IDebateScanner>();

            foreach (var asset in assets)
                ReadAssetText((ScriptableObject)asset);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void ReadAssetText(Object obj)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(assetPath))
            {
                UnityEngine.Debug.LogError("ALERT: Could not find asset path for the selected object.");

                return;
            }

            if (File.Exists(assetPath))
            {
                AssetDatabase.StartAssetEditing();

                string content = File.ReadAllText(assetPath);

                string updated = content.Replace(lookFor, replaceWith);

                File.WriteAllText(assetPath, updated);

                UnityEngine.Debug.Log("REPORT: Applied: " + assetPath);

                AssetDatabase.StopAssetEditing();
            }
        }
    }
}