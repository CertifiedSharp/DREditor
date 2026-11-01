//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.LocalDialogues;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class LocalDialogueSubsequentData : GenericSubsequent, ISubsequentConvert
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public LocalDialogue localDialogue = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="localDialogue"></param>
        public virtual void SetLocalDialogue(LocalDialogue localDialogue) => this.localDialogue = localDialogue;


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericSubsequent genericSubsequent)
        {
            var subsequent = (Subsequent)genericSubsequent;

            return subsequent.localDialogue.dialogue != null || subsequent.localDialogue.dialogueIfFlagNotMet != null;
        }

        /// <inheritdoc/>
        public virtual void Convert(List<GenericSubsequent> genericSubsequents, GenericSubsequent genericSubsequent)
        {
            var subsequent = (Subsequent)genericSubsequent;

            var newData = new LocalDialogueSubsequentData(subsequent.Type) {  localDialogue = subsequent.localDialogue };

            genericSubsequents.Add(newData);
        }

        /// <inheritdoc/>
        public override GenericSubsequent MergeInstance(GenericSubsequent instance) => this;


        public LocalDialogueSubsequentData() { }

        public LocalDialogueSubsequentData(string Type) : base(Type) { }
    }
}