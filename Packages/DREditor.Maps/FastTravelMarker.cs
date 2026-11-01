//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public class FastTravelMarker : MonoBehaviour, IPlaceMarker
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFastTravel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color selectColor;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color disableColor = Color.gray;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image markerImage = null;


        /// <summary>
        ///
        /// </summary>
        protected Color defaultColor;

        /// <summary>
        ///
        /// </summary>
        protected PlaceEntry currentEntry;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => InitializeColor();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeColor()
        {
            if (markerImage == null)
                enabled = false;
            else
                defaultColor = markerImage.color;

            SetSelectionColor(defaultColor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        protected virtual void SetSelectionColor(Color color)
        {
            if (markerImage != null)
            {
                markerImage.color = currentEntry != null && currentEntry.enableFastTravelForThisEntryInstance ? color : disableColor;

                markerImage.color = currentEntry == null ? color : markerImage.color;

                markerImage.color = !canFastTravel.Value || defaultColor == disableColor ? disableColor : color;
            }
        }


        /// <inheritdoc/>
        public virtual void Initialize(object data) => markerImage.enabled = ((PlaceData)data).fastTravelable;

        /// <inheritdoc/>
        public virtual void LoadEntry(object entry, List<Image> markers)
        {
            currentEntry = (PlaceEntry)entry;

            if (!currentEntry.enableFastTravelForThisEntryInstance || !canFastTravel.Value)
            {
                markerImage.color = disableColor;

                defaultColor = disableColor;
            }
        }

        /// <inheritdoc/>
        public virtual void Select() => SetSelectionColor(selectColor);

        /// <inheritdoc/>
        public virtual void Deselect() => SetSelectionColor(defaultColor);
    }
}