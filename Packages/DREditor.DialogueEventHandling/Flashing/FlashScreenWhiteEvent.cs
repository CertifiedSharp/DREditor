//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.DialogueEventHandling.Flashing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(FlashEventHandlerBase)  + ", " + nameof(FlashEventSimpleHandlerBase) +
        "\n\n")]
    public class FlashScreenWhiteEvent : FlashEventSimpleHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Graphic flashWhiteGraphic = null;


        /// <inheritdoc/>
        protected override bool FailFlash()
        {
            bool result = flashWhiteGraphic == null;

            if (result)
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(FlashScreenWhiteEvent)} " +
                    $"doesn't have an graphic referenced! Ignoring Event.");

            return result;
        }

        /// <inheritdoc/>
        protected override void FlashUpdate(float value)
        {
            if (value < 0)
                value *= -1;

            flashWhiteGraphic.SetFade(value);
        }

        /// <inheritdoc/>
        protected override void ResetFlash() => flashWhiteGraphic.SetFade(0);

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public virtual void SetDialogueString() => SetDefaultEventString("FlashDialogueScreenWhite");

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set CG Event")]
        public virtual void SetCGString() => SetDefaultEventString("CGFlash");
    }
}