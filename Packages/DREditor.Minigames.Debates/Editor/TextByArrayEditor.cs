//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class TextByArrayEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected ITextByArray display;

        /// <summary>
        ///
        /// </summary>
        protected readonly string textName;


        public TextByArrayEditor(string textName = "Text") => this.textName = textName;


        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        /// <param name="wordArray"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual string[] DisplayWordEditor(ITextByArray display, string[] wordArray, string text)
        {
            this.display = display;

            display.SetCount(DisplayWordCount(display.WordCount));

            string[] newWordArray = UpdateWordArray(wordArray, display.WordCount, text);

            if (newWordArray.Length == 0)
            {
                display.SetText("");

                display.SetStart(0);

                return newWordArray;
            }

            display.SetStart(DisplayWordStart(display.WordStart, newWordArray));

            GUILayout.Space(5);

            display.SetText(UpdateText(newWordArray));

            if (string.IsNullOrWhiteSpace(display.Text))
                display.SetStart(0);

            return newWordArray;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wordCount"></param>
        /// <returns></returns>
        protected virtual int DisplayWordCount(int wordCount)
        {
            using (new EditorGUILayout.HorizontalScope())
                return HandyFields.IntField($"Words in {textName}: ", wordCount, (int)PanelEditorConstants.EditorSpacing);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wordStart"></param>
        /// <param name="wordArray"></param>
        /// <returns></returns>
        protected virtual int DisplayWordStart(int wordStart, string[] wordArray)
        {
            return HandyFields.Popup($"Start of {textName} :", wordStart,
                HandyFields.CreateUniqueDropdownLabels(wordArray), 100, 6);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wordArray"></param>
        /// <param name="wordCount"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual string[] UpdateWordArray(string[] wordArray, int wordCount, string text)
        {
            bool emptyWordCount = wordCount == 0;

            bool emptyText = string.IsNullOrWhiteSpace(text);

            wordArray = emptyWordCount || emptyText ? new string[0] : text.Split(' ');

            return wordArray;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newWordArray"></param>
        /// <returns></returns>
        protected virtual string UpdateText(string[] newWordArray)
        {
            string errorMessage = VerifyText(newWordArray, display.WordStart, display.WordCount);

            if (errorMessage != null)
            {
                HandyFields.Label(errorMessage);

                return "";
            }
            else if (display.WordCount == 1)
                return newWordArray[display.WordStart];
            else
                return string.Join(" ", newWordArray, display.WordStart, display.WordCount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wordArray"></param>
        /// <param name="wordStart"></param>
        /// <param name="wordCount"></param>
        /// <returns></returns>
        protected virtual string VerifyText(string[] wordArray, int wordStart, int wordCount)
        {
            if (wordCount < 0)
                return "ALERT: Word count cannot be negative.";
            else if (wordCount > wordArray.Length)
                return "ALERT: No words represent the word count index!";
            else if (wordArray.Length - 1 == wordStart && wordCount > 1)
                return "ALERT: Can't end with a word that's before the starting one!";
            else if (wordStart >= wordArray.Length)
                return "ALERT: Start position exceeds word count.";
            else if (wordStart + wordCount > wordArray.Length)
                return "ALERT: Selection extends beyond available words.";

            return null;
        }
    }
}