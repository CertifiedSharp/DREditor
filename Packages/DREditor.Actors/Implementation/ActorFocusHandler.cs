//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessorRoutine) +
        "\n\n")]
    public class ActorFocusHandler : DialogueComponent, ILineProcessorRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
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
        [SerializeField] protected BoolWithEvent inCGMode = null;

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
        [SerializeField] protected BoolWithEvent inVideoMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float characterPanWaitTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int fastForwardBufferedFrames = 5;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FocusHandler focusHandler = null;


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine) 
            => currentLine.ContainsEventType<ShakeObject>() || currentLine.PanToChar || ShouldUsePanBufferTime(currentLine);

        /// <inheritdoc/>
        public virtual IEnumerator LineProcessRoutine(Line currentLine) => EvaluateCameraMovement(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator EvaluateCameraMovement(Line currentLine)
        {
            if (currentLine.PanToChar)
                yield return PanToCharacterEvent(currentLine);
            else if (ShouldUsePanBufferTime(currentLine))
                yield return FocusCharacter();

            yield return InvokeCameraDialogueEvent<ShakeObject>(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator PanToCharacterEvent(Line currentLine)
        {
            Actor actorPan = (Actor)ServiceLocator.GetService<IObjectPooler<Talkable>>()
                .GetObjectByName(currentLine.CharToPan.FirstName);

            if (!actorPan)
                UnityEngine.Debug.LogWarning(ToString() + ": ALERT: PAN TO CHARACTER ACTOR WAS NOT FOUND IN THE SCENE");

            focusHandler.ChangeFocus(actorPan, (Actor)lastActor.Value);

            lastActor.SetValue(actorPan);

            yield return new WaitForSeconds(characterPanWaitTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual bool ShouldUsePanBufferTime(Line currentLine)
        {
            bool notInCGState = !inVideoMode.Value && !inCGMode.Value;

            bool currentActorIsValid = (Actor)currentActor.Value != null;

            bool lastActorIsntCurrent = (Actor)lastActor.Value != (Actor)currentActor.Value;

            bool protagIsntSpeaking = !isProtagSpeaking.Value;

            //bool actorIsntLeaving = !currentLine.Leave; Leave is removed so needs testing.

            bool validActorStates = currentActorIsValid && lastActorIsntCurrent;

            bool validStates = validActorStates && notInCGState && protagIsntSpeaking;// && actorIsntLeaving;

            bool lineValidStates = !currentLine.PanToChar && !currentLine.DontPan;

            return lineValidStates && validStates;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FocusCharacter()
        {
            if (isLoadingFile.Value)
                yield return new WaitForSeconds(Time.unscaledDeltaTime);

            /* The Above had to be added because for some reason loading on the line to kai before the
             * tree room shakes : it'd put the camera on the wrong angle
             * D_Ch0_Inv_ToTrial_AfterStatue Line before tree vid
             */

            focusHandler.ChangeFocus((Actor)currentActor.Value, (Actor)lastActor.Value);

            yield return new WaitForSeconds(Time.unscaledDeltaTime);

            if (inFastForwardMode.Value)
                yield return new WaitForSeconds(Time.unscaledDeltaTime * fastForwardBufferedFrames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator InvokeCameraDialogueEvent<T>(Line currentLine)
        {
            var cameraEvents = currentLine.DiaEvents.Where(dialogueEvent => dialogueEvent.GetType() == typeof(T));

            if (cameraEvents.Count() > 0 && !inFastForwardMode.Value)
            {
                if (ShouldUsePanBufferTime(currentLine))
                    yield return new WaitForSeconds(characterPanWaitTime);

                foreach (IDialogueEvent cameraEvent in cameraEvents)
                    cameraEvent.TriggerDialogueEvent();
            }
        }
    }
}