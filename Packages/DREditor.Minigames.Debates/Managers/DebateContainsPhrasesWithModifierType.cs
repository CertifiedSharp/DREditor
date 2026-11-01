//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(EvaluateCondition) +
        "\n\n")]
    public class DebateContainsPhrasesWithModifierType : EvaluateCondition
    {
        /// <summary>
        ///
        /// </summary>
        [SelectType]
        [Tooltip("")]
        [SerializeReference] protected IPhraseModifier modifierType;


        /// <inheritdoc/>
        protected override bool OnEvaluateCondition()
        {
            var manager = FindAnyObjectByType<DebateManagerBase<Panel>>();

            if (manager == null)
                return false;

            Type targetType = modifierType.GetType();

            return manager.GetLines().Any(line =>
                line.Phrases.Any(phrase => phrase.Has(targetType)));
        }
    }
}