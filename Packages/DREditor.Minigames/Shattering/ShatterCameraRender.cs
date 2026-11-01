//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
using DREditor.Cameras;
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ShatterCameraRender : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera shatterCamera;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;


        /// <summary>
        ///
        /// </summary>
        protected IShatterGraphic Shatter => GetComponentInChildren<IShatterGraphic>();

        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera TrialCamera =>
            FindAnyObjectByType<DRTrialCamera>().GetComponentInChildren<UnityEngine.Camera>();


        protected virtual void OnEnable()
        {
            Shatter?.OnPreShatter.AddListener(InitializeCamera);

            Shatter?.OnMeshShattering.AddListener(ResolveCamera);
        }

        protected virtual void OnDisable()
        {
            Shatter?.OnPreShatter.RemoveListener(InitializeCamera);

            Shatter?.OnMeshShattering.RemoveListener(ResolveCamera);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeCamera()
        {
            shatterCamera.transform
                .SetPositionAndRotation(TrialCamera.transform.position, TrialCamera.transform.rotation);

            Graphic shatterGraphic = Shatter as Graphic;

            shatterCamera.targetTexture = (RenderTexture)shatterGraphic.mainTexture;

            cameraEnabled.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveCamera()
        {
            shatterCamera.targetTexture = null;

            cameraEnabled.SetValue(false);
        }
    }
}