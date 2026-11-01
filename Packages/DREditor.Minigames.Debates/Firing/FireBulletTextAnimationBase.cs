//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FireBulletTextAnimationBase : FireTruthAnimationBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RenderTexture fireBulletRenderTexture = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FireTransformCalculatorBase fireTransformCalculator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<RectTransform> cutOutTexts = new();


        /// <summary>
        ///
        /// </summary>
        protected TMP_Text bulletText;

        /// <summary>
        ///
        /// </summary>
        protected RectTransform rect;

        /// <summary>
        ///
        /// </summary>
        protected RawImage fireBulletRawImage = null;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 firePosition;


        protected virtual void Awake()
        {
            bulletText = GetComponent<TMP_Text>();

            rect = GetComponent<RectTransform>();

            var allComponents = transform.root.GetComponentsInChildren<Component>();

            var rawImage = allComponents
                .OfType<RawImage>()
                .Where(raw => raw.texture == fireBulletRenderTexture)
                .FirstOrDefault();

            if (rawImage != null)
                fireBulletRawImage = rawImage;
            else
                UnityEngine.Debug.LogError($"ALERT: Initializing {nameof(fireBulletRawImage)} failed!");
        }


        /// <inheritdoc/>
        public override IEnumerator FireAnimation()
        {
            FireBulletCameraEnabled(true);

            CalculateFirePositionAndRotation();

            AnimateFiredBullet();

            yield return new WaitForSecondsRealtime(CalculateFireAnimationDuration());

            StartCoroutine(ResolveFireAnimation());
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void AnimateFiredBullet();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract float CalculateFireAnimationDuration();

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveFireAnimation()
        {
            yield return new WaitForSecondsRealtime(CalculateRemainingDuration());

            FireBulletCameraEnabled(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CalculateFirePositionAndRotation()
        {
            firePosition = fireTransformCalculator.CalculateFirePosition();

            transform.localRotation = fireTransformCalculator.CalculateFireRotation();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enabled"></param>
        protected virtual void FireBulletCameraEnabled(bool enabled)
        {
            cameraEnabled.SetValue(enabled);

            fireBulletRawImage.enabled = enabled;
        }
    }
}