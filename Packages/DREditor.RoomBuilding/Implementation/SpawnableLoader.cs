//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Spawnables;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<SpawnableStateData, GameObject>) + 
        " for types <" + nameof(SpawnableStateData) + ", " + nameof(GameObject) + ">" +
        "\n\n")]
    public class SpawnableLoader : RoomStateHandlerBase<SpawnableStateData, GameObject>
    {
        /// <inheritdoc/>
        public override List<GameObject> Save() => SaveSpawnables();

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<GameObject> baseDatas, List<GameObject> instanceDatas = null)
        {
            LoadSpawnables(baseDatas);

            if (instanceDatas != null && instanceDatas.Count != 0)
                LoadSpawnables(instanceDatas);
        }

        /// <inheritdoc/>
        public override void Unload(bool inEditor) => UnloadSpawnables(true);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> SaveSpawnables()
        {
            List<GameObject> objects = new();
            var spawnables = FindObjectsByType<Spawnable>();

#if UNITY_EDITOR
            foreach (Spawnable spawnable in spawnables)
                if (PrefabUtility.GetCorrespondingObjectFromSource(spawnable.gameObject) is GameObject prefab &&
                    !objects.Contains(prefab))
                    objects.Add(prefab);
#endif

            return objects;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnables"></param>
        public static void LoadSpawnables(List<GameObject> spawnables)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                SpawnObjects(spawnables);
            else
                LoadSpawnablesInEditor(spawnables);
#else
            SpawnObjects(spawnables);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnables"></param>
        static void SpawnObjects(List<GameObject> spawnables)
        {
            foreach (GameObject gameObject in spawnables)
                SpawnObject(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnable"></param>
        static void SpawnObject(GameObject spawnable)
        {
            if (spawnable != null)
                ClearCloneName(Instantiate(spawnable));
            else
                UnityEngine.Debug.LogWarning("There's a Missing Prefab Reference in this room save!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObject"></param>
        static void ClearCloneName(GameObject gameObject) =>
            gameObject.name = gameObject.name.Replace("(Clone)", "");
#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnables"></param>
        static void LoadSpawnablesInEditor(List<GameObject> spawnables)
        {
            foreach (GameObject spawnable in spawnables)
                if (spawnable != null)
                    PrefabUtility.InstantiatePrefab(spawnable);
                else
                    UnityEngine.Debug.LogWarning("There's a Missing Prefab Reference in this room save!");
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="inEditor"></param>
        public static void UnloadSpawnables(bool inEditor)
        {
            var spawnables = FindObjectsByType<Spawnable>();

            foreach (Spawnable spawnable in spawnables)
                if (inEditor)
                    DestroyImmediate(spawnable.gameObject);
                else
                    Destroy(spawnable.gameObject);
        }
    }
}