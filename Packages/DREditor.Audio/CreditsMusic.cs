//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CreditsMusic : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onLeavingCredits = null;


        /// <summary>
        ///
        /// </summary>
        protected abstract void Start();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => onLeavingCredits.Response.AddListener(FadeMusicOnLeaving);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => onLeavingCredits.Response.RemoveListener(FadeMusicOnLeaving);

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeMusicOnLeaving() => ServiceLocator.GetService<MusicManagerBase>().FadeMusic();


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musicEvents"></param>
        /// <returns></returns>
        protected virtual IEnumerator PlayMusicTracks<T>(List<T> musicEvents)
        {
            for (int i = 0; i < musicEvents.Count; i++)
            {
                ServiceLocator.GetService<MusicManagerBase>().PlayMusic(musicEvents[i], true);

                yield return new WaitUntil(() => ServiceLocator.GetService<MusicManagerBase>().MusicIsPlaying());

                while (ServiceLocator.GetService<MusicManagerBase>().MusicIsPlaying())
                    yield return null;
            }

            yield break;
        }
    }
}