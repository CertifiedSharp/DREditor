//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class MusicFMOD
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("Title")]
        [SerializeField] protected string title;

        /// <summary>
        ///
        /// </summary>
        public string Title { set { title = value; } get { return title; } }

        /// <summary>
        ///
        /// </summary>
        [EventRef] public string BGM;
    }
}
#endif