//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class ResolveCapturedFrame : DialogueComponent, IResolvableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveOrder = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float waitForCaptureResolveOrder = 6;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent freezeUIVisible = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool waitForCaptureToFadeOut = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isGoingToMainMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float captureImageFadeOutDuration = 0.5f;


        public float Order { get => waitForCaptureToFadeOut? waitForCaptureResolveOrder : resolveOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => waitForCaptureToFadeOut;

        /// <inheritdoc/>
        public virtual void Resolve() => StartCoroutine(ResolveRoutine());

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            freezeUIVisible.SetValue(false);

            if (waitForCaptureToFadeOut && !isGoingToMainMenu.Value)
                yield return new WaitForSecondsRealtime(captureImageFadeOutDuration);

            yield break;
        }
    }
}