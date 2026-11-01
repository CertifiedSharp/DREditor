//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Rendering.Universal;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PostProcessState : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [FormerlySerializedAs("id")]
        public string cameraObjectName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public LayerMask volumeMask;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool enabled;


        public PostProcessState() { }

        public virtual PostProcessState RecordState(UnityEngine.Camera camera)
        {
            cameraObjectName = camera.gameObject.name;

            enabled = camera.IsPostProcessingEnabled();

            volumeMask = camera.GetVolumeMask();

            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enabled"></param>
        public virtual void SetEnabled(bool enabled) => this.enabled = enabled;

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        public virtual void ApplyState(UnityEngine.Camera camera)
        {
            camera.SetPostProcessingTo(enabled);

            camera.GetUniversalAdditionalCameraData().volumeLayerMask = volumeMask;
        }
    }
}