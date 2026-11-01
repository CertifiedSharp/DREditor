//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PreparableDialogueStarter : DialogueStarter, IDialoguePrepareEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent dialogueStarted = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int collectionID = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string orderFieldName = "prepareOrder";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueComponentCollection<IPreparableDialogueComponent> preparableCollection = new();

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("PreparableComponents")]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> overrideComponents = new();


        public event Action OnDialoguePrepare;


        protected virtual void Awake() => preparableCollection.Initialize(transform, orderFieldName, collectionID);

        protected virtual void OnValidate() => preparableCollection.Initialize(transform, orderFieldName, collectionID);

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeDialoguePreparationEvent()
        {
            OnDialoguePrepare?.Invoke();

            if (dialogueStarted != null)
                dialogueStarted.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator PrepareDialogueComponents()
        {
            IEnumerable componentList = overrideComponents.Count > 0 ? overrideComponents : preparableCollection.components;

            foreach (var component in componentList)
                if (component != null && component is IPreparableDialogueComponent preparable)
                    if (preparable.UseRoutineToPrepare())
                        yield return preparable.PrepareRoutine();
                    else
                        preparable.Prepare();

            yield break;
        }
    }
}