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
        protected string[] packageNames;


        /// <summary>
        /// 
        /// </summary>
        [MenuItem("DREditor/Import/Packages en masse", priority = 1)]
        public static void DisplaySampleImporter() => DisplayWizard<PackageInstallerWizard>("Import Packages en masse", "Import");

        protected override Request<PackageInfo[]> CreateRequest() => Client.SearchAll(false);

        protected override void OnRetrievingPackagesSucceded()
        {
            InitializePackagesToInstall();

            if (ThereArePackagesToDownload())
                InstallPackages();
            else
                OnNoPackagesToInstall();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializePackagesToInstall() => packageNames ??= GetPackageNamesToInstall().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetPackageNamesToInstall()
        {
            IEnumerable<string> packageNames = PackageInfo.GetAllRegisteredPackages().Select(package => package.name);

            var installed = new HashSet<string>(packageNames);

            return GetTargetedPackageNames().Where(name => !installed.Contains(name));
        }

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
        /// <returns></returns>
        protected virtual bool ThereArePackagesToDownload() => packageNames?.Length != 0;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InstallPackages()
        {
            Debug.Log($"REPORT: Installing {packageNames.Length} package(s).");

            var addRequest = Client.AddAndRemove(packageNames, null);

            void Poll()
            {
                if (!addRequest.Processable())
                    return;

                EditorApplication.update -= Poll;

                if (addRequest.Succeded())
                    Debug.Log("REPORT: All packages installed successfully.");
                else
                    Debug.LogError($"ALERT: Package install Failed: {addRequest.Error.message} .");
            }

            EditorApplication.update += Poll;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnNoPackagesToInstall() =>
            Debug.LogWarning($"ALERT: No scoped registry packages found to install matching: {targetSearchTerm} .");
    }
}