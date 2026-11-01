//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILoadedThroughDialogue) +
        "\n\n")]
    public class EventsOnLoadingThroughSceneTransition : DialogueComponent, ILoadedThroughDialogue
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> dialogueEvents = new() { "CheckFocus" };


        /// <inheritdoc/>
        public virtual void OnLoadedThroughDialogue(DialogueCore dialogue)
        {
            if (dialogue is Dialogue defaultDialogue && defaultDialogue.SceneTransition.Enabled)
                DialogueEventSystem.TriggerEvents(dialogueEvents);
        }
    }
}