//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UISpriteHandling
{
    /// <summary>
    ///
    /// </summary>
    public static class SpriteChangeUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="texture"></param>
        public static void SizeDeltaToTextureSize(this RectTransform rectTransform, Texture texture)
            => rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
    }
}