//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using UnityEngine;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    public class DirectToTransitionalEndEventHandler : DirectToEndEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneTransitionEndEventHandler SceneTransitionEndEventHandler = null;


        /// <inheritdoc/>
        protected override void OnDirectToValid<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
        {
            base.OnDirectToValid(currentDialogue, interruptDialogue);

            if (currentDialogue.SceneTransition.Enabled && !currentDialogue.SceneTransition.AtEnd)
                SceneTransitionEndEventHandler.SetSaveTransitionNum(saveDirectToNum);
        }
    }
}