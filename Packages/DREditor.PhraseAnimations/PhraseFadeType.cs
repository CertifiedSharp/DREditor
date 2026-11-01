//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PhraseFadeType : PhraseAnimationCompact<float>
    {
        public override int CurveCount => 1;


        /// <inheritdoc/>
        public override void Initialize(AnimationClip clip)
            => clip.SetCurve("", typeof(TMPro.TMP_Text), "m_fontColor.a", animationCurves[0]);
    }
}