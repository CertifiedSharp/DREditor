//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class DatabaseUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static bool InitializeDatabase<T>(T variable, out T database) where T : UnityEngine.Object
        {
            database = variable;

            if (database == null)
                return GetDatabase(out database) == 1;
            else
                return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <returns></returns>
        private static int GetDatabase<T>(out T database) where T : UnityEngine.Object
        {
            string[] _databaseguids = AssetDatabase.FindAssets($"t:{typeof(T)}");

            database = null;

            if (_databaseguids.Length == 1)
            {
                database = LoadDatabaseAtPath<T>(AssetDatabase.GUIDToAssetPath(_databaseguids[0]));

                return 1;
            }
            else if (_databaseguids.Length > 1)
            {
                OnMultipleDatabasesFound<T>(_databaseguids);

                return 2;
            }
            else
            {
                OnDatabaseMissing<T>();

                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_databasepath"></param>
        /// <returns></returns>
        private static T LoadDatabaseAtPath<T>(string _databasepath) where T : UnityEngine.Object
        {
            try
            {
                EditorGUILayout.LabelField($"Loading {typeof(T)} at {_databasepath}.");
            }
            catch { }

            return AssetDatabase.LoadAssetAtPath<T>(_databasepath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_databaseguids"></param>
        private static void OnMultipleDatabasesFound<T>(string[] _databaseguids)
        {
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField($"There are more than one {typeof(T)} Assets found in your project.");

                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("Here are the path of all the files:");

                for (int i = 0; i < _databaseguids.Length; i++)
                {
                    string _path = AssetDatabase.GUIDToAssetPath(_databaseguids[i]);

                    EditorGUILayout.LabelField($"	- {_path}");
                }

                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField($"Only one {typeof(T)} is allowed. " +
                    "Please delete all the duplicates until one remains.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void OnDatabaseMissing<T>()
        {
            if (typeof(T).GetCreateAssetMenuName() is string createAssetMenuPath && createAssetMenuPath != "")
                using (new EditorGUILayout.VerticalScope())
                    DatabaseMissingLabels<T>(createAssetMenuPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createAssetMenuPath"></param>
        private static void DatabaseMissingLabels<T>(string createAssetMenuPath)
        {
            EditorGUILayout.LabelField($"There is no {typeof(T)} Asset found in your project.");

            EditorGUILayout.LabelField("Please create one by right clicking in the Project Window and navigating to: " +
                $"\n {createAssetMenuPath}", GUILayout.Height(50));

            EditorGUILayout.LabelField($"Do not create more than one. Only one {typeof(T)} is allowed.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetCreateAssetMenuName(this Type type) =>
            type.GetCustomAttribute<CreateAssetMenuAttribute>()?.menuName;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadDatabase<T>() where T : UnityEngine.Object
        {
            Type type = typeof(T);

            string[] databaseguids = AssetDatabase.FindAssets("t:" + type.ToString());

            if (databaseguids.Length > 0)
            {
                string databasepath = AssetDatabase.GUIDToAssetPath(databaseguids[0]);

                try
                {
                    EditorGUILayout.LabelField($"Loading {type} at {databasepath}.");
                }
                catch { }

                return AssetDatabase.LoadAssetAtPath<T>(databasepath);
            }

            UnityEngine.Debug.LogWarning("ALERT: Could not load database of type: " + type.Name);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static bool TryCreateDatabaseAt<T>(string folderPath, out T database) where T : ScriptableObject
        {
            UnityEngine.Debug.Log($"REPORT: Couldn't find Database of Type {typeof(T).Name}, spawning new one.");

            folderPath = FolderUtility.CreateIntermediateFolders(folderPath);

            T newDatabase = ScriptableObject.CreateInstance<T>();

            string databasePath = folderPath + $"/{typeof(T).Name}.asset";

            AssetDatabase.CreateAsset(newDatabase, databasePath);

            database = newDatabase;

            bool result = database != null;

            if (result)
                UnityEngine.Debug.Log($"REPORT: New {typeof(T).Name} Creation Complete.");
            else
                UnityEngine.Debug.LogWarning($"ALERT: Creating New {typeof(T).Name} Failed!");

            return result;
        }
    }
}