//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IShatterableCanvas) +
        "\n\n")]
    [RequireComponent(typeof(Canvas))]
    public class RenderCanvasOnShatter : MonoBehaviour, IShatterableCanvas
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool disableCanvasOnOverlay = true;


        /// <summary>
        ///
        /// </summary>
        protected Canvas canvas;


        protected virtual void Start() => canvas = GetComponent<Canvas>();

        /// <inheritdoc/>
        public virtual void SetRenderingCamera(UnityEngine.Camera shatterCamera)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            canvas.worldCamera = shatterCamera;
        }

        /// <inheritdoc/>
        public virtual void SetRenderingOverlay()
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvas.worldCamera = null;

            if (disableCanvasOnOverlay)
                canvas.enabled = false;
        }
    }
}