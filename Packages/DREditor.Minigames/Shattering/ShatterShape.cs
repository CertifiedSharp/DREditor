// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ShatterShape
    {
        /// <summary>
        ///
        /// </summary>
        protected Vector3 center;

        /// <summary>
        ///
        /// </summary>
        public Vector3 Center { get => center; set => center = value; }

        /// <summary>
        ///
        /// </summary>
        public readonly int[] vertexes;


        public ShatterShape(int vertexCount) => vertexes = new int[vertexCount];


        /// <summary>
        ///
        /// </summary>
        /// <param name="Offset"></param>
        /// <param name="verts"></param>
        /// <param name="frozenZ"></param>
        public virtual void Move(Vector3 Offset, Vector3[] verts, bool frozenZ)
        {
            Center += Offset;

            for (int i = 0; i < vertexes.Length; i++)
            {
                if (frozenZ)
                {
                    float z = verts[vertexes[i]].z;

                    verts[vertexes[i]] += Offset;

                    verts[vertexes[i]].z = z;
                }
                else
                    verts[vertexes[i]] += Offset;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="verts"></param>
        public virtual void Rotate(Vector3 offset, Vector3[] verts)
        {
            for (int i = 0; i < vertexes.Length; i++)
                verts[vertexes[i]] = RotateVertex(verts[vertexes[i]], offset);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="original"></param>
        /// <param name="rotationOffset"></param>
        /// <returns></returns>
        protected virtual Vector3 RotateVertex(Vector3 original, Vector3 rotationOffset)
        {
            Vector3 dir = original - Center;

            dir = Quaternion.Euler(rotationOffset) * dir;

            original = dir + Center;

            return original;
        }
    }
}