//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Death/Dead Trial Chapter", fileName = "DeadTrialChapter")]
    [Serializable]
    public class DeadTrialChapter : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int chapterIndex = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DeadTrialCharacter> deadCharacters = new();


        /// <summary>
        ///
        /// </summary>
        public virtual int GetChapterIndex() => chapterIndex;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetUpDeadCharacters()
        {
            foreach (var deadChar in deadCharacters)
                deadChar.SetUpDeadCharacterInTrial();
        }
    }
}