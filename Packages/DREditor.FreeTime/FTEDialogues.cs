//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class FTEDialogues : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        public abstract Character GetCharacter();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract List<DialogueBase<T>> GetDialogues<T>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        public abstract bool Contains(DialogueCore dialogue);
    }
}