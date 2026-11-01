//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
    public abstract class HoverReticle : Reticle
    {
        [Header("")]
        public UnityEvent OnConfirmSelection;

        [Header("")]
        public UnityEvent OnHoverSelection;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<ReticleComponent> reticleComponents = new();


        /// <summary>
        ///
        /// </summary>
        protected bool hovering = false;

        /// <summary>
        ///
        /// </summary>
        protected object hoveredObject = null;

        /// <summary>
        ///
        /// </summary>
        private RaycastHit hitPoint;


        /// <summary>
        ///
        /// </summary>
        public virtual RaycastHit HitPoint { get => hitPoint; protected set => hitPoint = value; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="hoveredObject"></param>
        protected virtual void SetHoveredObject(object hoveredObject) => this.hoveredObject = hoveredObject;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T GetHoveredObject<T>() where T : class
            => hoveredObject != null ? (T)hoveredObject : null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="hoveredObject"></param>
        protected virtual void HoverObject(RaycastHit hit, object hoveredObject)
        {
            HitPoint = hit;

            HoverObject(hit.transform, hoveredObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="hoveredObject"></param>
        protected virtual void HoverObject(Transform hit, object hoveredObject)
        {
            SetHoveredObject(hoveredObject);

            if (!hovering)
                OnHoverSelection?.Invoke();

            hovering = true;

            reticleComponents.ForEach(component => component.OnHoveringObject(hit));
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void StopHoveringObject()
        {
            if (ShouldStopHovering())
                OnStoppedHoveringObject();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldStopHovering() => hovering;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnStoppedHoveringObject()
        {
            hovering = false;

            reticleComponents.ForEach(component => component.OnStoppedHoveringObject(null));

            SetHoveredObject(null);
        }


        /// <inheritdoc/>
        public override void SetLock(bool to)
        {
            reticleComponents.ForEach(component => component.ReticleVisibilityChanged(!to));

            base.SetLock(to);
        }
    }
}