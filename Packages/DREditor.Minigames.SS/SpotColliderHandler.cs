//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SpotSelectionComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class SpotColliderHandler : SpotSelectionComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform parentTransform = null;


        /// <summary>
        ///
        /// </summary>
        protected List<BoxCollider2D> colliders = new();


        /// <inheritdoc/>
        public override void Initialize(SpotSelectionBuilder builder)
        {
            for (int i = 0; i < builder.spots.Count; i++)
            {
                Spot spot = builder.spots[i];

                GameObject gameObject = new(spot.spotName);

                gameObject.transform.parent = parentTransform;

                gameObject.transform.localScale = new Vector3(1, 1, 1);

                gameObject.transform.position = spot.position * parentTransform.localScale.x;

                gameObject.transform.eulerAngles = spot.rotation;

                BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

                boxCollider.size = spot.size;

                boxCollider.offset = spot.center;

                colliders.Add(boxCollider);
            }
        }

        /// <inheritdoc/>
        public override void Show() => colliders.ForEach(collider => collider.enabled = true);

        /// <inheritdoc/>
        public override void Hide() => colliders.ForEach(collider => collider.enabled = false);

        /// <inheritdoc/>
        public override IEnumerator WaitForChangeToComplete()
        {
            yield break;
        }
    }
}