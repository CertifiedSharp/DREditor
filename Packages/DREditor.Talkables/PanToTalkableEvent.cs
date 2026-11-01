//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using UnityEngine;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class PanToTalkableEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string toggleBlurEventString = "ToggleBlur";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueEventStringMoveCamera = "PanToObject";


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (o == null)
                UnityEngine.Debug.LogWarning($"REPORT: {ToString()}: Sent talkable was null.");

            Talkable talkable = (Talkable)o;

            if (talkable is not ItemActor)
                DialogueEventSystem.TriggerEvent(toggleBlurEventString, true);

            if (talkable != null)
                DialogueEventSystem.TriggerEvent(dialogueEventStringMoveCamera, talkable.body);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("PanToTalkable");
    }
}