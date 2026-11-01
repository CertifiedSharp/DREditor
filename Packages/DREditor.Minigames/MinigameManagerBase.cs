//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MinigameManagerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string endMinigameEvent = "EndMinigame";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float destroyBufferDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialMistake = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMinigame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent playerHasDied = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialTimeUp = null;


        /// <summary>
        ///
        /// </summary>
        public TrialTimerBase TrialTimer => GetComponentInChildren<TrialTimerBase>();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            inTrialMinigame.SetValue(true);

            playerHasDied.AddListener(EvaluatePlayerDeath);
            
            OnMinigameEnable();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnMinigameEnable()
        {
            if (TrialTimer != null)
                TrialTimer.TimeUp += OnTimeUp;

            trialMistake.AddListener(EvaluateMistake);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            inTrialMinigame.SetValue(false);

            playerHasDied.RemoveListener(EvaluatePlayerDeath);

            OnMinigameDisable();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnMinigameDisable()
        {
            if (TrialTimer != null)
                TrialTimer.TimeUp -= OnTimeUp;

            trialMistake.RemoveListener(EvaluateMistake);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="fuckUpDialogue"></param>
        protected virtual void TrialMistake(DialogueCore fuckUpDialogue)
        {
            trialMistake.SetValue(true);

            fuckUpDialogue.InvokeDialogue(ServiceLocator.GetService<IDialogueManager>());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FinishedTrialMistake()
        {
            if (!trialMistake.Value)
                trialMistake.Invoke();
            else
                trialMistake.SetValue(false);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnTimeUp() => trialTimeUp.SetValue(true);


        /// <summary>
        ///
        /// </summary>
        /// <param name="playerHasDied"></param>
        protected virtual void EvaluatePlayerDeath(bool playerHasDied)
        {
            if (playerHasDied)
                OnPlayerDeath();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPlayerDeath() => TrialTimer.ResetTimer();

        /// <summary>
        ///
        /// </summary>
        /// <param name="inMistake"></param>
        protected virtual void EvaluateMistake(bool inMistake)
        {
            if (!inMistake)
                Restart();
        }


        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator ResolveMinigame()
        {
            EndMinigame();

            yield return new WaitForSeconds(destroyBufferDuration);

            Destroy(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="action"></param>
        protected virtual void InvokeComponents<T2>(Action<T2> action)
        {
            var components = GetComponentsInChildren<Component>().OfType<T2>();

            foreach (var component in components)
                action(component);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void Restart();

        /// <summary>
        ///
        /// </summary>
        public virtual void EndMinigame() => DialogueEventSystem.TriggerEvent(endMinigameEvent);
    }
}