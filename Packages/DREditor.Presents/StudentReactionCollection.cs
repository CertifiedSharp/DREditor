//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class StudentReactionCollection : IPresentData
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        public List<IReactionEntry> studentReactions = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual IReactionEntry GetReactionByName(string name)
        {
            foreach (var reaction in studentReactions)
                if (reaction.Character.name == name)
                    return reaction;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="reactionIndex"></param>
        public virtual void SetReaction(string name, int reactionIndex) => GetReactionByName(name).ReactionIndex = reactionIndex;


        /// <inheritdoc/>
        public virtual bool IsValid() => studentReactions != null && studentReactions.Count > 0;
    }
}