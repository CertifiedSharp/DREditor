//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    public class CaptureFrameEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera eventCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            bool cameraState = cameraEnabled.Value;

            if (!cameraEnabled.Value)
            {
                UnityEngine.Debug.Log($"REPORT: PlayerManager: eventCamera: {eventCamera.gameObject.name} wasn't on before capturing.");

                cameraEnabled.SetValue(true);
            }

            RenderTexture rt = new(Screen.width, Screen.height, 32);

            ((RawImage)o).texture = rt;

            eventCamera.targetTexture = rt;

            eventCamera.Render();

            eventCamera.targetTexture = null;

            cameraEnabled.SetValue(cameraState);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("CaptureMainCamera"); //CaptureProtagCamera
    }
}