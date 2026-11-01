//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Firing;
using DREditor.Minigames.Debates.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FiredTruthHandler) +
        "\n\n")]
    public class FiredSliceHandler : FiredTruthHandler
    {
        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IPhraseManager> phraseManagers;


        /// <inheritdoc/>
        protected override Component[] InitializeComponents()
        {
            var allComponents = base.InitializeComponents();

            phraseManagers = allComponents.OfType<IPhraseManager>();

            return allComponents;
        }

        /// <inheritdoc/>
        protected override IEnumerator EvaluateBulletHit()
        {
            if (ShouldHitPhrase(out PhraseTextBase phraseText))
            {
                if (phraseText is IPhraseHittable hitable)
                    hitable.Hit(phraseText.transform.position);

                InvokePhraseShot();
            }
            else
                yield return WaitForFireAnimation();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phraseText"></param>
        /// <returns></returns>
        protected virtual bool ShouldHitPhrase(out PhraseTextBase phraseText)
        {
            phraseText = GetHittablePhrase();

            if (phraseText == null)
                return false;

            IPhraseShatterable shatter = phraseText.GetComponent<IPhraseShatterable>();

            PhraseBase gamePhrase = phraseText.textPhrase;

            if (gamePhrase == null || shatter == null)
                return false;

            bool notShattering = !shatter.IsShattering();

            bool isHittable = gamePhrase.Has<HittableModifierBase>();

            return notShattering && isHittable;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PhraseTextBase GetHittablePhrase()
        {
            PhraseTextBase fallbackPhrase = null;

            foreach (var manager in phraseManagers)
                foreach (var phrase in manager.GetActivePhrases())
                    if (phrase.textPhrase.Has<HittableModifierBase>())
                        return phrase;
                    else
                        fallbackPhrase = phrase;

            return fallbackPhrase;
        }
    }
}