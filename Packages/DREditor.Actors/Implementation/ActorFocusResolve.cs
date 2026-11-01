//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class ActorFocusResolve : DialogueComponent, IResolvableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveOrder = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent lastActor = null;


        public float Order { get => resolveOrder; }


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => false;

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            Resolve();

            yield break;
        }

        /// <inheritdoc/>
        public virtual void Resolve()
        {
            SetActorToThreeDState((Actor)lastActor.Value);

            if (ServiceLocator.GetService<DialogueObjectManager>().GetStartObject() is Actor actor)
                SetActorToThreeDState(actor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void SetActorToThreeDState(Actor actor)
        {
            if (actor != null && !actor.GetComponentInChildren<MeshRenderer>().enabled)
            {
                actor.GetComponent<LayerHandler>().UnFocusActor();

                actor.GetComponent<LayerHandler>().ToggleRender();
            }
        }
    }
}