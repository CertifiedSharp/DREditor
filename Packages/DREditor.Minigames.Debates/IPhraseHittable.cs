//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhraseHittable
    {
        /// <summary>
        ///
        /// </summary>
        UnityEvent<IPhraseShatterable, PhraseBase, Vector3> OnHitEvaluate { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitPoint"></param>
        void Hit(Vector3 hitPoint);
    }
}