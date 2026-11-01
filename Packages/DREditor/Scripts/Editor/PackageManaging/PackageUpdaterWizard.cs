//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    public class PackageUpdaterWizard : RequestPackageWizardBase<PackageCollection>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string targetSearchTerm = "net.dreditor.";


        /// <summary>
        ///
        /// </summary>
        protected List<string> packagesToUpdate;


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Dependencies/Update Package Wizard")]
        public static void DisplaySampleImporter() => DisplayWizard<PackageUpdaterWizard>("Update Package Wizard", "Update");

        protected override Request<PackageCollection> CreateRequest() => Client.List(false, false);

        protected override void OnRetrievingPackagesSucceded()
        {
            InitializePackagesToUpdate();

            if (ThereArePackagesToUpdate())
                UpdateAllPackages();
            else
                OnNoPackagesToUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePackagesToUpdate() =>
            packagesToUpdate = request.Result
                .Where(package => PackageUtility.PackageContainsSearchTerm(package, targetSearchTerm))
                .Select(package => package.name)
                .ToList();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ThereArePackagesToUpdate() => packagesToUpdate.Count > 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnNoPackagesToUpdate() => Debug.Log("REPORT: No installed packages found for scope: " + targetSearchTerm);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateAllPackages() => CreateRequester().Import(CreateUpdateRequest());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PackageRequester<PackageCollection> CreateRequester() => new(OnRequestCompleted);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Request<PackageCollection> CreateUpdateRequest() => Client.AddAndRemove(packagesToUpdate.ToArray(), null);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRequestCompleted() => Debug.Log("REPORT: Updating all packages successful.");
    }
}