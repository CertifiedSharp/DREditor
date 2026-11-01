//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class MoveCameraToCamera : DialogueEventHandler, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent resetPlayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera mainCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera dialogueCamera = null;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            resetPlayer.RegisterListener(this);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            resetPlayer.UnregisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
            => dialogueCamera.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);

        /// <inheritdoc/>
        public virtual void OnEventRaised() => TriggerEvent();


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("DialogueCameraToMainCamera");

        protected virtual void Reset() => SetDefaultEventString("DialogueCameraToMainCamera");
    }
}