//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using UnityEngine;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DebateEvaluator) +
        "\n\n")]
    public class BulletEvaluator : DebateEvaluator
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent bulletText = null;

        /// <inheritdoc/>
        public override bool Evaluate(IBulletSwapperUI bulletHandler, PhraseBase textPhrase)
            => textPhrase.Get<BulletAnswerModifier>() is BulletAnswerModifier sentPhrase &&
                bulletHandler.GetCurrentBullet() == sentPhrase.AnswerBullet && sentPhrase.AnswerBullet.DisplayName == bulletText.Value;
    }
}