//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;
using AnimationEvent = DREditor.Dialogues.Events.AnimationEvent;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    public class AnimationEventHandler : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugLogs = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Animator animator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int animatorLayer = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string oneShotAnimName = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool DestroyOnLoadFile = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Animation clip = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool waitForClipEnd = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool DestroyOnEnd = false;


        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o = null) => OnAnimationEventTriggered(o);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void OnAnimationEventTriggered(object o = null)
        {
            if (debugLogs)
                UnityEngine.Debug.Log($"{ToString()}: " + dialogueEventString + " was called.");

            if (isLoadingFile.Value)
                OnLoadingFile((AnimationEvent)o);
            else
                OnAnimationEvent((AnimationEvent)o);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationEvent"></param>
        protected virtual void OnLoadingFile(AnimationEvent animationEvent)
        {
            if (DestroyOnLoadFile)
                Destroy(gameObject);
            else if (clip != null)
                CallClip(animationEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationEvent"></param>
        protected virtual void OnAnimationEvent(AnimationEvent animationEvent)
        {
            if (clip != null)
                CallClip(animationEvent);
            else
                StartCoroutine(AnimationRoutine(animationEvent));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationEvent"></param>
        /// <returns></returns>
        protected virtual IEnumerator AnimationRoutine(AnimationEvent animationEvent)
        {
            if (debugLogs)
                UnityEngine.Debug.Log($"{ToString()}: Starting Animation Routine");

            if (debugLogs && oneShotAnimName != "")
                UnityEngine.Debug.Log($"{ToString()}: calling one-shot animation: " + oneShotAnimName);

            if (oneShotAnimName != "")
                animator.Play(oneShotAnimName);


            float aLength = animator.GetCurrentAnimatorStateInfo(animatorLayer).length;

            if (!float.IsPositiveInfinity(aLength))
                yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime > 1.0f);


            animationEvent.SetFinished();

            if (debugLogs)
                UnityEngine.Debug.Log($"{ToString()}: Finished animation event: " + dialogueEventString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationEvent"></param>
        protected virtual void CallClip(AnimationEvent animationEvent)
        {
            clip.Play();

            if (waitForClipEnd)
                StartCoroutine(WaitForClip(animationEvent));
            else
                animationEvent.SetFinished();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="animationEvent"></param>
        protected virtual IEnumerator WaitForClip(AnimationEvent animationEvent)
        {
            yield return new WaitForSeconds(clip.clip.length);

            animationEvent.SetFinished();

            if (DestroyOnEnd)
                Destroy(gameObject);
        }


        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public void TestAnimation()
        {
            if (Application.isPlaying)
                OnAnimationEventTriggered(new AnimationEvent(dialogueEventString));
        }
    }
}