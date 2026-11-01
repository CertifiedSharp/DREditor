//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessorRoutine) +
        "\n\n")]
    public class DialogueLineSceneEventManager : DialogueComponent, ILineProcessorRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent eventTrigger = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine) => currentLine.Events.Count != 0 && !isLoadingFile.Value;

        /// <inheritdoc/>
        public virtual IEnumerator LineProcessRoutine(Line currentLine) => EvaluateSceneEvents(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual IEnumerator EvaluateSceneEvents(Line currentLine)
        {
            if (currentLine.Events.Count != 0 && !isLoadingFile.Value)
                foreach (SceneEvent sceneEvent in currentLine.Events.ToArray())
                    yield return TriggerSceneEvent(sceneEvent);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneEvent"></param>
        /// <returns></returns>
        protected virtual IEnumerator TriggerSceneEvent(SceneEvent sceneEvent)
        {
            sceneEvent.Raise();

            // Will wait until trigger is true from Event Ending
            while (!eventTrigger.Value)
                yield return null;

            eventTrigger.SetValue(false);

            yield break;
        }
    }
}