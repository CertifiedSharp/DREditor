//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    public static class DependencyUtility
    {
        /// <summary>
        ///
        /// </summary>
        public static void ImportPackage() => EditorApplication.ExecuteMenuItem("Assets/Import Package/Custom Package...");

        /// <summary>
        ///
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="defineSymbol"></param>
        /// <returns></returns>
        public static bool AddedDependency(string assemblyName, string defineSymbol)
        {
            bool dependencyUsed = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetName().Name)
                .Contains(assemblyName);

            var buildTarget = UnityEditor.Build.NamedBuildTarget.Standalone;

            string currentDefines = UnityEditor.PlayerSettings.GetScriptingDefineSymbols(buildTarget);

            bool definesAlreadyAdded = currentDefines.Contains(defineSymbol);

            if (dependencyUsed && definesAlreadyAdded)
                return true;

            bool definesNowAdded = dependencyUsed && !definesAlreadyAdded;

            if (definesNowAdded)
            {
                string newDefines = string.IsNullOrEmpty(currentDefines)
                    ? defineSymbol
                    : currentDefines + ";" + defineSymbol;

                UnityEditor.PlayerSettings.SetScriptingDefineSymbols(buildTarget, newDefines);
            }

            return definesNowAdded;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [MenuItem("Assets/Find Prefabs Referencing This Scriptable Object", true, 30)]
        public static bool FindPrefabsValidate() => Selection.activeObject as ScriptableObject != null;

        /// <summary>
        ///
        /// </summary>
        [MenuItem("Assets/Find Prefabs Referencing This Scriptable Object", false, 30)]
        public static void FindPrefabs()
        {
            ScriptableObject selectedSO = Selection.activeObject as ScriptableObject;

            LogPrefabsReferencing(selectedSO, GetPrefabsReferencing(selectedSO));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptableObject"></param>
        /// <returns></returns>
        private static List<GameObject> GetPrefabsReferencing(ScriptableObject scriptableObject)
        {
            string soPath = AssetDatabase.GetAssetPath(scriptableObject);

            string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");

            List<GameObject> referencingPrefabs = new();

            foreach (string guid in allPrefabGuids)
                AttemptAddingSODependency(soPath, guid, referencingPrefabs);

            return referencingPrefabs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="soPath"></param>
        /// <param name="guid"></param>
        /// <param name="prefabs"></param>
        private static void AttemptAddingSODependency(string soPath, string guid, List<GameObject> prefabs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);

            string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);

            foreach (string dependency in dependencies)
            {
                if (dependency == soPath)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                    prefabs.Add(prefab);

                    break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptableObject"></param>
        /// <param name="prefabs"></param>
        private static void LogPrefabsReferencing(ScriptableObject scriptableObject, List<GameObject> prefabs)
        {
            Debug.Log($"REPORT: <b>{prefabs.Count} Prefabs referencing {scriptableObject.name}:</b>");

            foreach (var prefab in prefabs)
                Debug.Log($"REPORT: {scriptableObject.name} is referenced in Prefab: {prefab.name}", prefab);
        }
    }
}