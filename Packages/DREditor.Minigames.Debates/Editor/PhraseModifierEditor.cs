//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PhraseModifierEditor : PhraseSectionEditor<IPhraseModifier>
    {
        public PhraseModifierEditor(Color color, float endSpace) : base(color, endSpace) { }

        /// <inheritdoc/>
        protected override string TypeDisplayName() => "Modifier";

        /// <inheritdoc/>
        protected override string SectionLabel() => "Phrase Modifiers";

        /// <inheritdoc/>
        protected override string InstancesPropertyName() => nameof(currPhrase.modifiers);

        /// <inheritdoc/>
        protected override List<IPhraseModifier> TypeInstances() => currPhrase.modifiers;

        /// <inheritdoc/>
        protected override string GetInstanceDisplayName(IPhraseModifier instance) => instance.DisplayName;

        /// <inheritdoc/>
        protected override void UpdateTypeList(out List<Type> types, out string[] choices)
            => PhraseModifierList.UpdateTypeList(out types, out choices);

        /// <inheritdoc/>
        protected override bool DisplayCustomClassEditor() => ClassEditorFactory.DrawEditorFor(currentInstance, panel, currPhrase);
    }
}