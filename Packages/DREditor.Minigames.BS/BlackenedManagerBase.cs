//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BlackenedManagerBase : MinigameManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference move = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference controls = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool moving = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string controlsKey = "bs";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent callControlsUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected QuestionToggle questionToggle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float cameraPositionOffset = -0.26f;


        /// <inheritdoc/>
        protected override void OnMinigameDisable()
        {
            DeactivateControls();

            base.OnMinigameDisable();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateControls()
        {
            move.action.started += MoveSelect;

            submit.action.started += SelectCharacter;

            controls.action.started += ShowControls;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DeactivateControls()
        {
            move.action.started -= MoveSelect;

            submit.action.started -= SelectCharacter;

            controls.action.started -= ShowControls;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected abstract void MoveSelect(CallbackContext context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected abstract void SelectCharacter(CallbackContext context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ShowControls(CallbackContext context)
        {
            if (moving)
                return;

            DeactivateControls();

            callControlsUI.SetValue(true);

            StartCoroutine(WaitForControlsToEnd());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForControlsToEnd()
        {
            while (inMenu.Value)
                yield return null;

            if (!trialTimeUp.Value)
                ActivateControls();

            callControlsUI.SetValue(false);
        }
    }
}