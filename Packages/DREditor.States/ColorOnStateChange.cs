//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ColorOnStateChange : UpdateOnStateChange<Color>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected List<Graphic> graphics = new();


        /// <inheritdoc/>
        protected override void OnStateChanged()
        {
            foreach (Graphic graphic in graphics)
                SetGraphicColor(graphic);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="graphic"></param>
        protected virtual void SetGraphicColor(Graphic graphic) => graphic.color = activeValue;
    }
}