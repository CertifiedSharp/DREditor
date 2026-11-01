//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Services;
using DREditor.Talkables;
using System.Linq;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class ActorCheckFocusEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            var actors = ServiceLocator.GetService<IObjectPooler<Talkable>>()
                .GetRegisteredObjects().OfType<Actor>().Where(actor => actor.GetComponentInChildren<SpriteRenderer>().enabled);

            foreach (Actor actor in actors)
                if (actor != null)
                    FixIncorrectlyFocusedActor(actor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void FixIncorrectlyFocusedActor(Actor actor)
        {
            var layerHandler = actor.GetComponent<LayerHandler>();

            layerHandler.ToggleRender();

            layerHandler.UnFocusActor();
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("CheckFocus");
    }
}