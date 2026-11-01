//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
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
    public class TrialCameraAnimationHandler : TrialCameraDialogueHandler, ILineProcessor, ILoadToLineable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TrialCameraAnimDatabase tCAD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int noCameraAnimationIndex = 10;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useRebind = true;


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            if (currentLine is TrialLine currentTrialLine && currentTrialLine != null)
                TriggerCamAnim(trialCamera, currentTrialLine.CamAnimIdx);
        }

        /// <inheritdoc/>
        public virtual void OnLoadToLine(Line currentLine) => LineProcess(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="trialCamera"></param>
        /// <param name="camAnimIdx"></param>
        protected virtual void TriggerCamAnim(DRTrialCamera trialCamera, int camAnimIdx)
        {
            if (camAnimIdx != noCameraAnimationIndex)
            {
                if (useRebind)
                    trialCamera.CameraAnimator.Rebind();

                trialCamera.TriggerAnim(tCAD.GetNames()[camAnimIdx]);
            }
        }
    }
}