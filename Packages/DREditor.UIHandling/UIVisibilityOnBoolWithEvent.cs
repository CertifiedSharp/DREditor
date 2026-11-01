//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(UIVisibilityHandler) +
        "\n\n")]
    public class UIVisibilityOnBoolWithEvent : UIVisibilityHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool targetState = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool visibility = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent boolWithEvent = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool setVisibilityToInverseBoolEvent = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool matchVisibilityWithEvent = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition condition = null;


        /// <summary>
        ///
        /// </summary>
        protected bool localStateInitialized = false;

        /// <summary>
        ///
        /// </summary>
        protected bool localState = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            boolWithEvent.AddListener(EvaluateEvent);

            if (matchVisibilityWithEvent && localStateInitialized && localState != boolWithEvent.Value)
                EvaluateEvent(boolWithEvent.Value);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => boolWithEvent.RemoveListener(EvaluateEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventState"></param>
        protected virtual void EvaluateEvent(bool eventState)
        {
            if (condition != null && !condition.Evaluate())
                return;

            if (setVisibilityToInverseBoolEvent)
                visibleUI.SetVisibility(!eventState);
            else if (matchVisibilityWithEvent)
            {
                localStateInitialized = true;

                localState = eventState;

                visibleUI.SetVisibility(eventState);
            }
            else if (eventState == targetState)
                visibleUI.SetVisibility(visibility);
        }
    }
}