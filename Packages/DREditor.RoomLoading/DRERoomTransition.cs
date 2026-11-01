//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class DRERoomTransition : ServiceHolder<IRoomTransition>, IRoomTransition
    {
        public bool IsTransitioning => isTransitioning;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool replaceActiveTransition = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLoading = null;


        /// <summary>
        ///
        /// </summary>
        protected IRoomTransition previousTransition;

        /// <summary>
        ///
        /// </summary>
        protected bool isTransitioning;


        /// <summary>
        ///
        /// </summary>
        protected override void Awake()
        {
            if (replaceActiveTransition &&
                ServiceLocator.HasService<IRoomTransition>() &&
                ServiceLocator.GetService<IRoomTransition>() != this as IRoomTransition)
                StartCoroutine(AttemptReplacingActiveTransition());
            else
                base.Awake();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator AttemptReplacingActiveTransition()
        {
            yield return new WaitUntil(() => !ServiceLocator.GetService<IRoomTransition>().IsTransitioning && !inLoading.Value);

            ReplaceActiveTransition();

            base.Awake();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ReplaceActiveTransition()
        {
            previousTransition = ServiceLocator.GetService<IRoomTransition>();

            ServiceLocator.Unregister(previousTransition);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterTransition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnregisterTransition()
        {
            if (ServiceLocator.HasService<IRoomTransition>() && ServiceLocator.GetService<IRoomTransition>() == this as IRoomTransition)
            {
                ServiceLocator.Unregister<IRoomTransition>(this);

                if (previousTransition != null)
                    ServiceLocator.Register(previousTransition);
            }
        }


        /// <inheritdoc/>
        public virtual void StartTransition() => StartCoroutine(StartTransitionRoutine());

        /// <inheritdoc/>
        public abstract IEnumerator StartTransitionRoutine();

        /// <inheritdoc/>
        public virtual void EndTransition() => StartCoroutine(EndTransitionRoutine());

        /// <inheritdoc/>
        public abstract IEnumerator EndTransitionRoutine();
    }
}