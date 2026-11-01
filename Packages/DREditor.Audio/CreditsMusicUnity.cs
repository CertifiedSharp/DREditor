//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    public class CreditsMusicUnity : CreditsMusic
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<AudioClip> musicEvents = new();


        /// <inheritdoc/>
        protected override void Start() => StartCoroutine(PlayMusicTracks(musicEvents));
    }
}