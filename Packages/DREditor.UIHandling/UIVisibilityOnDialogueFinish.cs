//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Finishing;
using DREditor.Dialogues;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IFinishableDialogueComponent) +
        "\n\n")]
    public class UIVisibilityOnDialogueFinish : DialogueComponent, IFinishableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition condition = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool visibility = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float finishOrder = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int finisherID = 1;


        public float Order { get => finishOrder; }

        public int ExclusiveHandlerID => finisherID;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (uiComponent is not IUIVisibility)
                UnityEngine.Debug.LogError($"{gameObject.name + " " + ToString()}: " +
                    $"The UI component assigned is not of type {typeof(IUIVisibility)}.");
        }


        /// <inheritdoc/>
        public virtual void OnFinishDialogue()
        {
            if (inLeaveProcess == null || (!inLeaveProcess.Value && CheckCondition()))
                SetVisibility();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckCondition()
        {
            if (condition == null)
                return true;

            return condition.Evaluate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetVisibility()
        {
            if (uiComponent is IUIVisibility visibleUI)
                if (visibility)
                    visibleUI.Show();
                else
                    visibleUI.Hide();
        }
    }
}