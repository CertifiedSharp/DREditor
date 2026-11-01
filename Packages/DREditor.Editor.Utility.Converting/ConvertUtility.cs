//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility.Converting
{
    /// <summary>
    ///
    /// </summary>
    public static class ConvertUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool HasValidIndexFor<T>(this List<T> list, int index) => index >= 0 && index < list.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static string GetFolderPath(this Object asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);

            assetPath = assetPath.Replace("/" + asset.name + ".asset", "");

            return assetPath;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static T GetAssetFromGUID<T>(string guid) where T : UnityEngine.Object
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void CreateAssetAt(UnityEngine.Object obj, string path)
        {
            if (!path.EndsWith(".asset"))
                path += ".asset";

            AssetDatabase.CreateAsset(obj, path);

            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(obj);
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

                if (shellFolder == "")
                {
                    UnityEngine.Debug.LogWarning("Could not create folder. Canceling Initialization.");

                    UnityEngine.Debug.Log("Builder Path: " + assetPath);

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

            return Directory.Exists(fullPath);
        }
    }
}