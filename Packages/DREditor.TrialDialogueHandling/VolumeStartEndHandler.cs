//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Starting;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(IPreparableDialogueComponent) + ", " + nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class VolumeStartEndHandler : DialogueComponent, IPreparableDialogueComponent, IResolvableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float prepareOrder = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveOrder = 6;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Volume volume = null;


        float IDialogueComponent<IPreparableDialogueComponent>.Order { get => prepareOrder; }

        float IDialogueComponent<IResolvableDialogueComponent>.Order { get => resolveOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToPrepare() => false;

        /// <inheritdoc/>
        public virtual void Prepare()
        {
            if (volume != null)
                volume.enabled = true;
        }

        /// <inheritdoc/>
        public virtual IEnumerator PrepareRoutine()
        {
            Prepare();

            yield break;
        }


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => false;

        /// <inheritdoc/>
        public virtual void Resolve()
        {
            if (volume != null)
                volume.enabled = false;
        }

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            Resolve();

            yield break;
        }
    }
}