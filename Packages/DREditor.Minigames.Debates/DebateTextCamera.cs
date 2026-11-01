//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class DebateTextCamera : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDebate = null;


        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera textCamera = null;

        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera trialCamera;


        protected virtual void Awake()
        {
            textCamera = GetComponent<UnityEngine.Camera>();

            trialCamera = FindAnyObjectByType<DRTrialCamera>().GetComponentInChildren<UnityEngine.Camera>();
        }

        protected virtual void Update()
        {
            if (inDebate.Value)
                CamUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CamUpdate()
        {
            textCamera.transform
                .SetPositionAndRotation(trialCamera.transform.position, trialCamera.transform.rotation);

            textCamera.fieldOfView = trialCamera.fieldOfView;
        }
    }
}