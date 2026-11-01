//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(OptionWithEventListener<bool, BoolEvent>) +
        "\n\n")]
    [DefaultExecutionOrder(1)]
    public class OptionToggleUI : OptionWithEventListener<bool, BoolEvent>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Selectable onSelectable = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Selectable offSelectable = null;


        /// <summary>
        ///
        /// </summary>
        protected OptionUIGroup optionGroup = null;

        /// <summary>
        ///
        /// </summary>
        protected SelectableUIOption toggleOption;

        /// <summary>
        ///
        /// </summary>
        protected SelectableUIOption offOption;

        /// <summary>
        ///
        /// </summary>
        protected bool isOn;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            toggleOption = onSelectable.GetComponent<SelectableUIOption>();

            optionGroup = GetComponent<OptionUIGroup>();

            offOption = offSelectable.GetComponent<SelectableUIOption>();

            offSelectable.interactable = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateUIState(valueWithEvent.Value);

            if (valueWithEvent != null && !valueWithEvent.Value)
            {
                SetToggleTo(valueWithEvent.Value);

                offOption.Selection.Invoke();

                optionGroup.OnSetup.AddListener(FocusOff);
            }

            optionGroup.OnBeforeSetup.AddListener(BeforeSetUp);

            optionGroup.OnSetup.AddListener(FocusOff);

            optionGroup.OnUnSetup.AddListener(UnSetUp);

            OnValueChangedEvent.AddListener(UpdateUIState);

            toggleOption.Selection.AddListener(ToggleTrue);

            offOption.Selection.AddListener(ToggleFalse);

            InitializeOffPiece();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void BeforeSetUp() => offSelectable.interactable = true;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnSetUp() => offSelectable.interactable = false;

        /// <summary>
        ///
        /// </summary>
        protected virtual void FocusOff()
        {
            if (valueWithEvent != null && !valueWithEvent.Value)
                optionGroup.RewireNavigation(offSelectable);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeOffPiece()
        {
            var navigation = onSelectable.navigation;

            var offNav = offSelectable.navigation;

            UpdateOffDirection(offNav.selectOnDown, navigation.selectOnDown, Direction.Down);

            UpdateOffDirection(offNav.selectOnUp, navigation.selectOnUp, Direction.Up);

            UpdateOffDirection(offNav.selectOnLeft, navigation.selectOnLeft, Direction.Left);

            UpdateOffDirection(offNav.selectOnRight, navigation.selectOnRight, Direction.Right);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="current"></param>
        /// <param name="direction"></param>
        protected virtual void UpdateOffDirection(Selectable target, Selectable current, Direction direction)
        {
            if (current != offSelectable && target != onSelectable && target != current)
                optionGroup.options.ChangeGroupNav(current, direction);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            OnValueChangedEvent.RemoveListener(UpdateUIState);

            toggleOption.Selection.RemoveListener(ToggleTrue);

            offOption.Selection.RemoveListener(ToggleFalse);

            optionGroup.OnSetup.RemoveListener(FocusOff);

            optionGroup.OnBeforeSetup.RemoveListener(BeforeSetUp);

            optionGroup.OnSetup.RemoveListener(FocusOff);

            optionGroup.OnUnSetup.RemoveListener(UnSetUp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="param"></param>
        protected virtual void UpdateUIState(bool param)
        {
            if (param)
                UpdateToggleState(onSelectable, offSelectable);
            else
                UpdateToggleState(offSelectable, onSelectable);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="opposite"></param>
        protected virtual void UpdateToggleState(Selectable selectable, Selectable opposite)
        {
            if (ValidObjectNotSelected(selectable))
            {
                if (selectable.TryGetComponent<SelectableUIOption>(out var toggleOption))
                    toggleOption.InvokeSelect();

                var oppositeSelections = opposite.GetComponents<SelectHandler>();

                foreach (var selection in oppositeSelections)
                    if (selection != null)
                        selection.Deselect();

                optionGroup.RewireNavigation(selectable);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        /// <returns></returns>
        protected virtual bool ValidObjectNotSelected(Selectable selectable)
            => selectable != null &&
            EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != selectable.gameObject;


        /// <summary>
        ///
        /// </summary>
        protected virtual void ToggleTrue() => SetToggleTo(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void ToggleFalse() => SetToggleTo(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <remarks>
        ///
        /// </remarks>
        public virtual void SetToggleTo(bool to)
        {
            toggleOption.Selection.RemoveListener(ToggleTrue);

            offOption.Selection.RemoveListener(ToggleFalse);

            isOn = to;

            if (valueWithEvent != null)
                valueWithEvent.SetValue(isOn);

            OnValueChangedEvent?.Invoke(isOn);

            toggleOption.Selection.AddListener(ToggleTrue);

            offOption.Selection.AddListener(ToggleFalse);
        }
    }
}