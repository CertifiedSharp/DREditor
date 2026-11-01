//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public abstract class MeshGenerator : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int horizontalResolution = 16;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int verticalResolution = 9;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float horizontalSize = 1920;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float verticleSize = 1080;


        /// <summary>
        ///
        /// </summary>
        protected float Width => horizontalSize / horizontalResolution;

        /// <summary>
        ///
        /// </summary>
        protected float Height => verticleSize / verticalResolution;


        /// <summary>
        ///
        /// </summary>
        /// <param name="targetMeshFilter"></param>
        /// <param name="uiVertices"></param>
        public abstract void GenerateMesh(MeshFilter targetMeshFilter, List<UIVertex> uiVertices);

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiVertices"></param>
        /// <returns></returns>
        public virtual Vector3[] InitializeVelocity(List<UIVertex> uiVertices) =>
            new Vector3[uiVertices.Count / 3];

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<Vector3> ComputeVertices()
        {
            List<Vector3> vertices = new();

            for (int x = 0; x < horizontalResolution; x++)
            {
                for (int y = 0; y < verticalResolution; y++)
                {
                    Vector3 offset = CalculateOffset(x, y);

                    Vector3[] quadVertices = GenerateQuad(Width, Height, offset);

                    vertices.AddRange(quadVertices);
                }
            }

            return vertices;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected virtual Vector3 CalculateOffset(float x, float y)
        {
            Vector3 initialOffset = new(Width * x, Height * y);

            Vector3 dividedSize = new(horizontalSize / 2, verticleSize / 2, 0);

            Vector3 dividedResolution = new(Width / 2, Height / 2, 0);

            return initialOffset - dividedSize + dividedResolution;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected virtual Vector3[] GenerateQuad(float width, float height, Vector3 offset)
        {
            float w2 = width / 2;

            float h2 = height / 2;

            Vector3[] verts = new Vector3[6];

            // Triangle 1
            verts[0] = new Vector3(-w2, -h2) + offset;

            verts[1] = new Vector3(-w2, h2) + offset;

            verts[2] = new Vector3(w2, h2) + offset;

            // Triangle 2
            verts[3] = new Vector3(-w2, -h2) + offset;

            verts[4] = new Vector3(w2, h2) + offset;

            verts[5] = new Vector3(w2, -h2) + offset;

            return verts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="computedVertices"></param>
        /// <param name="currentVertices"></param>
        /// <returns></returns>
        protected virtual List<Vector2> ComputeUVs(List<Vector3> computedVertices, List<UIVertex> currentVertices)
        {
            List<Vector2> uVs = new();

            for (int i = 0; i < computedVertices.Count; i++)
            {
                Vector3 a = computedVertices[i];

                Vector2 b = GetUV(computedVertices[i]);

                UIVertex uiv = UIVertex.simpleVert;

                uiv.position = a;

                uiv.uv0 = b;

                currentVertices.Add(uiv);

                uVs.Add(b);
            }

            return uVs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected virtual Vector2 GetUV(Vector2 position)
        {
            Vector2 offsetPosition = position + new Vector2(horizontalSize / 2f, verticleSize / 2f);

            return offsetPosition / new Vector2(horizontalSize, verticleSize);
        }
    }
}