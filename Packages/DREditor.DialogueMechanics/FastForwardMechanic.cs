//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDialogueReaderInitialize) + ", " + nameof(IDialogueReaderCleanUp) + ", "
        + nameof(IFinishableDialogueComponent) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class FastForwardMechanic : DialogueComponent, IDialogueReaderInitialize, IDialogueReaderCleanUp,
        IFinishableDialogueComponent, IToTitleScreen
    {
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
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

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
        [Tooltip("")]
        [SerializeField] protected FloatWithEvent fastForwardSpeed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float finishOrder = 5;


        public float Order { get => finishOrder; }


        protected virtual void OnEnable() => inFastForwardMode.AddListener(OnFastForwardUpdated);

        protected virtual void OnDisable() => inFastForwardMode.RemoveListener(OnFastForwardUpdated);

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        protected virtual void OnFastForwardUpdated(bool to) => Time.timeScale = to ? fastForwardSpeed.Value : 1;


        /// <summary>
        ///
        /// </summary>
        /// <param name="forcedOff"></param>
        protected virtual void SetFastForwardForcedEnabled(bool forcedOff)
        {
            if (!forcedOff)
                EnableAbilityToFF();
            else
                DisableAbilityToFF();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableAbilityToFF()
        {
            if (fastForwardModeEnabled.Value)
                return;

            fastForwardModeWasEnabled.RemoveListener(SetFastForwardForcedEnabled);

            fastForwardModeEnabled.SetValue(true);

            AllowInput(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableAbilityToFF()
        {
            if (!fastForwardModeEnabled.Value)
                return;

            fastForwardModeWasEnabled.AddListener(SetFastForwardForcedEnabled);

            fastForwardModeEnabled.SetValue(false);

            AllowInput(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void FFModeOn(CallbackContext context)
        {
            if (inFastForwardMode.Value || inMenu.Value || inInstant.Value)
                return;

            if (!fastForwardModeEnabled.Value)
            {
                AllowInput(false);

                return;
            }

            inFastForwardMode.SetValue(true);

            hasMenuAccess.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void FFModeOff(CallbackContext context)
        {
            if (!inFastForwardMode.Value)
                return;

            inFastForwardMode.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="allowed"></param>
        protected virtual void AllowInput(bool allowed)
        {
            if (allowed)
                EnableInput();
            else
                DisableInput();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableInput()
        {
            fastForward.action.started += FFModeOn;

            fastForward.action.canceled += FFModeOff;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableInput()
        {
            fastForward.action.started -= FFModeOn;

            fastForward.action.canceled -= FFModeOff;
        }


        /// <inheritdoc/>
        public virtual void OnDialogueReaderInitialize() => EnableAbilityToFF();

        /// <inheritdoc/>
        public virtual void OnDialogueReaderCleanUp()
        {
            if (inFastForwardMode.Value)
                FFModeOff(new CallbackContext());

            DisableAbilityToFF();
        }

        /// <inheritdoc/>
        public virtual void OnFinishDialogue() => FFModeOff(new CallbackContext());


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => DisableAbilityToFF();
    }
}