//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineWaitForClick) + ", " + 
        nameof(ILineEndProcess) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ToggleUIMechanic : DialogueComponent, ILineWaitForClick, ILineEndProcess, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference toggle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isUIToggled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;


        /// <summary>
        ///
        /// </summary>
        protected bool previousMenuAccesState = false;


        public virtual bool SkipOnFastForward => true;


        /// <inheritdoc/>
        public virtual IEnumerator LineWaitForClick<T>
            (DialogueBase<T> currentDialogue, Line currentLine, Action exitParentCallback = null) 
            where T : Line
        {
            isUIToggled.SetValue(true);

            toggle.action.started += OnToggleUIKeyPressed;

            yield break;
        }

        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null) 
            => toggle.action.started -= OnToggleUIKeyPressed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void OnToggleUIKeyPressed(CallbackContext ctx)
        {
            if (inMenu.Value)
                return;

            if (hasMenuAccess.Value)
                previousMenuAccesState = hasMenuAccess.Value;

            isUIToggled.SetValue(!isUIToggled.Value);

            hasMenuAccess.SetValue(previousMenuAccesState && isUIToggled.Value);

            if (previousMenuAccesState && isUIToggled.Value)
                previousMenuAccesState = false;
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            toggle.action.started -= OnToggleUIKeyPressed;

            previousMenuAccesState = false;
        }
    }
}