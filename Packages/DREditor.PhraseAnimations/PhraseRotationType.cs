//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PhraseRotationType : PhraseAnimationCompact<Vector4>
    {
        public override int CurveCount => 4;


        /// <inheritdoc/>
        public override void Initialize(AnimationClip clip)
        {
            clip.SetCurve("", typeof(Transform), "localEulerAngles.x", animationCurves[0]);

            clip.SetCurve("", typeof(Transform), "localEulerAngles.y", animationCurves[1]);

            clip.SetCurve("", typeof(Transform), "localEulerAngles.z", animationCurves[2]);

            clip.SetCurve("", typeof(Transform), "rotation.w", animationCurves[3]);
        }
    }
}