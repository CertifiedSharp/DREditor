//Author: Happy30
//Completely Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.Choices.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// The default type of choice option available for choice Dialogue End Events.
    /// </summary>
    [System.Serializable]
    public class Choice : EndEventBase, IChoice
    {
        /// <summary>
        /// The text displayed to the UI for this choice.
        /// </summary>
        [FormerlySerializedAs("ChoiceText")]
        public string choiceText;

        /// <summary>
        /// The dialogue to move to when this choice is selected.
        /// </summary>
        [FormerlySerializedAs("NextDialogue")]
        public Dialogue nextDialogue;

        /// <summary>
        /// The line index to start on when moving to the next dialogue when selecting this choice.
        /// </summary>
        [FormerlySerializedAs("NextIndexInDialogue")]
        public int nextIndexInDialogue;

        /// <summary>
        /// A manually called dialogue event string to invoke when this choice is selected.
        /// </summary>
        [FormerlySerializedAs("CustomCall")]
        public string customCall;

        /// <summary>
        /// When true, force exits the dialogue process to allow other process to take over.
        /// </summary>
        [FormerlySerializedAs("CallBreak")]
        public bool callBreak = false;


        /// <inheritdoc/>
        public virtual string ChoiceText { get => choiceText; set => choiceText = value; }

        /// <summary>
        /// The dialogue to move to when this choice is selected.
        /// </summary>
        public virtual Dialogue NextDialogue { get => nextDialogue; set => nextDialogue = value; }

        /// <summary>
        /// The line index to start on when moving to the next dialogue when selecting this choice.
        /// </summary>
        public virtual int NextIndexInDialogue { get => nextIndexInDialogue; set => nextIndexInDialogue = value; }

        /// <summary>
        /// A manually called dialogue event string to invoke when this choice is selected.
        /// </summary>
        public virtual string CustomCall { get => customCall; set => customCall = value; }

        /// <summary>
        /// When true, force exits the dialogue process to allow other process to take over.
        /// </summary>
        public virtual bool CallBreak { get => callBreak; set => callBreak = value; }
    }
}