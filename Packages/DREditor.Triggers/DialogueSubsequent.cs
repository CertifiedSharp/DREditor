//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
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
    public class DialogueSubsequent : MonoBehaviour, ISubsequentBehaviour, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Dialogue> dialogues = new();


        /// <summary>
        ///
        /// </summary>
        protected ScriptableObjectsSubsequentData data = new();


        /// <inheritdoc/>
        public virtual void Call()
        {
            GameObject tryActor = null;

            try
            {
                tryActor = GameObject.Find(dialogues[0].Lines[0].Speaker.FirstName);
            }
            catch (NullReferenceException)
            {
                UnityEngine.Debug.Log("REPORT: First Speaker on dialogue is null.");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("REPORT: Trouble getting first speaker: " + e.ToString());
            }

            SelectableObject actor = null;

            if (tryActor)
                tryActor.GetComponent<SelectableObject>();

            UnityEngine.Debug.Log("REPORT: Calling StartDialogue from Subsequent");

            ServiceLocator.GetService<IDialogueManager>().StartDialogue(actor, dialogues.ToArray());
        }

        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
        {
            if (data.scriptableObject is Dialogue dia)
                if (ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<Line>(dia))
                    return true;

            foreach (var dialogue in data.scriptableObjects)
                if (ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<Line>((Dialogue)dialogue))
                    return true;

            return false;
        }

        /// <inheritdoc/>
        public virtual object Save()
        {
            ScriptableObjectsSubsequentData newData = (ScriptableObjectsSubsequentData)data.Clone();

            newData.SetType(GetType().Name);

            newData.SetScriptableObject(data.scriptableObject);

            foreach(var dialogue in dialogues)
                if (!newData.scriptableObjects.Contains(dialogue))
                    newData.scriptableObjects.Add(dialogue);

            return newData;
        }

        /// <inheritdoc/>
        public virtual void Load(object ob)
        {
            data = (ScriptableObjectsSubsequentData)Convert.ChangeType(ob, typeof(ScriptableObjectsSubsequentData));

            foreach (var dialogue in data.scriptableObjects)
                dialogues.Add((Dialogue)dialogue);

            if (dialogues.Count == 0 && data.scriptableObject != null)
                dialogues.Add((Dialogue)data.scriptableObject);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Call")]
        public void DebugCall() => Call();
    }
}