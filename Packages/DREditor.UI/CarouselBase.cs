//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class CarouselBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected List<CarouselComponent> carouselObjects = new();


        [Header("")]
        [SerializeField] protected UnityEvent onCompleted = null;


        /// <summary>
        ///
        /// </summary>
        protected readonly int CarouselRate = 1;

        /// <summary>
        ///
        /// </summary>
        private bool isCompleted = false;

        /// <summary>
        ///
        /// </summary>
        private bool isRight = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual bool IsRight { get => isRight; }

        #region Initialization

        protected virtual void Start() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected abstract void Initialize();

        #endregion

        #region Carouselling

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void MoveRight() => UpdateDirectionThenMove(true);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void MoveLeft() => UpdateDirectionThenMove(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="isRight"></param>
        protected virtual void UpdateDirectionThenMove(bool isRight)
        {
            SetMovingRight(isRight);

            MoveDirection();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isRight"></param>
        protected virtual void SetMovingRight(bool isRight) => this.isRight = isRight;

        /// <summary>
        ///
        /// </summary>
        protected abstract void MoveDirection();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Index EndPieceIndex() => IsRight ? ^CarouselRate : 0;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int AddPieceOffset() => IsRight ? -CarouselRate : CarouselRate;

        #endregion

        #region Resetting

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void OnReset()
        {
            SetCompleted(false);

            ResetCarousel();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCarouselCompleted() => SetCompleted(true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="isCompleted"></param>
        protected virtual void SetCompleted(bool isCompleted)
        {
            if (this.isCompleted == isCompleted)
                return;

            this.isCompleted = isCompleted;

            if (isCompleted)
                onCompleted?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void ResetCarousel();

        #endregion
    }
}