//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if CINEMACHINE
using Cinemachine;
using DREditor.Minigames.Debates;
using System;
using UnityEngine;

namespace DREditor.TrackPhrase
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class TrackPath : IPhraseAnimationArchitecture
    {
        /// <summary>
        ///
        /// </summary>
        public Vector3 position;

        /// <summary>
        ///
        /// </summary>
        public Vector3 Position { get => position; set => position = value; }

        /// <summary>
        ///
        /// </summary>
        public Vector4 rotation;

        /// <summary>
        ///
        /// </summary>
        public Vector4 Rotation { get => rotation; set => rotation = value; }

        /// <summary>
        ///
        /// </summary>
        public CinemachinePath.Waypoint[] points;

        /// <summary>
        ///
        /// </summary>
        public CinemachinePath.Waypoint[] Points { get => points; set => points = value; }

        /// <summary>
        ///
        /// </summary>
        public AnimationCurve speedCurve;

        /// <summary>
        ///
        /// </summary>
        public AnimationCurve SpeedCurve { get => speedCurve; set => speedCurve = value; }


        /// <inheritdoc/>
        public virtual void Initialize(object o = null) { }

        /// <inheritdoc/>
        public virtual void Apply(IPhraseAnimationArchitecture architecture)
        {
            UnityEngine.Debug.Log("REPORT: The currently selected track animation architecture has been replaced (likely by a preset).");

            TrackPath trackPath = architecture as TrackPath;

            position = trackPath.position;

            rotation = trackPath.rotation;

            points = trackPath.points;

            speedCurve = trackPath.speedCurve;
        }
    }
}
#endif