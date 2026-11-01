//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IClearDialogueSaveData) + ", " + nameof(IDRESaveable) +
        "\n\n")]
    public class DialogueNameSaver : DialogueComponent, IClearDialogueSaveData, IDRESaveable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveDialogueNameKey = "DialogueName";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent saveDialogueName = null;


        /// <inheritdoc/>
        public virtual void ClearSaveData() => saveDialogueName.SetValue("");

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            if (!ServiceLocator.GetService<IDialogueManager>().GetComponentsInChildren<DialogueComponent>().Contains(this))
                return;

            saveData.Set(saveDialogueNameKey, saveDialogueName.Value);
        }
    }
}