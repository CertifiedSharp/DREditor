//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueUIComponent : DialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition audioCondition = null;

        [Header("")]
        [SerializeField] protected UnityEvent onPlaySFX;


        /// <summary>
        ///
        /// </summary>
        protected IUIVisibility visibleUI;


        protected virtual void Awake()
        {
            if (uiComponent is IUIVisibility visibleUI)
                this.visibleUI = visibleUI;
            else
                UnityEngine.Debug.LogWarning($"{gameObject.name + " " + ToString()}: " +
                    $"The UI component assigned is not of type {typeof(IUIVisibility)}.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator WaitForVisibilityChange()
        {
            yield return visibleUI?.WaitForChangeToComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void SetDialogueUIVisibility();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateUISFX()
        {
            if (CanPlayUISFX())
                PlaySFX();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanPlayUISFX() => audioCondition == null || audioCondition.Evaluate();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlaySFX() => onPlaySFX?.Invoke();
    }
}