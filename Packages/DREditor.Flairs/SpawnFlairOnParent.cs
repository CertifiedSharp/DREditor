//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SpawnFlairOnParent : SpawnFlairToParent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RectTransform flairSpawnPoint = null;


        /// <inheritdoc/>
        protected override Transform GetFlairParent() => flairSpawnPoint;
    }
}