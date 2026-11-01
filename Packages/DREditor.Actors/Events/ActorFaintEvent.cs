//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(ITweenWaiter) + 
        "\n\n")]
    public class ActorFaintEvent : DialogueEventHandler, ITweenWaiter
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent currentActor = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float divideYPos = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fallDownDuration = 1.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fallDownWaitTime = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float actorSpriteFallDownToValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float actorSpriteFallDownDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float actorSpriteFallWaitTime = 0.3f;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease fallDownEase = Ease.InExpo;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float characterYDiff = 5;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float teleportTime = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float shakeDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float shakeStrength = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int shakeVibrato = 15;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float shakeRandomness = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool shakeFadeOut = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float shakeWaitTime = 0.3f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float unFaintSetSpriteFadeTo = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float unFaintSetSpriteFadeDuration = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float unFaintYPosDuration = 1.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float unFaintYPosWaitTime = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float unFaintSpriteToValue = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float unFaintSpriteDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float unFaintSpriteWaitTime = 0.5f;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease unFaintEase = Ease.OutExpo;
#endif

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected bool isFainting = false;

#endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o)
        {
            FaintAnim data = (FaintAnim)o;

            if (data.faint)
                StartCoroutine(FaintAnim());
            else
                StartCoroutine(UnFaintAnimation(data.to));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator FaintAnim()
        {
            Actor actor = (Actor)currentActor.Value;

            isFainting = true;
#if DOTWEEN
            Transform actorParentTransform = actor.transform;

            float calculatedYPos = actorParentTransform.localPosition.y - 
                (actor.transform.GetComponentInChildren<MeshRenderer>().transform.localPosition.y / divideYPos);

            actorParentTransform.DOMoveY(calculatedYPos, fallDownDuration)
                .SetEase(fallDownEase);

            yield return new WaitForSeconds(fallDownWaitTime);

            actor.GetComponentInChildren<SpriteRenderer>().DOFade(actorSpriteFallDownToValue, actorSpriteFallDownDuration);
#endif
            yield return new WaitForSeconds(actorSpriteFallWaitTime);

            ShakeObject shakeEvent = new(SOEnum.Camera, shakeDuration, shakeStrength, 
                shakeVibrato, shakeRandomness, shakeFadeOut);

            shakeEvent.TriggerDialogueEvent();

            yield return new WaitForSeconds(shakeWaitTime);
#if DOTWEEN
            float teleportYPosition = actorParentTransform.localPosition.y - characterYDiff;

            actorParentTransform.DOMoveY(teleportYPosition, teleportTime);
#endif
            isFainting = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toYPosition"></param>
        /// <returns></returns>
        protected virtual IEnumerator UnFaintAnimation(float toYPosition)
        {
            Actor actor = (Actor)currentActor.Value;

            isFainting = true;
#if DOTWEEN
            actor.GetComponentInChildren<SpriteRenderer>().DOFade(unFaintSetSpriteFadeTo, unFaintSetSpriteFadeDuration);

            Transform actorParentTransform = actor.transform;

            actorParentTransform.DOMoveY(toYPosition, unFaintYPosDuration)
                .SetEase(unFaintEase);

            yield return new WaitForSeconds(unFaintYPosWaitTime);

            actor.GetComponentInChildren<SpriteRenderer>().DOFade(unFaintSpriteToValue, unFaintSpriteDuration);
#endif
            yield return new WaitForSeconds(unFaintSpriteWaitTime);

            isFainting = false;
        }


        /// <inheritdoc/>
        public virtual IEnumerator WaitForTweenToFinish()
        {
            while (isFainting)
                yield return null;
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("FaintAnim");
    }
}