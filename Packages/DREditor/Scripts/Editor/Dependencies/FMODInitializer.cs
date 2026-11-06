//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class FMODInitializer : DependencyInitializerBase
    {
        /// <inheritdoc/>
        public override float Priority => 1;

        /// <inheritdoc/>
        public override bool Required => false;

        /// <inheritdoc/>
        protected override string AssemblyName => "FMODUnity";

        /// <inheritdoc/>
        protected override string DefineSymbol => "FMOD_UNITY";

        /// <inheritdoc/>
        public override string DisplayName => "FMOD";

        /// <inheritdoc/>
        protected override string DependencyInstallURL => "https://www.fmod.com/download#fmodforunity";

        /// <summary>
        ///
        /// </summary>
        protected readonly string FMODTemplateURL = "https://drive.google.com/file/d/1KQDVa-Xp4iXRqj4sRqivytXUYP8xyf8S/view?usp=sharing";

        /// <summary>
        ///
        /// </summary>
        protected readonly string ProjectRootPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        /// <summary>
        ///
        /// </summary>
        protected readonly string FMODFolderName = "dre_audio";

        /// <summary>
        ///
        /// </summary>
        protected readonly string FMODGitIgnore =
            "\r\n# Never ignore DLLs in the FMOD subfolder." +
            "\r\n!/[Aa]ssets/Plugins/FMOD/**/lib/*" +
            "\r\n                    " +
            "\r\n# Don't ignore images and gizmos used by FMOD in the Unity Editor." +
            "\r\n!/[Aa]ssets/Gizmos/FMOD/*" +
            "\r\n!/[Aa]ssets/Editor Default Resources/FMOD/*" +
            "\r\n                    " +
            "\r\n# Ignore the Cache folder since it is updated locally." +
            "\r\n/[Aa]ssets/Plugins/FMOD/Cache/*" +
            "\r\n                    " +
            "\r\n# Ignore bank files in the StreamingAssets folder." +
            "\r\n/[Aa]ssets/StreamingAssets/**/*.bank" +
            "\r\n/[Aa]ssets/StreamingAssets/**/*.bank.meta" +
            "\r\n                    " +
            "\r\n# If the source bank files are kept outside of the StreamingAssets folder then these can be ignored." +
            "\r\n# Log files can be ignored." +
            "\r\nfmod_editor.log";

        /// <summary>
        ///
        /// </summary>
        protected readonly List<Func<bool>> Instructions;


        public FMODInitializer()
        {
            Instructions = new()
            {
                PromptWebsite,
                InstallerInstructions,
                OpenFMODTemplateDownload,
                ImportFMODTemplate,
                PromptPackageDownload,
                FMODLinking,
            };
        }


        /// <inheritdoc/>
        protected override void PostDownloadInstructions()
        {
            if (AllInstructionsFollowed())
                ImportPackage();
            else
                InstallDependency();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool AllInstructionsFollowed()
        {
            foreach (var instruction in Instructions)
                if (instruction == null || !instruction.Invoke())
                    return false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool PromptWebsite() =>
            EditorUtility.DisplayDialog("Import Instructions",
                "PROPOSAL: Follow these steps:" +
                "\n- A Website should've appeared, Click sign in so you can sign in or register." +
                "\n- Once Registered, return back to the download site." +
                "\n- At the very top, the drop down for \"FMOD Studio\" should have download links when you click it." +
                "\n- Click \"Unity Verified\" then click \"Older\" there should be version 2.01.11. Click download." +
                "\nIF THE DOWNLOAD DOESNT START USE AN INCOGNITO WINDOW." +
                "\n- Once you've downloaded the .exe, return back here and click the next step button.",
                "Next Step", "Restart");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool InstallerInstructions() =>
            EditorUtility.DisplayDialog("Import Instructions",
                "PROPOSAL: Follow these steps:" +
                "\n- Click on the installer, make sure that during the installation, uncheck \"Reboot\". " +
                "\nOther than that, follow the install instructions." +
                "\n- Once done, return back here and click the next step button.",
                "Next Step", "Restart");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool OpenFMODTemplateDownload()
        {
            bool result = EditorUtility.DisplayDialog("Import Instructions",
                "PROPOSAL: Follow this step:" +
                "\nNext, we're going to download an FMOD template for the system. " +
                "\nThere's one available in google drive, I'll open it now.",
                "Ready", "Restart");

            if (result)
                OpenInstallURL(FMODTemplateURL);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool ImportFMODTemplate()
        {
            string fullPath = GetFMODPath();

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            if (EditorUtility.DisplayDialog("Import Instructions",
                "REPORT: The directory for where to place the downloaded template has been created." +
                "\nPROPOSAL: When I open the explorer, place the zip into the folder opened, right click, and extract it." +
                "\nPROPOSAL: Copy the components inside the extracted folder and move them into the dre_audio folder." +
                "\nQUERY: Open Explorer now?",
                "Yes", "Skip"))
                EditorUtility.RevealInFinder(fullPath);

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected string GetFMODPath() => Path.Combine(ProjectRootPath, FMODFolderName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool PromptPackageDownload() =>
            EditorUtility.DisplayDialog("Import Instructions",
                "PROPOSAL: Follow these steps:" +
                "\n- Go back to the FMOD Download page and under the drop down \"FMOD for Unity\" will have download links when you click it." +
                "\n- Click \"Unity Verified\" then click \"Older\" there should be version 2.01.11. Click download." +
                "\nIF THE DOWNLOAD DOESNT START USE AN INCOGNITO WINDOW." +
                "\n- This should download a unity package, when the download is completed, return here to import the package.",
                "I've Downloaded the package.", "Restart");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool FMODLinking()
        {
            CopyFMODPathToClipboard();

            EditorUtility.DisplayDialog("REPORT",
                "For context:" +
                "\nYou'll notice in a tutorial video that the next window will display a file path." +
                "\nThe file path will not show up now, instead, " +
                "I've copied it to your clipboard for you so that it doesn't fill up the message window.",
                "Understood");

            return EditorUtility.DisplayDialog("Import Instructions",
                "PROPOSAL: Follow these steps:" +
                "\n- After pressing the \"Import Package\" button below, select the unity package you just downloaded." +
                "\n- After you click \"Import\", You'll be asked to replace something, just hit yes. " +
                "\n- Afterwards, an FMOD window will appear." +
                "\n- You are ONLY going to click on \"Linking\"." +
                $"\n- Then, press the top most button, and ctrl+v (paste) into the explorer bar " +
                $"to move to the folder and select the FMOD project." +
                $"\n- When you finish linking, just press the refresh button on the dependency manager.",
                "Import Package", "Restart");
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CopyFMODPathToClipboard() => EditorGUIUtility.systemCopyBuffer = GetFMODPath();

        protected override void OnPostImport()
        {
            CopyFMODPathToClipboard();

            if (EditorUtility.DisplayDialog("Import Instructions",
                $"Have you finished linking {GetFMODPath()} to your FMOD project?" +
                $"\nI've re-copied it to your clipboard just in case.",
                "Yes", "No"))
                OnCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        protected void OnCompleted()
        {
            base.OnPostImport();

            UpdateGitIgnore();

            EditorUtility.DisplayDialog("Import Instructions",
                "REPORT: FMOD Dependency Installation should now be complete.",
                "Acknowledged");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected void UpdateGitIgnore()
        {
            string filePath = Path.Combine(ProjectRootPath, ".gitignore.txt");

            try
            {
                if (!File.ReadAllText(filePath).Contains(FMODGitIgnore))
                    File.AppendAllText(filePath, FMODGitIgnore + System.Environment.NewLine);
            }
            catch
            {
                UnityEngine.Debug.LogWarning("ALERT: The Git Ignore file for FMOD could not be updated.");

                UnityEngine.Debug.LogWarning("HYPOTHESIS: It is likely that the user did not set up a github project before the system was installed.");

                UnityEngine.Debug.LogWarning("PROPOSAL: Remove DREditor from the system, make a github project first, then import DREditor.");
            }
        }
    }
}