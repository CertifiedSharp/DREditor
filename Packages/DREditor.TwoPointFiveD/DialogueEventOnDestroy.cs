//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class DialogueEventOnDestroy : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> dialogueOnDestroyEvents = new() { "ResetProtagCamera" };


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy()
        {
            foreach (string dialogueEvent in dialogueOnDestroyEvents)
                DialogueEventSystem.TriggerEvent(dialogueEvent);
        }


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Default Protag Event")]
        public virtual void DebugPopulateDefaultProtagEvent() => dialogueOnDestroyEvents.Add("ResetProtagCamera");
    }
}