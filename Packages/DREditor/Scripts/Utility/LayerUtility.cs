//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Utility class for Layering related functionality or LayerMasks.
    /// </summary>
    public static class LayerUtility
    {
        /// <summary>
        /// The max amount of layers unity can allow. (To our knowledge.)
        /// </summary>
        private static readonly int UnityMaxLayers = 32;


        /// <summary>
        /// Gets the first outlined layer in the mask.
        /// </summary>
        /// <param name="mask">The LayerMask being filtered.</param>
        /// <returns>Integer for the first outlined layer in the mask.</returns>
        public static int GetFirst(this LayerMask mask)
        {
            for (int i = 0; i < UnityMaxLayers; i++)
                if ((mask.value & (1 << i)) != 0)
                    return i;

            return -1;
        }

        /// <summary>
        /// Checks whether this mask contains the specified layer.
        /// </summary>
        /// <param name="mask">The target mask to filter through.</param>
        /// <param name="layer">The target layer being looked for.</param>
        /// <returns>True if this mask contains the specified layer.</returns>
        public static bool Contains(this LayerMask mask, int layer) => (mask.value & (1 << layer)) != 0;
    }
}