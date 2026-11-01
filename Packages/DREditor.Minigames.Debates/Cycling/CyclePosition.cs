//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class CyclePosition : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float gizmoSphereRadius = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Transform> subPositions = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<Vector3> GetSubPositions() => subPositions.Select(transform => transform.position).ToList();


        protected virtual void OnDrawGizmos()
        {
            if (transform.position == Vector3.zero)
                return;

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position, gizmoSphereRadius);

            var subPositions = GetSubPositions();

            for (int i = 0; i < subPositions.Count; i++)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawSphere(subPositions[i], gizmoSphereRadius);

                if (i == 0)
                    Gizmos.DrawLine(transform.position, subPositions[i]);
                else if (i + 1 != subPositions.Count)
                    Gizmos.DrawLine(subPositions[i], subPositions[i + 1]);
                else if (i == subPositions.Count - 1)
                    Gizmos.DrawLine(subPositions[i], subPositions[i - 1]);
            }
        }
    }
}