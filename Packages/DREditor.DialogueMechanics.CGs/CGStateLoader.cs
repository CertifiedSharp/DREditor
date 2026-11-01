//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CGStateLoader<T> : CGStateLoaderBase, ILoadToLineLastingEvent, IOnPrepareLoadedLine, ILoadEndState
        where T : IDialogueEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]

        [Space]

        [Header("")]

        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGStateMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgStateEnded = null;


        /// <summary>
        ///
        /// </summary>
        protected T lastCGEvent;


        public virtual IEnumerator LoadEndStates(Line currentLine = null)
        {
            if (inLoadCGState && lastCGEvent != null && (currentLine == null || !currentLine.DiaEvents.OfType<T>().Any()))
            {
                UnityEngine.Debug.Log($"REPORT: {gameObject.name}: " +
                    $"On Last active CG Type state: " + typeof(T).Name);

                lastCGEvent.TriggerDialogueEvent();

                yield return new WaitUntil(() => cgStateEnded.Value);

                cgStateEnded.SetValue(false);
            }

            yield break;
        }


        public virtual void OnPrepareLoadedLine(Line currentLine)
            => DialogueEventUtility.ContainsDialogueEvent<T>(currentLine, OnPrepareLoadedLineComplete);

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        protected abstract void OnPrepareLoadedLineComplete(T display);


        public virtual void OnLoadToLineLasting(Line currentLine) 
            => DialogueEventUtility.ContainsDialogueEvent<T>(currentLine, OnLoadToLineLastingComplete);

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        protected virtual void OnLoadToLineLastingComplete(T display)
        {
            SetLoadingCGState(ShouldBeTracked(display));

            if (inLoadCGState)
                StartTracking(display);
            else
                ClearTracking();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        protected virtual void StartTracking(T display)
        {
            lastCGEvent = display;

            ClearAllOtherCGLoaders();
        }

        protected override void ClearTracking()
        {
            base.ClearTracking();

            SetLastEventNull();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void SetLastEventNull() => lastCGEvent = default;



        /// <summary>
        ///
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        protected abstract bool ShouldBeTracked(T display);
    }
}