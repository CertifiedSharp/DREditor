//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<TutorialService>) +
        "\n\n")]
    public class TutorialService : ServiceHolder<TutorialService>
    {
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
        [SerializeField] protected TutorialAsset asset = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference toggle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference cancel = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TutorialManager manager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useToggleControls = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueEventString = "TrialTutorial";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent callTutorialUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;


        protected virtual void OnEnable() => ToggleListeners(true);

        protected virtual void OnDisable() => ToggleListeners(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="enable"></param>
        protected virtual void ToggleListeners(bool enable)
        {
            ToggleInvokeEvent(enable);

            ToggleShowControls(enable);

            ToggleDialogueEvent(enable);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startListening"></param>
        protected virtual void ToggleInvokeEvent(bool startListening)
        {
            if (callTutorialUI == null)
                return;

            if (startListening)
                callTutorialUI.AddListener(EvaluateGlobal);
            else
                callTutorialUI.RemoveListener(EvaluateGlobal);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="activateControls"></param>
        protected virtual void EvaluateGlobal(bool activateControls)
        {
            if (activateControls && !manager.Active)
                Show(new());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enable"></param>
        protected virtual void ToggleShowControls(bool enable)
        {
            if (!useToggleControls)
                return;

            if (enable)
                toggle.action.started += Show;
            else
                toggle.action.started -= Show;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startListening"></param>
        protected virtual void ToggleDialogueEvent(bool startListening)
        {
            if (startListening)
                DialogueEventSystem.StartListening(dialogueEventString, manager.TurnOn);
            else
                DialogueEventSystem.StopListening(dialogueEventString, manager.TurnOn);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void Show(CallbackContext ctx) => SetDisplay(true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void Hide(CallbackContext ctx) => SetDisplay(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="displaying"></param>
        protected virtual void SetDisplay(bool displaying)
        {
            if (displaying == manager.Active)
                return;

            ToggleShowControls(!displaying);

            ToggleHideControls(displaying);

            ToggleManager(displaying);

            UpdateMenuState(displaying);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enable"></param>
        protected virtual void ToggleHideControls(bool enable)
        {
            if (enable)
                cancel.action.started += Hide;
            else
                cancel.action.started -= Hide;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        protected virtual void ToggleManager(bool on)
        {
            if (on)
                manager.TurnOn(asset);
            else
                manager.TurnOff();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="menuState"></param>
        protected virtual void UpdateMenuState(bool menuState)
        {
            if (callTutorialUI != null && inMenu != null)
            {
                inMenu.SetValue(menuState);

                inMenu.Invoke();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (debugMode && asset != null)
                manager.TurnOn(asset);
        }
    }
}