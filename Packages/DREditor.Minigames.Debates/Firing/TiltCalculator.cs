//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TiltCalculatorBase) +
        "\n\n")]
    public class TiltCalculator : TiltCalculatorBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float tiltOffset = 20;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected float topAngleRange = 2.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float aboveFirePositionYTiltDivisor = 10;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected double firePositionYTiltMidpoint = 0.23;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float middleTilt = 15;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float belowYMidpointTilt = 20;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected float botAngleRange = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bottomTilt = 25;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected float firePositionXTiltMidpoint = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float belowFirePositionXTiltDivisor = 10;


        /// <inheritdoc/>
        public override float CalculateFiringTilt(Vector3 firePosition)
        {
            float tilt = ApplyYPositionTilt(firePosition);

            tilt += ApplyXPositionTilt(firePosition.x);

            return tilt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firePosition"></param>
        /// <returns></returns>
        protected virtual float ApplyYPositionTilt(Vector3 firePosition)
        {
            float tilt = tiltOffset;

            if (firePosition.y < botAngleRange)
                tilt = bottomTilt;
            else if (firePosition.y >= topAngleRange)
                tilt = CalculateTopTilt(firePosition.y, tilt);
            else
                tilt = middleTilt;

            if (firePosition.y < firePositionYTiltMidpoint)
                tilt = belowYMidpointTilt;

            return tilt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firePositionY"></param>
        /// <param name="tilt"></param>
        /// <returns></returns>
        protected virtual float CalculateTopTilt(float firePositionY, float tilt)
            => -tilt * firePositionY / aboveFirePositionYTiltDivisor;

        /// <summary>
        ///
        /// </summary>
        /// <param name="firePositionX"></param>
        /// <returns></returns>
        protected virtual float ApplyXPositionTilt(float firePositionX)
            => firePositionX < firePositionXTiltMidpoint ? firePositionX / belowFirePositionXTiltDivisor : 0;
    }
}