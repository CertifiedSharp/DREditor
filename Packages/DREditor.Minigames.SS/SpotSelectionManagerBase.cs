//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SpotSelectionManagerBase : MinigameManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Button yesButton = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Button noButton = null;


        /// <inheritdoc/>
        protected override void OnMinigameEnable()
        {
            base.OnMinigameEnable();

            yesButton.onClick.AddListener(ConfirmSelection);

            noButton.onClick.AddListener(LeaveConfirmUI);
        }

        /// <inheritdoc/>
        protected override void OnMinigameDisable()
        {
            base.OnMinigameDisable();

            yesButton.onClick.RemoveListener(ConfirmSelection);

            noButton.onClick.RemoveListener(LeaveConfirmUI);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public abstract void ConfirmSelection();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public abstract void LeaveConfirmUI();


        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableSubmit() => submit.action.started += SubmitSpot;

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableSubmit() => submit.action.started -= SubmitSpot;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected abstract void SubmitSpot(CallbackContext ctx);
    }
}