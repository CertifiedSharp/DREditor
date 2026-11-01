// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PageInitializerBase : PageHandlerBase, IClosingArgumentResetable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 pagesRotation = new(0, 0, -3.9f); //new(0, 0, -3.934f);

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector3 pagesResetLocalPosition = new(0, 0.93f, 0); //new(-0.49f, -3.3f, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 pagesResetRotation = Vector3.zero;


        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateRotation() => pages.transform.rotation = Quaternion.Euler(pagesRotation);


        /// <inheritdoc/>
        public virtual void CAReset()
        {
            ResetPositionAndRotation();

            DestroyPageChildren();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetPositionAndRotation()
        {
            pages.transform.localPosition = pagesResetLocalPosition;

            pages.transform.rotation = Quaternion.Euler(pagesResetRotation);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroyPageChildren()
        {
            foreach (Transform child in pages.transform)
                Destroy(child.gameObject);
        }
    }
}