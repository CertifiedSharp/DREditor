//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DigitArrow : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected bool debugReset = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 resetPosition;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected Vector3 direction = Vector3.up;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float duration = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float directionMultiplier = 7;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool ignoreTimeScale = false;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease easeCurve = Ease.Linear;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int loops = 0;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LoopType loopType = LoopType.Yoyo;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float delay = 0f;


        protected virtual void Awake() => Initialize();

        protected virtual void OnValidate() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize() => resetPosition = transform.localPosition;


        protected virtual void Start() => StartAnimatingArrow();

        protected virtual void Update()
        {
            if (debugReset)
                StartAnimatingArrow();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartAnimatingArrow()
        {
            ResetArrow();

            AnimateArrow();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetArrow()
        {
            debugReset = false;
#if DOTWEEN
            transform.DOKill();
#endif
            transform.localPosition = resetPosition;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateArrow()
        {
#if DOTWEEN
            transform.DOLocalMove(GetEndPosition(), duration)
                .SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops, loopType)
                .SetDelay(delay);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetEndPosition() => resetPosition + (direction * directionMultiplier);
    }
}