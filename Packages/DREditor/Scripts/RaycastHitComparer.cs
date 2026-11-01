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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>
        ///
        /// </returns>
        public readonly int Compare(RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance);
    }
}