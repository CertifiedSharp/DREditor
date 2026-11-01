//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;
using DREditor.EventObjects;

#if FMOD_UNITY
using FMODUnity;
#endif

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class OnVoiceLineSettingChanged : SceneEventListener
    {
        [Header("")]
        [Divider]
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [EventRef]
        [SerializeField] protected string dialogueTestLine = "event:/SFX/UI/Menus/Dialogue Test";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [EventRef]
        [SerializeField] protected List<string> lines = new();
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip dialogueTestLine = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<AudioClip> lines = new();
#endif

        /// <inheritdoc/>
        public override void OnEventRaised()
        {
            base.OnEventRaised();

            PlayVoice();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlayVoice()
        {
            if (lines.Count != 0)
                ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(lines[Random.Range(0, lines.Count - 1)]);
            else
                ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(dialogueTestLine);
        }
    }
}