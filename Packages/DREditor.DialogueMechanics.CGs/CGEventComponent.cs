//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CGEventComponent<T> : DialogueComponent where T : IDialogueEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgStateEnded = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual IEnumerator TriggerCGEvent(Line currentLine) 
            => DialogueEventUtility.TriggerDialogueEventRoutine<T>(currentLine, EvaluateCGEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        protected virtual IEnumerator EvaluateCGEvent(T display)
        {
            display.TriggerDialogueEvent();

            yield return new WaitUntil(() => cgStateEnded.Value);

            cgStateEnded.SetValue(false);
        }
    }
}