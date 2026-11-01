//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class SelectableOption : Selectable
    {
        [Header("")]
        [Divider]
        [Header("")]
        [Divider]
        [Header("")]
        public UnityEvent OnSelection;

        [Header("")]
        public UnityEvent OnDeselection;


        /// <summary>
        ///
        /// </summary>
        protected Sprite normal = null;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();
            
            normal = ((Image)targetGraphic).sprite;
        }

        /// <inheritdoc/>
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            OnSelection?.Invoke();
        }

        /// <inheritdoc/>
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            OnDeselection?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="keepSelected"></param>
        public virtual void KeepSelected(bool keepSelected) 
            => ((Image)targetGraphic).sprite = keepSelected ? spriteState.selectedSprite : normal;
    }
}