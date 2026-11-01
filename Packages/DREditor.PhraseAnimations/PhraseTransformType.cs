//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PhraseTransformType : PhraseAnimationCompact<Vector3>
    {
        public override int CurveCount => 3;


        /// <inheritdoc/>
        public override void Initialize(AnimationClip clip)
        {
            clip.SetCurve("", typeof(Transform), "localPosition.x", animationCurves[0]);

            clip.SetCurve("", typeof(Transform), "localPosition.y", animationCurves[1]);

            clip.SetCurve("", typeof(Transform), "localPosition.z", animationCurves[2]);
        }
    }
}