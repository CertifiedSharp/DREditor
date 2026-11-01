//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class PhraseAnimationTypeBase : IPhraseAnimationType
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] public float startTime;

        /// <summary>
        ///
        /// </summary>
        [SerializeField] public float duration;


        /// <summary>
        ///
        /// </summary>
        public abstract int CurveCount { get; }

        /// <summary>
        ///
        /// </summary>
        public List<AnimationCurve> animationCurves = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="startTime"></param>
        public virtual void SetStartTime(float startTime) => this.startTime = startTime;

        /// <summary>
        ///
        /// </summary>
        /// <param name="duration"></param>
        public virtual void SetDuration(float duration) => this.duration = duration;

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        public abstract void Initialize(AnimationClip clip);
    }
}