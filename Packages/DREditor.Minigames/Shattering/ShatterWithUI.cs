//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ShatterWithUI : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera shatterCamera = null;

        /// <summary>
        ///
        /// </summary>
        protected IShatterGraphic shatterGraphic;

        /// <summary>
        ///
        /// </summary>
        protected IShatterableCanvas[] canvases;


        protected virtual void Awake()
        {
            shatterGraphic = GetComponentInChildren<IShatterGraphic>();

            shatterCamera = GetComponentInChildren<UnityEngine.Camera>();

            canvases = transform.root.GetComponentsInChildren<IShatterableCanvas>();
        }

        protected virtual void OnEnable()
        {
            shatterGraphic?.OnPreShatter.AddListener(CanvasesRenderCamera);

            shatterGraphic?.OnPostShatter.AddListener(CanvasesRenderOverlay);
        }

        protected virtual void OnDisable()
        {
            shatterGraphic?.OnPreShatter.RemoveListener(CanvasesRenderCamera);

            shatterGraphic?.OnPostShatter.RemoveListener(CanvasesRenderOverlay);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CanvasesRenderCamera()
        {
            foreach (var canvas in canvases)
                canvas?.SetRenderingCamera(shatterCamera);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CanvasesRenderOverlay()
        {
            foreach (var canvas in canvases)
                canvas?.SetRenderingOverlay();
        }
    }
}