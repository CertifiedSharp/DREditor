// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Shattering/Triangle Shatter", fileName = "Triangle Shatter")]
    public class ShatterTriProcess : ShatterProcess<ShatterTri>
    {
        /// <summary>
        ///
        /// </summary>
        protected int pos1, pos2, pos3, offset1, offset2;

        /// <summary>
        ///
        /// </summary>
        protected int originalVertIdx = 0;

        /// <summary>
        ///
        /// </summary>
        protected int vertIdx = 0;

        /// <summary>
        ///
        /// </summary>
        protected Mesh mesh;


        /// <inheritdoc/>
        protected override void InitializeArrays(Mesh mesh)
        {
            int triCount = mesh.triangles.Length / 3 - 2;

            int newVertexCount = triCount * 3;

            shapes = new ShatterTri[triCount];

            vertexColors = new Color[newVertexCount];

            newVertices = new Vector3[newVertexCount];

            base.InitializeArrays(mesh);
        }

        /// <inheritdoc/>
        protected override void ShapeAngulate(Mesh mesh)
        {
            this.mesh = mesh;

            InitializeArrays(mesh);

            CreateTris();

            ApplyArraysToMesh();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateTris()
        {
            int triCount = mesh.triangles.Length / 3 - 2;

            int triPairs = triCount / 2;

            originalVertIdx = 0;

            vertIdx = 0;

            int triIdx = 0;

            for (int i = 0; i < triPairs; i++)
            {
                shapes[triIdx] = CreateTri(true);

                shapes[triIdx + 1] = CreateTri(false);

                originalVertIdx += 4;

                vertIdx += 6;

                triIdx += 2;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isLeft"></param>
        /// <returns></returns>
        protected virtual ShatterTri CreateTri(bool isLeft)
        {
            InitializeTriPositions(isLeft);

            UpdateArray(newVertices, mesh.vertices);

            UpdateArray(vertexColors, mesh.colors);

            UpdateArray(newUVs[0], mesh.uv);

            UpdateArray(newUVs[1], mesh.uv2);

            InitializeNewTris();

            return new(vertIdx, newVertices, new int[3] { pos1, pos2, pos3 });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isLeft"></param>
        protected virtual void InitializeTriPositions(bool isLeft)
        {
            int positionIndexOffset = isLeft ? 0 : 3;

            int offsetOffset = isLeft ? 0 : 1;

            pos1 = 0 + positionIndexOffset;

            pos2 = 1 + positionIndexOffset;

            pos3 = 2 + positionIndexOffset;

            offset1 = 1 + offsetOffset;

            offset2 = 2 + offsetOffset;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="get"></param>
        protected virtual void UpdateArray<T>(T[] set, T[] get)
        {
            set[vertIdx + pos1] = get[originalVertIdx];

            set[vertIdx + pos2] = get[originalVertIdx + offset1];

            set[vertIdx + pos3] = get[originalVertIdx + offset2];
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeNewTris()
        {
            newTriangles[vertIdx + pos1] = vertIdx + pos1;

            newTriangles[vertIdx + pos2] = vertIdx + pos2;

            newTriangles[vertIdx + pos3] = vertIdx + pos3;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyArraysToMesh()
        {
            mesh.vertices = newVertices;

            mesh.triangles = newTriangles;

            mesh.uv = newUVs[0];

            mesh.uv2 = newUVs[1];

            mesh.RecalculateBounds();

            mesh.RecalculateNormals();
        }
    }
}