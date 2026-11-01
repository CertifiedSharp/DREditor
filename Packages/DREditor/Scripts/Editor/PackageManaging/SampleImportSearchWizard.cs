//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System.Collections.Generic;
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
    public class SampleImportSearchWizard : RequestPackageWizardBase<PackageInfo[]>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string targetSearchTerm = "net.dreditor";


        /// <summary>
        ///
        /// </summary>
        protected int importedCount = 0;


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Import/Samples via Search", priority = 1)]
        public static void DisplaySampleImporter() => DisplayWizard<SampleImportSearchWizard>("Import Samples via Search", "Import");


        protected override Request<PackageInfo[]> CreateRequest() => Client.SearchAll();

        protected override void OnRetrievingPackagesSucceded()
        {
            InitializeImportingSamples();

            ImportSamples();

            ResolveImportingSamples();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeImportingSamples() => importedCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportSamples() => SampleImportUtility.ImportSamplesBy(GetMatchingPackages, OnSampleImportSuccess);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSampleImportSuccess() => importedCount++;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<PackageInfo> GetMatchingPackages() => PackageUtility.GetMatchingPackagesFrom(request, targetSearchTerm);

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveImportingSamples()
        {
            Debug.Log($"REPORT: Finished importing {importedCount} sample(s).");

            AssetDatabase.Refresh();
        }
    }
}