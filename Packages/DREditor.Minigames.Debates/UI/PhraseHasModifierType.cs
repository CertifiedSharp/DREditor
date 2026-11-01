//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(EvaluateContextCondition<PhraseBase>) + " sending type: " + nameof(PhraseBase) +
        "\n\n")]
    [MovedFrom(true, "DREditor.EG", "EG")]
    public class PhraseHasModifierType : EvaluateContextCondition<PhraseBase>
    {
        /// <summary>
        ///
        /// </summary>
        [SelectType]
        [Tooltip("")]
        [SerializeReference] protected IPhraseModifier modifierType;


        /// <inheritdoc/>
        protected override bool OnEvaluateCondition() => (bool)(context?.Has(modifierType.GetType()));
    }
}