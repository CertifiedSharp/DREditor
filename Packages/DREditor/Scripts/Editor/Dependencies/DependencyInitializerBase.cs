//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class DependencyInitializerBase
    {
        /// <summary>
        ///
        /// </summary>
        public virtual bool InProgress
        {
            get => EditorPrefs.HasKey(InProgressKey) && EditorPrefs.GetBool(InProgressKey);
            set => EditorPrefs.SetBool(InProgressKey, value);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool Ignored
        {
            get => EditorPrefs.HasKey(IgnoreKey) && EditorPrefs.GetBool(IgnoreKey);
            set => EditorPrefs.SetBool(IgnoreKey, value);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool Skipped
        {
            get => EditorPrefs.HasKey(SkippedKey) && EditorPrefs.GetBool(SkippedKey);
            set => EditorPrefs.SetBool(SkippedKey, value);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool Completed
        {
            get => EditorPrefs.HasKey(CompletedKey) && EditorPrefs.GetBool(CompletedKey);
            set => EditorPrefs.SetBool(CompletedKey, value);
        }

        /// <summary>
        ///
        /// </summary>
        public abstract float Priority { get; }

        /// <summary>
        ///
        /// </summary>
        public abstract bool Required { get; }

        /// <summary>
        ///
        /// </summary>
        protected abstract string AssemblyName { get; }

        /// <summary>
        ///
        /// </summary>
        protected abstract string DefineSymbol { get; }

        /// <summary>
        ///
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        ///
        /// </summary>
        protected abstract string DependencyInstallURL { get; }

        /// <summary>
        ///
        /// </summary>
        protected string IgnoreKey => "Ignore" + DisplayName;

        /// <summary>
        ///
        /// </summary>
        protected string SkippedKey => "Skip" + DisplayName;

        /// <summary>
        ///
        /// </summary>
        protected string InProgressKey => "InProgress" + DisplayName;

        /// <summary>
        ///
        /// </summary>
        protected string CompletedKey => "Completed" + DisplayName;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool TryToComplete() => Completed = TryInstallingDependency();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool TryInstallingDependency()
        {
            bool result = DependencyExists();

            if (!result && InProgress)
                return false;

            if (result && InProgress)
                OnPostImport();

            if (!Ignored && !result)
                InstallDependency();

            return Ignored || result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DependencyExists() => DependencyUtility.AddedDependency(AssemblyName, DefineSymbol);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPostImport() => InProgress = false;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InstallDependency()
        {
            InProgress = true;

            DependencyDownloadPrompt();

            if (!Skipped && !Ignored)
                PostDownloadInstructions();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DependencyDownloadPrompt()
        {
            if (TryDownloadPrompt())
                OpenInstallURL(DependencyInstallURL);
            else
                CheckSkipOrIgnore();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool TryDownloadPrompt() =>
            EditorUtility.DisplayDialog("Missing Dependency! Download it?",
                "ALERT: You are missing the dependency: " + DisplayName +
                "\nIt must be downloaded manually, would you like to download it now?",
                "Yes", "No");

        /// <summary>
        ///
        /// </summary>
        protected virtual void OpenInstallURL(string url)
        {
            if (EditorUtility.DisplayDialog("Open Download Link?",
                "REPORT: A link is about to open, Would you like to open it?",
                "Yes", "Skip"))
                Application.OpenURL(url);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckSkipOrIgnore()
        {
            if (SkippedOnPrompt())
                OnDependencySkipped();
            else
                OnIgnoreAttempt();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkippedOnPrompt() =>
            EditorUtility.DisplayDialog("Skip or Ignore Dependency?",
                $"REPORT: For now, would you like to skip installing: {DisplayName}?",
                "Skip", "I do not wish to use this dependency.");

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDependencySkipped() => Skipped = true;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnIgnoreAttempt()
        {
            if (Required)
                OnTriedToIgnoreRequiredDependency();
            else
                IgnoreDependency();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnTriedToIgnoreRequiredDependency()
        {
            EditorUtility.DisplayDialog("Dependency is Required.",
                $"ANALYSIS: The dependency {DisplayName} is a required part of the system, and cannot be ignored.",
                "Ok");

            InstallDependency();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void IgnoreDependency()
        {
            Ignored = true;

            InProgress = false;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void ClearIgnore() => EditorPrefs.DeleteKey(IgnoreKey);


        /// <summary>
        ///
        /// </summary>
        protected abstract void PostDownloadInstructions();


        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportPackage() => DependencyUtility.ImportPackage();
    }
}