//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Mechanics;
using UnityEngine;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DebateEvaluator : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletHandler"></param>
        /// <param name="textPhrase"></param>
        /// <returns></returns>
        public abstract bool Evaluate(IBulletSwapperUI bulletHandler, PhraseBase textPhrase);
    }
}