//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public static class DependencySystemInitializer
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly string OpenedWindowKey = "OpenedDependencyManager";


        static DependencySystemInitializer()
        {
            InitializeDREditorLayers();

            if (!DependencyManager.AllDependenciesInstalled() &&
                !EditorWindow.HasOpenInstances<DependencyManagerWindow>() &&
                !EditorPrefs.GetBool(OpenedWindowKey))
                OpenManagerWindow();
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Dependencies/Open Dependency Manager", priority = 1)]
        public static void OpenManagerWindow()
        {
            var window = EditorWindow.GetWindow<DependencyManagerWindow>(
                        utility: true,
                        title: $"Dependency Manager");

            window.minSize = new Vector2(600f, 300f);

            EditorPrefs.SetBool(OpenedWindowKey, true);
        }


        /// <summary>
        /// 
        /// </summary>
        public static void InitializeDREditorLayers()
        {
            string[] dreLayers =
            {
                "Dialogue Object",
                "Menu Object",
                "Focus",
                "PostProcessing",
                "Particles",
                "Item Object",
                "NSD Text",
                "NSD Character Mask",
                "Door",
                "ObserveAddOn",
                "RaycastBlock",
                "LightingMask",
                "MinigameOnly"
            };

            if (!ProjectContainsAllLayers(dreLayers))
                CreateNewLayers(dreLayers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layerNames"></param>
        /// <returns></returns>
        private static bool ProjectContainsAllLayers(string[] layerNames)
        {
            if (layerNames == null || layerNames.Length == 0)
                return false;
            else
                return layerNames.All(name => LayerMask.NameToLayer(name) != -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layerNames"></param>
        private static void CreateNewLayers(string[] layerNames)
        {
            foreach (var layerName in layerNames)
                CreateNewLayer(layerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layerName"></param>
        private static void CreateNewLayer(string layerName)
        {
            SerializedObject tagManagerSettings = new(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layersProperty = tagManagerSettings.FindProperty("layers");

            for (int i = 6; i < 32; i++)
            {
                if (i == 12)
                    continue;

                SerializedProperty layerSlot = layersProperty.GetArrayElementAtIndex(i);

                if (layerSlot != null && layerSlot.stringValue == layerName)
                    return;
                else if (layerSlot != null && string.IsNullOrEmpty(layerSlot.stringValue))
                {
                    layerSlot.stringValue = layerName;

                    tagManagerSettings.ApplyModifiedProperties();

                    UnityEngine.Debug.Log($"REPORT: Successfully created layer: '{layerName}' at slot {i}");

                    return;
                }
            }

            UnityEngine.Debug.LogError("ALERT: Could not create layer.");
        }
    }
}