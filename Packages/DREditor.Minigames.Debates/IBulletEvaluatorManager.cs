//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IBulletEvaluatorManager
    {
        /// <summary>
        ///
        /// </summary>
        UnityEvent<PhraseBase> OnCorrectPhraseHit { get; }

        /// <summary>
        ///
        /// </summary>
        UnityEvent<PhraseBase> OnIncorrectPhraseHit { get; }

        /// <summary>
        ///
        /// </summary>
        event Action<DialogueCore> OnStopPanels;

        /// <summary>
        ///
        /// </summary>
        event Action OnCounterFinished;


        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="textPhrase"></param>
        /// <param name="hitPoint"></param>
        void EvaluateBulletHit(IPhraseShatterable phrase, PhraseBase textPhrase, Vector3 hitPoint);
    }
}