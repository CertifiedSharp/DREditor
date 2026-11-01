//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PanelCounterBase : MonoBehaviour, IDebateUIVisibility
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float visibleValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float hiddenValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float initializedShowDuration = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float panelCounterShowDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float panelCounterHideDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected bool initialized = false;


        /// <inheritdoc/>
        public virtual void Show()
        {
            float duration = initialized ? panelCounterShowDuration : initializedShowDuration;

            if (!initialized)
                initialized = true;

            ChangeVisibility(visibleValue, duration);
        }

        /// <inheritdoc/>
        public virtual void Hide() => ChangeVisibility(hiddenValue, panelCounterHideDuration);


        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        protected abstract void ChangeVisibility(float to, float duration);


        /// <inheritdoc/>
        public abstract IEnumerator WaitForChangeToComplete();
    }
}