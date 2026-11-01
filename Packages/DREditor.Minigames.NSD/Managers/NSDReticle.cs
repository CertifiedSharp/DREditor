//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Cameras;
using DREditor.Minigames.Debates;
using DREditor.UIHandling.Reticles;
using DREditor.Utility;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.NSD.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HoverReticle) + ", " + nameof(Reticle) + ", " + nameof(IDebateUIVisibility) +
        ", " + nameof(IPhraseRetriever) +
        "\n\n")]
    public class NSDReticle : HoverReticle, IDebateUIVisibility, IPhraseRetriever
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LayerMask hoverableLayersMask;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LayerMask backBlockingLayersMask;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int maxDetectionAmount = 10;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float frontFacingThreshold = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool usePixelPerfectBackCheck = true;


        /// <summary>
        ///
        /// </summary>
        protected RaycastHit[] hitResultsBuffer;

        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera trialCamera;

        /// <summary>
        ///
        /// </summary>
        protected LayerMask detectableLayersMask;

        /// <summary>
        ///
        /// </summary>
        protected PhraseTextBase hoveredText;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            detectableLayersMask = hoverableLayersMask | backBlockingLayersMask;

            hitResultsBuffer = new RaycastHit[maxDetectionAmount];

            var trialCameraDR = FindAnyObjectByType<DRTrialCamera>();

            trialCamera = trialCameraDR.GetComponentInChildren<UnityEngine.Camera>();
        }

        /// <inheritdoc/>
        protected override void Update()
        {
            base.Update();

            if (CanHoverRaycast())
                CheckHoveringRaycast();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanHoverRaycast() => !locked;

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckHoveringRaycast()
        {
            int hitCount = Physics.RaycastNonAlloc(trialCamera.ScreenPointToRay(transform.position), hitResultsBuffer, detectableLayersMask);

            if (hitCount != 0)
                CheckObjectsHit(hitCount);
            else
                StopHoveringObject();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitCount"></param>
        protected virtual void CheckObjectsHit(int hitCount)
        {
            Array.Sort(hitResultsBuffer, 0, hitCount, RaycastHitComparer.Instance);

            bool hitValidObject = false;

            for (int i = 0; i < hitCount; i++)
            {
                if (backBlockingLayersMask.Contains(hitResultsBuffer[i].collider.gameObject.layer))
                {
                    if (HitFrontOrPassable(hitResultsBuffer[i]))
                        continue;
                    else
                        break;
                }
                else
                {
                    HoverObject(hitResultsBuffer[i], hitResultsBuffer[i].transform);

                    hitValidObject = true;

                    break;
                }
            }

            if (!hitValidObject)
                StopHoveringObject();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        protected virtual bool HitFrontOrPassable(RaycastHit hit) => HitFront(hit) || PassedPixelCheck(hit);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        protected virtual bool PassedPixelCheck(RaycastHit hit) => 
            usePixelPerfectBackCheck == false || 
            FailedGetRenderer(hit, out SpriteRenderer renderer)  ||
            PixelUtility.GetPixelColorUsingSpriteBounds(hit.point, renderer).a < 0.5f;

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="renderer"></param>
        /// <returns></returns>
        protected virtual bool FailedGetRenderer(RaycastHit hit, out SpriteRenderer renderer)
        {
            renderer = null;

            var renderers = hit.collider.GetComponentsInChildren<SpriteRenderer>();

            foreach (var spriteRenderer in renderers)
                if (spriteRenderer.sprite != null)
                    renderer = spriteRenderer;

            return renderer == null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        protected virtual bool HitFront(RaycastHit hit) => Vector3.Dot(hit.transform.forward, hit.normal) > frontFacingThreshold;


        /// <inheritdoc/>
        protected override void HoverObject(RaycastHit hit, object hoveredObject)
        {
            if (hoveredObject != this.hoveredObject || HitPoint.collider != hit.collider)
            {
                if (hit.transform.parent.TryGetComponent(out hoveredText))
                    base.HoverObject(hit, hoveredObject);
                else
                    StopHoveringObject();
            }
        }

        /// <inheritdoc/>
        protected override void OnStoppedHoveringObject()
        {
            base.OnStoppedHoveringObject();

            hoveredText = null;
        }


        /// <inheritdoc/>
        public virtual void Show() => SetLock(false);

        /// <inheritdoc/>
        public virtual void Hide() => SetLock(true);

        /// <inheritdoc/>
        public virtual IEnumerator WaitForChangeToComplete()
        {
#if DOTWEEN
            yield return new WaitUntil(() => !DOTween.IsTweening(this));
#else
            yield return null;
#endif
        }

        /// <inheritdoc/>
        public virtual bool TryGetAimedPhrase(out PhraseTextBase phraseText) => phraseText = hoveredText != null ? hoveredText : null;
    }
}