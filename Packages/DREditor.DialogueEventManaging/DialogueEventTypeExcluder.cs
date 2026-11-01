//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueEventTypeExcluder : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        public List<IDialogueEvent> exclusionTypes = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        /// <returns></returns>
        public abstract bool IsExcludedEvent(IDialogueEvent dialogueEvent);
    }
}