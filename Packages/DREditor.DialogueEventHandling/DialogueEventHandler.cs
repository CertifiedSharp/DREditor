//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using UnityEngine;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueEventHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueEventString = "";


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <remarks></remarks>
        public virtual void TriggerEvent(object o = null) => DialogueEventSystem.TriggerEvent(dialogueEventString, o);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => DialogueEventSystem.StartListening(dialogueEventString, OnDialogueEventTriggered);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueEventSystem.StopListening(dialogueEventString, OnDialogueEventTriggered);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <remarks></remarks>
        protected abstract void OnDialogueEventTriggered(object o = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventString"></param>
        protected virtual void SetDefaultEventString(string eventString) => dialogueEventString = eventString;

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Trigger Parameterless Event")]
        public void TriggerParameterlessEvent() => DialogueEventSystem.TriggerEvent(dialogueEventString);
    }
}