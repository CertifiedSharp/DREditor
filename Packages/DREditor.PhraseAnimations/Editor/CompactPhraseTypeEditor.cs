//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System;
using UnityEngine;

namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public abstract class CompactPhraseTypeEditor<T, T2> : PhraseAnimationTypeEditor<T>
        where T : PhraseAnimationCompact<T2>
        where T2 : struct
    {
        /// <summary>
        ///
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        ///
        /// </summary>
        public abstract Func<string, T2, int, int, T2> FieldFunc { get; }

        protected override void DisplayType()
        {
            typeTarget.SetStartValue(FieldFunc($"Starting {TypeName}: ", typeTarget.startValue, 200, 6));

            typeTarget.SetEndValue(FieldFunc($"Ending {TypeName}: ", typeTarget.endValue, 200, 6));
        }

        protected override void DisplayCurves() => PopulateCurves(DisplayCurve);

        protected override void UpdateCurves() => PopulateCurves(UpdateKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="operation"></param>
        protected virtual void PopulateCurves(Func<AnimationCurve, float, float, AnimationCurve> operation)
        {
            if (typeof(T2) == typeof(float))
                typeTarget.animationCurves[0] = operation(
                    typeTarget.animationCurves[0],
                    (float)(object)typeTarget.startValue,
                    (float)(object)typeTarget.endValue);
            else
                EditVectorCurves(typeTarget.startValue, typeTarget.endValue, operation);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T3"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="operation"></param>
        /// <param name="offset"></param>
        protected virtual void EditVectorCurves<T3>
            (T3 start, T3 end, Func<AnimationCurve, float, float, AnimationCurve> operation, int offset = 0)
            where T3 : struct
        {
            for (int i = 0; i < typeBaseTarget.CurveCount; i++)
            {
                int index = i + offset;

                float startVal = GetVectorComponent(start, i);

                float endVal = GetVectorComponent(end, i);

                typeBaseTarget.animationCurves[index] = operation(typeBaseTarget.animationCurves[index], startVal, endVal);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T4"></typeparam>
        /// <param name="vector"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected float GetVectorComponent<T4>(T4 vector, int index) where T4 : struct
        {
            return vector switch
            {
                Vector2 v2 => v2[index],
                Vector3 v3 => v3[index],
                Vector4 v4 => v4[index],
                _ => throw new ArgumentException($"Unsupported vector type: {typeof(T4)}")
            };
        }
    }
}
#endif