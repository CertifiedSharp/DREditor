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
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class AnimatorUIVisibility : UIVisibilityComponent, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Animator animator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string showStateName = "ShowMainUI"; // ShowProtag ShowInstant

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideStateName = "HideMainUI"; // HideProtag HideInstant

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int animatorLayer = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool rebindOnPlay = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool rebindOnTitleScreen = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool hideOnTitleScreen = true;


        protected virtual void Start()
        {
            if (animator == null && !TryGetComponent(out animator))
                UnityEngine.Debug.LogError("ALERT: An animator component must be referenced or on the gameobject of this component!");
        }

        /// <inheritdoc/>
        protected override void ShowUI() => PlayAnimation(showStateName, animatorLayer);

        /// <inheritdoc/>
        protected override void HideUI() => PlayAnimation(hideStateName, animatorLayer);

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="animatorLayer"></param>
        protected virtual void PlayAnimation(string animationName, int animatorLayer)
        {
            if (rebindOnPlay)
                animator.Rebind();

            if (!string.IsNullOrEmpty(animationName))
                animator.Play(animationName, animatorLayer);
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