//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling.Reticles;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HoverReticle) + ", " + nameof(Reticle) +
        "\n\n")]
    public class SpotSelectionReticle : HoverReticle
    {
        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            Cursor.lockState = CursorLockMode.Confined;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (locked || hoveredObject != null)
                return;

            HoverObject(collision.transform, collision);
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (locked || hoveredObject != null)
                return;

            HoverObject(collision.transform, collision);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (locked || collision != GetHoveredObject<Collider2D>())
                return;

            StopHoveringObject();
        }
    }
}