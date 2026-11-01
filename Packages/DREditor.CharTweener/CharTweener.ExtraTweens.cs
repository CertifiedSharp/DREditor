//Author: Copyright (c) 2021 Matthew Dechatech under MIT License
#if DOTWEEN
using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CharTween
{
    // Contains tweens that don't exist in DOTween and are included as extra utilities
    public partial class CharTweener : MonoBehaviour
    {
        /// <summary></summary>
        /// <param name="charIndex"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="snapping"></param>
        public Tweener DOOffsetMove(int charIndex, Vector3 endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMove(endValue, duration, snapping));
        }

        /// <summary></summary>
        /// <param name="charIndex"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="snapping"></param>
        public Tweener DOOffsetMoveX(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMoveX(endValue, duration, snapping));
        }

        /// <summary></summary>
        /// <param name="charIndex"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="snapping"></param>
        public Tweener DOOffsetMoveY(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMoveY(endValue, duration, snapping));
        }

        /// <summary></summary>
        /// <param name="charIndex"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="snapping"></param>
        public Tweener DOOffsetMoveZ(int charIndex, float endValue, float duration, bool snapping = false)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            return proxy.AddTween(proxy.Target.DOLocalMoveZ(endValue, duration, snapping));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="charIndex"></param>
        /// <param name="radius"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public Tweener DOMoveCircle(int charIndex, float radius, float duration)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            DOMoveCircleHandler handler = new DOMoveCircleHandler(proxy.Target, radius, duration);
            return proxy.AddTween(DOTween.To(handler.GetTime, handler.SetTime, duration, duration)
                .SetEase(Ease.Linear)
                .SetTarget(proxy.Target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="charIndex"></param>
        /// <param name="strength"></param>
        /// <param name="vibrato"></param>
        /// <param name="fadeInDuration"></param>
        /// <returns></returns>
        public Tweener DODriftPosition(int charIndex, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            DODriftPositionHandler handler = new DODriftPositionHandler(proxy.Target, strength, vibrato);
            return proxy.AddTween(DOTween.To(handler.GetTime, handler.SetTime, DriftTime, DriftTime)
                .SetEase(Ease.Linear)
                .SetTarget(proxy.Target));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="charIndex"></param>
        /// <param name="strength"></param>
        /// <param name="vibrato"></param>
        /// <param name="fadeInDuration"></param>
        /// <returns></returns>
        public Tweener DODriftRotation(int charIndex, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
        {
            NotifyActiveTransformTween();
            ProxyTransform proxy = GetProxyTransform(charIndex);
            DODriftRotationHandler handler = new DODriftRotationHandler(proxy.Target, strength, vibrato);
            return proxy.AddTween(DOTween.To(handler.GetTime, handler.SetTime, DriftTime, DriftTime)
                .SetEase(Ease.Linear)
                .SetTarget(proxy.Target));
        }

        // Helper classes 
        private class DOMoveCircleHandler
        {
            public Transform Target;
            public float Radius;
            public float Duration;

            public float Time;

            public DOMoveCircleHandler(Transform target, float radius, float duration)
            {
                Target = target;
                Radius = radius;
                Duration = duration;
            }

            public float GetTime()
            {
                return Time;
            }

            public void SetTime(float value)
            {
                Time = value;
                float angle = Mathf.Lerp(0, 2 * Mathf.PI, Time / Duration);
                Target.localPosition = new Vector3(Mathf.Cos(angle) * Radius, Mathf.Sin(angle) * Radius, 0);
            }
        }

        private const float DriftTime = 10000f;
        private class DODriftPositionHandler
        {
            public Transform Target;
            public Vector3 Strength;
            public Vector3 Vibrato;
            public float FadeInDuration;

            public float Time;

            private float seedX;
            private float seedY;
            private float seedZ;

            public DODriftPositionHandler(Transform target, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
            {
                Target = target;
                Strength = strength;
                Vibrato = vibrato;
                FadeInDuration = fadeInDuration;

                seedX = Random.Range(0f, 1000f);
                seedY = Random.Range(0f, 1000f);
                seedZ = Random.Range(0f, 1000f);
            }

            public float GetTime()
            {
                return Time;
            }

            public void SetTime(float time)
            {
                Time = time;
                float fadeIn = Mathf.Clamp01(Time / Mathf.Max(0.0001f, FadeInDuration));
                Target.localPosition = new Vector3(
                    (Mathf.PerlinNoise(Time * Vibrato.x, seedX) * 2 - 1) * Strength.x * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.y, seedY) * 2 - 1) * Strength.y * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.z, seedZ) * 2 - 1) * Strength.z * fadeIn
                );

                // If completing the tween, return character to start position
                if (time == DriftTime)
                    Target.localPosition = Vector3.zero;
            }
        }

        private class DODriftRotationHandler
        {
            public Transform Target;
            public Vector3 Strength;
            public Vector3 Vibrato;
            public float FadeInDuration;

            public float Time;

            private float seedX;
            private float seedY;
            private float seedZ;

            public DODriftRotationHandler(Transform target, Vector3 strength, Vector3 vibrato, float fadeInDuration = 1)
            {
                Target = target;
                Strength = strength;
                Vibrato = vibrato;
                FadeInDuration = fadeInDuration;

                seedX = Random.Range(0f, 1000f);
                seedY = Random.Range(0f, 1000f);
                seedZ = Random.Range(0f, 1000f);
            }

            public float GetTime()
            {
                return Time;
            }

            public void SetTime(float time)
            {
                Time = time;
                float fadeIn = Mathf.Clamp01(Time / Mathf.Max(0.0001f, FadeInDuration));
                Target.localEulerAngles = new Vector3(
                    (Mathf.PerlinNoise(Time * Vibrato.x, seedX) * 2 - 1) * Strength.x * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.y, seedY) * 2 - 1) * Strength.y * fadeIn,
                    (Mathf.PerlinNoise(Time * Vibrato.z, seedZ) * 2 - 1) * Strength.z * fadeIn
                );

                // If completing the tween, return character to original rotation
                if (time == DriftTime)
                    Target.localEulerAngles = Vector3.zero;
            }
        }
    }
}
#endif