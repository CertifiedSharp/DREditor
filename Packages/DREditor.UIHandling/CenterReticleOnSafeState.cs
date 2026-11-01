//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public class CenterReticleOnSafeState : ConditionOnSceneEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform reticle = null;

        /// <inheritdoc/>
        protected override void OnPassedCondition() => reticle.localPosition = Vector3.zero;
    }
}