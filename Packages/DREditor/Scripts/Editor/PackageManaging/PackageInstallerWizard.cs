//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    public class PackageInstallerWizard : RequestPackageWizardBase<PackageInfo[]>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string targetSearchTerm = "net.dreditor.";


        /// <summary>
        ///
        /// </summary>
        protected Queue<string> packagesToInstall;


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Import/Packages en masse", priority = 1)]
        public static void DisplaySampleImporter() => DisplayWizard<PackageInstallerWizard>("Import Packages en masse", "Import");

        protected override Request<PackageInfo[]> CreateRequest() => Client.SearchAll(false);

        protected override void OnRetrievingPackagesSucceded()
        {
            InitializePackagesToInstall();

            if (ThereArePackagesToInstall())
                ProcessNextInstallation();
            else
                OnNoPackagesToInstall();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePackagesToInstall() => packagesToInstall = new Queue<string>(GetTargetedPackageNames());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ThereArePackagesToInstall() => packagesToInstall.Count > 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnNoPackagesToInstall() => Debug.LogWarning("ALERT: No matching scoped packages found to install.");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetTargetedPackageNames() =>
            PackageUtility.GetMatchingPackagesFrom(request, targetSearchTerm)
            .Select(package => package.name);

        /// <summary>
        ///
        /// </summary>
        protected virtual void ProcessNextInstallation()
        {
            if (AllPackagesInstalled())
                OnAllPackagesInstalled();
            else
                ImportNextPackage();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AllPackagesInstalled() => packagesToInstall.Count == 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnAllPackagesInstalled() => Debug.Log("REPORT: All packages have been installed successfully.");

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportNextPackage() => CreateImporter().Import(GetNextInstallRequest());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PackageRequester<PackageInfo> CreateImporter() => new(ProcessNextInstallation);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual AddRequest GetNextInstallRequest() => Client.Add(packagesToInstall.Dequeue());
    }
}