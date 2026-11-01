//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Audio.FMOD
{
    /// <summary>
    ///
    /// </summary>
    public class CreditsMusicFMOD : CreditsMusic
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField][EventRef] protected List<string> musicEvents = new();


        /// <inheritdoc/>
        protected override void Start() => StartCoroutine(PlayMusicTracks(musicEvents));
    }
}