//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplicateScenesInFolder : DuplicateWizardBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string sceneTemplatePrefix = "DRE_";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool replaceExisting = false;


        /// <summary>
        /// 
        /// </summary>
        protected string selectedFolderPath = null;

        /// <summary>
        /// 
        /// </summary>
        protected List<SceneAsset> scenes = new();


        /// <summary>
        /// 
        /// </summary>
        private const string MenuPath = "Assets/DREditor/Duplicate Scenes in Folder";


        protected override bool DisplayTargetDirectory { get => false; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MenuItem(MenuPath, true)]
        private static bool VerifyFolderSelected() => FolderUtility.SelectedObjectIsFolder();


        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath, false, 60)]
        public static void OpenWizard()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            DuplicateScenesInFolder wizard = DisplayWizard<DuplicateScenesInFolder>("Duplicate Scenes in Folder", "Duplicate");

            wizard.SetPath(path + "/");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedFolderPath"></param>
        public virtual void SetPath(string selectedFolderPath) => this.selectedFolderPath = selectedFolderPath;


        protected override bool OnEnableWizardValid() => VerifyFolderSelected();

        protected override bool ImportValid()
        {
            InitializeScenesInSelectedFolder();

            ChooseTargetDirectoryInAssets();

            return SceneListPopulated() && FolderUtility.ProjectFolderExists(targetDirectory);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeScenesInSelectedFolder()
        {
            string[] guids = AssetDatabase.FindAssets("t:Scene", new string[] { selectedFolderPath });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);

                scenes.Add(scene);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool SceneListPopulated()
        {
            bool result = scenes.Count > 0;

            if (result)
                Debug.Log($"REPORT: Found {scenes.Count} scene(s) in folder: {selectedFolderPath}");
            else
                Debug.LogWarning("ALERT: No scenes were found in selected folder.");

            return result;
        }


        protected override void Import()
        {
            foreach (var scene in scenes)
                if (ShouldDuplicate(scene) && TryDuplicateAsset(scene, out SceneAsset duplicatedScene))
                    Debug.Log("REPORT: Successfully duplicated to new scene: " + duplicatedScene.name);
                else if (!replaceExisting)
                    Debug.LogWarning("ALERT: The system could not duplicate scene: " + scene.name);

            Debug.Log("REPORT: Duplication process completed.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        protected virtual bool ShouldDuplicate(SceneAsset scene)
        {
            string duplicatedScenePath = BuildDuplicatedPath(scene.name, AssetDatabase.GetAssetPath(scene));

            return replaceExisting || !File.Exists(duplicatedScenePath);
        }

        protected override string BuildDuplicatedAssetName(string objectToDuplicateName, string objectToDuplicatePath) =>
            base.BuildDuplicatedAssetName(objectToDuplicateName, objectToDuplicatePath)
            .Replace(sceneTemplatePrefix, "");
    }
}