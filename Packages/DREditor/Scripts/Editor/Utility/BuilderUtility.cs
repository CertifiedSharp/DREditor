//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class BuilderUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="targetDirectory"></param>
        public static void CreateAsset(UnityEngine.Object obj, string targetDirectory)
        {
            string folder = FolderUtility.CreateIntermediateFolders(targetDirectory);

            try
            {
                string diaPath = folder + "/" + obj.name + ".asset";

                AssetDatabase.CreateAsset(obj, diaPath);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="folders"></param>
        /// <returns></returns>
        public static T SetAssetByType<T>(string s = "", string[] folders = null) where T : UnityEngine.Object
        {
            if (FindAssetsByType<T>(s, folders) is List<T> databases && databases.Count > 0)
                return databases[0];

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindAssetByType<T>() where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                    return asset;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static List<T2> FindAssetsByInterface<T, T2>() where T : UnityEngine.Object
        {
            List<T2> assets = new();

            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset is T2 foundType)
                    assets.Add(foundType);
            }

            return assets;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new ();

            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                    assets.Add(asset);
            }

            return assets;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static T FindAssetByType<T>(string filter = "") where T : UnityEngine.Object
        {
            if (FindAssetsByType<T>(filter) is List<T> assets && assets.Count > 0)
                return assets[0];

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="folders"></param>
        /// <returns></returns>
        public static List<T> FindAssetsByType<T>(string filter, string[] folders = null) where T : UnityEngine.Object
        {
            List<T> assets = new();

            string[] guids = AssetDatabase.FindAssets(filter, folders);

            //UnityEngine.Debug.Log("GUIDs: " + guids.Length);

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                    assets.Add(asset);
            }

            return assets;
        }
    }
}