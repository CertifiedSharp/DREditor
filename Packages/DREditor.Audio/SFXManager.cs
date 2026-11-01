//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


#if FMOD_UNITY
using FMODUnity;
#endif

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    public class SFXManager : SFXManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] bool debugLogs = false;


        [Header("")]
        [Divider]
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter sfxSource = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource sfxSource = null;
#endif

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [FormerlySerializedAs("SubmitSource")]
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter submitSource = null;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("CancelSource")]
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter cancelSource = null;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("SelectSource")]
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter selectSource = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [FormerlySerializedAs("SubmitSource")]
        [Tooltip("")]
        [SerializeField] protected AudioSource submitSource = null;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("CancelSource")]
        [Tooltip("")]
        [SerializeField] protected AudioSource cancelSource = null;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("SelectSource")]
        [Tooltip("")]
        [SerializeField] protected AudioSource selectSource = null;
#endif

        /// <inheritdoc/>
        protected override void InitializeService()
        {
            base.InitializeService();
#if FMOD_UNITY
            if (sfxSource == null)
                sfxSource = GetComponentInChildren<StudioEventEmitter>();
#else
            if (sfxSource == null)
                sfxSource = GetComponentInChildren<AudioSource>();
#endif
        }

        /// <inheritdoc/>
        public override void PlaySubmit() => PlaySource(submitSource);

        /// <inheritdoc/>
        public override void PlayCancel() => PlaySource(cancelSource);

        /// <inheritdoc/>
        public override void PlaySelect() => PlaySource(selectSource);


        /// <inheritdoc/>
        public override void PlaySFX<T>(T name)
        {
#if FMOD_UNITY
            if (name is string eventName)
            {
                if (eventName == "")
                {
                    if (debugLogs)
                        UnityEngine.Debug.LogWarning("ALERT: CANCELED SFX: " + eventName);

                    return;
                }

                if (debugLogs)
                    UnityEngine.Debug.Log("REPORT: Playing SFX: " + eventName);

                if (!sfxSource.IsPlaying())
                    PlaySource(sfxSource, eventName);
                else
                    PlayAdditionalSoundInstance(eventName);
            }
#else
            if (name is AudioClip eventName)
            {
                if (eventName == null)
                {
                    if (debugLogs)
                        UnityEngine.Debug.LogWarning("ALERT: CANCELED SFX: " + eventName);

                    return;
                }

                if (debugLogs)
                    UnityEngine.Debug.Log("REPORT: Playing SFX: " + eventName);

                if (!sfxSource.isPlaying)
                    PlaySource(sfxSource, eventName);
                else
                    PlayAdditionalSoundInstance(eventName);
            }
#endif
        }

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventName"></param>
        protected virtual void PlayAdditionalSoundInstance(string eventName)
        {
            var emitter = gameObject.AddComponent<StudioEventEmitter>();

            emitter.Event = eventName;

            emitter.TriggerOnce = true;

            emitter.Play();

            StartCoroutine(WaitForInstanceSFXToFinish(emitter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        /// <returns></returns>
        protected virtual IEnumerator WaitForInstanceSFXToFinish(StudioEventEmitter emitter)
        {
            while (emitter.IsPlaying())
                yield return null;

            Destroy(emitter);

            yield break;
        }

        /// <inheritdoc/>
        public override void StopSoundFX()
        {
            sfxSource.Stop();

            StopAllInstanceEmitters();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopAllInstanceEmitters()
        {
            AffectInstanceEmitters((emitter) => emitter.Stop());

            AffectInstanceEmitters((emitter) => Destroy(emitter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="affectEmitter"></param>
        protected virtual void AffectInstanceEmitters(Action<StudioEventEmitter> affectEmitter)
        {
            List<StudioEventEmitter> emitters = GetComponents<StudioEventEmitter>().ToList();

            if (emitters.Count > 0)
                foreach (StudioEventEmitter emitter in emitters)
                    if (emitter != null)
                        affectEmitter(emitter);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        protected virtual void PlaySource(StudioEventEmitter emitter)
        {
            if (!(emitter != null) || emitter.Event == "")
                return;

            emitter.Play();
        }

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
        /// <param name="eventName"></param>
        protected virtual void PlayAdditionalSoundInstance(AudioClip eventName)
        {
            var emitter = gameObject.AddComponent<AudioSource>();

            emitter.clip = eventName;

            emitter.Play();

            StartCoroutine(WaitForInstanceSFXToFinish(emitter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        /// <returns></returns>
        protected virtual IEnumerator WaitForInstanceSFXToFinish(AudioSource emitter)
        {
            while (emitter.isPlaying)
                yield return null;

            Destroy(emitter);

            yield break;
        }

        public override void StopSoundFX()
        {
            sfxSource.Stop();

            StopAllInstanceEmitters();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopAllInstanceEmitters()
        {
            AffectInstanceEmitters((emitter) => emitter.Stop());

            AffectInstanceEmitters((emitter) => Destroy(emitter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="affectEmitter"></param>
        protected virtual void AffectInstanceEmitters(Action<AudioSource> affectEmitter)
        {
            List<AudioSource> emitters = GetComponents<AudioSource>().ToList();

            if (emitters.Count > 0)
                foreach (AudioSource emitter in emitters)
                    if (emitter != null)
                        affectEmitter(emitter);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        protected virtual void PlaySource(AudioSource source)
        {
            if (!(source != null) || source.clip == null)
                return;

            source.Play();
        }

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
    }
}