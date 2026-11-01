//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadable) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class MusicSaver : MonoBehaviour, IDRESaveable, IDRELoadable, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadingOutOfLoading = null;

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MusicManager musicManager;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string musicSaveKey = "MusicData";
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MusicManager musicManager;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Playlist jukeBox = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string musicSaveKey = "MusicData";
#endif

        /// <summary>
        ///
        /// </summary>
        protected string loadedMusic = "";

        /// <summary>
        ///
        /// </summary>
        protected bool registered = false;


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas) => saveDatas[musicSaveKey] = SaveMusic();

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => LoadMusic(saveDatas[musicSaveKey] as string);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string SaveMusic() => musicManager.GetLastPlayedMusic();

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        protected virtual void LoadMusic(string name)
        {
            if (name is string loadName && loadName != "")
            {
                loadedMusic = loadName;

                if (registered)
                    return;

                onFadingOutOfLoading.RegisterListener(this);

                registered = true;
            }
            else
                musicManager.OnLoadedMusicMissing();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadName"></param>
        protected virtual void LoadValidMusic(string loadName)
        {
#if FMOD_UNITY
            musicManager.PlayValidMusic(loadName);
#else
            musicManager.PlayMusic(jukeBox.GetAudioClip(loadName));
#endif
        }


        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            if (!registered)
                return;

            onFadingOutOfLoading.UnregisterListener(this);

            registered = false;

            LoadValidMusic(loadedMusic);
        }

        protected virtual void OnDestroy() => onFadingOutOfLoading.UnregisterListener(this);
    }
}