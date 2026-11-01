//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Processables;
using UnityEngine;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ReticleComponent) +
        "\n\n")]
    public class ObjectNameOnReticle : ReticleComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentDisplayName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform currentlyHoveredObject = null;


        /// <inheritdoc/>
        public override void ReticleVisibilityChanged(bool to) { }

        /// <inheritdoc/>
        public override void OnHoveringObject(Transform hit)
        {
            if (hit != currentlyHoveredObject)
            {
                currentDisplayName.SetValue(GetHitDisplayName(hit));

                currentlyHoveredObject = hit;
            }
        }

        /// <inheritdoc/>
        public override void OnSelected(Transform hit) => OnStoppedHoveringObject(hit);

        /// <inheritdoc/>
        public override void OnStoppedHoveringObject(Transform hit)
        {
            currentlyHoveredObject = null;

            currentDisplayName.SetValue("");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        protected virtual string GetHitDisplayName(Transform hit)
        {
            var displayable = hit.GetComponent<INameDisplayable>();

            return displayable != null ? displayable.DisplayName : hit.gameObject.name;
        }
    }
}