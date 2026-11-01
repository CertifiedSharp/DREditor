//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Minigames/Shattering/Split Mesh Generator", fileName = "RS Mesh Generator")]
    public class SplitShatter : MeshGenerator
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float forceMultiplier = 550;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector2 vertexOffset = new(500, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int triNum = 1;

        
        /// <inheritdoc/>
        public override void GenerateMesh(MeshFilter targetMeshFilter, List<UIVertex> uiVertices)
        {
            Mesh mesh = new()
            {
                vertices = ComputeVertices().ToArray()
            };

            mesh.uv = ComputeUVs(mesh.vertices.ToList(), uiVertices).ToArray();

            mesh.RecalculateBounds();

            targetMeshFilter.mesh = mesh;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override List<Vector3> ComputeVertices()
        {
            List<Vector3> vertices = base.ComputeVertices();

            vertices[8] = OffsetVertex(vertices[8], vertexOffset.x, vertexOffset.y);

            vertices[10] = OffsetVertex(vertices[10], vertexOffset.x, vertexOffset.y);

            vertices[11] = OffsetVertex(vertices[11], -vertexOffset.x, vertexOffset.y);

            // midpoint is (count / 2) - 1

            vertices[12] = OffsetVertex(vertices[12], -vertexOffset.x, vertexOffset.y);

            vertices[13] = OffsetVertex(vertices[13], vertexOffset.x, vertexOffset.y);

            vertices[15] = OffsetVertex(vertices[15], -vertexOffset.x, vertexOffset.y);

            return vertices;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        protected virtual Vector3 OffsetVertex(Vector3 position, float x, float y, float z = 0)
            => new(position.x + x, position.y + y, position.z + z);

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiVertices"></param>
        /// <returns></returns>
        public override Vector3[] InitializeVelocity(List<UIVertex> uiVertices)
        {
            Vector3[] triangleVelocity = base.InitializeVelocity(uiVertices);

            int counter = 0;

            Vector3 direction = Vector3.left;

            for (int i = 0; i < uiVertices.Count / 3; i++, counter++)
            {
                triangleVelocity[i] = direction * forceMultiplier;

                if (counter == triNum && i != (uiVertices.Count / 3) - 2)
                {
                    direction = direction == Vector3.left ? Vector3.right : Vector3.left;

                    counter = 0;
                }
            }

            return triangleVelocity;
        }
    }
}