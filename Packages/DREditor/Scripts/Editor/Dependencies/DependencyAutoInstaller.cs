//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    public static class DependencyAutoInstaller
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly string AutoInstallKey = "AutoInstalledDependencies";

        /// <summary>
        ///
        /// </summary>
        private static readonly Dictionary<string, string> RequiredDependencies = new()
        {
            { "com.unity.nuget.newtonsoft-json", "com.unity.nuget.newtonsoft-json@3.2.2" },
            { "com.unity.cinemachine", "com.unity.cinemachine@2.10.7" },
        };


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool Initialized() => EditorPrefs.GetBool(AutoInstallKey);

        /// <summary>
        ///
        /// </summary>
        public static void Install() => EditorApplication.delayCall += StartPreCheck;

        /// <summary>
        ///
        /// </summary>
        private static void StartPreCheck()
        {
            var ListRequest = Client.List(offlineMode: true);

            void MonitorList()
            {
                if (ListRequest == null || !ListRequest.IsCompleted) 
                    return;

                EditorApplication.update -= MonitorList;

                if (ListRequest.Status == StatusCode.Success)
                    ProcessNextPackage(new InstallationState(CreateInstallQueue(ListRequest)));
                else
                    Debug.LogError($"ALERT: Failed to scan project packages: {ListRequest.Error.message}");
            }

            EditorApplication.update += MonitorList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ListRequest"></param>
        /// <returns></returns>
        private static Queue<string> CreateInstallQueue(ListRequest ListRequest)
        {
            var installedPackageIds = ListRequest.Result.Select(package => package.name).ToList();

            var installQueue = new Queue<string>();

            foreach (KeyValuePair<string, string> dependency in RequiredDependencies)
                if (!installedPackageIds.Contains(dependency.Key) &&
                    EditorUtility.DisplayDialog("Dependency Missing",
                    $"ALERT: {dependency.Key} missing! Add to download queue?",
                        "Yes", "No"))
                    installQueue.Enqueue(dependency.Value);

            return installQueue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        private static void ProcessNextPackage(InstallationState state)
        {
            if (state.PackageQueueInvalid())
                return;

            string packageTarget = state.PackagesToInstall.Dequeue();

            state.CurrentAddRequest = Client.Add(packageTarget);

            void MonitorInstall()
            {
                if (state.AddRequestInvalid()) 
                    return;

                EditorApplication.update -= MonitorInstall;

                if (state.CurrentRequestSucceded())
                    Debug.Log($"REPORT: Successfully installed: {state.CurrentAddRequest.Result.name}");
                else
                    Debug.LogError($"ALERT: Failed to download dependency. Error: {state.CurrentAddRequest.Error.message}");

                state.CurrentAddRequest = null;

                ProcessNextPackage(state);
            }

            EditorApplication.update += MonitorInstall;
        }
    }
}