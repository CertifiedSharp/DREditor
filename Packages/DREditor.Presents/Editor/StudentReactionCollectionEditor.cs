// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System.Collections.Generic;

namespace DREditor.Presents.Editor
{
    /// <summary>
    /// The filled out Editor for the POCO class <see cref="StudentReactionCollection"/>. <br/>
    /// Displays a dynamic list of <see cref="IReactionEntry"/>.
    /// </summary>
    [CustomClassEditor(typeof(StudentReactionCollection))]
    public class StudentReactionCollectionEditor : NestedClassEditorBase<IReactionEntry>
    {
        /// <inheritdoc/>
        protected override string TargetFieldName => "studentReactions";

        /// <inheritdoc/>
        protected override List<IReactionEntry> Instances => ((StudentReactionCollection)baseTarget).studentReactions;

        /// <inheritdoc/>
        protected override bool ClearInstancesButtonAllowed => false;
    }
}

#endif