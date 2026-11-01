//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.Utility;
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class LayerFlair : IFlairStep
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected LayerMask focusLayer;


        /// <inheritdoc/>
        public virtual void ApplyStep(IFlairEvent eventInstance, GameObject spawnedFlair)
        {
            int layerIndex = focusLayer.GetFirst();

            if (layerIndex == -1)
                return;

            spawnedFlair.layer = layerIndex;

            var children = spawnedFlair.GetComponentsInChildren<Transform>();

            foreach (Transform child in children)
                child.gameObject.layer = layerIndex;
        }
    }
}