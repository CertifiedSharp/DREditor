//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.UIHandling.Reticles;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FiredTruthHandler) + ", " + nameof(IFireBulletUI) +
        "\n\n")]
    public class FiredBulletHandler : FiredTruthHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float timeToRemove = 15;


        /// <summary>
        ///
        /// </summary>
        protected TimerBase timer = null;

        /// <summary>
        ///
        /// </summary>
        protected HoverReticle reticle = null;

        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera trialCameraComponent;


        /// <inheritdoc/>
        protected override Component[] InitializeComponents()
        {
            var allComponents = base.InitializeComponents();

            var trialCamera = FindAnyObjectByType<DRTrialCamera>();

            if (trialCamera != null)
                trialCameraComponent = trialCamera.GetComponentInChildren<UnityEngine.Camera>();

            if (trialCameraComponent == null)
                UnityEngine.Debug.LogError("ALERT: Initializing trial camera failed!");

            reticle = allComponents.OfType<HoverReticle>().FirstOrDefault();

            if (reticle == null)
                UnityEngine.Debug.LogError("ALERT: Initializing reticle failed!");

            timer = allComponents.OfType<TrialTimerBase>().FirstOrDefault();

            if (timer == null)
                UnityEngine.Debug.LogError("ALERT: Initializing trial timer failed!");

            return allComponents;
        }

        /// <summary>
        ///
        /// </summary>
        protected override IEnumerator EvaluateBulletHit()
        {
            var hitObject = reticle.GetHoveredObject<Transform>();

            if (hitObject != null && 
                !reticle.HitPoint.collider.isTrigger && 
                hitObject.parent.TryGetComponent(out PhraseTextBase phraseTextBase))
            {
                if (phraseTextBase is IPhraseHittable hitable &&
                    phraseTextBase.textPhrase != null &&
                    phraseTextBase.textPhrase.Get<HittableModifierBase>() != null)
                    hitable?.Hit(reticle.HitPoint.point);
                else
                {
                    if (timer != null)
                        timer.RemoveTime(timeToRemove);

                    yield return WaitForFireAnimation();
                }

                InvokePhraseShot();
            }
            else
                yield return WaitForFireAnimation();
        }
    }
}