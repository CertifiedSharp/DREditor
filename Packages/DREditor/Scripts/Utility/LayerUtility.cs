//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class LayerUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static int GetFirst(this LayerMask mask)
        {
            for (int i = 0; i < 32; i++)
                if ((mask.value & (1 << i)) != 0)
                    return i;

            return -1;
        }
    }
}