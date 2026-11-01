//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    [AddComponentMenu("DREditor/Actors/DREditor/Actor Body Debugger")]
    public class ActorBodyDebugger : ServiceHolder<ActorBodyDebugger>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugBody = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useKeyBind = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputAction action = null;

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
        [SerializeField] protected string setCameraPositionEventString = "SetDialogueCameraPosition";


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (useKeyBind)
            {
                action.started += ToggleDebugBody;

                action.Enable();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (useKeyBind)
            {
                action.started -= ToggleDebugBody;

                action.Disable();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void ToggleDebugBody(CallbackContext ctx) => debugBody = !debugBody;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Update() => CheckDebugBody();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckDebugBody()
        {
            if (debugBody)
                MoveDialogueCameraToBodyPosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveDialogueCameraToBodyPosition()
        {
            if (currentActor.Value != null)
                DialogueEventSystem.TriggerEvent(setCameraPositionEventString, ((Actor)currentActor.Value).body.position, false);
            else if (lastActor.Value != null)
                DialogueEventSystem.TriggerEvent(setCameraPositionEventString, ((Actor)lastActor.Value).body.position, false);
        }
    }
}