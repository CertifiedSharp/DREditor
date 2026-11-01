//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FireTruthAnimationBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        public abstract IEnumerator FireAnimation();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract float CalculateRemainingDuration();
    }
}