// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Shattering/Quad Shatter", fileName = "Quad Shatter")]
    public class ShatterQuadProcess : ShatterProcess<ShatterQuad>
    {
        /// <inheritdoc/>
        protected override void ShapeAngulate(Mesh mesh)
        {
            InitializeArrays(mesh);

            InitializeShapes();

            ClearEmptyVerticies();

            mesh.RecalculateBounds();

            mesh.RecalculateNormals();
        }

        /// <inheritdoc/>
        protected override void InitializeArrays(Mesh mesh)
        {
            int triCount = mesh.triangles.Length / 3 - 2;

            vertexColors = mesh.colors;

            shapes = new ShatterQuad[triCount];

            newVertices = mesh.vertices;

            base.InitializeArrays(mesh);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeShapes()
        {
            for (int i = 0; i < shapes.Length; i++)
                shapes[i] = new(i * 4, newVertices);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearEmptyVerticies()
        {
            List<ShatterQuad> newShapes = new();

            for (int i = 0; i < shapes.Length; i++)
                if (!shapes[i].IsEmpty())
                    newShapes.Add(shapes[i]);

            shapes = newShapes.ToArray();
        }
    }
}