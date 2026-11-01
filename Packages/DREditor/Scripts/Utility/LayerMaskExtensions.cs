//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class LayerMaskExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer) => (mask.value & (1 << layer)) != 0;
    }
}