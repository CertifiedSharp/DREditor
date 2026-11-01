//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IToTitleScreen) +
        "\n\n")]
    public class TextureOnTitleScreen : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
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