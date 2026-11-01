//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
#endif
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CounterHandlerBase) + ", " + nameof(IPanelLineListener) +
        "\n\n")]
    public class CounterHandler : CounterHandlerBase, IPanelLineListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float counterBufferWaitTime = 1;

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField][EventRef] protected string counterSound;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip counterSound;
#endif

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<CounterType> counterProcesses = new();


        /// <summary>
        ///
        /// </summary>
        protected Line currentPanel;


        /// <inheritdoc/>
        public override IEnumerator DisplayCounter(PhraseBase textPhrase)
        {
            yield return new WaitForSeconds(counterBufferWaitTime);

            ServiceLocator.GetService<SFXManagerBase>().PlaySFX(counterSound);

            CounterType counterType = EvaluateCounter(textPhrase);

            if (counterType != null)
            {
                yield return counterType.DisplayCounter(textPhrase, currentPanel);

                counterType.OnCompletedCounter();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        /// <returns></returns>
        protected virtual CounterType EvaluateCounter(PhraseBase textPhrase)
        {
            foreach (CounterType counterType in counterProcesses)
                if (counterType.Evaluate(textPhrase))
                    return counterType;

            UnityEngine.Debug.LogWarning("ALERT: No counters claimed to be the correct one for this scenario," +
                "are you missing a counter type?" +
                "\nPROPOSAL: Add a counter type that will display during this scenario.");

            return null;
        }


        /// <inheritdoc/>
        public virtual void SetCurrentLine(Line currentLine) => currentPanel = currentLine;
    }
}