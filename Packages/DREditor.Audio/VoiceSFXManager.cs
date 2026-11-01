//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

#if FMOD_UNITY
using FMODUnity;
#endif

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(VOManagerBase) +
        "\n\n")]
    public class VoiceSFXManager : VOManagerBase
    {
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter voiceSFXSource = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource voiceSFXSource = null;
#endif

        /// <inheritdoc/>
        protected override void InitializeService()
        {
            base.InitializeService();
#if FMOD_UNITY
            if (voiceSFXSource == null)
                voiceSFXSource = GetComponentInChildren<StudioEventEmitter>();
#else
            if (voiceSFXSource == null)
                voiceSFXSource = GetComponentInChildren<AudioSource>();
#endif
        }

        /// <inheritdoc/>
        public override void PlayVoiceLine<T>(T name)
        {
#if FMOD_UNITY
            if (name is string eventName && eventName != null && eventName != "")
                PlaySource(voiceSFXSource, eventName);
#else
            if (name is AudioClip clip && clip != null)
                PlaySource(voiceSFXSource, clip);
#endif
        }

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventName"></param>
        protected virtual void PlaySource(StudioEventEmitter source, string eventName)
        {
            if (eventName == "" || source == null)
                return;

            source.Stop();

            source.Event = eventName;

            source.Play();
        }
#else
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="clip"></param>
        protected virtual void PlaySource(AudioSource source, AudioClip clip)
        {
            if (clip == null || source == null)
                return;

            source.Stop();

            source.clip = clip;

            source.Play();
        }
#endif

#if FMOD_UNITY
        /// <inheritdoc/>
        public override bool VoicePlaying() => voiceSFXSource != null && voiceSFXSource.IsPlaying();
#else
        public override bool VoicePlaying() => voiceSFXSource != null && voiceSFXSource.isPlaying;
#endif
        /// <inheritdoc/>
        public override void StopVoiceLine()
        {
            if (voiceSFXSource != null)
                voiceSFXSource.Stop();
        }
    }
}