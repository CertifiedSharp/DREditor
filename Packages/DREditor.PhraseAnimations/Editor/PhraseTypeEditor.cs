//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PhraseTypeEditor : ClassEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected PhraseAnimationTypeBase typeBaseTarget;


        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            typeBaseTarget = baseTarget as PhraseAnimationTypeBase;

            DisplayType();

            using (new EditorGUILayout.HorizontalScope())
            {
                DisplayStartTime();

                DisplayDuration();

                DisplayCurveButtons();
            }

            DisplayGenerateCurves();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStartTime()
        {
            typeBaseTarget.SetStartTime(HandyFields.FloatField("Start Time: ", typeBaseTarget.startTime, 30));

            if (typeBaseTarget.startTime < 0)
                typeBaseTarget.SetStartTime(0);

        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDuration()
        {
            float fallBackDuration = typeBaseTarget.duration;

            typeBaseTarget.SetDuration(HandyFields.FloatField("Duration: ", typeBaseTarget.duration, 30));

            if (typeBaseTarget.duration <= 0)
            {
                if (fallBackDuration == 0)
                    typeBaseTarget.SetDuration(1);
                else
                    typeBaseTarget.SetDuration(fallBackDuration);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCurveButtons()
        {
            if (typeBaseTarget.animationCurves.Count != 0)
            {
                if (GUILayout.Button("Update Curves"))
                    UpdateCurves();

                if (GUILayout.Button("Reset Curves"))
                    ResetCurves();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayGenerateCurves()
        {
            if (typeBaseTarget.animationCurves.Count == 0)
            {
                if (GUILayout.Button("Generate Curves"))
                    GenerateCurves();
            }
            else
                using (new EditorGUILayout.HorizontalScope())
                    DisplayCurves();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void GenerateCurves()
        {
            for (int i = 0; i < typeBaseTarget.CurveCount; i++)
                typeBaseTarget.animationCurves.Add(new AnimationCurve());
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayType();

        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayCurves();

        /// <summary>
        ///
        /// </summary>
        protected abstract void UpdateCurves();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetCurves()
        {
            typeBaseTarget.animationCurves.Clear();

            GenerateCurves();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected virtual AnimationCurve DisplayCurve(AnimationCurve curve, float start, float end)
        {
            if (curve.keys.Length == 0)
            {
                curve.AddKey(typeBaseTarget.startTime, start);

                curve.AddKey(typeBaseTarget.startTime + typeBaseTarget.duration, end);
            }

            try // Clicking the curve field would call an internal unity error but still works so this try is here to mitigate that.
            {
                curve = EditorGUILayout.CurveField(curve);
            }
            catch { }

            return curve;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected virtual AnimationCurve UpdateKey(AnimationCurve curve, float start, float end)
        {
            Keyframe oldStartTimeKey = curve.keys[0];

            Keyframe oldDurationTimeKey = curve.keys[^1];

            curve.keys = ValidateCurves(oldStartTimeKey, oldDurationTimeKey, curve, start, end);

            return curve;
        }

        #region Keyframe Validation

        /// <summary>
        ///
        /// </summary>
        /// <param name="oldStart"></param>
        /// <param name="oldDuration"></param>
        /// <param name="curve"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected virtual Keyframe[] ValidateCurves(Keyframe oldStart, Keyframe oldDuration, AnimationCurve curve, float start, float end)
        {
            if (ValidateMovedKey(curve, start, end))
                return curve.keys;

            Keyframe[] newKeys = ValidateNewKeys(oldStart, oldDuration, curve);

            newKeys[0] = new(typeBaseTarget.startTime, start);

            newKeys[curve.keys.Length - 1] = new(typeBaseTarget.startTime + typeBaseTarget.duration, end);

            return newKeys;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected virtual bool ValidateMovedKey(AnimationCurve curve, float start, float end)
        {
            Keyframe newStart = new(typeBaseTarget.startTime, start);

            Keyframe newDuration = new(typeBaseTarget.startTime + typeBaseTarget.duration, end);

            if (curve.keys[^1].time == newDuration.time && curve.keys[0].time == newStart.time)
            {
                if (curve.keys[^1].value == newDuration.value && curve.keys[0].value == newStart.value)
                    return true;
                else
                {
                    curve.MoveKey(0, new(0, start));

                    curve.MoveKey(curve.keys.Length - 1, new(typeBaseTarget.startTime + typeBaseTarget.duration, end));

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="oldStart"></param>
        /// <param name="oldDuration"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        protected virtual Keyframe[] ValidateNewKeys(Keyframe oldStart, Keyframe oldDuration, AnimationCurve curve)
        {
            Keyframe[] newKeys = new Keyframe[curve.keys.Length];

            if (curve.keys.Length > 2)
                UpdateNewKeys(newKeys, oldStart, oldDuration, curve);

            return newKeys;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newKeys"></param>
        /// <param name="oldStart"></param>
        /// <param name="oldDuration"></param>
        /// <param name="curve"></param>
        protected virtual void UpdateNewKeys(Keyframe[] newKeys, Keyframe oldStart, Keyframe oldDuration, AnimationCurve curve)
        {
            float startOffset = typeBaseTarget.startTime - oldStart.time;

            float durationOffset = (typeBaseTarget.startTime + typeBaseTarget.duration) - oldDuration.time;

            for (int i = 1; i <= newKeys.Length - 2; i++)
                if (curve.keys[i].time <= typeBaseTarget.startTime)
                    newKeys[i] = new(curve.keys[i].time + startOffset, curve.keys[i].value);
                else if (curve.keys[i].time >= typeBaseTarget.startTime + typeBaseTarget.duration)
                    newKeys[i] = new(curve.keys[i].time + durationOffset, curve.keys[i].value);
                else
                {
                    float behind = curve.keys[i].time - curve.keys[i - 1].time;

                    newKeys[i] = new(curve.keys[i].time + behind, curve.keys[i].value);
                }
        }

        #endregion
    }
}
#endif