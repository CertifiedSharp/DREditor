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
    [CreateAssetMenu(menuName = "DREditor/Minigames/Shattering/Bullet Shatter Mesh Generator", fileName = "NSD Mesh Generator")]
    public class BulletShatter : MeshGenerator
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float forceMultiplier = 550;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maxOffset = 0.36942f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float centerBreakFactor = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float initialExplosionRadius = 1200;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float explosionRadius = 960;


        /// <inheritdoc/>
        public override void GenerateMesh(MeshFilter targetMeshFilter, List<UIVertex> uiVertices)
        {
            Mesh mesh = new()
            {
                vertices = ComputeVertices().ToArray()
            };

            mesh.uv = ComputeUVs(mesh.vertices.ToList(), uiVertices).ToArray();

            // Set Triangles
            mesh.triangles = Enumerable.Range(0, verticalResolution * horizontalResolution * 6).ToArray();

            mesh.RecalculateBounds();

            targetMeshFilter.mesh = mesh;
        }

        /// <inheritdoc/>
        protected override List<Vector3> ComputeVertices()
        {
            List<Vector3> vertices = base.ComputeVertices();

            RandomizeMesh(vertices);

            return vertices;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertices"></param>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void RandomizeMesh(List<Vector3> vertices)
        {
            float averageSize = (Width + Height) / 2;

            float localMaxOffset = maxOffset * averageSize;

            int currentVertexIdx = 2;

            int columns = horizontalResolution - 1;

            int rows = verticalResolution - 1;

            int rowStepOffset = verticalResolution * 6;

            for (int i = 0; i < columns * rows; i++)
            {
                // The vertices that we are suppose to move at each intersection
                int[] sharedVertexIndices =
                {
                    currentVertexIdx,
                    currentVertexIdx + 2,
                    currentVertexIdx + 9,
                    currentVertexIdx + rowStepOffset - 1,
                    currentVertexIdx + rowStepOffset + 4,
                    currentVertexIdx + rowStepOffset + 7
                };

                Vector3 randomOffset = (Vector3)UnityEngine.Random.insideUnitCircle * localMaxOffset;

                foreach (int index in sharedVertexIndices)
                {
                    Vector3 newPos = vertices[index] + randomOffset;

                    vertices[index] = GetNewPosition(newPos);
                }

                // Jump to the next column or skip to the next row
                bool isEndOfRow = (i + 1) % rows == 0;

                if (isEndOfRow)
                    currentVertexIdx += 12; // Skip edge vertices to the next row
                else
                    currentVertexIdx += 6;  // Move to the next quad in the column
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected virtual Vector3 GetNewPosition(Vector3 position)
        {
            float distToCenter = Vector3.Distance(Vector3.zero, position);

            float multiplier = distToCenter / (explosionRadius / 2);

            return centerBreakFactor * multiplier * position;
        }

        /// <inheritdoc/>
        public override Vector3[] InitializeVelocity(List<UIVertex> uiVertices)
        {
            Vector3[] triangleVelocity = base.InitializeVelocity(uiVertices);

            for (int i = 0; i < uiVertices.Count / 3; i++)
                triangleVelocity[i] = CalculateTriVelocity(i, uiVertices);

            return triangleVelocity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="uiVertices"></param>
        /// <returns></returns>
        protected virtual Vector3 CalculateTriVelocity(int i, List<UIVertex> uiVertices)
        {
            Vector3 a = uiVertices[i * 3].position;

            Vector3 b = uiVertices[i * 3 + 1].position;

            Vector3 c = uiVertices[i * 3 + 2].position;

            Vector3 center = (a + b + c) / 3f;

            Vector3 direction = center.normalized;

            float t = center.magnitude / initialExplosionRadius;

            t = 1 - t;

            return forceMultiplier * t * direction;
        }
    }
}