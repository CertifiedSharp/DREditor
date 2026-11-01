//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class CamerasToTPFDPosition : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<UnityEngine.Camera> cameras = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int delayedFrames = 2;


        /// <summary>
        ///
        /// </summary>
        protected PnCCamera pnc;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => pnc = FindAnyObjectByType<PnCCamera>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => StartCoroutine(DelayCam(o));

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual IEnumerator DelayCam(object o = null)
        {
            for (int i = 0; i < delayedFrames; i++)
                yield return null;

            Vector3 position = pnc.GetCalculatedPosition();

            Quaternion rotation = pnc.GetCalculatedRotation();

            foreach (var camera in cameras)
                camera.transform.SetPositionAndRotation(position, rotation);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ToTPFDFromDialogue");

        protected virtual void Reset() => SetDefaultEventString("ToTPFDFromDialogue");
    }
}