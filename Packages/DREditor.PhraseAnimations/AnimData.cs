//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class AnimData
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        public IPhraseAnimationType animationType;

        /// <summary>
        ///
        /// </summary>
        public int typeIndex;


        /// <summary>
        ///
        /// </summary>
        /// <param name="animationType"></param>
        public virtual void SetType(IPhraseAnimationType animationType) => this.animationType = animationType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeIndex"></param>
        public virtual void SetIndex(int typeIndex) => this.typeIndex = typeIndex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        public virtual void Initialize(AnimationClip clip) => animationType?.Initialize(clip);
    }
}