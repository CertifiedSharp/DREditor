//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.AudioLineHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AudioLineHandler) + ", " + nameof(DialogueComponent) +
        "\n\n")]
    public class PlayAudioHandlers : AudioLineHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<AudioLineHandler> audioHandlers = new();


        /// <inheritdoc/>
        protected override void PlayLineAudio(Line currentLine)
        {
            foreach (var handler in audioHandlers)
                handler.LineProcess(currentLine);
        }
    }
}