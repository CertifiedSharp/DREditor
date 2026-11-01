//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
    public static class CameraPostProcessingExtentions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="color"></param>
        public static void ApplyColorNoAlpha(this Graphic graphic, Color color) 
            => graphic.color = new Color(color.r, color.g, color.b);

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="to"></param>
        public static void SetPostProcessingTo(this UnityEngine.Camera camera, bool to) 
            => camera.GetUniversalAdditionalCameraData().renderPostProcessing = to;

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsPostProcessingEnabled(this UnityEngine.Camera camera) 
            => camera.GetUniversalAdditionalCameraData().renderPostProcessing;

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static LayerMask GetVolumeMask(this UnityEngine.Camera camera) 
            => camera.GetUniversalAdditionalCameraData().volumeLayerMask;
    }
}