//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProjectVariantWizard : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ProjectPrefabVariantSettings settings = null;


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Purposly made a serialize field so the reference stays during a project reload when the wizard is open.</remarks>
        [HideInInspector]
        [SerializeField] protected GameObject target;


        /// <summary>
        /// 
        /// </summary>
        protected GameObject sourceAsset;

        /// <summary>
        /// 
        /// </summary>
        protected string assetPath;

        /// <summary>
        /// 
        /// </summary>
        protected string sourceName;

        /// <summary>
        /// 
        /// </summary>
        protected string trimmed;

        /// <summary>
        /// 
        /// </summary>
        protected bool running = false;


        /// <summary>
        /// 
        /// </summary>
        protected virtual bool SourceIsEmpty => sourceAsset == null;

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool SuffixIsEmpty => trimmed.Length == 0;

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool SuffixInvalid => trimmed.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;


        /// <summary>
        /// 
        /// </summary>
        private const string FolderPanelTitle = "Choose output folder for the Project Prefab Variant";

        /// <summary>
        /// 
        /// </summary>
        private const string MenuPath = "GameObject/DREditor/Create Project Prefab Variant";

        /// <summary>
        /// 
        /// </summary>
        private const int Priority = -20;

        /// <summary>
        /// 
        /// </summary>
        private const string PrefabExtension = ".prefab";


        /// <summary>
        /// 
        /// </summary>
        protected UnityEditor.Editor cachedEditor;


        protected virtual void OnDisable()
        {
            if (cachedEditor != null)
                DestroyImmediate(cachedEditor);
        }


        protected override bool DrawWizardGUI()
        {
            bool result = base.DrawWizardGUI();
            
            UnityEditor.Editor.DrawFoldoutInspector(settings, ref cachedEditor);
            OnWizardUpdate();
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MenuItem(MenuPath, true)]
        public static bool IsSelectingPrefab() => DRETemplateUtility.GetSelectedPrefab() != null;

        /// <summary>
        /// 
        /// </summary>
        [MenuItem(MenuPath, false, Priority)]
        public static void OpenWizard()
        {
            if (DRETemplateUtility.GetSelectedPrefab() is not GameObject instance)
                return;

            var wizard = DisplayWizard<CreateProjectVariantWizard>("Create Project Prefab Variant", "Create");

            wizard.target = instance;

            wizard.OnWizardUpdate();
        }


        protected virtual void OnWizardUpdate()
        {
            InitializeUpdate();

            if (FoundNoIssues())
                DisplayValidState();


        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeUpdate()
        {
            isValid = false;

            errorString = "";

            helpString = "";

            settings = ProjectPrefabVariantSettings.Instance;

            sourceAsset = target != null ? PrefabUtility.GetCorrespondingObjectFromSource(target) : null;

            if (sourceAsset != null)
            {
                sourceName = DRETemplateUtility.GetPrefabName(sourceAsset);

                trimmed = (settings.suffix ?? "").Trim();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool FoundNoIssues() =>
            !FoundIssue(SourceIsEmpty, OnSourceEmpty) &&
            !FoundIssue(SuffixIsEmpty, OnSuffixEmpty) &&
            !FoundIssue(SuffixInvalid, OnSuffixInvalid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issuePresent"></param>
        /// <param name="setWizardString"></param>
        /// <returns></returns>
        protected virtual bool FoundIssue(bool issuePresent, Action setWizardString)
        {
            if (issuePresent)
                setWizardString();

            return issuePresent;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSourceEmpty() => errorString = "ALERT: " +
            "\nThe selected object is no longer a valid prefab instance.";

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSuffixEmpty() =>
            helpString = $"ALERT: " +
            $"\nSource prefab: {sourceName}\nEnter a suffix for the new Prefab Variant.";

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSuffixInvalid() =>
            errorString = "ALERT: " +
            "\nThe suffix contains characters that are not allowed in a file name.";


        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayValidState()
        {
            ResolveUpdate();

            CheckAutoApply();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ResolveUpdate()
        {
            helpString = $"REPORT: " +
                $"\nSource prefab: {sourceName}" +
                $"\nNew prefab: {DRETemplateUtility.CreateProjectVariantName(sourceAsset, trimmed)}.prefab";

            isValid = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CheckAutoApply()
        {
            if (settings.permanentlyUseSettings && !running)
                OnWizardCreate();
        }


        protected virtual void OnWizardCreate()
        {
            running = true;

            if (VariantCanBeCreated())
                CreateProjectPrefabVariant();

            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool VariantCanBeCreated() =>
            SourceAssetValid() &&
            TrySelectingOutputFolder(out string selectedPath) &&
            !DRETemplateUtility.AssetExistsAt(selectedPath, out assetPath);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool SourceAssetValid()
        {
            bool result = target != null && IsSelectingPrefab() && sourceAsset != null;

            if (!result)
                Debug.LogError("Create Prefab Variant: the selected object is no longer a prefab instance.");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        protected virtual bool TrySelectingOutputFolder(out string assetPath)
        {
            string projectVariantName = DRETemplateUtility.CreateProjectVariantName(sourceAsset, settings.suffix.Trim());

            bool selectedFolder = FolderUtility.SelectFolderPanel(FolderPanelTitle, settings.targetDirectory, out string selectedFolderPath);

            assetPath = selectedFolder ? $"{selectedFolderPath}/{projectVariantName + PrefabExtension}" : string.Empty;

            return selectedFolder;
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateProjectPrefabVariant() =>
            DRETemplateUtility.CreateProjectPrefabVariant(target, assetPath);
    }
}