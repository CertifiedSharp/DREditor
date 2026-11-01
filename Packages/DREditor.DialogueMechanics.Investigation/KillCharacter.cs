//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class KillCharacter : IDialogueEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string firstName = string.Empty;


        /// <inheritdoc/>
        public virtual void TriggerDialogueEvent() => DialogueEventSystem.TriggerEvent(nameof(KillCharacter), firstName);
    }
}