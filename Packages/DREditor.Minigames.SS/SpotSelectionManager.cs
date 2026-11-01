//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UI;
using DREditor.UIHandling;
using DREditor.UIHandling.Reticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MinigameManagerBase) + ", " +
        "\n\n")]
    public class SpotSelectionManager : SpotSelectionManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent displayNameVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected QuestionToggle questionToggle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected HoverReticle reticle = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpotSelectionBuilder builder = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent introUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase confirmUI = null;


        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent answerUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float menuFadeWaitTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postIntroBufferDuration = 0.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveRestartBufferDuration = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<SpotSelectionComponent> spotSelectionComponents = new();


        [Header("")]
        [SerializeField] protected UnityEvent OnMinigameStarted;


        /// <summary>
        ///
        /// </summary>
        protected bool wasVisible = false;


        /// <summary>
        ///
        /// </summary>
        protected Collider2D selectedCollider = null;


        

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (debugMode)
                StartSelection(builder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        public void PlaySpotSelect(ScriptableObject asset) 
            => FindAnyObjectByType<SpotSelectionManager>().StartSelection((SpotSelectionBuilder)asset);

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        public virtual void StartSelection(SpotSelectionBuilder builder)
        {
            this.builder = builder;

            StartCoroutine(BeginSelection());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BeginSelection()
        {
            spotSelectionComponents.ForEach(component => component.Initialize(builder));

            OnMinigameStarted?.Invoke();

            yield return introUI.ShowTillComplete();

            if (TrialTimer != null)
                TrialTimer.ShowInitialize(builder.times, false);

            yield return new WaitForSecondsRealtime(postIntroBufferDuration);

            questionToggle.Initialize(builder.question);

            if (TrialTimer != null)
                TrialTimer.ResumeTimer();

            reticle.SetLock(false);

            EnableSubmit();
        }

        /// <inheritdoc/>
        protected override void SubmitSpot(CallbackContext ctx)
        {
            selectedCollider = reticle.GetHoveredObject<Collider2D>();

            if (selectedCollider != null && !confirmUI.isActive)
                EnterConfirmUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnterConfirmUI()
        {
            DisableSubmit();

            // Below used to be a coroutine where a frame was waited before calling below code.
            UnityEngine.Debug.Log(nameof(SpotSelectionManager) + ": On box: " + selectedCollider.gameObject.name);

            reticle.SetLock(true);

            confirmUI.Show();
        }

        /// <inheritdoc/>
        public override void LeaveConfirmUI()
        {
            EventSystem.current.SetSelectedGameObject(null);

            confirmUI.Hide();

            // Below used to be a coroutine where a frame was waited before calling below code.
            reticle.SetLock(false);

            EnableSubmit();
        }

        /// <inheritdoc/>
        public override void ConfirmSelection()
        {
            TrialTimer.PauseTimer();

            HideVisuals();

            Spot chosenSpot = builder.GetSpotWithName(selectedCollider.gameObject.name);

            if (chosenSpot.isAnswer)
                StartCoroutine(ResolveMinigame());
            else
                TrialMistake(chosenSpot.wrongDialogue);
        }

        /// <inheritdoc/>
        public override IEnumerator ResolveMinigame()
        {
            yield return new WaitForSeconds(menuFadeWaitTime);

            if (answerUI != null)
                yield return answerUI.ShowTillComplete();

            yield return base.ResolveMinigame();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideVisuals()
        {
            EventSystem.current.SetSelectedGameObject(null);

            confirmUI.Hide();

            if (displayNameVisible.Value)
                wasVisible = true;

            reticle.SetLock(true);

            reticle.enabled = false;

            spotSelectionComponents.ForEach(component => component.Hide());

            TrialTimer.Hide();

            questionToggle.Deactivate();

            displayNameVisible.SetValue(false);
        }
        
        /// <inheritdoc/>
        protected override void Restart()
        {
            spotSelectionComponents.ForEach(component => component.Show());

            TrialTimer.Show();

            questionToggle.Activate();

            StartCoroutine(ResolveRestart());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveRestart()
        {
            yield return new WaitForSecondsRealtime(resolveRestartBufferDuration);

            TrialTimer.ResumeTimer();

            reticle.enabled = true;

            reticle.SetLock(false);

            if (wasVisible)
            {
                wasVisible = false;

                displayNameVisible.SetValue(true);
            }

            EnableSubmit();
        }

        /// <inheritdoc/>
        protected override void OnTimeUp()
        {
            DisableSubmit();

            HideVisuals();

            base.OnTimeUp();
        }
    }
}