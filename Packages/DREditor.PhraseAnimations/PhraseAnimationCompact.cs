//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine.Serialization;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class PhraseAnimationCompact<T> : PhraseAnimationTypeBase
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("startScale")]
        [FormerlySerializedAs("startPosition")]
        [FormerlySerializedAs("startRotation")]
        [FormerlySerializedAs("startFade")]
        public T startValue;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("endScale")]
        [FormerlySerializedAs("endPosition")]
        [FormerlySerializedAs("endRotation")]
        [FormerlySerializedAs("endFade")]
        public T endValue;


        /// <summary>
        ///
        /// </summary>
        /// <param name="startValue"></param>
        public virtual void SetStartValue(T startValue) => this.startValue = startValue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="endValue"></param>
        public virtual void SetEndValue(T endValue) => this.endValue = endValue;
    }
}