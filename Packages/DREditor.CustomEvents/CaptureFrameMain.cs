//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.CustomEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class CaptureFrameMain : DialogueEventHandler, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage captureImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent freezeUIVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureCameraEventString = "CaptureMainCamera";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            bool overriding = o != null && (bool)o;

            bool inInstantMode = inInstant != null && inInstant.Value;

            if (!overriding && (inInstantMode || freezeUIVisible.Value))
            {
                UnityEngine.Debug.Log($"REPORT: {nameof(CaptureFrameMain)}: NOTICE: Ignored Trying to Capture Main");

                return;
            }

            UnityEngine.Debug.Log($"REPORT: {nameof(CaptureFrameMain)}: Captured Main, Freeze Image on.");

            DialogueEventSystem.TriggerEvent(captureCameraEventString, captureImage);

            freezeUIVisible.SetValue(true);

            SetCaptureImageToUsableState();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetCaptureImageToUsableState()
        {
            Vector2 v = captureImage.rectTransform.parent.GetComponent<RectTransform>().sizeDelta;

            captureImage.rectTransform.sizeDelta = v;

            captureImage.color = Color.white;
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            captureImage.texture = null;
#if DOTWEEN
            captureImage.DOKill();
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("CaptureMain");
    }
}