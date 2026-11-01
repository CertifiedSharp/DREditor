//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CounterType : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        /// <returns></returns>
        public abstract bool Evaluate(PhraseBase textPhrase);

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        /// <param name="currentPanel"></param>
        public abstract IEnumerator DisplayCounter(PhraseBase textPhrase, Line currentPanel);


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void OnCompletedCounter() { }
    }
}