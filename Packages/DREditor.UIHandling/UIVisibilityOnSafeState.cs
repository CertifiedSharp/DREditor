//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class UIVisibilityOnSafeState : ConditionOnSceneEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<UIVisibilityComponent> uiComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool visibility = true;


        /// <inheritdoc/>
        protected override void OnPassedCondition()
        {
            if (inCGMode != null)
                CheckOutOfCGMode();
            else
                SetComponentsVisibility();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckOutOfCGMode()
        {
            if (!inCGMode.Value)
                SetComponentsVisibility();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetComponentsVisibility()
        {
            foreach (var uiComponent in uiComponents)
                if (uiComponent is IUIVisibility visibleUI)
                    if (visibility)
                        visibleUI.Show();
                    else
                        visibleUI.Hide();
        }
    }
}