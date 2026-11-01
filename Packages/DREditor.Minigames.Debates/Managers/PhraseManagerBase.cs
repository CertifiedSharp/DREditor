//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PhraseManagerBase : MonoBehaviour, IPhraseManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<PhraseTextBase> phraseTexts = new();


        [Header("")]
        [SerializeField] protected UnityEvent<Panel, PhraseTextBase, float> onPhraseInitialized;


        public UnityEvent<Panel, PhraseTextBase, float> OnPhraseInitialized { get => onPhraseInitialized; }


        /// <summary>
        ///
        /// </summary>
        protected readonly List<PhraseTextBase> activePhrases = new();


        /// <inheritdoc/>
        public abstract float PlayPhrases(Panel panel);


        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="duration"></param>
        protected virtual IEnumerator RemovePhraseWhenExpired(PhraseTextBase phrase, float duration)
        {
            yield return new WaitForSeconds(duration);

            if (activePhrases.Contains(phrase))
                activePhrases.Remove(phrase);
        }


        /// <inheritdoc/>
        public virtual List<PhraseTextBase> GetActivePhrases() => activePhrases;
    }
}