//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [System.Serializable]
    public class Panel : TrialLine, IDebateScanner
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected float waitTime = 0;

        /// <summary>
        ///
        /// </summary>
        public float WaitTime { get => waitTime; set => waitTime = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected string[] wordArray;

        /// <summary>
        ///
        /// </summary>
        public string[] WordArray { get => wordArray; set => wordArray = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        public List<PhraseBase> Phrases = new();


        /// <inheritdoc/>
        public virtual bool Scan(out string log)
        {
            log = "";

            bool result = false;

            foreach (var phrase in Phrases)
                if (phrase is IDebateScanner scannable && scannable.Scan(out string phraseLog))
                {
                    log += $"\n on Phrase index: {Phrases.IndexOf(phrase)} {phraseLog}";

                    result = true;
                }

            return result;
        }
    }
}