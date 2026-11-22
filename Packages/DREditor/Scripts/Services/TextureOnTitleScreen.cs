//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    public class TextureOnTitleScreen : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header(nameof(IToTitleScreen) +
            "\n\n")]
        [Tooltip("")]
        [SerializeField] protected RawImage rawImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Texture2D textureOnTitleScreen = null;


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => rawImage.texture = textureOnTitleScreen;
    }
}