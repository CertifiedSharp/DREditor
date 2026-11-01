//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/FTE/FTE Character Dialogues", fileName = "FTE Character Dialogues")]
    public class FTECharacterDialogues : FTEDialogues
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Character character = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Dialogue> freeTimeDialogues = new();


        /// <inheritdoc/>
        public override bool Contains(DialogueCore dialogue)
        {
            foreach(var fteDialogue in freeTimeDialogues)
                if (fteDialogue != null && fteDialogue.IsNested(dialogue, new List<DialogueCore>()))
                    return true;

            return freeTimeDialogues.Contains(dialogue as Dialogue);
        }

        /// <inheritdoc/>
        public override Character GetCharacter() => character;

        /// <inheritdoc/>
        public override List<DialogueBase<T>> GetDialogues<T>() => freeTimeDialogues.Cast<DialogueBase<T>>().ToList();

#if UNITY_EDITOR
        public virtual void SetCharacter(Character character) => this.character = character;

        public virtual void SetDialogues(List<Dialogue> freeTimeDialogues) => this.freeTimeDialogues = freeTimeDialogues;
#endif
    }
}