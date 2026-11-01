//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.TextDisplaying;
using UnityEngine;

namespace DREditor.DialogueMechanics.AutomaticLine
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AutoMechanic) + ", " + nameof(AutoMechanicBase) +
        "\n\n")]
    public class TextBoxAutoMechanic : AutoMechanic
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float lengthTextMultiplier = 0.05f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FloatWithEvent maxAutoWaitTime = null;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override float CalculateAutoWaitTime()
        {
            float waitTime = ServiceLocator.GetService<ITextDisplayerHandler>().GetTextBox().text.Length * lengthTextMultiplier;

            waitTime = waitTime > maxAutoWaitTime.Value ? maxAutoWaitTime.Value : waitTime;

            return waitTime;
        }
    }
}