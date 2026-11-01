// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ShatterTri : ShatterShape
    {
        public ShatterTri() : base(3) { }

        public ShatterTri(int vertIdx, Vector3[] newVertices, int[] pos) : base(3)
        {
            // Center in object space.
            Center = (newVertices[vertIdx + pos[0]] + newVertices[vertIdx + pos[1]] + newVertices[vertIdx + pos[2]]) / 3;

            // Triangle order (clockwise).
            vertexes[0] = vertIdx + pos[0];

            vertexes[1] = vertIdx + pos[1];

            vertexes[2] = vertIdx + pos[2];
        }
    }
}