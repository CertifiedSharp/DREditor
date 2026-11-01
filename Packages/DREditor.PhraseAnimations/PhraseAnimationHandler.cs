//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Processables;
using TMPro;
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPhraseAnimationComponent) + ", " + nameof(IPhraseHandlerComponent) + ", " +
        nameof(IPhraseShatterListener) + ", " + nameof(IPhraseResetter) + ", " + nameof(IPrioritizable) +
        "\n\n")]
    public class PhraseAnimationHandler : MonoBehaviour, IPhraseAnimationComponent, IPhraseShatterListener, IPrioritizable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int priority = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Animation compiledAnimation;

        public int Priority { get => priority; set => priority = value; }


        /// <summary>
        ///
        /// </summary>
        protected readonly string animationClipName = "transform";

        /// <summary>
        ///
        /// </summary>
        protected float duration;

        /// <summary>
        ///
        /// </summary>
        protected PhraseAnimation phraseAnimation = null;


        protected virtual void Awake() => compiledAnimation = GetComponent<Animation>();


        /// <inheritdoc/>
        public virtual void StartPhrase<T>(T phrase) where T : PhraseBase
        {
            InitializePhraseAnimation(phrase);

            if (phraseAnimation != null)
            {
                InitializeText();

                CompilePhraseAnimation();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        protected virtual void InitializePhraseAnimation(PhraseBase phrase)
        {
            phraseAnimation = null;

            foreach (var architecture in phrase.animations)
                if (architecture is PhraseAnimation phraseDataAnimation)
                    phraseAnimation = phraseDataAnimation;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeText()
        {
            var tmpText = GetComponent<TMP_Text>();

            tmpText.transform.localScale = Vector3.one;

            //Below makes it to where using the dolly track only works once 
            if (phraseAnimation.spawnPoint == Vector3.zero || phraseAnimation.spawnPoint == null)
                tmpText.transform.position = phraseAnimation.spawnPoint;
            else
                tmpText.transform.localPosition = phraseAnimation.spawnPoint;

            Vector4 rot = phraseAnimation.spawnAngle;

            tmpText.transform.localEulerAngles = new(rot.x, rot.y, rot.z);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CompilePhraseAnimation()
        {
            compiledAnimation.enabled = true;

            AnimationClip animationClip = new()
            {
                legacy = true
            };

            phraseAnimation.Initialize(animationClip);

            compiledAnimation.AddClip(animationClip, animationClipName);

            compiledAnimation.Play(animationClipName);

            duration = animationClip.length;
        }


        /// <inheritdoc/>
        public virtual void OnShattered()
        {
            compiledAnimation.Stop();

            compiledAnimation.enabled = false;
        }

        /// <inheritdoc/>
        public virtual float GetPhraseDuration() => duration;

        /// <inheritdoc/>
        public virtual void Reset() => compiledAnimation.enabled = false;
    }
}