//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// The filled out Editor for the End Event POCO class <see cref="ChoiceEvent"/>. <br/>
    /// Displays a dynamic list of <see cref="IChoice"/>.
    /// </summary>
    [CustomClassEditor(typeof(ChoiceEvent))]
    public class ChoiceEventEditor : NestedClassEditorBase<IChoice>
    {
        /// <inheritdoc/>
        protected override string TargetFieldName => "choices";

        /// <inheritdoc/>
        protected override List<IChoice> Instances => ((ChoiceEvent)baseTarget).Choices;
    }
}
#endif