//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using System.Collections.Generic;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// The filled out Editor for the End Event POCO class <see cref="DialogueEventEndEvent"/>. <br/>
    /// Displays a dynamic list of <see cref="IDialogueEvent"/>.
    /// </summary>
    [CustomClassEditor(typeof(DialogueEventEndEvent))]
    public class DialogueEventEndEventEditor : NestedClassEditorBase<IDialogueEvent>
    {
        /// <inheritdoc/>
        protected override string TargetFieldName => "dialogueEvents";

        /// <inheritdoc/>
        protected override List<IDialogueEvent> Instances => ((DialogueEventEndEvent)baseTarget).DialogueEvents;
    }
}
#endif