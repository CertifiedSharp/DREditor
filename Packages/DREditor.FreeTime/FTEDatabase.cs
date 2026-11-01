//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/FTE/FTE Database", fileName = "FTE Database")]
    public class FTEDatabase : ScriptableObject, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        public List<FTEDialogues> freeTimeCharacterDialogues = new();


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
        {
            foreach (var fteDialogues in freeTimeCharacterDialogues)
                if (ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogues<Line>(fteDialogues.GetDialogues<Line>()))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public virtual Character GetCharacterFromDialogue(DialogueCore dialogue)
        {
            var found = freeTimeCharacterDialogues.Find(fteDialogues => fteDialogues.Contains(dialogue));

            return found != null ? found.GetCharacter() : null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual DialogueCore GetFTE(Character character, int index)
        {
            FTEDialogues fteDialogues = freeTimeCharacterDialogues.Find(dialogues => dialogues.GetCharacter() == character);

            if (fteDialogues != null && index >= 0 && index < freeTimeCharacterDialogues.Count)
                return fteDialogues.GetDialogues<Line>()[index];

            UnityEngine.Debug.LogWarning("ALERT: GetFTE dialogue returned null!");

            return null;
        }
    }
}