//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.PhraseAnimations
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class PhraseAnimation : IPhraseAnimationArchitecture
    {
        /// <summary>
        ///
        /// </summary>
        public Vector3 spawnPoint;

        /// <summary>
        ///
        /// </summary>
        public Vector4 spawnAngle;

        /// <summary>
        ///
        /// </summary>
        public List<AnimData> animations = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnPoint"></param>
        public virtual void SetSpawnPoint(Vector3 spawnPoint) => this.spawnPoint = spawnPoint;

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnAngle"></param>
        public virtual void SetSpawnAngle(Vector4 spawnAngle) => this.spawnAngle = spawnAngle;


        /// <inheritdoc/>
        public virtual void Initialize(object o = null)
        {
            if (o is AnimationClip clip)
                foreach (AnimData anim in animations)
                    anim.Initialize(clip);
            else
                UnityEngine.Debug.LogWarning($"ALERT: Failed to initialize animation architecture: {nameof(PhraseAnimation)}" +
                    $" because the sent value for initialization was not of type {nameof(AnimationClip)}.");
        }

        /// <inheritdoc/>
        public virtual void Apply(IPhraseAnimationArchitecture architecture)
        {
            PhraseAnimation animation = architecture as PhraseAnimation;

            spawnPoint = animation.spawnPoint;

            spawnAngle = animation.spawnAngle;

            foreach (var newAnimation in animation.animations)
            {
                if (animations.Any(animation => animation.animationType?.GetType() == newAnimation.animationType?.GetType()))
                    UnityEngine.Debug.Log("REPORT: The phrases simple animation contains duplicate types." +
                        "\nPROPOSAL: Test animation to ensure no animations conflict with each other.");

                animations.Add(newAnimation);
            }
        }
    }
}