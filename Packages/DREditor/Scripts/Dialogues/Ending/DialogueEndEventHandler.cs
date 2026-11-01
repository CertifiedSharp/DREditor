//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueEndEventHandler : MonoBehaviour, IDialogueInteruptable, ISceneEventListener
    {
        public event Action<DialogueCore[]> OnInteruptWithNewDialogue;


        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent cleanUpDialogueReader = null;


        /// <summary>
        ///
        /// </summary>
        protected Action OnReaderShouldCleanUp;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => cleanUpDialogueReader.RegisterListener(this);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => cleanUpDialogueReader.UnregisterListener(this);

        /// <inheritdoc/>
        public virtual void DialogueInterruptedInvoke(DialogueCore[] dialogues) => OnInteruptWithNewDialogue.Invoke(dialogues);

        /// <summary>
        ///
        /// </summary>
        /// <param name="function"></param>
        public virtual void SubscribeToCleanUp(Action function) => OnReaderShouldCleanUp += function;

        /// <summary>
        ///
        /// </summary>
        /// <param name="function"></param>
        public virtual void UnsubscribeToCleanUp(Action function) => OnReaderShouldCleanUp -= function;


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogueCore"></param>
        /// <param name="exitParentRoutine"></param>
        public abstract IEnumerator EndDialogueEventOptions<T>(DialogueBase<T> currentDialogueCore, Action exitParentRoutine)
            where T : Line;


        /// <inheritdoc/>
        public virtual void OnEventRaised() => OnReaderShouldCleanUp?.Invoke();
    }
}