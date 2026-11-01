//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IntWithEventListener) +
        "\n\n")]
    public class SpriteOnIntEvent : IntWithEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setOnAwake = true;
        

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<MaskableGraphic> graphics = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Sprite> spritesByIndex = new();


        protected virtual void Awake()
        {
            if (setOnAwake)
                OnValueChanged(valueWithEvent.Value);
        }


        /// <inheritdoc/>
        protected override void OnValueChanged(int value)
        {
            if (graphics.Count > 0 && TryGetComponent(out MaskableGraphic validGraphic))
                graphics.Add(validGraphic);

            if (value < spritesByIndex.Count)
                foreach (var graphic in graphics)
                    SetTexture(graphic, spritesByIndex[value]);
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
    }
}