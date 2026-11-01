//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public class LocationTitle : MonoBehaviour, IDRESaveable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string title;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKey = "location";


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) => saveData.Set(saveKey, title);
    }
}