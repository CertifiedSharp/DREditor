//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PhraseBaseEditor : PhraseEditorBase<PhraseBase>
    {
        /// <inheritdoc/>
        protected override List<PhraseSectionEditorBase> InitializeSections()
        {
            return new()
            {
                new PhraseBaseDataEditor(Color.magenta, 5),
                new PhraseModifierEditor(Color.blue, 5),
                new AnimationArchitectureEditor(Color.green, 10)
            };
        }


        /// <inheritdoc/>
        public override void OnGUI(object target, params object[] objects)
        {
            HandyFields.Label("REPORT: The fallback editor is being used.");

            base.OnGUI(target, objects);
        }
    }
}