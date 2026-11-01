//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Gates;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Trials/Trial Builder Asset", fileName = "Trial Asset")]
    public class TrialBuilder : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("TrialSequences")]
        public List<TrialSequence> sequences = new();

        /// <summary>
        ///
        /// </summary>
        public string EndSceneName = "";

        /// <summary>
        ///
        /// </summary>
        public Gate EndGate = null;


        /// <summary>
        ///
        /// </summary>
        public virtual void SetEndSceneName(string EndSceneName) => this.EndSceneName = EndSceneName;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetEndGate(Gate EndGate) => this.EndGate = EndGate;
    }
}