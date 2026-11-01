//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using TMPro;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    public static class LineTextExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetLineText(this TMP_TextInfo textInfo, int index)
        {
            string lineText = "";

            TMP_LineInfo[] lineInfo = textInfo.lineInfo;

            TMP_WordInfo[] wordInfos = textInfo.wordInfo;

            int wordInPhrase = 0;

            for (int i = 0; i < lineInfo.Length; i++) // For each line
            {
                TMP_LineInfo line = lineInfo[i];

                for (int j = 0; j < line.wordCount; j++) // For each word in the line
                {
                    TMP_WordInfo word = wordInfos[wordInPhrase];

                    if (i == index)
                        lineText += word.GetWord();

                    wordInPhrase++;
                }
            }

            return lineText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        /// <returns></returns>
        public static int GetRealLineCount(this TMP_TextInfo textInfo)
        {
            int realLineCount = 0;

            for (int i = 0; i < textInfo.lineInfo.Length; i++) // For each line in the phrase
                if (textInfo.GetLineText(i) != "")
                    realLineCount++;

            return realLineCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        /// <returns></returns>
        public static List<int> GetValidLineIndexes(this TMP_TextInfo textInfo)
        {
            List<int> list = new();

            for (int i = 0; i < textInfo.lineInfo.Length; i++) // For each line in the phrase
                if (textInfo.GetLineText(i) != "")
                    list.Add(i);

            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetRealIndexOf(this TMP_TextInfo textInfo, int index) => textInfo.GetValidLineIndexes().IndexOf(index);
    }
}