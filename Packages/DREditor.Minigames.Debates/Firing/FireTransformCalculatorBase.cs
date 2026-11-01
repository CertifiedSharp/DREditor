//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FireTransformCalculatorBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 CalculateFirePosition();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract Quaternion CalculateFireRotation();
    }
}