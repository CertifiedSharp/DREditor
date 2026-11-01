//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILoadToLineEnd) +
        "\n\n")]
    public class ActorFocusLoadEnd : DialogueComponent, ILoadToLineEnd
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent loadingToLine = null;

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
        [Tooltip("")]
        [SerializeField] protected FocusHandler FocusHandler = null;


        /// <summary>
        ///
        /// </summary>
        protected Line currentLine;


        /// <inheritdoc/>
        public virtual void OnLoadToLineEnd(Line currentLine) => InitializeActorFocus(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void InitializeActorFocus(Line currentLine)
        {
            this.currentLine = currentLine;

            EvaluateActorFocus();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateActorFocus()
        {
            if (LinePansToCharacter())
                FocusPanToCharacterProcess();
            else if (NotSkippingPanning())
                FocusCurrentActor();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool LinePansToCharacter() => currentLine.PanToChar;

        /// <summary>
        ///
        /// </summary>
        protected virtual void FocusPanToCharacterProcess()
        {
            Actor actorPan = null;

            if (currentLine.CharToPan != null)
                actorPan = (Actor)ServiceLocator.GetService<IObjectPooler<Talkable>>()
                    .GetObjectByName(currentLine.CharToPan.FirstName);

            if (actorPan != null)
                FocusHandler.ChangeFocus(actorPan, (Actor)lastActor.Value);

            lastActor.SetValue(actorPan);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool NotSkippingPanning() => currentActor.Value != null && !currentLine.DontPan && 
            (!loadingToLine.Value || lastActor.Value != currentActor.Value);

        /// <summary>
        ///
        /// </summary>
        protected virtual void FocusCurrentActor() => FocusHandler.ChangeFocus((Actor)currentActor.Value, (Actor)lastActor.Value);
    }
}