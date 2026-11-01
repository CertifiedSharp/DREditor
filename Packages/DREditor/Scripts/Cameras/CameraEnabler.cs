//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Cameras
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
	[ClassHeader("")]
    public class CameraEnabler : MonoBehaviour, ICameraEnabler
    {
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;

        [Tooltip("")]
        [SerializeField] protected bool disableOnDestroy = false;


        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera cam;


        protected virtual void Awake() => cam = GetComponent<UnityEngine.Camera>();

        protected virtual void OnEnable() => cameraEnabled.AddListener(SetCameraEnabled);

        protected virtual void OnDisable() => cameraEnabled.RemoveListener(SetCameraEnabled);

        /// <summary>
        ///
        /// </summary>
        /// <param name="enabled"></param>
        protected virtual void SetCameraEnabled(bool enabled) => cam.enabled = enabled;
        

        public virtual void InvokeCameraEnabled(bool enabled) => cameraEnabled.SetValue(enabled);


        protected virtual void OnDestroy()
        {
            if (disableOnDestroy)
                cameraEnabled.SetValue(false);
        }
    }
}