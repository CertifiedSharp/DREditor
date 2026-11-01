//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class SpawnFlairToParent : SpawnFlairStepBase
    {
        /// <inheritdoc/>
        protected override GameObject Spawn(GameObject prefab) =>
            TryGetFlairParent(out Transform validParent) ? MonoBehaviour.Instantiate(prefab, validParent) : base.Spawn(prefab);


        /// <summary>
        ///
        /// </summary>
        /// <param name="flairParent"></param>
        /// <returns></returns>
        private bool TryGetFlairParent(out Transform flairParent)
        {
            flairParent = GetFlairParent();

            if (flairParent == null)
                FailedToSpawnFlair($"The assigned flair parent was null!");

            return flairParent;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract Transform GetFlairParent();
    }
}