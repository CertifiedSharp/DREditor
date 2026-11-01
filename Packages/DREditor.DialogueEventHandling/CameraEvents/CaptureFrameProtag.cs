//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + "\n\n")]
    public class CaptureFrameProtag : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage protagCaptureImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string triggerDialogueEventOnInvoke = "CaptureProtagCamera";


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
            => DialogueEventSystem.TriggerEvent(triggerDialogueEventOnInvoke, protagCaptureImage);


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("CaptureProtag");
    }
}