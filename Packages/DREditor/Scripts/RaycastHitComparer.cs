//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// A struct comparer that sorts <see cref="RaycastHit"/> arrays by distance.
    /// </summary>
    /// <remarks>
    /// This was made to help the NSD Reticle sort Physics.RaycastNonAlloc hits by distance.
    /// </remarks>
    public readonly struct RaycastHitComparer : IComparer<RaycastHit>
    {
        /// <summary>
        /// Insance for this comparer, only cached once so calling it on update doesn't make everything blow up.
        /// </summary>
        public static readonly IComparer<RaycastHit> Instance = new RaycastHitComparer();


        /// <summary>
        /// Compares <see cref="RaycastHit"/> instances based on distance.
        /// </summary>
        /// <param name="first">The first of the raycasts to compare.</param>
        /// <param name="second">The second of the raycasts to compare.</param>
        /// <returns>
        /// An integer that represents the values of <paramref name="first"/> and <paramref name="second"/>'s distances:
        /// <list type="bullet">
        /// <item>Less than zero: <paramref name="first"/> is closer compared to <paramref name="second"/>.</item>
        /// <item>Zero: <paramref name="first"/> and <paramref name="second"/> are the same distance apart.</item>
        /// <item>Greater than zero: <paramref name="first"/> is farther away than <paramref name="second"/>.</item>
        /// </list>
        /// </returns>
        public readonly int Compare(RaycastHit first, RaycastHit second) => first.distance.CompareTo(second.distance);
    }
}