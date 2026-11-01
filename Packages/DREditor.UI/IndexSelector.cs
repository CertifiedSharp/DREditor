//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class IndexSelector : ControlledComponent, ITrackableIndex
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int previousIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int selectedIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] private int selectionMaximum = 0;

        /// <summary>
        ///
        /// </summary>
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected int selectionRate = 1;

        /// <summary>
        ///
        /// </summary>
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] private int inputDirection = 0;


        [Header("")]
        [FormerlySerializedAs("OnForwardChapter")]
        public UnityEvent OnForward;

        [Header("")]
        [FormerlySerializedAs("OnBackwardChapter")]
        public UnityEvent OnBackward;

        [Header("")]
        public UnityEvent<int> OnNavigated;

        [Header("")]
        public UnityEvent OnReset;


        /// <summary>
        ///
        /// </summary>
        private int UpdatedIndex => SelectedIndex + InputDirection;

        /// <summary>
        ///
        /// </summary>
        private Action<int, int> movedIndexFrom;

        /// <summary>
        ///
        /// </summary>
        protected virtual int InputDirection { get => inputDirection; private set => inputDirection = value; }

        /// <summary>
        ///
        /// </summary>
        public virtual int SelectedIndex { get => selectedIndex; protected set => selectedIndex = value; }

        /// <summary>
        ///
        /// </summary>
        public virtual int SelectionMaximum { get => selectionMaximum; set => selectionMaximum = value; }

        /// <summary>
        ///
        /// </summary>
        public Action<int, int> MovedIndexFrom { get => movedIndexFrom; set => movedIndexFrom = value; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="InputDirection"></param>
        protected virtual void Move(int InputDirection)
        {
            InitializeInputtedDirection(InputDirection);

            ApplyMovement();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="InputDirection"></param>
        protected virtual void InitializeInputtedDirection(int InputDirection) => this.InputDirection = InputDirection;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyMovement()
        {
            if (OutOfRange())
                return;

            UpdateSelectedIndexes();

            InvokeMovedIndexFrom();

            InvokeDirection();

            InvokeNavigated();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OutOfRange() =>
            UpdatedIndex == SelectedIndex ||
            UpdatedIndex > SelectionMaximum ||
            UpdatedIndex < 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateSelectedIndexes()
        {
            previousIndex = selectedIndex;

            selectedIndex += InputDirection;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeMovedIndexFrom() => MovedIndexFrom?.Invoke(previousIndex, selectedIndex);

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeDirection() => (InputDirection == selectionRate ? OnForward : OnBackward)?.Invoke();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ResetIndexSelector()
        {
            ResetSelectedIndex();

            InvokeNavigated();

            InvokeReset();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetSelectedIndex() => SelectedIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeNavigated() => OnNavigated?.Invoke(SelectedIndex);

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeReset() => OnReset?.Invoke();
    }
}