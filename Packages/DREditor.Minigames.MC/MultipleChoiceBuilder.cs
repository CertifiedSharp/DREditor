//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.MC
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Multiple Choice", fileName = "MC Builder")]
    public class MultipleChoiceBuilder : MinigameBuilderBase
    {
        /// <summary>
        ///
        /// </summary>
        public List<MultipleChoiceOption> choices = new() { new(), new(), new(), new() };

        /// <summary>
        ///
        /// </summary>
        public string question = "";

        /// <summary>
        ///
        /// </summary>
        public int damageOnWrong = 1;

        /// <summary>
        ///
        /// </summary>
        public float timerMinutes;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 59)]
        public float timerSeconds;


        /// <summary>
        ///
        /// </summary>
        /// <param name="question"></param>
        public virtual void SetQuestion(string question) => this.question = question;

        /// <summary>
        ///
        /// </summary>
        /// <param name="damageOnWrong"></param>
        public virtual void SetDamageOnWrong(int damageOnWrong) => this.damageOnWrong = damageOnWrong;


        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual MultipleChoiceOption GetOptionWithText(string text)
        {
            for (int i = 0; i < choices.Count; i++)
                if (choices[i].text == text)
                    return choices[i];

            UnityEngine.Debug.LogWarning("ALERT: Selected choice text did not contain an existing answer.");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetChoiceTexts() => choices.Select(choice => choice.text).ToList();
    }
}