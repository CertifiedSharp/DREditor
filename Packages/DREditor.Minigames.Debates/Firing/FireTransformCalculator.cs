//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling.Reticles;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FireTransformCalculatorBase) +
        "\n\n")]
    public class FireTransformCalculator : FireTransformCalculatorBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TiltCalculatorBase tiltCalculator = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float cameraFarPlane = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fireOffsetX = -204;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fireOffsetY = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int divideWidth = 8;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int divideHeight = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float angleOffsetMultiplier = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float angleOffset = -204;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float additionalAngleOffset = 10;


        /// <summary>
        ///
        /// </summary>
        protected Transform reticleTransform = null;


        protected virtual void Awake()
        {
            var allComponents = transform.root.GetComponentsInChildren<Component>();

            var reticle = allComponents.OfType<HoverReticle>().FirstOrDefault();

            if (reticle != null)
                reticleTransform = reticle.transform;
            else
                throw new System.NullReferenceException($"ALERT: Initializing {nameof(reticleTransform)} failed!");
        }

        /// <inheritdoc/>
        public override Vector3 CalculateFirePosition()
        {
            var fireXPosition = reticleTransform.position.x * ((float)divideWidth / Screen.width) + fireOffsetX;

            var fireYPosition = reticleTransform.position.y * ((float)divideHeight / Screen.height) + fireOffsetY;

            return new Vector3(fireXPosition, fireYPosition, cameraFarPlane);
        }


        /// <inheritdoc/>
        public override Quaternion CalculateFireRotation()
        {
            Vector3 firePosition = CalculateFirePosition();

            float tilt = tiltCalculator.CalculateFiringTilt(firePosition);

            float angleOffset = angleOffsetMultiplier * (firePosition.x - this.angleOffset) + additionalAngleOffset;

            return Quaternion.Euler(0, angleOffset, transform.localRotation.x + tilt);
        }
    }
}