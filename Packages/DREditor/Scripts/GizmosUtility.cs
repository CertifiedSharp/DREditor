//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Utility class for drawing specific gizmos.
    /// </summary>
    public static class GizmosUtility
    {
        /// <summary>
        /// Draws a gizmo line from two positions and makes a gizmo sphere at each position.
        /// </summary>
        /// <param name="startPosition">The starting position of the line and the position of the first sphere.</param>
        /// <param name="endPosition">The ending position of the line and the position of the second sphere.</param>
        /// <param name="firstSphere">The color for the first sphere.</param>
        /// <param name="secondSphere">The color for the second sphere.</param>
        public static void DrawSphereLine(Vector3 startPosition, Vector3 endPosition, Color firstSphere, Color secondSphere)
        {
            Gizmos.color = firstSphere;

            Gizmos.DrawSphere(startPosition, GizmosSettings.Instance.gizmoSphereRadius);

            Gizmos.color = secondSphere;

            Gizmos.DrawSphere(endPosition, GizmosSettings.Instance.gizmoSphereRadius);

            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}