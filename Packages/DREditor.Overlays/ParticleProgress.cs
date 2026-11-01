//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
    [DefaultExecutionOrder(1)]
    public abstract class ParticleProgress : GameplayOverlay
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent changingObserve = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debug = false;


        /// <summary>
        ///
        /// </summary>
        protected readonly List<ParticleSystem> particles = new();

        /// <summary>
        ///
        /// </summary>
        protected bool changing = false;


        /// <inheritdoc/>
        protected override void EvaluateOverlay(CallbackContext context)
        {
            if (!changing && changingObserve.Value && displayOverlayCondition.Evaluate())
                StartCoroutine(ToggleOverlay());
            else if (debug)
                UnityEngine.Debug.Log($"REPORT: FAILED OVERLAY {!changing} {changingObserve.Value} {displayOverlayCondition.Evaluate()}");
        }

        /// <inheritdoc/>
        protected override IEnumerator ToggleOverlay()
        {
            if (debug)
                UnityEngine.Debug.Log($"REPORT: OVERLAY SUCCEDED!");

            changing = true;

            SetOverlay(!overlaying);

            yield return new WaitUntil(() => !changingObserve.Value);

            changing = false;
        }


        /// <inheritdoc/>
        public override void SetOverlay(bool to)
        {
            base.SetOverlay(to);

            if (to)
                TurnOnOverlay();
            else
                TurnOffOverlay();
        }

        /// <inheritdoc/>
        protected override void TurnOffOverlay()
        {
            particles.ForEach(particleSystem => particleSystem.Stop());

            foreach (var particle in particles)
                Destroy(particle.gameObject);

            particles.Clear();
        }

        /// <inheritdoc/>
        protected override void TurnOnOverlay()
        {
            IObjectPooler<SelectableObject> pooler = ServiceLocator.GetService<IObjectPooler<SelectableObject>>();

            List<SelectableObject> selectableObjects = pooler.GetRegisteredObjects();

            foreach (var selectableObject in selectableObjects)
                if (selectableObject.selectable)
                    ApplyParticleToObject(selectableObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectableObject"></param>
        protected abstract void ApplyParticleToObject(SelectableObject selectableObject);
    }
}