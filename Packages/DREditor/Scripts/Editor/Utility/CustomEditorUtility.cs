//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public static class CustomEditorUtility
    {
        #region Asset Creation

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T CreateAsset<T>(string name = "NewAsset") where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
                path = "Assets";

            if (!System.IO.Directory.Exists(path))
                path = System.IO.Path.GetDirectoryName(path);

            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{name}.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            return asset;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="sentPath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T CreateAssetWithFolders<T>(string fileName = null, string sentPath = null) where T : ScriptableObject
        {
            fileName = CreateValidFileName<T>(fileName);

            string assetPath = CreateValidFolderPathForFile(sentPath, fileName);

            UnityEngine.Debug.Log("REPORT: Creating file: " + fileName + " at path: " + assetPath + fileName);

            T asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, assetPath + fileName);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();

            return asset;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static string CreateValidFileName<T>(string fileName) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = typeof(T).Name + ".asset";
            else if (!fileName.Contains(".asset"))
                fileName += ".asset";

            return fileName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sentPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static string CreateValidFolderPathForFile(string sentPath, string fileName)
        {
            string assetPath = sentPath;

            if (string.IsNullOrEmpty(sentPath))
                assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(GetPathFromSelectedObject(), fileName));
            else if (Path.GetDirectoryName(sentPath) is string directory && !AssetDatabase.IsValidFolder(directory))
                EnsureFolders(directory);

            return assetPath.Replace(fileName, "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        static string GetPathFromSelectedObject()
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);

                if (File.Exists(path))
                    path = Path.GetDirectoryName(path);

                break;
            }

            return path;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        public static void EnsureFolders(string path)
        {
            string[] folders = path.Split('/');

            string currentPath = "";

            foreach (string folder in folders)
            {
                if (string.IsNullOrEmpty(folder))
                    continue;

                currentPath = string.IsNullOrEmpty(currentPath) ? folder : Path.Combine(currentPath, folder);

                if (!AssetDatabase.IsValidFolder(currentPath))
                {
                    string parent = Path.GetDirectoryName(currentPath).Replace("\\", "/");

                    AssetDatabase.CreateFolder(parent, Path.GetFileName(currentPath));
                }
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToDuplicate"></param>
        /// <param name="buildPathWith"></param>
        /// <param name="duplicatedObject"></param>
        /// <returns></returns>
        public static bool TryDuplicateAsset<T>(T objectToDuplicate, Func<string, string, string> buildPathWith, out T duplicatedObject)
            where T : UnityEngine.Object
        {
            duplicatedObject = null;

            if (objectToDuplicate == null)
                return false;

            string objectToDuplicatePath = AssetDatabase.GetAssetPath(objectToDuplicate);

            string newDuplicatedPath = buildPathWith(objectToDuplicate.name, objectToDuplicatePath);

            bool result = AssetDatabase.CopyAsset(objectToDuplicatePath, newDuplicatedPath);

            if (result)
                duplicatedObject = AssetDatabase.LoadAssetAtPath<T>(newDuplicatedPath);
            else
                Debug.LogWarning("ALERT: Failed to create asset at path: " + newDuplicatedPath);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="removeAssetPrefix"></param>
        /// <returns></returns>
        public static string GetCurrentProjectWindowPath(bool removeAssetPrefix = true)
        {
            MethodInfo getActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

            string path = (string)getActiveFolderPath.Invoke(null, null);

            return removeAssetPrefix ? path.Replace("Assets/", "") : path;
        }

        #endregion

        #region Scene Management in Editor

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<SceneAsset> GetAllValidSceneAssets()
        {
            string[] validScenes = GetAllValidScenes();

            List<SceneAsset> scenes = new();

            foreach (string scene in validScenes)
                scenes.Add(GetAssetFromGUID<SceneAsset>(scene));

            return scenes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        static string[] GetAllValidScenes() => AssetDatabase.FindAssets("t:scene");

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid"></param>
        /// <returns></returns>
        static T GetAssetFromGUID<T>(string guid) where T : UnityEngine.Object
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public static void LoadScene(SceneAsset obj)
        {
            string scenePath = AssetDatabase.GetAssetPath(obj);

            EditorSceneManager.OpenScene(scenePath);
        }

        #endregion

        #region Misc Function(s)

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T DeepCopyJson<T>(T input)
        {
            string jsonString = JsonUtility.ToJson(input);

            T clonedObject = JsonUtility.FromJson<T>(jsonString);

            return clonedObject;
        }

        #endregion
    }
}