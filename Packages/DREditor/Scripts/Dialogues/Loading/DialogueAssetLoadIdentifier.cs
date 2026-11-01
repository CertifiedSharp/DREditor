//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueLoadIdentifier) + ", " + nameof(IDRELoadable) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class DialogueAssetLoadIdentifier : DialogueLoadIdentifier, IDRELoadable, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent loadDialogueFound = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveDialogueNameKey = "DialogueName";


        /// <summary>
        ///
        /// </summary>
        protected string SaveDialogueName = "";


        /// <inheritdoc/>
        public override bool CheckDialogues<T>(List<DialogueBase<T>> dialogues)
        {
            foreach(DialogueBase<T> dialogue in dialogues)
                if (CheckDialogue(dialogue))
                    return true;

            return false;
        }

        /// <inheritdoc/>
        public override bool CheckDialogue<T>(DialogueBase<T> dialogue)
        {
            if (dialogue != null && !loadDialogueFound.Value)
            {
                bool found = IsDialogueToLoad(dialogue);

                if (found)
                    return found;
                else if (dialogue.DirectTo.NewDialogue != null)
                    return CheckDialogue<T>(dialogue.DirectTo.NewDialogue);
            }

            return false;
        }

        /// <inheritdoc/>
        protected override bool IsDialogueToLoad(DialogueCore dialogue)
        {
            bool isDialogueToLoad = CheckIdentification(dialogue);

            if (isDialogueToLoad)
                OnIdentificationMatched(dialogue);

            return isDialogueToLoad;
        }

        /// <inheritdoc/>
        protected override bool CheckIdentification(DialogueCore dialogue) => dialogue.name == SaveDialogueName;

        /// <inheritdoc/>
        protected override void OnIdentificationMatched(DialogueCore dialogue)
        {
            UnityEngine.Debug.Log("REPORT: Dialogue to load found: " + dialogue.name);

            LoadDialogue = dialogue;

            loadDialogueFound.SetValue(true);
        }

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas)
        {
            if (string.IsNullOrEmpty(SaveDialogueName))
                SaveDialogueName = saveDatas.Get<string>(saveDialogueNameKey);
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => SaveDialogueName = "";
    }
}