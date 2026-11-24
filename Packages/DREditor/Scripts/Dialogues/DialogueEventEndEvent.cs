//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.EndEvents or Dialogue Events.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// A Dialogue End Event to invoke Dialogue Events at the point in which Dialogue End Events are processed.
    /// </summary>
    [System.Serializable]
    public class DialogueEventEndEvent : EndEventBase, IDialogueEventCollection
    {
        /// <summary>
        /// The Collection of <see cref="IDialogueEvent"/>s held by the implementer.
        /// </summary>
        [SerializeReference]
        protected List<IDialogueEvent> dialogueEvents = new();


        /// <inheritdoc/>
        public virtual List<IDialogueEvent> DialogueEvents => dialogueEvents;
    }
}
