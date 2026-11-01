//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.Minigames.Stats;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEventRoutine) +
        "\n\n")]
    public class PlayerDeathEndEventHandler : DialogueComponent, IDialogueEndEventRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMinigame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialContinued = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialMistake = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialTimeUp = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent playerHasDied = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent health = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent hideTrialDialogueUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TrialStatsBase TrialStats = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float playerDeathBufferTime = 1;


        /// <inheritdoc/>
        public virtual IEnumerator EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action exitParentEarly)
            where T : Line
        {
            if (PlayerShouldBeDead())
            {
                SetPlayerDead();

                yield return new WaitForSeconds(playerDeathBufferTime);

                HideTrialStats();

                yield break;
            }
            else
                OnPlayerNotDead();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayerShouldBeDead() => !trialContinued.Value && OnTrialMistake() && health.Value <= 0 || trialTimeUp.Value;

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetPlayerDead()
        {
            UnityEngine.Debug.Log($"REPORT: Player has Died!");

            playerHasDied.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideTrialStats()
        {
            if (TrialStats != null)
                TrialStats.HideTrialStats();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPlayerNotDead()
        {
            HideDialogueDuringMinigame();

            HideStatsOnMistake();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideDialogueDuringMinigame()
        {
            if (CanHideUIDuringMinigame())
                HideTrialDialogueUI();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanHideUIDuringMinigame() => inTrialMinigame.Value && hideTrialDialogueUI != null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideTrialDialogueUI() => hideTrialDialogueUI.Raise();


        /// <summary>
        ///
        /// </summary>
        protected virtual void HideStatsOnMistake()
        {
            if (OnTrialMistake())
                HideTrialStats();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnTrialMistake() => trialMistake.Value;
    }
}