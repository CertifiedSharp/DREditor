//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/States/Asset", fileName = "State Asset")]
    public class StateAsset : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string stateName;
    }
}