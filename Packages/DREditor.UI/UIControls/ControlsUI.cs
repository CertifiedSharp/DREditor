//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuGroupBase) + 
        "\n\n")]
    [DefaultExecutionOrder(1)]
    public class ControlsUI : MenuGroupBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference move = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference fastForward = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
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
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent changingObserve = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ScriptableObject controlsDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent callControlsUI = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI controlsModeTitle;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<ControlsUITab> controlTabs = new();


        [Header("")]
        [SerializeField] protected UnityEvent OnTabSwitched = null;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected int activeTab = 0;

        /// <summary>
        ///
        /// </summary>
        protected IControlsUIPanel activePanel;

        /// <summary>
        ///
        /// </summary>
        protected CallbackContext backInputContext;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            callControlsUI.AddListener(EvaluateGlobal);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            callControlsUI.RemoveListener(EvaluateGlobal);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="activateControls"></param>
        protected virtual void EvaluateGlobal(bool activateControls)
        {
            if (activateControls && !isActive)
                Show();
        }

        /// <inheritdoc/>
        public override void Show()
        {
            activeTab = 0;

            activePanel = ((IControlsUIRepository)controlsDatabase).GetPanel(currentControlsKey.Value);

            if (activePanel == null)
            {
                UnityEngine.Debug.LogWarning("ALERT: ACTIVE PANEL FOR CONTROLS UI WAS NULL.");

                return;
            }

            UnityEngine.Debug.Log("REPORT: Controls Key is: " + currentControlsKey.Value);

            BeforeStart?.Invoke();

            PopulateUI();

            Time.timeScale = 0;

            inMenu.SetValue(true);

            inMenu.Invoke();

            StartCoroutine(ShowProcess());

            if (fastForwardModeEnabled != null && fastForwardModeEnabled.Value)
                fastForwardModeWasEnabled.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowProcess()
        {
            visibilityComponent.Show();

            yield return visibilityComponent.WaitForChangeToComplete();

            AddBackInput();

            move.action.performed += SwitchTabs;

            StartEvents?.Invoke();

            SetActive(true);

            ServiceLocator.GetService<MenuState>().SetChanging(false);
        }


        /// <inheritdoc/>
        public override void BackGroup(CallbackContext context)
        {
            backInputContext = context;

            if (backGroup != null)
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
            RemoveBackInput();

            move.action.performed -= SwitchTabs;

            EventSystem.current.SetSelectedGameObject(null);

            EndEvents?.Invoke();

            StartCoroutine(HideProcess());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator HideProcess()
        {
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

            visibilityComponent.Hide();

            yield return visibilityComponent.WaitForChangeToComplete();

            Time.timeScale = 1;

            if (fastForwardModeWasEnabled.Value)
            {
                if (backInputContext.action == fastForward.action || fastForward.action.inProgress)
                    yield return new WaitUntil(() => !fastForward.action.inProgress);

                fastForwardModeWasEnabled.SetValue(false);
            }

            inMenu.SetValue(false);

            inMenu.Invoke();

            SetActive(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SwitchTabs(CallbackContext context)
        {
            SwitchTab();

            PopulateUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SwitchTab()
        {
            if (controlTabs.Count == 1)
                return;

            if (activeTab + 1 >= controlTabs.Count)
                activeTab = 0;
            else
                activeTab++;

            OnTabSwitched?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PopulateUI()
        {
            controlsModeTitle.text = activePanel.Title;

            for (int i = 0; i < controlTabs.Count; i++)
                controlTabs[i].Display(i == activeTab, activePanel);
        }


        /// <inheritdoc/>
        public override void ChangeGroup(MenuGroupBase group)
        {
            if (Changing)
            {
                UnityEngine.Debug.LogWarning("ALERT: Menus are still changing!");

                return;
            }

            ServiceLocator.GetService<MenuState>().SetChanging(true);

            EventSystem.current.SetSelectedGameObject(null);

            Hide();

            group.Show();
        }

        /// <inheritdoc/>
        protected override void ResetGroup() => move.action.performed -= SwitchTabs;

        /// <inheritdoc/>
        public override void SetEvaluateSelect() => PopulateUI();
    }
}