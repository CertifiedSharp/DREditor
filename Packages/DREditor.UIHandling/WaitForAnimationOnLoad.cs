//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IWaitAnimationCompleteOnLoad) +
        "\n\n")]
    public class WaitForAnimationOnLoad : MonoBehaviour, IWaitAnimationCompleteOnLoad
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Animator animator;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int layer = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maxWaitTime = 3f;


        /// <inheritdoc/>
        public virtual IEnumerator WaitForAnimationToComplete()
        {
            if (animator != null)
            {
                float clipTime = animator.GetCurrentAnimatorStateInfo(layer).length;

                float waitTime = clipTime < maxWaitTime ? clipTime : maxWaitTime;

                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}