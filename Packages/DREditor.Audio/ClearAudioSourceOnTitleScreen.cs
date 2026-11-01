//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
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
    [ClassHeader(nameof(IToTitleScreen) +
        "\n\n")]
    public class ClearAudioSourceOnTitleScreen : MonoBehaviour, IToTitleScreen
    {
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter audioSource = null;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource audioSource = null;
#endif

        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            if (audioSource != null)
                ClearSource();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearSource()
        {
#if FMOD_UNITY
            audioSource.Event = "";
#else
            audioSource.clip = null;
#endif
        }
    }
}