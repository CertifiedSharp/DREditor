//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.DialogueMechanics.AutomaticLine
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AutoMechanicBase) +
        "\n\n")]
    public class AutoMechanic : AutoMechanicBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;


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
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FloatWithEvent autoWaitTime = null;


        /// <summary>
        ///
        /// </summary>
        protected bool goingToNextLine = false;


        /// <inheritdoc/>
        public override bool InAutoMode()
        {
            if (inFastForwardMode != null && inFastForwardMode.Value)
                return false;

            return base.InAutoMode();
        }

        /// <inheritdoc/>
        public override IEnumerator WaitForAutoMechanicToResolve()
        {
            goingToNextLine = false;

            StartCoroutine(AutoMechanicProcess());

            yield return WhileAutoModeIsntFinished();

            yield break;
        }

        /// <inheritdoc/>
        protected override IEnumerator AutoMechanicProcess()
        {
            yield return new WaitForSeconds(CalculateAutoWaitTime());

            EndAutoProcess();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator WhileAutoModeIsntFinished()
        {
            bool clickNextInputTriggered = submit.action.triggered;

            while (!goingToNextLine)
            {
                if ((!inMenu.Value && clickNextInputTriggered) || !inAutoMode.Value)
                    break;
                else
                    yield return null;
            }

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateAutoWaitTime() => autoWaitTime.Value;

        /// <summary>
        ///
        /// </summary>
        protected virtual void EndAutoProcess()
        {
            if (inAutoMode.Value)
                goingToNextLine = true;
        }
    }
}