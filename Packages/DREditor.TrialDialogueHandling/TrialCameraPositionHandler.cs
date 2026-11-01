//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using UnityEngine;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TrialCameraDialogueHandler) + ", " + nameof(ILineProcessor) + ", " + nameof(ILoadToLineable) +
        "\n\n")]
    public class TrialCameraPositionHandler : TrialCameraDialogueHandler, ILineProcessor, ILoadToLineable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int numberOfCharacters = 16;


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            if (currentLine is TrialLine currentTrialLine && currentTrialLine != null)
                SetCamPosition(currentTrialLine.Speaker, trialCamera, currentTrialLine.Co, currentTrialLine.DontPan);
        }

        /// <inheritdoc/>
        public virtual void OnLoadToLine(Line currentLine) => LineProcess(currentLine);


        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="trialCamera"></param>
        /// <param name="trialCameraOverride"></param>
        /// <param name="dontPan"></param>
        protected virtual void SetCamPosition(Character character, DRTrialCamera trialCamera, TCO trialCameraOverride, bool dontPan)
        {
            if (trialCameraOverride != null && trialCameraOverride.enabled)
                trialCamera.ApplyOverrides(trialCameraOverride.height, trialCameraOverride.distance, trialCameraOverride.seatFocus, dontPan);
            else
                EvaluateSetCameraPosition(character, trialCamera, dontPan);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="trialCamera"></param>
        /// <param name="dontPan"></param>
        protected virtual void EvaluateSetCameraPosition(Character character, DRTrialCamera trialCamera, bool dontPan)
        {
            trialCamera.SetDefaultValues();

            bool characterValid = character != null;

            if (!characterValid)
                return;

            bool characterPositionValid = character.TrialPosition < numberOfCharacters;

            bool cameraShouldPan = !dontPan;

            if (characterValid && characterPositionValid && cameraShouldPan)
                trialCamera.SeatFocus = character.TrialPosition;
        }
    }
}