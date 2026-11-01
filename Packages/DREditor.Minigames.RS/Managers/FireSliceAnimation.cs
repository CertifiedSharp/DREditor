//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Firing;
using DREditor.Minigames.Debates.Managers;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FireTruthAnimationBase) +
        "\n\n")]
    public class FireSliceAnimation : FireTruthAnimationBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent bulletText;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text sliceText = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector3 startPosition = new(3.30999994f, 4.78000021f, 2.97000003f);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 startRotation = new(-1.70754731e-06f, 47.9529953f, 269.800018f);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 endPosition = new(3.30999994f, 4.78000021f, 2.97000003f);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 endRotation = new(-5.12264205e-06f, 47.9529991f, 89.2081604f);


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float sliceDuration = 0.8f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resetDuration = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float initialDivisor = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float splitDivisor = 2;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease ease = Ease.Linear;
#endif

        /// <summary>
        ///
        /// </summary>
        protected IPhraseManager[] phraseManagers;

        /// <summary>
        ///
        /// </summary>
        protected float initialDuration;

        /// <summary>
        ///
        /// </summary>
        protected float splitDuration;


        protected virtual void Awake()
        {
            Initialize();

            ValidateScript();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            initialDuration = sliceDuration / initialDivisor;

            splitDuration = sliceDuration / splitDivisor;

            phraseManagers = transform.root.GetComponentsInChildren<IPhraseManager>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ValidateScript()
        {
            if (bulletText == null)
                DestroyOnMissingAssignments(nameof(StringWithEvent), nameof(bulletText));
            else if (sliceText == null)
                DestroyOnMissingAssignments(nameof(TMP_Text), nameof(sliceText));
            else
#if DOTWEEN
                sliceText.DOFade(0, 0);
#else
                sliceText.color = new(sliceText.color.r, sliceText.color.g, sliceText.color.b, 0);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variableName"></param>
        protected virtual void DestroyOnMissingAssignments(string type, string variableName)
        {
            UnityEngine.Debug.LogError($"ALERT: You must assign the {type} field: {variableName}!" +
                    $"\nDestroying Animation Object.");

            Destroy(gameObject);
        }


        /// <inheritdoc/>
        public override IEnumerator FireAnimation()
        {
#if DOTWEEN
            if (sliceText != null)
                sliceText.DOFade(1, 0);
#endif
            AnimateTransform(endPosition, endRotation, sliceDuration);

            yield return new WaitForSecondsRealtime(initialDuration);

            yield return AnimateBounceback();

            StartCoroutine(LateAnimation());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator AnimateBounceback()
        {
            if (ShouldBounceBack())
            {
                AnimateTransform(startPosition, startRotation, splitDuration);

                yield return new WaitForSecondsRealtime(splitDuration);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldBounceBack()
        {
            PhraseTextBase phraseText = GetHittablePhrase();

            if (phraseText == null)
                return true;

            IPhraseShatterable shatter = phraseText.GetComponent<IPhraseShatterable>();

            PhraseBase gamePhrase = phraseText.textPhrase;

            if (gamePhrase == null || shatter == null)
                return true;

            BulletAnswerModifier data = gamePhrase.Get<BulletAnswerModifier>();

            bool phraseIsShattering = shatter.IsShattering();

            bool notHittable = !gamePhrase.Has<HittableModifierBase>();

            bool answerIsWrong = data != null && bulletText.Value != data.AnswerBullet.DisplayName;

            bool bounceBack = phraseIsShattering || notHittable || answerIsWrong;

            return bounceBack;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PhraseTextBase GetHittablePhrase()
        {
            PhraseTextBase fallbackPhrase = null;

            foreach (var manager in phraseManagers)
                foreach (var phrase in manager.GetActivePhrases())
                    if (phrase.textPhrase.Has<HittableModifierBase>())
                        return phrase;
                    else
                        fallbackPhrase = phrase;

            return fallbackPhrase;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator LateAnimation()
        {
            yield return new WaitForSecondsRealtime(splitDuration);

            AnimateTransform(startPosition, startRotation, resetDuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="duration"></param>
        protected virtual void AnimateTransform(Vector3 position, Vector3 rotation, float duration)
        {
#if DOTWEEN
            sliceText.transform.DOKill();

            sliceText.transform.DOMove(position, duration).SetEase(ease);

            sliceText.transform.DORotate(rotation, duration).SetEase(ease);
#endif
        }


        /// <inheritdoc/>
        public override float CalculateRemainingDuration() => splitDuration;


        /// <summary>
        ///
        /// </summary>
        [BehaviourButton("Set Start Transform to Slice Text Transform")]
        public void SetStart()
        {
            startPosition = sliceText.transform.position;

            startRotation = sliceText.transform.eulerAngles;
        }

        /// <summary>
        ///
        /// </summary>
        [BehaviourButton("Set End Transform to Slice Text Transform")]
        public void SetEnd()
        {
            endPosition = sliceText.transform.position;

            endRotation = sliceText.transform.eulerAngles;
        }
    }
}