// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeotheDev
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.UI;
using DREditor.UIHandling;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Backlogging
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ScrollGridUI) +
        "\n\n")]
    public class BacklogUI : ScrollGridUI
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Space]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference backlogInput = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference fastForward = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Backlog backlog = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inPause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent fastForwardModeEnabled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent fastForwardModeWasEnabled = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition openBacklogConditions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float voicelineCooldown = 1;


        /// <summary>
        ///
        /// </summary>
        protected CallbackContext backInputContext;

        /// <summary>
        ///
        /// </summary>
        protected int lineCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected bool canPlayVoiceline = true;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            if (debugMode)
            {
                hasMenuAccess.Value = true;

                backlog.CreateDebugBacklogLines();

                UnityEngine.Debug.LogWarning($"ALERT: {nameof(BacklogUI)} is currently in debug mode! " +
                    $"Please ensure that it is turned off when you are no longer debugging!");
            }
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            backlogInput.action.started += AttemptShowBacklog;
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            backlogInput.action.started -= AttemptShowBacklog;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void AttemptShowBacklog(CallbackContext context)
        {
            if (openBacklogConditions.Evaluate())
                Show();
        }


        /// <inheritdoc/>
        protected override void SubscribeControls()
        {
            base.SubscribeControls();

            AddBackInput();

            submit.action.performed += OnSelect;
        }

        /// <inheritdoc/>
        protected override void UnsubscribeControls()
        {
            base.UnsubscribeControls();

            RemoveBackInput();

            submit.action.performed -= OnSelect;
        }


        /// <inheritdoc/>
        public override void Show()
        {
            BeforeStart?.Invoke();

            if (backGroup != null && backGroup.isActive)
                backGroup.Hide();

            EventSystem.current.SetSelectedGameObject(null);

            Time.timeScale = 0;

            inMenu.Value = true;

            StartCoroutine(ShowProcess());

            if (!inPause.Value && fastForwardModeEnabled.Value)
                fastForwardModeWasEnabled.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowProcess()
        {
            if (visibilityComponent != null)
                visibilityComponent.Show(); // We show before initialization so that the grid initializes properly.

            InitializeScrollGrid();

            RegenerateUI();

            if (visibilityComponent != null)
                yield return visibilityComponent.WaitForChangeToComplete();

            SubscribeControls();

            StartEvents?.Invoke();

            SetActive(true);

            ServiceLocator.GetService<MenuState>().SetChanging(false);
        }


        /// <inheritdoc/>
        public override void BackGroup(CallbackContext context)
        {
            backInputContext = context;

            if (backGroup != null && inPause.Value)
            {
                backGroup.SetEvaluateSelect();

                backGroup.AddBackInput();

                base.BackGroup(context);
            }
            else
            {
                OnBack?.Invoke();

                Hide();
            }
        }

        /// <inheritdoc/>
        public override void Hide()
        {
            UnsubscribeControls();

            EventSystem.current.SetSelectedGameObject(null);

            Time.timeScale = 1;

            EndEvents?.Invoke();

            StartCoroutine(HideProcess());

            if (ServiceLocator.HasService<VOManagerBase>(out var voManager))
                voManager.StopVoiceLine();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator HideProcess()
        {
            if (visibilityComponent != null)
                yield return visibilityComponent.HideTillComplete();

            if (!inPause.Value && fastForwardModeWasEnabled.Value)
            {
                if (backInputContext.action == fastForward.action || fastForward.action.inProgress)
                    yield return new WaitUntil(() => !fastForward.action.inProgress);

                fastForwardModeWasEnabled.SetValue(false);
            }

            if (!inPause.Value)
                inMenu.Value = false;

            SetActive(false);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnSelect(CallbackContext context)
        {
            if (lineCount == 0)
                return;

            BacklogLines entry = backlog.Lines.ToArray().Reverse().ToArray()[selectedSlotIndex];

            if (entry.Voice == null || entry.Voice == string.Empty || !canPlayVoiceline)
                return;

            canPlayVoiceline = false;

            StartCoroutine(VoiceLineCooldown());

            ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(entry.Voice);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator VoiceLineCooldown()
        {
            yield return new WaitForSecondsRealtime(voicelineCooldown);

            canPlayVoiceline = true;
        }


        /// <inheritdoc/>
        public override void SetEvaluateSelect() => RegenerateUI();

        /// <inheritdoc/>
        protected override void ResetGroup() => UnsubscribeControls();

        /// <inheritdoc/>
        protected override int InitializeTotalObjectCount()
        {
            var lines = backlog.Lines.ToArray().Reverse().ToArray();

            lineCount = lines.Length < backlog.Capacity ? lines.Length : backlog.Capacity;

            return lineCount;
        }
    }
}