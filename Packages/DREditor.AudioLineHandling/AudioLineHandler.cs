//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.AudioLineHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class AudioLineHandler : DialogueComponent, ILineProcessor
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            if (!isLoadingFile.Value)
                PlayLineAudio(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected abstract void PlayLineAudio(Line currentLine);
    }
}