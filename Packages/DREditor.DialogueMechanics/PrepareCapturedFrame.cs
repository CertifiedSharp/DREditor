//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Starting;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IPreparableDialogueComponent) +
        "\n\n")]
    public class PrepareCapturedFrame : DialogueComponent, IPreparableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float prepareOrder = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureFreezeFrameEventString = "CaptureMain";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string disableCameraEventString = "DisableMainCamera";


        public float Order { get => prepareOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToPrepare() => true;

        /// <inheritdoc/>
        public virtual void Prepare() => StartCoroutine(PrepareRoutine());

        /// <inheritdoc/>
        public virtual IEnumerator PrepareRoutine() => CaptureFreezeFrame();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator CaptureFreezeFrame()
        {
            yield return new WaitForEndOfFrame();

            DialogueEventSystem.TriggerEvent(captureFreezeFrameEventString);

            yield return null;

            DialogueEventSystem.TriggerEvent(disableCameraEventString);

            yield break;
        }
    }
}