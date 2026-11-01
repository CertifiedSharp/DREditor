//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using DREditor.UIHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuModifier) + ", " + nameof(RegulationsUIBase) +
        "\n\n")]
    public class RegulationsUI : RegulationsUIBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("Options")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useYInput = false;

        /// <summary>
        ///
        /// </summary>
        [Range(-1, 1)]
        [Tooltip("")]
        [SerializeField] protected float rightThreshold = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Range(-1, 1)]
        [Tooltip("")]
        [SerializeField] protected float leftThreshold = -0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int moveRegulationAmount = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RegulationDisplayer displayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent leftArrow = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent rightArrow = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bufferChangeDuration = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<UIVisibilityComponent> rightChangeComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<UIVisibilityComponent> leftChangeComponents = new();


        /// <summary>
        ///
        /// </summary>
        [Header("Debug Viewing")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int regIndex = 0;


        /// <summary>
        /// 
        /// </summary>
        [Header("")]
        [SerializeField] protected UnityEvent OnUINavigated = null;


        /// <summary>
        /// 
        /// </summary>
        protected bool isChanging = false;


        protected override void ResetUI()
        {
            if (regulations == null)
                return;

            regIndex = 0;

            SetArrows();

            DisplayRegulation();
        }

        
        protected override void ChangeRegulation(CallbackContext context)
        {
            if (ChangeAttemptInvalid())
                return;

            InvokeUINavigated();

            ChangeRegulation();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ChangeAttemptInvalid() => isChanging || regulations == null;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InvokeUINavigated() => OnUINavigated?.Invoke();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ChangeRegulation()
        {
            Vector2 nav = navigate.action.ReadValue<Vector2>();

            float inputDirection = !useYInput ? nav.x : nav.y;

            if (inputDirection > rightThreshold && IndexIsValidAndActive())
                StartCoroutine(ChangeRegulationSequence(moveRegulationAmount));
            else if (inputDirection < leftThreshold && regIndex > 0)
                StartCoroutine(ChangeRegulationSequence(-moveRegulationAmount));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool IndexIsValidAndActive() 
            => regIndex < regulations.Count - 1 && regIndex + 1 < regulations.Count && regulations[regIndex + 1].IsActiveRegulation();

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        protected virtual IEnumerator ChangeRegulationSequence(int direction)
        {
            isChanging = true;

            yield return new WaitForEndOfFrame();

            List<UIVisibilityComponent> components = direction == 1 ? rightChangeComponents : leftChangeComponents;

            HideComponents(components);

            regIndex += direction;

            SetArrows();

            yield return new WaitForSecondsRealtime(bufferChangeDuration);

            DisplayRegulation();

            yield return new WaitForSecondsRealtime(bufferChangeDuration);

            ShowComponents(components);

            isChanging = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        protected virtual void ShowComponents(List<UIVisibilityComponent> components)
        {
            foreach (var component in components)
                component.Show();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        protected virtual void HideComponents(List<UIVisibilityComponent> components)
        {
            foreach (var component in components)
                component.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetArrows()
        {
            if (regulations.Count < 2 || regIndex == 0)
                leftArrow.Hide();
            else
                leftArrow.Show();

            if (regulations.Count < 2 ||
                regIndex + 1 < regulations.Count && !regulations[regIndex + 1].IsActiveRegulation() ||
                regIndex == regulations.Count - 1)
                rightArrow.Hide();
            else
                rightArrow.Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRegulation() => displayer.DisplayRegulation(regulations[regIndex]);
    }
}