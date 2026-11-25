//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Utility
{
    /// <summary>
    /// Utility class for manipulating colors, images, and graphics.
    /// </summary>
    public static class ImageUtility
    {
        /// <summary>
        /// Sets the alpha value on the color of the Graphic.
        /// </summary>
        /// <param name="graphic">The target graphic to apply the alpha value to.</param>
        /// <param name="alpha">The new alpha value to apply to the graphic.</param>
        public static void SetAlpha(this Graphic graphic, float alpha) => 
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);

        /// <summary>
        /// Sets the color an Image, ignoring the alpha value.
        /// </summary>
        /// <param name="image">The target image to apply the color to.</param>
        /// <param name="color">The new color to apply to the image.</param>
        public static void SetColor(this Image image, Color color) => 
            image.color = new Color(color.r, color.g, color.b, image.color.a);

        /// <summary>
        /// Sets the colors alpha to <paramref name="alpha"/>.
        /// </summary>
        /// <param name="color">The color to have it's alpha changed.</param>
        /// <param name="alpha">The level of visibility to display the color at.</param>
        /// <returns>The color, except with it's alpha at the value sent.</returns>
        public static Color WithAlpha(this Color color, float alpha) => new(color.r, color.g, color.b, alpha);

        /// <summary>
        /// Sets the colors alpha to Zero. (Not Visible)
        /// </summary>
        /// <param name="color">The color to set it's alpha to Zero.</param>
        /// <returns>The color, except with it's alpha at Zero. (Not Visible)</returns>
        public static Color Invisible(this Color color) => WithAlpha(color, 0);

        /// <summary>
        /// Sets an multiple image's GameObject active state based on whether the image's sprite is filled.
        /// </summary>
        /// <param name="images">The target images whose GameObject active state will be manipulated.</param>
        public static void SetActiveBySprite(params Image[] images)
        {
            foreach (var image in images)
                image.SetActiveBySprite();
        }

        /// <summary>
        /// Sets an image's GameObject active state based on whether the image's sprite is filled.
        /// </summary>
        /// <param name="image">The target image whose GameObject active state will be manipulated.</param>
        private static void SetActiveBySprite(this Image image)
        {
            if (image != null)
                image.gameObject.SetActive(image.sprite != null);
        }
    }
}