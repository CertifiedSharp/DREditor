//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Finishing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueAssetFinisher : DialogueAssetFinisherBase, IInstantDialogueFinisherSubscriber
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected DialogueFinisher InstantDialogueFinisher = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int collectionID = 1;

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
        public override void FinishDialogue()
        {
            EnableMenuAccessIfNotInTrial();

            CallOnDialogueEnd();

            SetNoLongerInDialogueValues();

            FinishDialogueComponents();

            SetControlsBasedOnCurrentMode();

            CallOnFinishedDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FinishDialogueComponents()
        {
            IEnumerable componentList = overrideComponents.Count > 0 ? overrideComponents : finishableCollection.components;

            foreach (DialogueComponent finishable in componentList)
                if (finishable != null && finishable is IFinishableDialogueComponent component)
                    component.OnFinishDialogue();
        }


        /// <inheritdoc/>
        public void SubscribeToOnFinishOfInstantDialogue(Action function) 
            => InstantDialogueFinisher.SubscribeToOnFinishOfDialogue(function);

        /// <inheritdoc/>
        public void UnsubscribeToOnFinishOfInstantDialogue(Action function) 
            => InstantDialogueFinisher.UnsubscribeToOnFinishOfDialogue(function);
    }
}