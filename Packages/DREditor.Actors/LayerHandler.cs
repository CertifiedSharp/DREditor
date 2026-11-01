//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    public abstract class LayerHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        public abstract void ToggleRender();

        /// <summary>
        ///
        /// </summary>
        public abstract void CheckSpriteLayer();

        /// <summary>
        ///
        /// </summary>
        public abstract void FocusActor();

        /// <summary>
        ///
        /// </summary>
        public abstract void UnFocusActor();
    }
}