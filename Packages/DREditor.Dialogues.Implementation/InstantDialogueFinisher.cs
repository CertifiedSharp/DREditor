//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueAssetFinisherBase) + ", " + nameof(DialogueFinisher) + ", " + 
        nameof(IEndDialogueEvent) + ", " + nameof(IDialogueFinisher) +
        "\n\n")]
    public class InstantDialogueFinisher : DialogueAssetFinisherBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] BoolWithEvent inInstant = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] BoolWithEvent isGoingToMainMenu = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int collectionID = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string orderFieldName = "finishOrder";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueComponentCollection<IFinishableDialogueComponent> finishableCollection = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> overrideComponents = new();


        protected virtual void Awake() => finishableCollection.Initialize(transform, orderFieldName, collectionID);

        protected virtual void OnValidate() => finishableCollection.Initialize(transform, orderFieldName, collectionID);


        /// <inheritdoc/>
        public override void FinishDialogue() => StartCoroutine(FinishDialogueRoutine());

        /// <inheritdoc/>
        public override IEnumerator FinishDialogueRoutine(bool finishDialogue = true)
        {
            yield return FinishDialogueComponents();

            SetInstantValueFalse();

            CallOnDialogueEnd(); // NOTE: It is unknown why this is called twice but in the past caused issues when removed.

            SetNoLongerInDialogueValues();

            if (finishDialogue)
                MoveToOriginalGameplayState();

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Instant dialogue has finished.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetInstantValueFalse() => inInstant.Value = false;

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToOriginalGameplayState()
        {
            if (!isGoingToMainMenu.Value)
                EnableMenuAccessIfNotInTrial();

            CallOnDialogueEnd();

            SetControlsBasedOnCurrentMode();

            CallOnFinishedDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FinishDialogueComponents()
        {
            IEnumerable componentList = overrideComponents.Count > 0 ? overrideComponents : finishableCollection.components;

            foreach (DialogueComponent component in componentList)
                if (component != null && component is IComplexFinishableDialogueComponent finishable)
                    if (finishable.UseCoroutine())
                        yield return finishable.OnFinishDialogueRoutine();
                    else
                        finishable.OnFinishDialogue();
                else
                    yield return EvaluateFinishingDialogueComponent(component);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        protected virtual IEnumerator EvaluateFinishingDialogueComponent(DialogueComponent component)
        {
            if (component != null && component is IFinishableDialogueComponent finishable)
                finishable.OnFinishDialogue();
            else if (component != null && component is IFinishableDialogueComponentRoutine finishableRoutine)
                yield return finishableRoutine.OnFinishDialogueRoutine();
        }
    }
}