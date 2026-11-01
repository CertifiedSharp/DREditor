//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjectServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UpdateOnStateChange<Sprite>) + ", " + nameof(IntWithEventListener) + ", " + nameof(IStateList) +
        "\n\n")]
    public class SpriteOnStateChange : UpdateOnStateChange<Sprite>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<MaskableGraphic> graphics = new();


        /// <inheritdoc/>
        protected override void OnStateChanged()
        {
            foreach (var graphic in graphics)
                SetTexture(graphic, activeValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="sprite"></param>
        protected virtual void SetTexture(MaskableGraphic graphic, Sprite sprite)
        {
            if (graphic is Image image)
                image.sprite = sprite;
            else if (graphic is RawImage rawImage)
                rawImage.texture = sprite.texture;
        }


        /// <inheritdoc/>
        public override void OnInitializeStateList()
        {
            base.OnInitializeStateList();

            if (graphics.Count == 0 && GetComponent<MaskableGraphic>() is MaskableGraphic validGraphic)
                graphics.Add(validGraphic);
        }
    }
}