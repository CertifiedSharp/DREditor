//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Services;
using DREditor.UIHandling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<ITieBreaker>) + ", " + nameof(ITieBreaker) +
        "\n\n")]
    public class CrossSwordsMechanic : ServiceHolder<ITieBreaker>, ITieBreaker
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent duelStartedEvent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent duelEndedEvent;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent introAnimation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent successAnimation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent failureAnimation = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int totalTaps = 8;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float duelDuration = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int enemyIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float enemyAttackRate = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float damage = 5;


        [Header("")]
        [SerializeField] protected UnityEvent onDuelStarted = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SoundPlayerDRE successSFX;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SoundPlayerDRE failureSFX;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SoundPlayerDRE clashingSFX;


        /// <summary>
        ///
        /// </summary>
        protected int currentTaps = 0;

        /// <summary>
        ///
        /// </summary>
        protected ISplitScreen splitService;


        protected virtual void Start() => InitializeMechanic();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeMechanic()
        {
            if (!InitializationValid())
                OnInitializationFailed();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitializationValid() => ServiceLocator.HasService(out splitService);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInitializationFailed()
        {
            UnityEngine.Debug.LogWarning($"ALERT: The {nameof(CrossSwordsMechanic)} requires " +
                    $"an active {nameof(ISplitScreen)} service to function.\n" +
                    $"PROPOSAL: Ensure that a service registering {nameof(ISplitScreen)} is available.");

            UnityEngine.Debug.Log($"REPORT: Destroying {nameof(CrossSwordsMechanic)}.");

            Destroy(gameObject);
        }


        /// <inheritdoc/>
        public virtual IEnumerator TieBreakerProcess(Action<bool> result)
        {
            UnityEngine.Debug.Log("REPORT: Starting CrossSwords.");

            InitializeDuel();

            InvokeDuelStarted();

            yield return DisplayStartingDuel();

            RaiseEvent(duelStartedEvent);

            EnablePlayerDueling();

            yield return Duel();

            DisablePlayerDueling();

            RaiseEvent(duelEndedEvent);

            yield return ResolveDuel();

            result(splitService.WillAdvanceOnTie());

            UnityEngine.Debug.Log("REPORT: Ending CrossSwords.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDuel()
        {
            splitService.AnimateToResetState();

            currentTaps = totalTaps;

            var speeds = transform.root.GetComponentsInChildren<SpeedHandlerBase>();

            foreach (var speed in speeds)
            {
                speed.OnPanelsStopped();

                speed.DeactivateControls();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DisplayStartingDuel()
        {
            if (introAnimation != null)
            {
                yield return introAnimation.ShowTillComplete();

                introAnimation.Hide();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneEvent"></param>
        protected virtual void RaiseEvent(SceneEvent sceneEvent)
        {
            if (sceneEvent != null)
                sceneEvent.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeDuelStarted() => onDuelStarted?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnablePlayerDueling() => submit.action.started += Tap;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void Tap(CallbackContext ctx)
        {
            currentTaps++;

            splitService.PlayerAttack(damage);

            if (clashingSFX != null)
                clashingSFX.PlaySFX();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator Duel()
        {
            float timeRemaining = duelDuration;

            while (timeRemaining > 0 && currentTaps != 0 && currentTaps <= totalTaps)
            {
                yield return new WaitForSeconds(enemyAttackRate);

                timeRemaining -= enemyAttackRate;

                splitService.EnemyAttack(enemyIndex, damage);

                currentTaps--;

                if (clashingSFX != null)
                    clashingSFX.PlaySFX();

                UnityEngine.Debug.Log("REPORT: Enemy Attacked.");

                yield return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisablePlayerDueling() => submit.action.started -= Tap;

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveDuel()
        {
            bool playerWon = splitService.WillAdvanceOnTie();

            var resultSound = playerWon ? successSFX : failureSFX;

            var resultAnimation = playerWon ? successAnimation : failureAnimation;

            if (playerWon)
                splitService.PlayerAttack(damage);
            else
                splitService.EnemyAttack(enemyIndex, damage);

            if (resultSound != null)
                resultSound.PlaySFX();

            if (resultAnimation != null)
            {
                yield return resultAnimation.ShowTillComplete();

                resultAnimation.Hide();
            }
        }


        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            DisablePlayerDueling();
        }
    }
}