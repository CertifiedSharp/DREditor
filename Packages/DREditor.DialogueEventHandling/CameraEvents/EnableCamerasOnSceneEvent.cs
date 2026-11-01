//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class EnableCamerasOnSceneEvent : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<BoolWithEvent> enableCameras = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<BoolWithEvent> disableCameras = new();


        /// <inheritdoc/>
        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            UpdateCameras();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateCameras()
        {
            foreach (var camera in enableCameras)
                camera.SetValue(true);

            foreach (var camera in disableCameras)
                camera.SetValue(false);
        }
    }
}