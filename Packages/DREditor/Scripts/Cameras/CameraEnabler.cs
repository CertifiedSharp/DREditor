//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Cameras
{
    /// <summary>
    /// Manages keeping track of enabling or disabling cameras through events.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraEnabler : MonoBehaviour, ICameraEnabler
    {
        [Header(nameof(ICameraEnabler) +
            "\n\nManages keeping track of enabling or disabling cameras through events.")]
        [Space]
        [Tooltip("Event asset to represent the currently enabled state of the camera.")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;

        [Tooltip("When true sets the bool with event to false when this game object is destroyed." +
           "\nANALYSIS: This should never be checked on the Player System due to RTMM.")]
        [SerializeField] protected bool disableOnDestroy = false;


        /// <summary>
        /// The camera having it's enabled state controlled by this script and corresponding event.
        /// </summary>
        protected Camera cam;


        protected virtual void Awake() => cam = GetComponent<Camera>();

        protected virtual void OnEnable() => cameraEnabled.AddListener(SetCameraEnabled);

        protected virtual void OnDisable() => cameraEnabled.RemoveListener(SetCameraEnabled);

        /// <summary>
        /// Sets the camera script to the sent state.
        /// </summary>
        /// <param name="enabled">The state to apply to the cameras behaviour script.</param>
        protected virtual void SetCameraEnabled(bool enabled) => cam.enabled = enabled;


        /// <inheritdoc/>
        public virtual void InvokeCameraEnabled(bool enabled) => cameraEnabled.SetValue(enabled);


        protected virtual void OnDestroy()
        {
            if (disableOnDestroy)
                cameraEnabled.SetValue(false);
        }
    }
}