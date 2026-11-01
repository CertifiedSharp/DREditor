//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class SetDialogueCameraEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera dialogueCamera = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => dialogueCamera.transform.position = (Vector3)o;


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("SetDialogueCameraPosition");
    }
}