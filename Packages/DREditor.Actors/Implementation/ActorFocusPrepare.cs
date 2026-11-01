//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPreparableDialogueComponent) +
        "\n\n")]
    public class ActorFocusPrepare : DialogueComponent, IPreparableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float prepareOrder = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent lastActor = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string panToTalkableEventString = "PanToTalkable";


        public float Order { get => prepareOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToPrepare() => false;

        /// <inheritdoc/>
        public virtual void Prepare() => PrepareFocusing();

        /// <inheritdoc/>
        public virtual IEnumerator PrepareRoutine()
        {
            Prepare();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareFocusing()
        {
            Talkable selectedObject = ServiceLocator.GetService<DialogueObjectManager>().GetStartObject() as Talkable;

            if (selectedObject != null && selectedObject is Actor actor && actor != null)
                actor.GetComponent<LayerHandler>().ToggleRender();

            PanToFirstObjectInDialogue(selectedObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedObject"></param>
        protected virtual void PanToFirstObjectInDialogue(Talkable selectedObject)
        {
            if (selectedObject != null)
                if (selectedObject is Actor actor)
                    PanToFirstActorInDialogue(actor);
                else if(selectedObject is Talkable item)
                    DialogueEventSystem.TriggerEvent(panToTalkableEventString, item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void PanToFirstActorInDialogue(Actor actor)
        {
            if (!isLoadingFile.Value)
                DialogueEventSystem.TriggerEvent(panToTalkableEventString, actor);

            if (actor != null)
                actor.GetComponent<LayerHandler>().FocusActor();

            lastActor.SetValue(actor);
        }
    }
}