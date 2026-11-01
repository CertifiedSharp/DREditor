//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Processables;
using DREditor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public class CarouselSelectTweening : CarouselBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected float tweenDuration = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endPieceDuration = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool isVertical = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int middleIDX = 3;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
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

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float delay = 0f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected List<CarouselComponent> resetObjects = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Vector3> positions = new();

        #region Initialization

        /// <inheritdoc/>
        protected override void Initialize()
        {
            OrderCarousel();

            InitializePositions();

            InitializeComponents();

            UpdateComponentSelection();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OrderCarousel() =>
            carouselObjects = carouselObjects.OrderBy(component => OrderedAxisOf(component)).ToList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        protected virtual float OrderedAxisOf(MonoBehaviour component) =>
            !isVertical ? component.transform.localPosition.x : component.transform.localPosition.y;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePositions()
        {
            foreach (var obj in carouselObjects)
            {
                positions.Add(obj.transform.localPosition);

                resetObjects.Add(obj);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeComponents() =>
            carouselObjects.InvokeComponents<IInitializable>((component) => component.Initialize());

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateComponentSelection() => carouselObjects.InvokeComponents(UpdateComponent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        protected virtual void UpdateComponent(CarouselComponent component) => component.OnFocusStateUpdated(IsComponentFocused(component));

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        protected virtual bool IsComponentFocused(CarouselComponent component) => carouselObjects.IndexOf(component) == middleIDX;

        #endregion

        #region Carouselling

        /// <inheritdoc/>
        protected override void MoveDirection()
        {
#if DOTWEEN
            KillAllObjectTweens();
#endif
            SetCompleted(false);

            Carousel();

            UpdateComponentOrder();

            UpdateComponentSelection();
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected virtual void KillAllObjectTweens() =>
            carouselObjects.InvokeComponents((component) => component.transform.DOKill());
#endif
        /// <summary>
        ///
        /// </summary>
        protected virtual void Carousel() => carouselObjects.InvokeComponents(TweenObject);

        /// <summary>
        ///
        /// </summary>
        /// <param name="carouselObject"></param>
        protected virtual void TweenObject(CarouselComponent carouselObject)
        {
#if DOTWEEN
            int i = carouselObjects.IndexOf(carouselObject);

            bool isEndPiece = IsDirectionEndPiece(i);

            Vector3 endPosition = isEndPiece ? positions[EndPieceIndex()] : positions[i + AddPieceOffset()];

            float duration = isEndPiece ? endPieceDuration : tweenDuration;

            carouselObject.transform.DOLocalMove(endPosition, duration)
                .SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops)
                .SetDelay(delay)
                .OnComplete(isEndPiece ? (TweenCallback)null : OnCarouselCompleted);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool IsDirectionEndPiece(int i) => 
            IsRight ? !(i - CarouselRate >= 0) : i == carouselObjects.Count - CarouselRate;

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void UpdateComponentOrder() =>
            carouselObjects = IsRight ? carouselObjects.CarouselLeft(CarouselRate) : carouselObjects.CarouselRight(CarouselRate);

#endregion

        #region Resetting

        /// <inheritdoc/>
        protected override void ResetCarousel()
        {
            ResetObjectOrder();

            ResetComponents();

            UpdateComponentSelection();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetObjectOrder()
        {
            for (int i = 0; i < carouselObjects.Count; i++)
                carouselObjects[i] = resetObjects[i];
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetComponents() =>
            carouselObjects.InvokeComponents<IResetable>((component) => component.OnReset());

        #endregion
    }
}