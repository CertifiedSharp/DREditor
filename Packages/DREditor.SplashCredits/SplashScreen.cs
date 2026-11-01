//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.Video;

namespace DREditor.SplashCredits
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Splash Credits/Splash Screen", fileName = "Splash Screen")]
    public class SplashScreen : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Texture Screen;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public float screenTime = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool skippable = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public VideoClip clip;
    }
}