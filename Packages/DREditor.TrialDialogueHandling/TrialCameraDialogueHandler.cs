//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Dialogues;
using System.Linq;
using UnityEngine;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    public class TrialCameraDialogueHandler : DialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected DRTrialCamera trialCamera = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => InitializeTrialCamera();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTrialCamera()
        {
            if (trialCamera == null)
            {
                var localCameras = transform.root.GetComponentsInChildren<DRTrialCamera>();

                var globalCamera = FindObjectsByType<DRTrialCamera>()
                    .Where(camera => !localCameras.Contains(camera))
                    .FirstOrDefault();

                if (globalCamera != null)
                    trialCamera = globalCamera;
            }
        }
    }
}