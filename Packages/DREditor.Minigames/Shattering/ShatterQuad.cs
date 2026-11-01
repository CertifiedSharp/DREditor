// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ShatterQuad : ShatterShape
    {
        public ShatterQuad() : base(4) { }

        public ShatterQuad(int vertIdx, Vector3[] newVertices) : base(4)
        {
            try
            {
                // Center in object space.
                Center = (newVertices[vertIdx] + newVertices[vertIdx + 1] + newVertices[vertIdx + 2] + newVertices[vertIdx + 3]) / 4;

                vertexes[0] = vertIdx;

                vertexes[1] = vertIdx + 1;

                vertexes[2] = vertIdx + 2;

                vertexes[3] = vertIdx + 3;
            }
            catch
            {
                Center = Vector3.zero;

                vertexes[0] = 0;

                vertexes[1] = 0;

                vertexes[2] = 0;

                vertexes[3] = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty()
        {
            foreach (int vertex in vertexes)
                if (vertex != 0)
                    return false;

            return true;
        }
    }
}