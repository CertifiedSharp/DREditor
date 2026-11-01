//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    public interface IShatterableCanvas
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="shatterCamera"></param>
        void SetRenderingCamera(UnityEngine.Camera shatterCamera);

        /// <summary>
        ///
        /// </summary>
        void SetRenderingOverlay();
    }
}