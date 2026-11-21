//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public readonly struct RaycastHitComparer : IComparer<RaycastHit>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly IComparer<RaycastHit> Instance = new RaycastHitComparer();


        /// <summary>
        ///
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>
        ///
        /// </returns>
        public readonly int Compare(RaycastHit first, RaycastHit second) => first.distance.CompareTo(second.distance);
    }
}