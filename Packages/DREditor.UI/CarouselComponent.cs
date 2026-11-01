//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.UIHandling;
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public class CarouselComponent : UIVisibilityHandler, IInitializable, IResetable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected Vector3 resetPosition;


        /// <inheritdoc/>
        public virtual void Initialize() => resetPosition = transform.localPosition;

        /// <inheritdoc/>
        public virtual void OnReset() => transform.localPosition = resetPosition;

        public virtual void OnFocusStateUpdated(bool isFocused)
        {
            if (isFocused)
                visibleUI?.Show();
            else
                visibleUI?.Hide();
        }
    }
}