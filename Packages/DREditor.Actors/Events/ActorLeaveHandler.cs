//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class ActorLeaveHandler : ActorFadeEvent, IToTitleScreen
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
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

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


        [Header("")]
        [Divider]
        [SerializeField] protected Vector3 designatedExitPosition = new(0, -20, 0);


        /// <summary>
        ///
        /// </summary>
        protected IDialogueEnder DialogueEnder = null;

        /// <summary>
        ///
        /// </summary>
        protected ILineReader LineReader = null;

        /// <summary>
        ///
        /// </summary>
        protected bool exit = false;

        /// <summary>
        ///
        /// </summary>
        protected readonly List<Action> delegates = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            CharacterLeave characterLeaveTuple = (CharacterLeave)o;

            Talkable talkable = characterLeaveTuple.current ? (Talkable)currentActor.Value : (Talkable)lastActor.Value;

            if (talkable is Actor actor)
                EvaluateCharacterLeave(characterLeaveTuple, actor);
            else
                UnityEngine.Debug.LogWarning($"{ToString()}: WARNING!: Could Not get the actor for Character Leave");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="leaveEvent"></param>
        /// <param name="actor"></param>
        protected virtual void EvaluateCharacterLeave(CharacterLeave leaveEvent, Actor actor)
        {
            exit = leaveEvent.exit;

            if (leaveEvent.appear)
                spriteFader.ActorAppear(actor);
            else if (leaveEvent.current && !isProtagSpeaking.Value)
                ActorLeft(actor);
            else if (lastActor.Value is Actor actorToLeave)
                ActorLeft(actorToLeave);
            else
                UnityEngine.Debug.LogWarning($"{ToString()}: WARNING!: Could Not get the actor for Character Leave");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void ActorLeft(Actor actor)
        {
            spriteFader.ActorDisappear(actor);

            if (exit)
                ActorLeaveExitProcess(actor);
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="actor"></param>
        protected virtual void ActorLeaveExitProcess(Actor actor)
        {
            if (ExitProcessShouldBeCanceled(actor))
                return;

            if (!ServiceLocator.GetService<IDialogueManager>().GetReader().IsOnLastLine())
            {
                LineReader = ServiceLocator.GetService<IDialogueManager>()
                .GetComponentsInChildren<MonoBehaviour>()
                .OfType<ILineReader>()
                .FirstOrDefault();

                void handler()
                {
                    ExitChar(actor);

                    LineReader.OnLineStart -= handler;

                    delegates.Remove(handler);
                }

                delegates.Add(handler);

                LineReader.OnLineStart += handler;
            }
            else
            {
                UnityEngine.Debug.Log($"{ToString()}: REPORT: Adding exit character to the end of the line." +
                    $"\nPROPOSAL: If any bugs occur, contact the developer and notify them of the bug that occured " +
                    $"if it happened almost immediately after the current dialogue ends.");

                DialogueEnder = ServiceLocator.GetService<IDialogueManager>().GetEnder();

                void handler()
                {
                    ExitChar(actor);

                    DialogueEnder.OnDialogueResolving -= handler;

                    delegates.Remove(handler);
                }

                delegates.Add(handler);

                DialogueEnder.OnDialogueResolving += handler;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="actor"></param>
        /// <returns></returns>
        protected virtual bool ExitProcessShouldBeCanceled(Actor actor)
        {
            bool result = actor == null || isLoadingFile.Value;

            if (actor == null)
                UnityEngine.Debug.LogWarning($"{ToString()}: REPORT: ExitChar was called but the actor was null or destroyed.");

            if (isLoadingFile.Value)
            {
                UnityEngine.Debug.Log($"{ToString()}: Loading File Skip Leaving: " + actor.DisplayName);

                ExitChar(actor);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void ExitChar(Actor actor)
        {
            UnityEngine.Debug.Log(ToString() + ": Exiting character: " + actor.gameObject.name);

            actor.transform.position = designatedExitPosition;
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => RemoveAllEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveAllEvents()
        {
            foreach (Action eh in delegates)
            {
                if (LineReader != null)
                    LineReader.OnLineStart -= eh;

                if (DialogueEnder != null)
                    DialogueEnder.OnDialogueResolving -= eh;
            }

            delegates.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("CharacterLeave");
    }
}