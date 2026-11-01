//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueEventExcluder : DialogueEventTypeExcluder
    {
        /// <inheritdoc/>
        public override bool IsExcludedEvent(IDialogueEvent dialogueEvent)
        {
            foreach (IDialogueEvent type in exclusionTypes)
                if (dialogueEvent.GetType() == type.GetType())
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Set Default Exclusion Types")]
        public virtual void SetDefaultExclusionTypes() => exclusionTypes = new()
        {
            Create<ShakeObject>(), Create<CGDisplay>(), Create<VideoDisplay>(), Create<FaintAnim>(), Create<CharacterLeave>(),
            Create<Flashback>()
        };

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IDialogueEvent Create<T>() where T : IDialogueEvent => (IDialogueEvent)Activator.CreateInstance(typeof(T));
    }
}