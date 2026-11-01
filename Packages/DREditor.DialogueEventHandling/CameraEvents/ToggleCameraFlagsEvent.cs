//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    [ClassHeader(nameof(DialogueEventHandler) +
        "\nRequires component: " + nameof(UnityEngine.Camera) +
        "\n\n")]
    public class ToggleCameraFlagsEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CameraClearFlags toggledFlags = CameraClearFlags.SolidColor;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CameraClearFlags defaultFlags = CameraClearFlags.Skybox;

        
        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera eventCamera = null;


        protected virtual void Awake() => eventCamera = GetComponent<UnityEngine.Camera>();


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if ((bool)o)
                eventCamera.clearFlags = toggledFlags;
            else
                eventCamera.clearFlags = defaultFlags;
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ToggleCameraFlags");
    }
}