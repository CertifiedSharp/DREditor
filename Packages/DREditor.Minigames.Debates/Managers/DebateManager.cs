//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DebateManager<T> : DebateManagerBase<T>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFire = null;


        /// <summary>
        ///
        /// </summary>
        protected IBulletEvaluatorManager bulletEvaluator = null;


        protected virtual void Awake() => bulletEvaluator = GetComponentInChildren<IBulletEvaluatorManager>();

        protected override void OnMinigameEnable()
        {
            base.OnMinigameEnable();

            bulletEvaluator.OnStopPanels += StopPanels;

            bulletEvaluator.OnCounterFinished += OnCountered;
        }

        protected override void OnMinigameDisable()
        {
            base.OnMinigameDisable();

            inputAllowed.SetValue(false);

            bulletEvaluator.OnStopPanels -= StopPanels;

            bulletEvaluator.OnCounterFinished -= OnCountered;
        }


        protected override void OnTimeUp() => StartCoroutine(TimeUpRoutine(base.OnTimeUp));

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator TimeUpRoutine(Action callback)
        {
            while (!canFire.Value || inMenu.Value)
                if (inCycle.Value)
                    break;
                else
                    yield return null;

            StopPanels();

            callback?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fuckUp"></param>
        protected virtual void StopPanels(DialogueCore fuckUp = null)
        {
            InvokeComponents<IPanelsStoppedListener>(listener => listener.OnPanelsStopped());

            StopCoroutine(playPanelRoutine);

            HideUI();

            inDebate.SetValue(false);

            inputAllowed.SetValue(false);

            if (fuckUp != null)
                TrialMistake(fuckUp);
        }
    }
}