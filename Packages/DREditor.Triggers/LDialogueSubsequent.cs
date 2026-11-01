//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.LocalDialogues;
using DREditor.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISubsequentBehaviour) + ", " + nameof(IDialogueHolder) +
        "\n\n")]
    public class LDialogueSubsequent : MonoBehaviour, ISubsequentBehaviour, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LocalDialogue dialogue;


        /// <summary>
        ///
        /// </summary>
        protected LocalDialogueSubsequentData data = new();


        /// <inheritdoc/>
        public virtual void Call()
        {
            Dialogue[] dialogues = new Dialogue[1];

            dialogues[0] = ServiceLocator.GetService<LocalDialogueHandlerBase>().EvaluateDialogue(new List<LocalDialogue> { dialogue });

            if (dialogues[0] != null)
            {
                GameObject tryActor = GameObject.Find(dialogues[0].Lines[0].Speaker.FirstName);

                SelectableObject actor = null;

                if (tryActor)
                    tryActor.GetComponentInChildren<SelectableObject>();

                if (ServiceLocator.HasService<IDialogueManager>())
                    ServiceLocator.GetService<IDialogueManager>().StartDialogue(actor, dialogues);
                else
                    UnityEngine.Debug.LogWarning($"ALERT: {typeof(IDialogueManager)} for Dialogue not found.");
            }
            else
                UnityEngine.Debug.LogWarning($"ALERT: The Evaluated Local Dialogue was null!");
        }

        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
            => ServiceLocator.GetService<IDialogueManager>()
                .GetManagerComponent<DialogueLoadIdentifier>()
                .CheckDialogue<Line>(dialogue.dialogue)
                ||
               ServiceLocator.GetService<IDialogueManager>()
                .GetManagerComponent<DialogueLoadIdentifier>()
                .CheckDialogue<Line>(dialogue.dialogueIfFlagNotMet);

        /// <inheritdoc/>
        public virtual object Save()
        {
            LocalDialogueSubsequentData newData = (LocalDialogueSubsequentData)data.Clone();

            newData.SetType(GetType().Name);

            newData.SetLocalDialogue(dialogue);

            return newData;
        }

        /// <inheritdoc/>
        public virtual void Load(object ob)
        {
            data = (LocalDialogueSubsequentData)Convert.ChangeType(ob, typeof(LocalDialogueSubsequentData));

            dialogue = (LocalDialogue)data.localDialogue.Clone();
        }
    }
}