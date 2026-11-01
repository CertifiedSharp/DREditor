//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Minigames.Stats;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(BlackenedManagerBase) + ", " +nameof(MinigameManagerBase) + ", " +
        "\n\n")]
    public class BlackenedManager : BlackenedManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float debugbufferTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BlackenedSelectBuilder builder = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent minigameUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent selectionUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent answerUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float menuFadeWaitTime = 0.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float removeOffsetBufferDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float moveStallDuration = 0.3f;


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent OnMinigameStarted;

        [Header("")]
        [SerializeField] protected UnityEvent OnShowVisuals;

        [Header("")]
        [SerializeField] protected UnityEvent OnHideVisuals;

        [Header("")]
        [SerializeField] protected UnityEvent<string> OnChangeSelection;

        [Header("")]
        [SerializeField] protected UnityEvent OnSelected;

        [Header("")]
        [SerializeField] protected UnityEvent OnCorrectSelected;


        /// <summary>
        ///
        /// </summary>
        protected DRTrialCamera trialCamera;

        /// <summary>
        ///
        /// </summary>
        protected TrialStatsBase TrialStats;


        #region Initialization

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            InitializeTrialCamera();

            if (debug && builder != null)
                PlayBlackenedSelect(builder);

            TrialStats = ServiceLocator.GetService<TrialStatsBase>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTrialCamera()
        {
            if (trialCamera == null)
            {
                trialCamera = FindAnyObjectByType<DRTrialCamera>();

                trialCamera.SmoothFocus = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <remarks></remarks>
        public void PlayMinigame(ScriptableObject obj)
            => FindAnyObjectByType<BlackenedManager>().PlayBlackenedSelect((BlackenedSelectBuilder)obj);

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void PlayBlackenedSelect(BlackenedSelectBuilder asset)
        {
            builder = asset;

            StartCoroutine(BlackenedSelectStartRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator BlackenedSelectStartRoutine()
        {
            if (debug)
                yield return new WaitForSeconds(debugbufferTime);

            OnMinigameStarted?.Invoke();

            InitializeTrialCamera();

            trialCamera.TriggerAnim(trialCamera.SeatFocus + cameraPositionOffset);

            if (minigameUI != null)
                yield return minigameUI.ShowTillComplete();

            Initialize();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            currentControlsKey.SetValue(controlsKey);

            if (TrialTimer != null)
                TrialTimer.ShowInitialize(builder.times, false);

            if (questionToggle != null)
                questionToggle.Initialize(builder.questionText);

            InvokeChangedSelection();

            ActivateControls();
        }

        #endregion

        #region Move Select

        /// <inheritdoc/>
        protected override void MoveSelect(CallbackContext ctx)
        {
            Vector2 value = move.action.ReadValue<Vector2>();

            if (moving || value.x == 0)
                return;

            moving = true;

            DeactivateControls();

            if (value.x > 0)
            {
                // move right
                if ((trialCamera.SeatFocus - cameraPositionOffset + 1) > trialCamera.CharHeightOffset.Length - 1)
                    trialCamera.SeatFocus = 0 + cameraPositionOffset;
                else
                    trialCamera.SeatFocus += 1;
            }
            else if (value.x < 0)
            {
                //move left
                if ((trialCamera.SeatFocus - cameraPositionOffset - 1) < 0)
                    trialCamera.SeatFocus = trialCamera.CharHeightOffset.Length - 1 + cameraPositionOffset;
                else
                    trialCamera.SeatFocus -= 1;
            }

            StartCoroutine(MoveStall());

            InvokeChangedSelection();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator MoveStall()
        {
            yield return new WaitForSeconds(moveStallDuration);

            ActivateControls();

            moving = false;

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeChangedSelection() => OnChangeSelection?.Invoke(GetHoveredCharacterData()?.characterName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual BlackenedSelectData GetHoveredCharacterData()
        {
            foreach (var data in builder.selectDatas)
                if (Mathf.RoundToInt(trialCamera.SeatFocus) == characterDatabase.GetCharacter(data.characterName).TrialPosition)
                    return data;

            UnityEngine.Debug.LogError("The Character could not be found! " + trialCamera.SeatFocus);

            return null;
        }

        #endregion

        #region Select Character Resolve Minigame

        /// <inheritdoc/>
        protected override void SelectCharacter(CallbackContext ctx)
        {
            DeactivateControls();

            TrialTimer.PauseTimer();

            HideVisuals();

            OnSelected?.Invoke();

            StartCoroutine(SelectCharacterRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator SelectCharacterRoutine()
        {
            if (minigameUI != null)
                minigameUI.Hide();

            yield return new WaitForSeconds(removeOffsetBufferDuration);

            if (Mathf.RoundToInt(trialCamera.SeatFocus) >= trialCamera.CharHeightOffset.Length)
                trialCamera.SeatFocus = 0;

            trialCamera.SeatFocus = Mathf.RoundToInt(trialCamera.SeatFocus);

            if (minigameUI != null)
                yield return minigameUI.WaitForChangeToComplete();

            trialCamera.SmoothFocus = false;

            if (GetHoveredCharacterData() is BlackenedSelectData data)
            {
                if (data.isAnswer)
                    StartCoroutine(ResolveMinigame());
                else
                    TrialMistake(data.fuckUpDialogue);
            }
        }

        /// <inheritdoc/>
        public override IEnumerator ResolveMinigame()
        {
            if (TrialTimer != null)
                TrialTimer.HideStop();

            OnCorrectSelected?.Invoke();

            if (selectionUI != null)
                yield return selectionUI.ShowTillComplete();

            yield return new WaitForSeconds(menuFadeWaitTime);

            if (answerUI != null)
                yield return answerUI.ShowTillComplete();

            yield return base.ResolveMinigame();

            yield break;
        }

        #endregion

        #region Time Up, Player Death, Restart Minigame

        /// <inheritdoc/>
        protected override void OnTimeUp()
        {
            DeactivateControls();

            HideVisuals();

            base.OnTimeUp();
        }

        /// <inheritdoc/>
        protected override void Restart()
        {
            UnityEngine.Debug.Log("Restart Called");

            ShowVisuals();

            InvokeChangedSelection();

            StartCoroutine(ResolveRestart());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowVisuals()
        {
            if (questionToggle != null)
                questionToggle.Activate();

            if (TrialTimer != null)
                TrialTimer.Show();

            if (TrialTimer != null)
                TrialTimer.Restart();

            if (TrialStats != null)
                TrialStats.ShowTrialStats();

            if (minigameUI != null)
                minigameUI.Show();

            trialCamera.TriggerAnim(trialCamera.SeatFocus + cameraPositionOffset);

            trialCamera.SmoothFocus = true;

            OnShowVisuals?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideVisuals()
        {
            if (questionToggle != null)
                questionToggle.Deactivate();

            if (TrialTimer != null)
                TrialTimer.Hide();

            if (TrialStats != null)
                TrialStats.HideTrialStats();

            if (minigameUI != null)
                minigameUI.Hide();

            OnHideVisuals?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveRestart()
        {
            if (minigameUI != null)
                yield return minigameUI.WaitForChangeToComplete();

            ActivateControls();
        }

        #endregion
    }
}