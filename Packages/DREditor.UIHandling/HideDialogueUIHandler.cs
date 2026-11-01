//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueUIComponent) + ", " + nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class HideDialogueUIHandler : DialogueUIComponent, IResolvableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveOrder = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int preparerID = 1;


        public float Order { get => resolveOrder; }

        public int ExclusiveHandlerID => preparerID;


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => true;

        /// <inheritdoc/>
        public virtual void Resolve() => StartCoroutine(ResolveRoutine());

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            SetDialogueUIVisibility();

            EvaluateUISFX();

            yield return WaitForVisibilityChange();
        }

        /// <inheritdoc/>
        protected override void SetDialogueUIVisibility() => visibleUI?.Hide();
    }
}