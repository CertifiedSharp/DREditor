//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class FlairOffset : IFlairSetting
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector3 position;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Quaternion rotation = Quaternion.identity;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 scale = Vector3.zero;


        /// <inheritdoc/>
        public virtual void ApplySetting(GameObject spawnedFlair)
        {
            Vector3 newPosition = spawnedFlair.transform.localPosition + position;

            Quaternion newRotation = Quaternion.Euler(spawnedFlair.transform.localRotation.eulerAngles + rotation.eulerAngles);

            spawnedFlair.transform.SetLocalPositionAndRotation(newPosition, newRotation);

            spawnedFlair.transform.localScale += scale; 
        }
    }
}