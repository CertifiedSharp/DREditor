//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class GizmosUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        public static void DrawSwipeLineGizmo(Vector3 bottom, Vector3 top)
        {
            Gizmos.DrawLine(bottom, top);

            Gizmos.DrawSphere(top, GizmosSettings.Instance.gizmoSphereRadius);

            Gizmos.DrawSphere(bottom, GizmosSettings.Instance.gizmoSphereRadius);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        public static void DrawSphereLine(Vector3 startPosition, Vector3 endPosition)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawSphere(startPosition, GizmosSettings.Instance.gizmoSphereRadius);

            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(endPosition, GizmosSettings.Instance.gizmoSphereRadius);

            Gizmos.DrawLine(startPosition, endPosition);
        }
    }
}