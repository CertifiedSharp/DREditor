//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CGEventComponent<VideoDisplay>) + " for the CG Style Dialogue Event: " + nameof(VideoDisplay) +
        "\n\n")]
    public class CGVideoEvent : CGEventComponent<VideoDisplay>, ILineLastingEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        protected Action continueLineCallback = null;


        /// <inheritdoc/>
        public virtual IEnumerator LineLastingEvaluation(Line currentLine, Action continueLineCallback = null)
        {
            this.continueLineCallback = continueLineCallback;

            yield return TriggerCGEvent(currentLine);

            yield break;
        }

        /// <inheritdoc/>
        protected override IEnumerator EvaluateCGEvent(VideoDisplay display)
        {
            if (display.mainClip != null || display.playOnly || !isLoadingFile.Value)
            {
                yield return base.EvaluateCGEvent(display);

                if (display.playOnly)
                    continueLineCallback?.Invoke();
            }
        }
    }
}