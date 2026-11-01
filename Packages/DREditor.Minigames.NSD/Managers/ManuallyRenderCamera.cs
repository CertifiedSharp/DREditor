//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Minigames.NSD.Managers
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    [ClassHeader("Manually renders the camera through this monobehaviour." +
        "\nRequires Component: " + nameof(UnityEngine.Camera) +
        "\n\n")]
    public class ManuallyRenderCamera : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;


        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera renderCamera;


        protected virtual void Awake()
        {
            renderCamera = GetComponent<UnityEngine.Camera>();

            cameraEnabled.SetValue(false);
        }


        protected virtual void Update()
        {
            if (renderCamera != null)
                renderCamera.Render();
        }
    }
}