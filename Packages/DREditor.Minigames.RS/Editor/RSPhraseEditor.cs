//Author: Benjamin "Sweden" Jillson : sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Editor;
using DREditor.Minigames.RS.Managers;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.RS.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(RSPhrase))]
    public class RSPhraseEditor : PhraseEditorBase<RSPhrase>
    {
        /// <inheritdoc/>
        protected override List<PhraseSectionEditorBase> InitializeSections()
        {
            return new()
            {
                new PhraseDataEditor(Color.magenta, 5, DisplayHealth),
                new PhraseModifierEditor(Color.blue, 5),
                new AnimationArchitectureEditor(Color.green, 10)
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        protected virtual void DisplayHealth(PhraseBase phrase)
        {
            if (phrase is IPhraseHealth health)
            {
                int displayedHealth = HandyFields.IntField(new GUIContent("Phrase Health: "), health.GetInitialHealth());

                int finalHealth = displayedHealth <= 0 ? 1 : displayedHealth;

                health.SetInitialHealth(finalHealth);
            }
        }
    }
}