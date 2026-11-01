//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CallableUIBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference input;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent callControlsUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateBoolCondition condition = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="allowInput"></param>
        protected virtual void OnInputUpdated(bool allowInput)
        {
            if (input == null)
            {
                UnityEngine.Debug.LogError($"Action Reference is missing on {gameObject.name}!", this);

                return;
            }

            if (input.action == null)
            {
                UnityEngine.Debug.LogError($"The reference exists, but the InputAction asset link is null!", this);

                return;
            }

            if (allowInput)
                input.action.started += CallControls;
            else
                input.action.started -= CallControls;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void CallControls(InputAction.CallbackContext context)
        {
            if (condition != null && !condition.Evaluate())
                return;

            OpenControlsMenu();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OpenControlsMenu()
        {
            callControlsUI.SetValue(true);

            StartCoroutine(WaitForControlsEnd());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForControlsEnd()
        {
            while (inMenu.Value)
                yield return null;

            callControlsUI.SetValue(false);
        }
    }
}