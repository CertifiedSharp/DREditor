//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.ReportCards
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ReportCardComponent : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;


        /// <summary>
        ///
        /// </summary>
        protected List<string> names = new();

        /// <summary>
        ///
        /// </summary>
        protected int cardIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected Character character = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="names"></param>
        public virtual void SetCharacterNames(List<string> names) => this.names = names;

        /// <summary>
        ///
        /// </summary>
        /// <param name="cardIndex"></param>
        public virtual void SetCardIndex(int cardIndex)
        {
            this.cardIndex = cardIndex;

            UpdateCharacter();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateCharacter() => character = characterDatabase.GetCharacter(names[cardIndex]);
    }
}