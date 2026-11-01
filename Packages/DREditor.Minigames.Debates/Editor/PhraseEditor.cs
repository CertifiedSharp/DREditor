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
    [CustomClassEditor(typeof(Phrase))]
    public class PhraseEditor : PhraseEditorBase<Phrase>
    {
        /// <inheritdoc/>
        protected override List<PhraseSectionEditorBase> InitializeSections()
        {
            return new()
            {
                new PhraseDataEditor(Color.magenta, 5),
                new PhraseModifierEditor(Color.blue, 5),
                new AnimationArchitectureEditor(Color.green, 10)
            };
        }
    }
}