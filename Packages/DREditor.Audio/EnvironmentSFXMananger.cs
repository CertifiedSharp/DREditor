//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System.Collections.Generic;
#if !FMOD_UNITY
using System.Linq;
#endif
using UnityEngine;
#if FMOD_UNITY
using FMODUnity;
#endif

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(EnvironmentSFXManangerBase) + ", " + nameof(IDRESaveable) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    public class EnvironmentSFXMananger : EnvironmentSFXManangerBase, IDRESaveable, IDRELoadable
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
        [EventRef][SerializeField] protected string LastPlayedEnv = "";
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip LastPlayedEnv = null;
#endif

        [Header("")]
        [Divider]
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter environmentSFXSource = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource environmentSFXSource = null;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<AudioClip> envSounds = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string envSaveKey = "EnvData";

        /// <inheritdoc/>
        protected override void InitializeService()
        {
            base.InitializeService();
#if FMOD_UNITY
            if (environmentSFXSource == null)
                environmentSFXSource = GetComponentInChildren<StudioEventEmitter>();
#else
            if (environmentSFXSource == null)
                environmentSFXSource = GetComponentInChildren<AudioSource>();
#endif
        }

        /// <inheritdoc/>
        public override void PlayEnv<T>(T name)
        {
#if FMOD_UNITY
            if (name is string eventName && eventName != "")
            {
                environmentSFXSource.Stop();

                environmentSFXSource.Event = eventName;

                LastPlayedEnv = eventName;

                environmentSFXSource.Play();

                if (debugLogs)
                    UnityEngine.Debug.LogWarning("REPORT: Playing Environment: " + eventName);
            }
#else
            if (name is AudioClip clip && clip != null)
            {
                environmentSFXSource.Stop();

                environmentSFXSource.clip = clip;

                LastPlayedEnv = clip;

                environmentSFXSource.Play();

                if (debugLogs)
                    UnityEngine.Debug.LogWarning("Playing Environment: " + clip);
            }
#endif
        }

        /// <inheritdoc/>
        public override void StopEnv()
        {
            if (debugLogs)
                UnityEngine.Debug.LogWarning("REPORT: Reset Environment SFX.");
#if FMOD_UNITY
            LastPlayedEnv = "";
#else
            LastPlayedEnv = null;
#endif
            environmentSFXSource.Stop();
        }

#if FMOD_UNITY
        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas) => saveDatas[envSaveKey] = LastPlayedEnv;
#else
        public virtual void Save(Dictionary<string, object> saveData) => saveData[envSaveKey] = LastPlayedEnv.name;
#endif
        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => LoadEnv(saveDatas[envSaveKey]);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        protected virtual void LoadEnv<T>(T name)
        {
#if FMOD_UNITY
            if (name is string loadName)
                if (loadName == "" && debugLogs)
                    UnityEngine.Debug.LogWarning("ALERT: Couldn't find Saved Ambience, no Environment sound playing.");
                else if (loadName != "")
                    PlayEnv(loadName);
#else
            if (name is string loadName)
                if (loadName == null && debugLogs)
                    UnityEngine.Debug.LogWarning("ALERT: Couldn't find Saved Ambience, no Environment sound playing.");
                else if (loadName != null)
                    envSounds.FirstOrDefault(clip => clip.name == loadName);
#endif
        }
    }
}