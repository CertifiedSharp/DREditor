//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TiltCalculatorBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="firePosition"></param>
        /// <returns></returns>
        public abstract float CalculateFiringTilt(Vector3 firePosition);
    }
}