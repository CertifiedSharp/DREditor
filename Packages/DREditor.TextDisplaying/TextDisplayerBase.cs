//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Services;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TextDisplayerBase : MonoBehaviour, IToTitleScreen, ILineRoutine
    {
        /// <summary>
        ///
        /// </summary>
        protected TMP_Text box = null;

        /// <summary>
        ///
        /// </summary>
        protected bool isWaitingForClick = false;

        /// <summary>
        ///
        /// </summary>
        protected float textDisplaySpeed = 3.5f;


        /// <summary>
        ///
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="line"></param>
        public abstract void DisplayText(TMP_Text textBox, Line line);

        /// <summary>
        ///
        /// </summary>
        /// <param name="speed"></param>
        public virtual void SetTextDisplaySpeed(float speed) => textDisplaySpeed = speed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="waitingForClick"></param>
        public virtual void SetWaitingForClick(bool waitingForClick) => isWaitingForClick = waitingForClick;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsWaitingForClick() => isWaitingForClick;


        /// <summary>
        ///
        /// </summary>
        /// <param name="preparedText"></param>
        public virtual void ResolveDisplayingText(string preparedText)
        {
            if (box != null)
                PasteFinalDisplayText(preparedText);

            SetWaitingForClick(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        protected virtual void PasteFinalDisplayText(string text)
        {
            box.text = text;

            box.maxVisibleCharacters = int.MaxValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string GetText() => box != null ? box.text : "";


        /// <inheritdoc/>
        public abstract bool ShouldProcess(Line currentLine);


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => ClearText();

        /// <summary>
        ///
        /// </summary>
        public virtual void ClearText()
        {
            if (box != null)
            {
                box.maxVisibleCharacters = 0;

                box.text = "";
            }
        }
    }
}