//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/FTE/Present Reaction", fileName = "Present Reaction")]
    public class PresentReaction : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public int reactionKeyIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<Dialogue> reactions = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<DialogueBase<Line>> GetReactions() => new(reactions);
    }
}