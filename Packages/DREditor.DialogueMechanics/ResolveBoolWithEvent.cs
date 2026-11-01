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
    /// <remarks></remarks>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class ResolveBoolWithEvent : DialogueComponent, IResolvableDialogueComponent
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
        [SerializeField] protected BoolWithEvent boolWithEvent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setValueOnResolveTo = false;


        public float Order { get => resolveOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => false;

        /// <inheritdoc/>
        public virtual void Resolve() => boolWithEvent.SetValue(setValueOnResolveTo);

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            Resolve();

            yield break;
        }
    }
}