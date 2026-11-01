//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.TBS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Truth Bullet Select", fileName = "TBS Builder")]
    public class TBSelectBuilder : MinigameBuilderBase
    {
        /// <summary>
        ///
        /// </summary>
        public int chapter = 0;

        /// <summary>
        ///
        /// </summary>
        public int damageOnWrong = 1;

        /// <summary>
        ///
        /// </summary>
        public string question;

        /// <summary>
        ///
        /// </summary>
        public List<TBSelection> selections = new();


        /// <summary>
        ///
        /// </summary>
        public virtual void SetChapter(int chapter) => this.chapter = chapter;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetDamage(int damageOnWrong) => this.damageOnWrong = damageOnWrong;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetQuestion(string question) => this.question = question;


        /// <summary>
        ///
        /// </summary>
        /// <param name="chosenSelection"></param>
        /// <returns></returns>
        public virtual TBSelection GetSelection(string chosenSelection)
            => selections.Where(selection => chosenSelection == selection.bulletOption.DisplayName).FirstOrDefault();
    }
}