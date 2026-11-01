//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public class LoadDialogueEventExcluder : DialogueEventExcluder
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public BoolWithEvent isLoadingFile = null;


        /// <inheritdoc/>
        public override bool IsExcludedEvent(IDialogueEvent dialogueEvent)
        {
            if (isLoadingFile.Value)
                return base.IsExcludedEvent(dialogueEvent);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isLoadingFile"></param>
        public void SetIsLoadingFile(BoolWithEvent isLoadingFile) => this.isLoadingFile = isLoadingFile;


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Set Default Load Exclusion Types")]
        public void SetDefaultLoadExclusionTypes() => exclusionTypes.Add(Create<MoveCharacter>());
    }
}