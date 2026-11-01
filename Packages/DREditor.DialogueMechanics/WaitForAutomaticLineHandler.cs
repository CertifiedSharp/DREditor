//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessorRoutine) +
        "\n\n")]
    public class WaitForAutomaticLineHandler : DialogueComponent, ILineProcessorRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine) => !inFastForwardMode.Value && currentLine.AutomaticLine;

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        public virtual IEnumerator LineProcessRoutine(Line currentLine)
        {
            if (ShouldProcess(currentLine))
                yield return new WaitForSeconds(currentLine.TimeToNextLine);
        }
    }
}