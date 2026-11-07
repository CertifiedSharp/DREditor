//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Finishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEnder) + ", " +
        "\n\n")]
    public class InstantDialogueEnder : DialogueEnder
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] DialogueFinisher InstantDialogueFinisher = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int collectionID = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string orderFieldName = "resolveOrder";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueComponentCollection<IResolvableDialogueComponent> resolvableCollection = new();

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("ResolvableComponents")]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> overrideComponents = new();


        protected virtual void Awake() => resolvableCollection.Initialize(transform, orderFieldName, collectionID);

        protected virtual void OnValidate() => resolvableCollection.Initialize(transform, orderFieldName, collectionID);


        /// <inheritdoc/>
        public override void StopDialogue() => StartCoroutine(EndDialogue());

        /// <inheritdoc/>
        public override IEnumerator EndDialogue(bool finishDialogue = true)
        {
            UnityEngine.Debug.Log($"REPORT:  {ToString()}: Instant dialogue has started to resolve.");

            yield return ResolveDialogueComponents();

            ResolveDialogueEventHandler();

            yield return InstantDialogueFinisher.FinishDialogueRoutine(finishDialogue);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Instant dialogue has resolved.");

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveDialogueComponents()
        {
            IEnumerable componentList = overrideComponents.Count > 0 ? overrideComponents : resolvableCollection.components;

            foreach (var component in componentList)
                if (component != null && component is IResolvableDialogueComponent resolvable)
                    if (resolvable.UseRoutineToResolve())
                        yield return resolvable.ResolveRoutine();
                    else
                        resolvable.Resolve();

            yield break;
        }

        /// <inheritdoc/>
        public override void FinishDialogue() => InstantDialogueFinisher.FinishDialogue();
    }
}