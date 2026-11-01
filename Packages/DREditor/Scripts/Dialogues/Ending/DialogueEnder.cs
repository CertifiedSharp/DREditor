//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueEnder : MonoBehaviour, IDialogueEnder
    {
        public event Action OnDialogueResolving;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveDialogueEventHandler() => OnDialogueResolving?.Invoke();

        
        /// <inheritdoc/>
        public abstract void StopDialogue();

        /// <inheritdoc/>
        public abstract IEnumerator EndDialogue(bool useDialogueFinisher = true);

        /// <inheritdoc/>
        public abstract void FinishDialogue();
    }
}