//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineTextDisplayRoutine) +
        "\n\n")]
    public class TweenWaiterHandler : DialogueComponent, ILineTextDisplayRoutine
    {
        /// <summary>
        ///
        /// </summary>
        protected List<ITweenWaiter> tweenWaiters = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => InitializeTweenWaiters();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTweenWaiters()
        {
            MonoBehaviour[] monos = FindObjectsByType<MonoBehaviour>();

            tweenWaiters = monos.OfType<ITweenWaiter>().ToList();
        }


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine) => tweenWaiters.Count > 0;

        /// <inheritdoc/>
        public virtual IEnumerator LineTextDisplay(Line currentLine)
        {
            foreach (ITweenWaiter waiter in tweenWaiters)
                if (waiter != null)
                    yield return waiter.WaitForTweenToFinish();
        }
    }
}