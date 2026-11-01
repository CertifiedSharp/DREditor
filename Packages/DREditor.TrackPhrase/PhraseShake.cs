//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if CINEMACHINE
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Minigames.Debates;
using DREditor.Processables;
using UnityEngine;

namespace DREditor.TrackPhrase
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(PhraseTrack))]
    [ClassHeader(nameof(IPhraseHitListener) + ", " + nameof(IPhraseResetter) + ", " + nameof(IPrioritizable) +
        "\n\n")]
    public class PhraseShake : MonoBehaviour, IPhraseHitListener, IPrioritizable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int priority = 2;


        public int Priority { get => priority; set => priority = value; }


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float duration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float strength = 0.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float randomness = 45;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int vibrato = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool fadeOut = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool snapping = false;


        /// <summary>
        ///
        /// </summary>
        protected PhraseTrack track;


        protected virtual void Awake() => track = GetComponent<PhraseTrack>();


        /// <inheritdoc/>
        public virtual void OnHit() => Shake();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Shake()
        {
#if DOTWEEN
            if (track != null && track.IsUsingTrack())
                transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
            else
                transform.parent.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
#endif
        }

#if DOTWEEN
        /// <inheritdoc/>
        public virtual void Reset() => transform.DOKill();
#else
        /// <inheritdoc/>
        public virtual void Reset() { }
#endif
    }
}
#endif