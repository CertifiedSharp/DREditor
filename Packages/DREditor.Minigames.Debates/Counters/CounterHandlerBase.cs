//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CounterHandlerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        public abstract IEnumerator DisplayCounter(PhraseBase textPhrase);
    }
}