//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class TrialSequence
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public int SequenceTypeNumber;

        /// <summary>
        ///
        /// </summary>
        public string SequenceDescription;

        /// <summary>
        ///
        /// </summary>
        public ScriptableObject MinigameAsset;

        /// <summary>
        ///
        /// </summary>
        public MinigameType MinigameType;


        public TrialSequence() { }


        public TrialSequence(ScriptableObject MinigameAsset) => this.MinigameAsset = MinigameAsset;


        /// <summary>
        ///
        /// </summary>
        public virtual void SetSequenceTypeNumber(int SequenceTypeNumber) => this.SequenceTypeNumber = SequenceTypeNumber;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetSequenceDescription(string SequenceDescription) => this.SequenceDescription = SequenceDescription;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetMinigameAsset(ScriptableObject MinigameAsset) => this.MinigameAsset = MinigameAsset;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetMinigameType(MinigameType MinigameType) => this.MinigameType = MinigameType;
    }
}