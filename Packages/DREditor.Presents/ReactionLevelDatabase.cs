//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Presents/Reaction Level Database", fileName = "Reaction Level Database")]
    public class ReactionLevelDatabase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<PresentReactionLevel> reactionLevels = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetReactionNames() => reactionLevels.Select(reactionLevel => reactionLevel.name).ToArray();

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool ReactionIndexValid(int index) => index >= 0 && index < reactionLevels.Count;
    }
}