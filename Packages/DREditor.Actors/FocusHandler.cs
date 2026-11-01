//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
	[ClassHeader("")]
    public class FocusHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string checkFocusEventString = "CheckFocus";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string panToTalkableEventString = "PanToTalkable";


        /// <summary>
        ///
        /// </summary>
        /// <param name="newActor"></param>
        /// <param name="oldActor"></param>
        public virtual void ChangeFocus(SelectableObject newActor, SelectableObject oldActor)
        {
            if (newActor == null || !newActor.TryGetComponent(out LayerHandler handler))
                return;

            if (!isLoadingFile.Value)
                DialogueEventSystem.TriggerEvent(panToTalkableEventString, newActor);

            if (newActor != oldActor)
                SwapActorFocus(handler, oldActor);
            else if (!newActor.GetComponentInChildren<SpriteRenderer>().enabled)
                Focus(handler);

            newActor.GetComponent<LayerHandler>().CheckSpriteLayer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newActor"></param>
        /// <param name="oldActor"></param>
        protected virtual void SwapActorFocus(LayerHandler newActor, SelectableObject oldActor)
        {
            DialogueEventSystem.TriggerEvent(checkFocusEventString);

            Focus(newActor);

            UnFocus(oldActor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="handler"></param>
        protected virtual void Focus(LayerHandler handler)
        {
            handler.ToggleRender();

            handler.FocusActor();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void UnFocus(SelectableObject actor)
        {
            if (actor == null || !actor.TryGetComponent(out LayerHandler handler))
                return;

            if (actor.GetComponentInChildren<SpriteRenderer>() is SpriteRenderer sprite && sprite.enabled)
                handler.ToggleRender();

            handler.UnFocusActor();
        }
    }
}