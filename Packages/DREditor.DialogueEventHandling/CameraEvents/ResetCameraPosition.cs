//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class ResetCameraPosition : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera cameraToReset = null;


        /// <summary>
        ///
        /// </summary>
        protected Vector3 ogPos;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 ogRot;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            ogPos = cameraToReset.transform.localPosition;

            ogRot = cameraToReset.transform.localEulerAngles;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            cameraToReset.transform.localPosition = ogPos;

            cameraToReset.transform.localEulerAngles = ogRot;
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ResetProtagCamera");

        protected virtual void Reset() => SetDefaultEventString("ResetProtagCamera");
    }
}