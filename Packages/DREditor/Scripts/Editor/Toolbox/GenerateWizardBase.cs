//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    public abstract class GenerateWizardBase : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [ShowIf(nameof(DirectoryFieldVisible))]
        [SerializeField] protected string targetDirectory;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [ShowIf(nameof(AssetPrefixFieldVisible))]
        [SerializeField] protected string assetPrefix;

        /// <summary>
        /// 
        /// </summary>
        private bool valid = true;


        /// <summary>
        /// 
        /// </summary>
        private readonly string AssetsString = "Assets";


        /// <summary>
        /// 
        /// </summary>
        protected virtual bool DisplayTargetDirectory { get => true; }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool DisplayAssetPrefix { get => true; }


        protected GenerateWizardBase(bool ReplaceAssetPrefix) => SetDirectory(CustomEditorUtility.GetCurrentProjectWindowPath(ReplaceAssetPrefix));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetPrefix"></param>
        protected void SetAssetPrefix(string assetPrefix) => this.assetPrefix = assetPrefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetDirectory"></param>
        protected void SetDirectory(string targetDirectory) => this.targetDirectory = targetDirectory;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ChooseTargetDirectoryInAssets()
        {
            if (FolderUtility.SelectFolderPanel("Select Output Directory", AssetsString, out string openedFolderPath))
                SetDirectory(openedFolderPath);
            else
                Debug.LogError("ALERT: Please select a folder inside the Assets directory!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool DirectoryFieldVisible() => DisplayTargetDirectory;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool AssetPrefixFieldVisible() => DisplayAssetPrefix;


        protected virtual void OnEnable()
        {
            if (!OnEnableWizardValid())
                InitializeInvalidWizard();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeInvalidWizard()
        {
            EditorApplication.delayCall += Close;

            valid = false;
        }


        protected virtual void OnDisable() => ResolveInvalidWizard();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ResolveInvalidWizard() => EditorApplication.delayCall -= Close;


        protected virtual void OnWizardCreate()
        {
            if (!ImportValid())
                return;

            Import();

            if (!valid)
                Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnEnableWizardValid();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool ImportValid();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Import();
    }
}