//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader("" + nameof(ILineProcessor) + ", " + nameof(ILineEndProcess) + ", " + 
        nameof(ILoadToLineable) + ", " + nameof(ILoadToLineEnd) + ", " + nameof(IDialogueReaderCleanUp) + ", " + 
        nameof(IToTitleScreen) +
        "\n\n")]
    public class ActorOrganizer : DialogueComponent, ILineProcessor, ILineEndProcess, 
        ILoadToLineable, ILoadToLineEnd, IDialogueReaderCleanUp, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isProtagSpeaking = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent currentActor = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent lastActor = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        protected virtual void SetCurrentActorByName(string name)
        {
            currentActor.SetValue((Actor)ServiceLocator.GetService<IObjectPooler<Talkable>>().GetObjectByName(name));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void SetLastActorFromLineData(Line currentLine = null)
        {
            if (!isProtagSpeaking.Value && !currentLine.DontPan)
                lastActor.SetValue((Actor)currentActor.Value);
        }


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            if (currentLine.Speaker != null && !isProtagSpeaking.Value)
                SetCurrentActorByName(currentLine.Speaker.FirstName);
        }

        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null) => SetLastActorFromLineData(currentLine);

        /// <inheritdoc/>
        public virtual void OnLoadToLine(Line currentLine)
        {
            if (currentLine.Speaker != null)
                SetCurrentActorByName(currentLine.Speaker.FirstName);
        }

        /// <inheritdoc/>
        public virtual void OnLoadToLineEnd(Line currentLine) => SetLastActorFromLineData(currentLine);

        /// <inheritdoc/>
        public virtual void OnDialogueReaderCleanUp()
        {
            lastActor.SetValue(null);

            ToTitleScreen();
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => isProtagSpeaking.SetValue(false);
    }
}