//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.Choices.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// Dialogue End Event for displaying Choices to the player to select from. <br/>
    /// Allowing for dynamicly structured choices inheriting from <see cref="IChoice"/>.
    /// </summary>
    [System.Serializable]
    public class ChoiceEvent : EndEventBase
    {
        /// <summary>
        /// The list of dynamic choice options to display when this End Event is processes.
        /// </summary>
        [SerializeReference]
        protected List<IChoice> choices = new() { new Choice(), new Choice(), new Choice() };


        /// <summary>
        /// The list of dynamic choice options to display when this End Event is processes.
        /// </summary>
        public virtual List<IChoice> Choices => choices;
    }
}