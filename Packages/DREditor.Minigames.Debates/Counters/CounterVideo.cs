//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Minigames.Debates;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CounterType) +
        "\n\n")]
    public class CounterVideo : CounterType
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RawImage counterTexture = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected VideoPlayer counterVideoPlayer;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float subtractTimeOffset = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int amountOfFramesToWait = 3;


        /// <inheritdoc/>
        public override void OnCompletedCounter() => counterTexture.enabled = false;


        /// <inheritdoc/>
        public override IEnumerator DisplayCounter(PhraseBase textPhrase, Line currentPanel)
        {
            StartCoroutine(CounterPlay());

            yield return new WaitForSeconds((float)counterVideoPlayer.length - subtractTimeOffset);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator CounterPlay()
        {
            counterVideoPlayer.Play();

            yield return new WaitUntil(() => counterVideoPlayer.frame > amountOfFramesToWait);
            
            counterTexture.enabled = true;

            yield break;
        }


        /// <inheritdoc/>
        public override bool Evaluate(PhraseBase textPhrase) => counterVideoPlayer != null && !textPhrase.Has<GradientOverrideModifier>();
    }
}