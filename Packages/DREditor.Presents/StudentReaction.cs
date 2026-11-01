//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class StudentReaction : IReactionEntry
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected Character character;

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int reactionIndex = 2;


        public virtual Character Character { get => character; set => character = value; }

        public virtual int ReactionIndex { get => reactionIndex; set => reactionIndex = value; }
    }
}