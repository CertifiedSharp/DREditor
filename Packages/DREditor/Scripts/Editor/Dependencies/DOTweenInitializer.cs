//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class DOTweenInitializer : DependencyInitializerBase
    {
        /// <inheritdoc/>
        public override float Priority => 0;

        /// <inheritdoc/>
        public override bool Required => true;

        /// <inheritdoc/>
        protected override string AssemblyName => "DG.Tweening";

        /// <inheritdoc/>
        protected override string DefineSymbol => "DOTWEEN";

        /// <inheritdoc/>
        public override string DisplayName => "DOTween";

        /// <inheritdoc/>
        protected override string DependencyInstallURL => "https://dotween.demigiant.com/downloads/DOTween_1_3_030.zip";


        /// <summary>
        ///
        /// </summary>
        protected readonly string ShouldCreateAssemblyKey = "ShouldCreateAssembly";


        /// <inheritdoc/>
        protected override bool TryInstallingDependency()
        {
            if (ShouldCreateAssembly())
                return false;
            else
                return base.TryInstallingDependency();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldCreateAssembly()
        {
            bool result = EditorPrefs.GetBool(ShouldCreateAssemblyKey);

            if (result)
                CreateAssemblyWhileImporting();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateAssemblyWhileImporting()
        {
            CreateAssemblyDefinition();

            EditorPrefs.DeleteKey(ShouldCreateAssemblyKey);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateAssemblyDefinition()
        {
            string folderPath = "Assets/Plugins/Demigiant/DOTween/";

            string fileName = "DG.Tweening.asmdef";

            string fullPath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string jsonContent = @"{
            ""name"": ""DG.Tweening"",
            ""rootNamespace"": """",
            ""references"": [],
            ""includePlatforms"": [],
            ""excludePlatforms"": [],
            ""allowUnsafeCode"": false,
            ""overrideReferences"": false,
            ""precompiledReferences"": [],
            ""autoReferenced"": true,
            ""defineConstraints"": [],
            ""versionDefines"": [],
            ""noEngineReferences"": false }";

            File.WriteAllText(fullPath, jsonContent);

            AssetDatabase.Refresh();

            Debug.Log($"REPORT: Created assembly definition at: {fullPath}");
        }


        /// <inheritdoc/>
        protected override void PostDownloadInstructions()
        {
            if (ZipInstructionsClickedNext())
                ImportInstructions();
            else
                OnCanceled();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ZipInstructionsClickedNext() =>
            EditorUtility.DisplayDialog("Import Dependency",
                "REPORT: Once the Zip Finishes downloading, Right click it and click \"Extract All\" or \"Extract Here\", " +
                "this should give you a unity package." +
                "\nOnce done, hit next.",
                    "Next", "Cancel");

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCanceled() => InProgress = false;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportInstructions()
        {
            EditorPrefs.SetBool(ShouldCreateAssemblyKey, true);

            EditorUtility.DisplayDialog("Import Instructions",
                "Follow these steps:" +
                "\n- A window will show up to open a file, click the DOTween Unity package you just extracted and click \"Open\"." +
                "\n- There will be a window displaying all the DOTween files, just hit import." +
                "\n- Then a DOTween window will show up, press \"Open DOTween Utility Panel\"." +
                "\n- There will then be another window with many buttons, press \"Set up DOTween\"." +
                "\n- The window will then be gray as it loads, when it's done press \"Apply\"." +
                "\n\nThink you can press 5 buttons placed in front of you?",
                "There's no way I could mess this up.");

            ImportPackage();
        }
    }
}