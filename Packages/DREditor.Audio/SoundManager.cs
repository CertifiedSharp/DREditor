//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
#if FMOD_UNITY
using FMOD.Studio;
using FMODUnity;
#endif
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SoundManagerBase) +
        "\n\n")]
    public class SoundManager : SoundManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useDebugVolume = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0, 1)]
        [SerializeField] protected float debugMusic = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0, 1)]
        [SerializeField] protected float debugSFX = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0, 1)]
        [SerializeField] protected float debugDia = 1;


#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string masterBusString = "Bus:/Master/";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string musicBusName = "Music";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string sfxBusName = "SFX";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string voiceLineBusName = "Dialogue";


        /// <summary>
        ///
        /// </summary>
        Bus music;

        /// <summary>
        ///
        /// </summary>
        Bus sfx;

        /// <summary>
        ///
        /// </summary>
        Bus dialogue;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource musicSource = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource sfxSource = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource voSource = null;
#endif

        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            ServiceInitializer.Initialize<SoundManager>(this);
        }


#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (useDebugVolume)
                UseDebugVolumesFMOD();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UseDebugVolumesFMOD()
        {
            music = RuntimeManager.GetBus(masterBusString + musicBusName);

            sfx = RuntimeManager.GetBus(masterBusString + sfxBusName);

            dialogue = RuntimeManager.GetBus(masterBusString + voiceLineBusName);

            music.setVolume(debugMusic);

            sfx.setVolume(debugSFX);

            dialogue.setVolume(debugDia);
        }


        /// <inheritdoc/>
        public override void MuteAudio()
        {
            RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup masterGroup);

            masterGroup.setMute(true);
        }

        /// <inheritdoc/>
        public override void UnmuteAudio()
        {
            RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup masterGroup);

            masterGroup.setMute(false);
        }
#else
        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (useDebugVolume)
                UseDebugVolumesMixer();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UseDebugVolumesMixer()
        {
            musicSource.volume = debugMusic;

            sfxSource.volume = debugSFX;

            voSource.volume = debugDia;
        }


        public override void MuteAudio() => AudioListener.volume = 0;

        public override void UnmuteAudio() => AudioListener.volume = 1;
#endif
    }
}