//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) +
        "\n\n")]
    public class AnimatorBoolUIVisibility : UIVisibilityComponent, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debugLogs = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Animator animator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string showBoolTriggerName = "HideBox"; // ShowDUI MoveDUI ConfirmSelect Appear On DisplayName

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideBoolTriggerName = "HideBox"; // ShowDUI MoveDUI ConfirmSelect Appear On DisplayName

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setBoolOnShow = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setBoolOnHide = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int animatorLayer = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool rebindOnTitleScreen = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool hideOnTitleScreen = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool rebindOnHide = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool keepAnimatorStateOnDisable = false;


        protected virtual void Awake()
        {
            if (animator != null)
                animator.keepAnimatorStateOnDisable = keepAnimatorStateOnDisable;
        }

        /// <inheritdoc/>
        protected override void ShowUI() => UpdateVisibility(showBoolTriggerName, setBoolOnShow);

        /// <inheritdoc/>
        protected override void HideUI()
        {
            UpdateVisibility(hideBoolTriggerName, setBoolOnHide);

            if (rebindOnHide)
                animator.Rebind();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="triggerState"></param>
        protected virtual void UpdateVisibility(string trigger, bool triggerState)
        {
            if (debugLogs)
                UnityEngine.Debug.Log($"REPORT: {gameObject.name}: Is setting {trigger} to {triggerState}.");

            animator.SetBool(trigger, triggerState);
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete() => animator.WaitForStateToComplete(animatorLayer);


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            if (hideOnTitleScreen)
                Hide();

            if (rebindOnTitleScreen)
                animator.Rebind();
        }
    }
}