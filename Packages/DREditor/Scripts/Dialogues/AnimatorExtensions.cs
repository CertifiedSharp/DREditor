//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class AnimatorExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animatorLayer"></param>
        /// <param name="endState"></param>
        public static IEnumerator WaitForStateToComplete(this Animator animator, int animatorLayer = 0, float endState = 1f)
        {
            yield return null;

            endState = Mathf.Clamp(endState, 0f, 1f);

            while (animator.GetCurrentAnimatorStateInfo(animatorLayer).speed != 0 &&
                (animator.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime < endState ||
                animator.IsInTransition(animatorLayer)))
                yield return null;

            yield break;
        }

        public static AnimationClip GetAnimationClip(this Animator animator, string animName)
        {
            foreach (AnimationClip ac in animator.runtimeAnimatorController.animationClips)
                if (ac.name == animName)
                    return ac;

            UnityEngine.Debug.LogWarning($"ALERT: Animation Clip {animName} was Not found! Check the Animator and make sure it's there!");

            return null;
        }
    }
}