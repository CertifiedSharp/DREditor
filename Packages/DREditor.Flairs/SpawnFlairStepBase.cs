//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class SpawnFlairStepBase : IFlairSpawnStep
    {
        /// <inheritdoc/>
        public virtual GameObject SpawnFlair(IFlairEvent eventInstance)
        {
            GameObject prefabToSpawn = GetPrefabToSpawn(eventInstance);

            if (prefabToSpawn == null)
                FailedToSpawnFlair("Either the event or flair inside the event has not been assigned!");

            return prefabToSpawn != null ? Spawn(prefabToSpawn) : null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInstance"></param>
        /// <returns></returns>
        protected virtual GameObject GetPrefabToSpawn(IFlairEvent eventInstance) => eventInstance?.GetFlair()?.GetPrefab();

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        protected virtual GameObject Spawn(GameObject prefab) => MonoBehaviour.Instantiate(prefab);

        /// <summary>
        ///
        /// </summary>
        /// <param name="reason"></param>
        protected virtual void FailedToSpawnFlair(string reason) =>
            UnityEngine.Debug.LogWarning("ALERT: Spawning Flair Failed, Reason: " + reason);
    }
}