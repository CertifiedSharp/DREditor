//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
#if FMOD_UNITY
using FMODUnity;
#endif
using System;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class MusicManagerBase : SoundService<MusicManagerBase>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected StringWithEvent SetMusicText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent ClearMusicText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent ResetMusicText = null;

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        public event Action<StudioEventEmitter> OnMusicChange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="musicSource"></param>
        protected virtual void InvokeMusicChange(StudioEventEmitter musicSource) => OnMusicChange?.Invoke(musicSource);

#else
        /// <summary>
        ///
        /// </summary>
        public event Action<AudioSource> OnMusicChange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="musicSource"></param>
        protected virtual void InvokeMusicChange(AudioSource musicSource) => OnMusicChange?.Invoke(musicSource);
#endif



        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="skip"></param>
        public abstract void PlayMusic<T>(T eventName, bool skip = false);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract bool MusicIsPlaying();

        /// <summary>
        ///
        /// </summary>
        public abstract void StopPlayingMusic();

        /// <summary>
        ///
        /// </summary>
        public abstract void PlayLastMusic();

        /// <summary>
        ///
        /// </summary>
        protected abstract void ResetLastPlayedMusic();

        /// <summary>
        ///
        /// </summary>
        /// <param name="duration"></param>
        public abstract void FadeMusic(float duration = 1);

        /// <summary>
        ///
        /// </summary>
        public abstract void StopMusic();

        /// <summary>
        ///
        /// </summary>
        public abstract void StopSound();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract string GetLastPlayedMusic();
    }
}