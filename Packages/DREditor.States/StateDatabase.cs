//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/States/Database", fileName = "State Database")]
    public class StateDatabase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("states")]
        [Tooltip("")]
        public List<StateAsset> States = new();


        public virtual string GetStateName(int stateIndex) => States[stateIndex].stateName;

        public virtual string[] GetStateNames() => States.Select(state => state.stateName).ToArray();
    }
}