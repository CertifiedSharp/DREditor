//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IEarlyResolvableDialogueComponent) +
        "\n\n")]
    public class ResolveEarlyCG : DialogueComponent, IEarlyResolvableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideCGStateEventString = "HideVideo";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGStateMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgStateEnded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;


        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator ResolveEarly()
        {
            if (inCGStateMode.Value && !inTrialMode.Value)
            {
                DialogueEventSystem.TriggerEvent(hideCGStateEventString);

                yield return new WaitUntil(() => !inCGStateMode.Value && cgStateEnded.Value);

                cgStateEnded.SetValue(false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventString"></param>
        protected virtual void SetDefaultEventString(string eventString) => hideCGStateEventString = eventString;
    }
}