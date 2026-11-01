//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color WithAlpha(this Color color, float alpha) => new(color.r, color.g, color.b, alpha);

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Invisible(this Color color) => WithAlpha(color, 0);
    }
}