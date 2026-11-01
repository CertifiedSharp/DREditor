//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Flairs.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class FlairEditorTools
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Flair/Create Flairs From Selected Objects")]
        public static void CreateFlairsFromSelectedObjects()
        {
            if (NoSelectedScriptableObjects(out IEnumerable<GameObject> objects))
                return;

            AssetDatabase.StartAssetEditing();

            try
            {
                CreateFlairs(objects);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();

                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }

            UnityEngine.Debug.Log("REPORT: Process completed.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objects"></param>
        private static void CreateFlairs(IEnumerable<GameObject> objects)
        {
            foreach (var obj in objects)
                if (obj is GameObject prefab)
                    CreateFlair(prefab);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefab"></param>
        private static void CreateFlair(GameObject prefab)
        {
            var instance = CustomEditorUtility.CreateAsset<Flair>();

            instance.name = prefab.name;

            instance.SetPrefab(prefab);

            EditorUtility.SetDirty(instance);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        private static bool NoSelectedScriptableObjects(out IEnumerable<GameObject> objects)
        {
            objects = Selection.objects.OfType<GameObject>();

            bool result = objects.Count() == 0;

            if (result)
                UnityEngine.Debug.LogWarning("ALERT: Process likely aborted since no scriptable objects were selected.");

            return result;
        }
    }
}
#endif