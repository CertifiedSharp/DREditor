//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ReticleComponent : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        public abstract void ReticleVisibilityChanged(bool to);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        public abstract void OnHoveringObject(Transform hit);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        public abstract void OnStoppedHoveringObject(Transform hit);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        public abstract void OnSelected(Transform hit);
    }
}