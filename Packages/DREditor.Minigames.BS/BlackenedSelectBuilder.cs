//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Minigames/Blackened Select", fileName = "BS Builder")]
    [System.Serializable]
    public class BlackenedSelectBuilder : MinigameBuilderBase
    {
        /// <summary>
        ///
        /// </summary>
        public string questionText = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public List<BlackenedSelectData> selectDatas = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="questionText"></param>
        public virtual void SetQuestionText(string questionText) => this.questionText = questionText;
    }
}