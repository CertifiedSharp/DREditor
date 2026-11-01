//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.UI;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(ScrollGridUI) + ", " + nameof(MenuGroupBase) + ", " + 
        nameof(IUIVisibility) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class PresentUI : ScrollGridUI
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Space]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentSelectEvaluator presentSelectEvaluator = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inPause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform selector;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectedItemDisplayer displayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectedItemDisplayer[] indicatorDisplayers = null;


        /// <inheritdoc/>
        protected override void SubscribeControls()
        {
            base.SubscribeControls();

            AddBackInput();

            if (!inPause.Value)
            {
                presentSelectEvaluator.OnChoiceMenuStarted += UnsubscribeControls;

                presentSelectEvaluator.SubscribeCheckPresentSelect();
            }
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            presentSelectEvaluator.onGavePresentSelected.AddListener(OnGavePresent);

            presentSelectEvaluator.onDidntGivePresentSelected.AddListener(OnDidntGivePresent);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            presentSelectEvaluator.onGavePresentSelected.RemoveListener(OnGavePresent);

            presentSelectEvaluator.onDidntGivePresentSelected.RemoveListener(OnDidntGivePresent);
        }

        /// <inheritdoc/>
        protected override void UnsubscribeControls()
        {
            base.UnsubscribeControls();

            RemoveBackInput();

            if (!inPause.Value)
            {
                presentSelectEvaluator.OnChoiceMenuStarted -= UnsubscribeControls;

                presentSelectEvaluator.UnsubscribeCheckPresentSelect();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnGavePresent() => Hide();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDidntGivePresent() => SubscribeControls();


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
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator HideProcess()
        {
            if (visibilityComponent != null)
            {
                visibilityComponent.Hide();

                yield return visibilityComponent.WaitForChangeToComplete();
            }

            if (!inPause.Value)
                yield return presentSelectEvaluator.HandlePresentSelection(selectedSlotIndex);

            if (!inPause.Value)
                inMenu.Value = false;

            SetActive(false);
        }


        /// <inheritdoc/>
        protected override void OnSlotHovered()
        {
            UpdateSelector();

            UpdateDisplayers();

            presentSelectEvaluator.UpdateSelectedPresentIndex(selectedSlotIndex);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateSelector()
        {
            selector.SetParent(slots[GetSlotIndex()]);

            selector.localPosition = Vector3.zero;

            selector.localScale = Vector3.one;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateDisplayers()
        {
            displayer.DisplaySelectedItem(globalIndex);

            foreach (var displayer in indicatorDisplayers)
                displayer.DisplaySelectedItem(globalIndex);
        }


        /// <inheritdoc/>
        public override void SetEvaluateSelect() => RegenerateUI();

        /// <inheritdoc/>
        protected override void ResetGroup() => UnsubscribeControls();

        /// <inheritdoc/>
        protected override int InitializeTotalObjectCount() => presentDatabase.presents.Count;
    }
}