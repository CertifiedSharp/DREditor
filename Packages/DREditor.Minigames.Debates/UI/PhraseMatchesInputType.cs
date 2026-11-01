//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(EvaluateContextCondition<PhraseBase>) + " sending type: " + nameof(PhraseBase) +
        "\n\n")]
    public class PhraseMatchesInputType : EvaluateContextCondition<PhraseBase>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<InputModifierType> inputTypes = null;


        /// <inheritdoc/>
        protected override bool OnEvaluateCondition()
        {
            if (!context.TryGetModifier<InputModifier>(out var modifier))
                return false;

            foreach (var inputType in inputTypes)
                if (!modifier.inputModifiers.Contains(inputType))
                    return false;

            return true;
        }
    }
}