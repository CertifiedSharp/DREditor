//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;
using DREditor.Services;


#if FMOD_UNITY
using FMODUnity;
#endif

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [DefaultExecutionOrder(-1)]
    [ClassHeader(nameof(MusicManagerBase) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class MusicManager : MusicManagerBase, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] bool debugLogs = false;

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string LastPlayedMusic = "";
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip LastPlayedMusic = null;
#endif
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
#if FMOD_UNITY
        [Tooltip("")]
        [SerializeField] protected PlaylistFMOD jukeBox = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Playlist jukeBox = null;
#endif
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
#if FMOD_UNITY
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter musicSource = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource musicSource = null;
#endif

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool playOnStart = false;
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [EventRef][SerializeField] string startMusic = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string stopMusicCode = "Null";
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip startMusic = null;


        /// <summary>
        ///
        /// </summary>
        protected bool fading = false;
#endif


        /// <inheritdoc/>
        protected override void InitializeService()
        {
            base.InitializeService();

            if (musicSource == null)
#if FMOD_UNITY
                musicSource = GetComponentInChildren<StudioEventEmitter>();
#else
                musicSource = GetComponentInChildren<AudioSource>();
#endif
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (playOnStart)
#if FMOD_UNITY
                StartCoroutine(PlayMusicOnStartRoutine());
#else
                PlayMusic(startMusic);
#endif
        }
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator PlayMusicOnStartRoutine()
        {
            yield return new WaitUntil(() => RuntimeManager.IsInitialized);

            PlayMusic(startMusic);
        }
#endif

#if FMOD_UNITY
        /// <inheritdoc/>
        public override void PlayMusic<T>(T name, bool skip = false)
        {
            if (name is string eventName)
                if (musicSource != null && musicSource.Event == eventName)
                    return;
                else if (eventName == "" || eventName == stopMusicCode)
                    OnPlayedInvalidMusic();
                else
                    PlayValidMusic(eventName, skip);
        }

        /// <inheritdoc/>
        public override bool MusicIsPlaying() => musicSource != null && musicSource.IsPlaying();
#else
        public override void PlayMusic<T>(T name, bool skip = false)
        {
            if (name is string clipName && !string.IsNullOrEmpty(clipName))
                PlayValidMusic(jukeBox.GetAudioClip(clipName), skip);

            if (name is AudioClip eventName)
                if (musicSource != null && musicSource.clip == eventName)
                    return;
                else if (eventName == null)
                    OnPlayedInvalidMusic();
                else
                    PlayValidMusic(eventName, skip);
        }

        public override bool MusicIsPlaying() => musicSource.isPlaying;
#endif

        /// <inheritdoc/>
        public override void StopPlayingMusic() => OnPlayedInvalidMusic();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPlayedInvalidMusic()
        {
            if (musicSource != null)
                musicSource.Stop();

            ClearMusicText.Raise();

            ResetLastPlayedMusic();
        }
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="skip"></param>
        /// <remarks>
        /// Note: A FMOD bug was found where: The first music always works, <br/>
        /// but trying to change the music like normal just ends up playing the first one.
        /// <br/><br/>
        /// This bug is fixed using the StudioEventEmitterPatcher.cs.
        /// </remarks>
        public virtual void PlayValidMusic(string eventName, bool skip = false)
        {
            if (jukeBox != null)
                SetMusicText.SetValue(jukeBox.GetTitleFromClip(eventName));

            PlaySource(musicSource, eventName);

            if (!skip)
                LastPlayedMusic = eventName;

            StartCoroutine(WaitForUpdate());

            if (debugLogs && jukeBox != null)
                UnityEngine.Debug.LogWarning("REPORT: PLAYING Music: " + jukeBox.GetTitleFromClip(eventName) + "  " + eventName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator WaitForUpdate()
        {
            if (musicSource == null)
                yield break;

            yield return new WaitUntil(() => RuntimeManager.IsInitialized);

            yield return new WaitUntil(() => musicSource.IsPlaying());

            InvokeMusicChange(musicSource);
        }
#else
        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="skip"></param>
        protected virtual void PlayValidMusic(AudioClip clip, bool skip = false)
        {
            if (fading)
                fading = false;

            SetCurrentlyPlayingMusicText(clip);

            PlaySource(musicSource, clip);

            if (!skip)
                LastPlayedMusic = clip;

            InvokeMusicChange(musicSource);

            if (debugLogs && jukeBox != null)
                UnityEngine.Debug.LogWarning("REPORT: PLAYING Music: " + jukeBox.GetTitleFromClip(clip) + "  " + clip);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        protected virtual void SetCurrentlyPlayingMusicText(AudioClip clip)
        {
            if (jukeBox != null)
                SetMusicText.SetValue(jukeBox.GetTitleFromClip(clip));
        }
#endif
        /// <inheritdoc/>
        public override void StopMusic()
        {
            ClearMusicText.Raise();

            if (musicSource != null)
                musicSource.Stop();
        }

        /// <inheritdoc/>
        public override void StopSound() => StopMusic();

        /// <inheritdoc/>
        public override void PlayLastMusic() => PlayMusic(LastPlayedMusic);

        /// <inheritdoc/>
        protected override void ResetLastPlayedMusic()
        {
#if FMOD_UNITY
            if (musicSource != null)
                musicSource.Event = null;
#else
            if (musicSource != null)
                musicSource.clip = null;
#endif
            LastPlayedMusic = null;
        }

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="FadeTime"></param>
        /// <remarks></remarks>
        public override void FadeMusic(float FadeTime) => StopMusic();
#else
        /// <inheritdoc/>
        public override void FadeMusic(float FadeTime) => StartCoroutine(FadeOut(musicSource, FadeTime));

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        protected virtual IEnumerator FadeOut<T>(T source, float duration)
        {
            fading = true;

            if (source is AudioSource audioSource)
            {
                float startVolume = audioSource.volume;

                float time = 0f;

                while (time < duration && fading)
                {
                    audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);

                    time += Time.deltaTime;

                    yield return null;
                }

                audioSource.volume = 0f;

                audioSource.Stop();

                yield return null;

                audioSource.volume = startVolume;
            }

            fading = false;

            yield break;
        }
#endif

        /// <inheritdoc/>
        public override string GetLastPlayedMusic()
        {
#if FMOD_UNITY
            return LastPlayedMusic;
#else
            return LastPlayedMusic.name;
#endif
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void OnLoadedMusicMissing()
        {
            if (debugLogs)
                UnityEngine.Debug.LogWarning("ALERT: Couldn't find Saved Music, no music playing");

            ResetMusicText.Raise();

            ResetLastPlayedMusic();

            InvokeMusicChange(null);
        }
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventName"></param>
        protected virtual void PlaySource(StudioEventEmitter source, string eventName)
        {
            if (eventName == "" || source == null || !EventExists(eventName))
                return;

            source.Stop();

            source.Event = eventName;

            source.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual bool EventExists(string path)
        {
            bool result = FMODUnity.RuntimeManager.StudioSystem.getEvent(path, out _) == FMOD.RESULT.OK;

            if (!result)
                UnityEngine.Debug.LogWarning($"ALERT: The path: {path} does not match an existing event!");

            return result;
        }
#else
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="clip"></param>
        protected virtual void PlaySource(AudioSource source, AudioClip clip)
        {
            if (clip == null)
                return;

            source.Stop();

            source.clip = clip;

            source.Play();
        }
#endif

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => StopMusic();
    }
}