//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.LocalDialogues
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class LocalDialogue
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Dialogue dialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool triggered = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string requiredProgressionFlag = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Dialogue dialogueIfFlagNotMet = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("dialogueBoolTrigger")]
        public List<BoolTrigger> dialogueBoolTriggers = new();


        public LocalDialogue() { }

        public LocalDialogue(Dialogue dialogue, bool triggered, Dialogue dialogueIfFlagNotMet)
        {
            this.dialogue = dialogue;

            this.triggered = triggered;

            this.dialogueIfFlagNotMet = dialogueIfFlagNotMet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            LocalDialogue localDialogue = new(dialogue, triggered, dialogueIfFlagNotMet)
            {
                requiredProgressionFlag = requiredProgressionFlag,

                dialogueBoolTriggers = dialogueBoolTriggers
            };

            return localDialogue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagState"></param>
        public virtual void SetTriggered(bool flagState = true) => triggered = flagState;
    }
}