//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISwipeListener) +
        "\n\n")]
    public class SwipeVisualizer : MonoBehaviour, ISwipeListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image cutImage;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float invisibleValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float visibleValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setUpdate = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startDuration = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float swipeDuration = 0.2f;
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float diagonalAngle = 34.44f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float verticalAngle = 90;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float horizontalAngle = 0;
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Min(1)]
        [Tooltip("")]
        [SerializeField] protected float durationDivisor = 2;

        /// <summary>
        ///
        /// </summary>
        [Min(1)]
        [Tooltip("")]
        [SerializeField] protected float dimensionDivisor = 2;


        /// <summary>
        ///
        /// </summary>
        protected Vector3 swipeStart;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 swipeEnd;

        /// <summary>
        ///
        /// </summary>
        protected float dividedDuration;


        /// <inheritdoc/>
        public virtual void OnSwiped(Vector3 swipeStart, Vector3 swipeEnd)
        {
            Initialize(swipeStart, swipeEnd);

            VisualizeSwipeOnValid();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="swipeStart"></param>
        /// <param name="swipeEnd"></param>
        protected virtual void Initialize(Vector3 swipeStart, Vector3 swipeEnd)
        {
            this.swipeStart = swipeStart;

            this.swipeEnd = swipeEnd;

            dividedDuration = swipeDuration / durationDivisor;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void VisualizeSwipeOnValid()
        {
            if (cutImage != null)
                VisualizeSwipe();
            else
                UnityEngine.Debug.LogError($"ALERT: {nameof(cutImage)} not assigned on {name}");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void VisualizeSwipe()
        {
            InitializeSwipePositions();

            AngleSwipeImage();
#if DOTWEEN
            KillTweens();

            AnimateStart();

            AnimateSwipe();
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSwipePositions()
        {
            swipeEnd.x = EvaluateCoordinate(swipeEnd.x, Screen.width);

            swipeEnd.y = EvaluateCoordinate(swipeEnd.y, Screen.height);

            swipeStart.x = swipeEnd.x * -1;

            swipeStart.y = swipeEnd.y * -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        protected virtual float EvaluateCoordinate(float coordinate, float dimension)
        {
            if (coordinate < 0)
                return dimension / dimensionDivisor * -1;
            else if (coordinate > 0)
                return dimension / dimensionDivisor;
            else
                return 0;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AngleSwipeImage() => cutImage.transform.eulerAngles = new Vector3(0, 0, CalculateImageAngle());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateImageAngle()
        {
            if (swipeStart.x == 0)
                return verticalAngle;
            else if (swipeStart.y == 0)
                return horizontalAngle;
            else
                return swipeEnd.x * swipeEnd.y > 0 ? diagonalAngle : -diagonalAngle;
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected virtual void KillTweens()
        {
            cutImage.DOKill();

            cutImage.rectTransform.DOKill();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateStart()
        {
            cutImage.DOFade(invisibleValue, startDuration).SetUpdate(setUpdate);

            cutImage.rectTransform.DOLocalMove(swipeStart, startDuration).SetUpdate(setUpdate);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateSwipe()
        {
            cutImage.DOFade(visibleValue, dividedDuration).SetUpdate(setUpdate).OnComplete(OnCutComplete);

            cutImage.rectTransform.DOLocalMove(swipeEnd, swipeDuration).SetUpdate(setUpdate);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCutComplete() => cutImage.DOFade(invisibleValue, dividedDuration).SetUpdate(setUpdate);
#endif
    }
}