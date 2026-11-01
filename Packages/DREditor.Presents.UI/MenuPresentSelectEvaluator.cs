//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PresentSelectEvaluator) + 
        "\n\n")]
    public class MenuPresentSelectEvaluator : PresentSelectEvaluator
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase choiceMenuGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Selectable yesSelectable = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool callNextDayOnNoPresent = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string callNextDay = "NextDay";


        /// <inheritdoc/>
        protected override void OnEnable() => choiceMenuGroup.StartEvents.AddListener(OnChoiceMenuStart);

        /// <inheritdoc/>
        protected override void OnDisable() => choiceMenuGroup.StartEvents.RemoveListener(OnChoiceMenuStart);

        /// <inheritdoc/>
        protected override void OnSelectedValidPresent() => choiceMenuGroup.Show();

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void BeforeEvaluateChoice(CallbackContext ctx) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void AfterEvaluateChoice(CallbackContext ctx) => choiceMenuGroup.Hide();

        /// <inheritdoc/>
        protected override void EvaluatePresentChoice(CallbackContext ctx)
        {
            BeforeEvaluateChoice(ctx);

            base.EvaluatePresentChoice(ctx);

            if (EventSystem.current.currentSelectedGameObject == yesSelectable.gameObject)
                OnGivePresentSelected();
            else
                OnDontGivePresentSelected();

            EventSystem.current.SetSelectedGameObject(null);

            AfterEvaluateChoice(ctx);
        }

        /// <inheritdoc/>
        protected override void OnGivePresentSelected()
        {
            base.OnGivePresentSelected();

            if (!gavePresent && callNextDayOnNoPresent)
                DialogueEventSystem.TriggerEvent(callNextDay);
        }
    }
}