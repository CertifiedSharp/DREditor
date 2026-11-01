//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ActorFadeEvent) + ", " + nameof(DialogueEventHandler) +
        "\n\n")]
    public class ActorFadeHandler : ActorFadeEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent lastActor = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (o is Actor actorToFade)
                EvaluateSentActor(actorToFade);
            else if (o != null)
                UnityEngine.Debug.LogWarning($"Sent object was not of type {typeof(Actor)}. Was type: " + o.GetType().ToString());
            else
                EvaluateSentActor(null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actorToFade"></param>
        protected virtual void EvaluateSentActor(Actor actorToFade)
        {
            if (actorToFade == null)
                actorToFade = (Actor)lastActor.Value;

            spriteFader.ActorDisappear(actorToFade);
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ActorFade");
    }
}