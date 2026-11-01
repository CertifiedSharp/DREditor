//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<LetterBoxCanvasHolder>) +
        "\n\n")]
    public class LetterBoxCanvasHolder : ServiceHolder<LetterBoxCanvasHolder>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float aspectRatioX = 16;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float aspectRatioY = 9;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            CheckAspectRatio();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckAspectRatio()
        {
            float ratioX = Screen.width % aspectRatioX;

            float ratioY = Screen.height % aspectRatioY;

            bool nativeResolution = ratioX == 0 && ratioY == 0;

            if (GetComponent<Canvas>() is Canvas canvas)
                canvas.enabled = !nativeResolution;
        }
    }
}