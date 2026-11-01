//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.UI.Buttons;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Editor.Utility.Converting
{
    /// <summary>
    ///
    /// </summary>
    public static class MigrationUtility
    {
        /// <summary>
        ///
        /// </summary>
        static readonly string MonoscriptSearch = "t:MonoScript ";

        /// <summary>
        ///
        /// </summary>
        static readonly string PrefabSearch = "t:Prefab";

        /// <summary>
        ///
        /// </summary>
        static readonly string ScriptPropertyName = "m_Script";


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Convert/Buttons/Migrate Buttons to DREButton")]
        public static void MigrateButtons() => MigrateType<Button, DREButton>();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public static void MigrateType<T, T2>() where T : Object
        {
            if (FindTypeScript<T2>(out var childScript))
            {
                MigrateSceneObjects<T>(childScript);

                MigratePrefabs<T>(childScript);

                AssetDatabase.SaveAssets();
            }
            else
                UnityEngine.Debug.LogError($"ALERT: {typeof(T2).Name} script not found in project.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childScript"></param>
        /// <returns></returns>
        private static bool FindTypeScript<T>(out MonoScript childScript)
        {
            childScript = AssetDatabase.FindAssets(MonoscriptSearch + typeof(T).Name)
                .Select(guid => AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(s => s != null && s.GetClass() == typeof(T));

            return childScript != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childScript"></param>
        private static void MigrateSceneObjects<T>(MonoScript childScript) where T : Object
            => SwappedReferences(UnityEngine.Object.FindObjectsByType<T>(), childScript);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childScript"></param>
        private static void MigratePrefabs<T>(MonoScript childScript) where T : Object
        {
            foreach (string guid in AssetDatabase.FindAssets(PrefabSearch))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                GameObject prefab = PrefabUtility.LoadPrefabContents(path);

                bool modified = SwappedReferences(prefab.GetComponentsInChildren<Button>(true), childScript);

                if (modified)
                    PrefabUtility.SaveAsPrefabAsset(prefab, path);

                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="references"></param>
        /// <param name="childScript"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool SwappedReferences<T>(IEnumerable<T> references, MonoScript childScript) where T : Object
        {
            bool modified = false;

            foreach (T reference in references.Where(b => b.GetType() == typeof(T)))
            {
                SerializedObject serializedButton = new(reference);

                serializedButton.FindProperty(ScriptPropertyName).objectReferenceValue = childScript;

                serializedButton.ApplyModifiedPropertiesWithoutUndo();

                modified = true;
            }

            return modified;
        }
    }
}
#endif