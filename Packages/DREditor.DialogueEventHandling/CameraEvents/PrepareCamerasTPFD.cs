//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class PrepareCamerasTPFD : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera mainCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PnCCamera pnc = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int delayedFrames = 2;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            pnc.Controllable = true;

            StartCoroutine(DelayCam(o));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual IEnumerator DelayCam(object o = null)
        {
            for (int i = 0; i < delayedFrames; i++)
                yield return null;

            mainCamera.transform.rotation = pnc.GetCalculatedRotation();

            if ((bool)o)
                mainCamera.transform.position = pnc.GetCalculatedPosition();
            else
                mainCamera.transform.position = pnc.GetCalculatedPosition(true);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("PrepareCamerasTPFD");
    }
}