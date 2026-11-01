//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public class MapMarkerSelectable : Selectable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string markerName = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color availableColor = Color.white;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color unavailableColor = Color.white;


        [Header("")]
        public UnityEvent<string> OnHover;


        /// <inheritdoc/>
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            OnHover?.Invoke(markerName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string GetMarkerName() => markerName;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetAvailable()
        {
            if (targetGraphic != null)
                targetGraphic.color = availableColor;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SetUnavailable()
        {
            if (targetGraphic != null)
                targetGraphic.color = unavailableColor;
        }
    }
}