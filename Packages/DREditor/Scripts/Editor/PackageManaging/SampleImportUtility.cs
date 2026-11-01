//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.UI;
using UnityEngine;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    public static class SampleImportUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="onSampleImportSucceded"></param>
        public static void ImportSamplesBy(Func<List<PackageInfo>> query, Action onSampleImportSucceded)
        {
            if (TryGetMatchingPackages(query, out List<PackageInfo> matchingPackages))
                foreach (var package in matchingPackages)
                    ImportAllFrom(package, onSampleImportSucceded);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="matchingPackages"></param>
        /// <returns></returns>
        private static bool TryGetMatchingPackages(Func<List<PackageInfo>> query, out List<PackageInfo> matchingPackages)
        {
            matchingPackages = query();

            bool result = matchingPackages.Count != 0;

            if (!result)
                Debug.LogWarning($"ALERT: The query to filter packages for sample importing failed!");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="package"></param>
        /// <param name="onSampleImportSucceded"></param>
        public static void ImportAllFrom(PackageInfo package, Action onSampleImportSucceded)
        {
            var samples = Sample.FindByPackage(package.name, package.version);

            if (samples == null || !samples.Any())
                return;

            foreach (var sample in samples)
                if (TryImporting(sample, package))
                    onSampleImportSucceded();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public static bool TryImporting(Sample sample, PackageInfo package)
        {
            bool success = sample.Import(Sample.ImportOptions.OverridePreviousImports);

            if (success)
                Debug.Log($"REPORT: Successfully imported sample '{sample.displayName}' from '{package.name}'");
            else
                Debug.LogError($"ALERT: Failed to import sample '{sample.displayName}' from '{package.name}'");

            return success;
        }
    }
}