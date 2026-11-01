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
    public class AnimationArchitectureEditor : PhraseSectionEditor<IPhraseAnimationArchitecture>
    {
        public AnimationArchitectureEditor(Color color, float endSpace) : base(color, endSpace) { }

        /// <inheritdoc/>
        protected override string TypeDisplayName() => "Architecture";

        /// <inheritdoc/>
        protected override string SectionLabel() => "Animation Architectures";

        /// <inheritdoc/>
        protected override string InstancesPropertyName() => nameof(currPhrase.animations);

        /// <inheritdoc/>
        protected override List<IPhraseAnimationArchitecture> TypeInstances() => currPhrase.animations;

        /// <inheritdoc/>
        protected override string GetInstanceDisplayName(IPhraseAnimationArchitecture instance) => instance?.GetType().Name;

        /// <inheritdoc/>
        protected override void UpdateTypeList(out List<Type> types, out string[] choices)
            => PhraseAnimationArchitectureList.UpdateTypeList(out types, out choices);

        /// <inheritdoc/>
        protected override bool DisplayCustomClassEditor() => ClassEditorFactory.DrawEditorFor(currentInstance);
    }
}