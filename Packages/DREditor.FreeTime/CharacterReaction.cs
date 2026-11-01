//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
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
    [CreateAssetMenu(menuName = "DREditor/FTE/Character Reaction", fileName = "Character Reaction")]
    public class CharacterReaction : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public Character character = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<PresentReaction> Reactions = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public virtual PresentReaction GetReactionFromDialogue(Dialogue dialogue)
        {
            foreach (PresentReaction reaction in Reactions)
                if (reaction.reactions.Contains(dialogue))
                    return reaction;

            return null;
        }
    }
}