//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Services;
using DREditor.UIHandling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IBulletEvaluatorManager) +
        "\n\n")]
    public class BulletEvaluatorManager : MonoBehaviour, IBulletEvaluatorManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent preCounterUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool finishPreCounter = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float preCounterDuration = 0f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CounterHandlerBase counterHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postBulletMissBufferTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Graphic UIBreak = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DebateEvaluator[] evaluators;


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent<PhraseBase> onCorrectPhraseHit = null;

        [Header("")]
        [SerializeField] protected UnityEvent<PhraseBase> onIncorrectPhraseHit = null;

        public UnityEvent<PhraseBase> OnCorrectPhraseHit { get => onCorrectPhraseHit; }

        public UnityEvent<PhraseBase> OnIncorrectPhraseHit { get => onIncorrectPhraseHit; }


        public event Action<DialogueCore> OnStopPanels;

        public event Action OnCounterFinished;


        /// <summary>
        ///
        /// </summary>
        protected IBulletSwapperUI bulletHandler;


        protected virtual void Awake() => bulletHandler = transform.root.GetComponentInChildren<IBulletSwapperUI>();


        /// <inheritdoc/>
        public virtual void EvaluateBulletHit(IPhraseShatterable phrase, PhraseBase textPhrase, Vector3 hitPoint)
        {
            if (EvaluateBulletResult(textPhrase))
            {
                inputAllowed.SetValue(false);

                phrase?.Shatter(hitPoint);

                StartCoroutine(OnCorrectRoutine(textPhrase));
            }
            else
                StartCoroutine(OnIncorrectRoutine(textPhrase));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        /// <returns></returns>
        protected virtual bool EvaluateBulletResult(PhraseBase textPhrase)
        {
            foreach (var evaluator in evaluators)
                if (evaluator.Evaluate(bulletHandler, textPhrase))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        protected virtual IEnumerator OnCorrectRoutine(PhraseBase textPhrase)
        {
            OnCorrectPhraseHit?.Invoke(textPhrase);

            OnStopPanels?.Invoke(null);

            if (preCounterUI != null)
                yield return DisplayPreCounterUI();

            if (counterHandler != null)
                yield return counterHandler.DisplayCounter(textPhrase);

            if (UIBreak is IShatterGraphic shatter)
            {
                shatter.OnPostShatter?.AddListener(OnPostShatter);

                yield return shatter.ShatterTheGraphic();
            }

            if (ServiceLocator.HasService<MusicManagerBase>())
                ServiceLocator.GetService<MusicManagerBase>().StopMusic();

            OnCounterFinished?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplayPreCounterUI()
        {
            preCounterUI.Show();

            if (finishPreCounter)
                yield return preCounterUI.WaitForChangeToComplete();
            else
                yield return new WaitForSeconds(preCounterDuration);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPostShatter()
        {
            if (UIBreak is IShatterGraphic shatter)
                shatter.OnPostShatter?.RemoveListener(OnPostShatter);

            inputAllowed.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        protected virtual IEnumerator OnIncorrectRoutine(PhraseBase textPhrase)
        {
            OnIncorrectPhraseHit?.Invoke(textPhrase);

            yield return new WaitForSeconds(postBulletMissBufferTime);

            if (textPhrase is Phrase trialPhrase)
                OnStopPanels?.Invoke(trialPhrase.fuckUpDialogue);

            LogIncorrectResult(textPhrase);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        protected virtual void LogIncorrectResult(PhraseBase textPhrase)
        {
            bool isPhrase = textPhrase is Phrase;

            if (isPhrase && bulletHandler?.GetCurrentBullet() is var bullet && bullet != null)
                UnityEngine.Debug.Log("REPORT: You fucked up the shot! Current Bullet: " + bullet.DisplayName);

            if (!isPhrase)
                UnityEngine.Debug.LogWarning("ALERT: THE SENT PHRASE WAS NOT OF A TRIAL DIALOGUE AND TRIAL LINE LIKE INTENDED");

            if (isPhrase && textPhrase.Get<BulletAnswerModifier>() is BulletAnswerModifier data)
                UnityEngine.Debug.Log("REPORT: Was the right phrase, Answer Bullet: " + data.AnswerBullet.DisplayName);
        }


        protected virtual void OnDestroy()
        {
            if (UIBreak is IShatterGraphic shatter)
                shatter.OnPostShatter?.RemoveListener(OnPostShatter);
        }
    }
}