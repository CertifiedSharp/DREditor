//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class FolderUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static string GetAssetFolderPath(UnityEngine.Object asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);

            assetPath = assetPath.Replace("/" + asset.name + ".asset", "");

            return assetPath;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static bool TryCreateFolder(ref string assetPath, string assetName)
        {
            assetPath = assetPath.Replace(".asset", "");

            if (!ProjectFolderExists(assetPath + "/" + assetName))
            {
                string shellFolder = AssetDatabase.CreateFolder(assetPath, assetName);

                if (string.IsNullOrEmpty(shellFolder))
                {
                    UnityEngine.Debug.LogWarning("ALERT: Could not create folder. Canceling Initialization.");

                    UnityEngine.Debug.Log("REPORT: Builder Path: " + assetPath);

                    return false;
                }
            }

            assetPath = assetPath + "/" + assetName;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static bool ProjectFolderExists(string relativePath)
        {
            string fullPath = Path.Combine(Application.dataPath, "..", relativePath);

            return !string.IsNullOrEmpty(relativePath) && Directory.Exists(fullPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        public static string CreateIntermediateFolders(string targetDirectory)
        {
            string folderPath = targetDirectory.Replace('\\', '/');

            List<string> folders = folderPath.Split('/').ToList();

            string tempPath = "Assets";

            if (folders[0] == tempPath)
                folders.RemoveAt(0);

            foreach (string folder in folders)
            {
                if (folder.Length == 0)
                    continue;

                if (!Directory.Exists(tempPath + "/" + folder))
                    AssetDatabase.CreateFolder(tempPath, folder);

                tempPath += "/" + folder;
            }

            return tempPath;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool SelectedObjectIsFolder()
        {
            if (Selection.activeObject == null)
            {
                UnityEngine.Debug.LogWarning("ALERT: No object selected.");

                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(assetPath))
            {
                UnityEngine.Debug.LogWarning("ALERT: Selected object does not have a valid asset path.");

                return false;
            }

            return Directory.Exists(assetPath);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startingFolderPath"></param>
        /// <param name="selectedFolderPath"></param>
        /// <param name="relative"></param>
        /// <returns></returns>
        public static bool SelectFolderPanel(string title, string startingFolderPath, out string selectedFolderPath, bool relative = true)
        {
            Func<string, string, string> openFolderPanel = relative ? SelectRelativeFolderPanel : SelectFullFolderPanel;

            selectedFolderPath = openFolderPanel(title, startingFolderPath);

            return ProjectFolderExists(selectedFolderPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startingFolderPath"></param>
        /// <returns></returns>
        private static string SelectRelativeFolderPanel(string title, string startingFolderPath)
        {
            string selectedFolderPath = SelectFullFolderPanel(title, startingFolderPath);

            return GetRelativePath(selectedFolderPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startingFolderPath"></param>
        /// <returns></returns>
        private static string SelectFullFolderPanel(string title, string startingFolderPath)
        {
            string validStartFolder = CreateIntermediateFolders(startingFolderPath);

            string selectedFolderPath = EditorUtility.OpenFolderPanel(title, validStartFolder, "");

            // So folders made while the panels open are still valid :3
            AssetDatabase.Refresh();

            return selectedFolderPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static string GetRelativePath(string fullPath) => fullPath[(Application.dataPath.Length - "Assets".Length)..];
    }
}