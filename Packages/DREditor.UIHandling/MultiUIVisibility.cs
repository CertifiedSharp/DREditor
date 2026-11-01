//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) +
        "\n\n")]
    public class MultiUIVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useRoutineForShow = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useRoutineForHide = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool inverseListForHide = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool stopAllRoutinesOnChange = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<UIVisibilityComponent> uiComponents = new();


        [Header("")]
        [SerializeField] protected UnityEvent OnShowUI;


        [Header("")]
        [SerializeField] protected UnityEvent OnHideUI;


        /// <summary>
        ///
        /// </summary>
        protected List<IUIVisibility> visibleComponents = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => visibleComponents = uiComponents.Cast<IUIVisibility>().ToList();


        /// <inheritdoc/>
        protected override void ShowUI()
        {
            OnShowUI?.Invoke();

            if (stopAllRoutinesOnChange)
                StopAllCoroutines();

            if (useRoutineForShow)
                StartCoroutine(ChangeVisibility(true));
            else
                foreach (var component in visibleComponents)
                    component.Show();
        }
        
        /// <inheritdoc/>
        protected override void HideUI()
        {
            OnHideUI?.Invoke();

            if (stopAllRoutinesOnChange)
                StopAllCoroutines();

            if (useRoutineForHide)
                StartCoroutine(ChangeVisibility(false));
            else
                foreach (var component in visibleComponents)
                    component.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ChangeVisibility(bool makeVisible)
        {
            var components = inverseListForHide && !makeVisible ? 
                visibleComponents.AsEnumerable().Reverse().ToList() : visibleComponents;

            foreach (var component in components)
            {
                if (makeVisible)
                    component.Show();
                else
                    component.Hide();

                yield return component.WaitForChangeToComplete();
            }
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            foreach (var component in visibleComponents)
                yield return component.WaitForChangeToComplete();
        }
    }
}