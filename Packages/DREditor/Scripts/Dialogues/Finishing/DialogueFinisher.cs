//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Dialogues.Finishing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueFinisher : MonoBehaviour, IEndDialogueEvent, IDialogueFinisher
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEvent OnDialogueFinished = new();


        public event Action OnDialogueEnd;


        /// <summary>
        ///
        /// </summary>
        public Action OnFinishedDialogue;


        /// <inheritdoc/>
        public virtual void CallOnDialogueEnd() => OnDialogueEnd?.Invoke();

        /// <inheritdoc/>
        public virtual void CallOnFinishedDialogue()
        {
            OnFinishedDialogue?.Invoke();

            OnDialogueFinished?.Invoke();
        }

        
        /// <inheritdoc/>
        public abstract void FinishDialogue();

        
        /// <inheritdoc/>
        public virtual IEnumerator FinishDialogueRoutine(bool finishDialogue = true)
        {
            FinishDialogue();

            yield break;
        }
        
        /// <inheritdoc/>
        public virtual void SubscribeToOnFinishOfDialogue(Action function) => OnFinishedDialogue += function;

        /// <inheritdoc/>
        public virtual void UnsubscribeToOnFinishOfDialogue(Action function) => OnFinishedDialogue -= function;
    }
}