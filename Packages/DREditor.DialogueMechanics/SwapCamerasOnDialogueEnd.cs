//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class SwapCamerasOnDialogueEnd : DialogueComponent, IResolvableDialogueComponent
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
        [SerializeField] protected string disableDialogueCameraEventString = "DisableDialogueCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string disableBlurCameraEventString = "DisableBlurCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string enableMainCameraEventString = "EnableMainCamera";


        public float Order { get => resolveOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => false;

        /// <inheritdoc/>
        public virtual void Resolve() => TurnOffDialogueCameras();

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            Resolve();

            yield break;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void TurnOffDialogueCameras()
        {
            DialogueEventSystem.TriggerEvent(disableDialogueCameraEventString);

            DialogueEventSystem.TriggerEvent(disableBlurCameraEventString);

            DialogueEventSystem.TriggerEvent(enableMainCameraEventString);
        }
    }
}