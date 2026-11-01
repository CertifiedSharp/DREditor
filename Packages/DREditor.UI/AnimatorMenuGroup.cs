//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuGroup) +
        "\n\n")]
    public class AnimatorMenuGroup : MenuGroup
    {
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
        [SerializeField] protected string animatorTriggerShowString = "Reveal";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string animatorTriggerHideString = "Hide";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool autoEnableAnimator = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool waitForShowAnimationToFinish = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool waitForHideAnimationToFinish = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool leaveOnlyOnBack = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool individualAnimate = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string individualAnimateShowString = "Show";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string individualAnimateHideString = "Hide";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float asyncTime = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float individualAnimateEndState = 1f;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int individualAnimateLayer = 0;


        /// <summary>
        ///
        /// </summary>
        protected readonly List<Animator> animators = new();


        /// <inheritdoc/>
        protected override void TurnOffSelectables()
        {
            base.TurnOffSelectables();

            if (individualAnimate)
                foreach (Selectable selectable in selectables)
                    animators.Add(selectable.GetComponent<Animator>());
        }

        /// <inheritdoc/>
        protected override IEnumerator OnMovingToBackGroup()
        {
            yield return WaitForAnimatorComplete();

            if (!waitForHideAnimationToFinish)
                yield return base.OnMovingToBackGroup();

            yield break;
        }

        /// <inheritdoc/>
        public override void Show()
        {
            if (isActive)
            {
                UnityEngine.Debug.Log(gameObject.name + " was already Active");

                ServiceLocator.GetService<MenuState>().SetChanging(false);

                return;
            }

            ShowAnimation();

            if (individualAnimate)
                StartCoroutine(IndividualAnim(individualAnimateShowString));

            base.Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowAnimation()
        {
            if (animator != null)
            {
                if (autoEnableAnimator && !animator.enabled)
                {
                    animator.enabled = true;

                    animator.Rebind();
                }
                else
                {
                    UnityEngine.Debug.Log("REPORT: Menu Group " + gameObject.name + ": Setting Triggers");

                    animator.SetTrigger(animatorTriggerShowString);

                    animator.ResetTrigger(animatorTriggerHideString);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        protected virtual IEnumerator IndividualAnim(string to)
        {
            foreach (Animator animator in animators)
            {
                if (animator != null && animator.gameObject.activeSelf)
                {
                    animator.ResetTrigger(animatorTriggerHideString);

                    animator.SetTrigger(to);

                    yield return new WaitForSecondsRealtime(asyncTime);
                }
            }

            yield break;
        }

        /// <inheritdoc/>
        protected override IEnumerator ShowProcess()
        {
            if (waitForShowAnimationToFinish && !individualAnimate)
                yield return WaitForAnimatorComplete();

            UnityEngine.Debug.Log("REPORT: Revealing Animation for Group: " + gameObject.name);

            yield return base.ShowProcess();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForAnimatorComplete()
        {
            if (animator == null)
                yield break;

            yield return animator.WaitForStateToComplete(animatorLayer);

            yield break;
        }

        /// <inheritdoc/>
        protected override IEnumerator OnSelectionEvaluated(GameObject go)
        {
            if (individualAnimate && go.TryGetComponent<Animator>(out var animator))
                yield return animator.WaitForStateToComplete(individualAnimateLayer, individualAnimateEndState);

            yield return base.OnSelectionEvaluated(go);

            yield break;
        }

        /// <inheritdoc/>
        protected override bool HideCanvasEvaluation() => base.HideCanvasEvaluation() && !waitForHideAnimationToFinish;

        /// <inheritdoc/>
        protected override void OnHideComplete()
        {
            if (waitForHideAnimationToFinish)
                StartCoroutine(WaitForHide());
            else
                base.OnHideComplete();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator WaitForHide()
        {
            yield return WaitForAnimatorComplete();

            base.OnHideComplete();

            UnityEngine.Debug.Log("REPORT: " + gameObject.name + "'s WaitForHides process finished and should now not be active.");

            yield break;
        }


        /// <inheritdoc/>
        protected override void HideProcess(MenuGroupBase shownGroup = null)
        {
            if (individualAnimate)
                StartCoroutine(IndividualAnim(individualAnimateHideString));

            OnAnimatorHide(shownGroup);

            base.HideProcess(shownGroup);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="shownGroup"></param>
        protected virtual void OnAnimatorHide(MenuGroupBase shownGroup = null)
        {
            if (animator != null)
            {
                if (autoEnableAnimator && animator.enabled)
                    animator.enabled = false;

                if (waitForHideAnimationToFinish || autoEnableAnimator)
                    EventSystem.current.SetSelectedGameObject(null);

                if (!autoEnableAnimator && (!leaveOnlyOnBack || shownGroup == backGroup))
                    animator.SetTrigger(animatorTriggerHideString);

                if (!autoEnableAnimator)
                    animator.ResetTrigger(animatorTriggerShowString);
            }
        }
    }
}