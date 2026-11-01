//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ShatterGraphic) + ", " + nameof(Graphic) + ", " + nameof(IShatterGraphic) +
        "\n\n")]
    public partial class ShatterMeshGraphic : ShatterGraphic
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MeshFilter targetMeshFilter;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Mesh resetMesh = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float explosionTime = 7f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MeshGenerator meshGenerator = null;


        /// <summary>
        ///
        /// </summary>
        protected List<UIVertex> uiVertices = new();

        /// <summary>
        ///
        /// </summary>
        protected Vector3[] triangleVelocity;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            if (debugMode)
                DebugShatter();
        }


        /// <inheritdoc/>
        protected override void PostShatter()
        {
            base.PostShatter();

            uiVertices.Clear();
        }

        /// <inheritdoc/>
        protected override void ShatterMesh()
        {
            meshGenerator.GenerateMesh(targetMeshFilter, uiVertices);

            base.ShatterMesh();
        }

        /// <inheritdoc/>
        protected override IEnumerator ShatterMeshProcess()
        {
            triangleVelocity = meshGenerator.InitializeVelocity(uiVertices);

            yield return ShatteringMesh();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShatteringMesh()
        {
            float time = 0;

            while (time <= explosionTime)
            {
                time += Time.deltaTime;

                UpdateShatter();

                yield return null;

                SetVerticesDirty();

                if (!Application.isPlaying)
                    ResetProcess();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateShatter()
        {
            for (int i = 0; i < uiVertices.Count / 3; i++)
            {
                CalculateUpdatedVertex(i, 0);

                CalculateUpdatedVertex(i, 1);

                CalculateUpdatedVertex(i, 2);

                DecayVelocity(i);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="offset"></param>
        protected virtual void CalculateUpdatedVertex(int i, int offset)
        {
            Vector3 position = uiVertices[i * 3 + offset].position;

            position += triangleVelocity[i] * Time.deltaTime;

            UIVertex uiVertex = uiVertices[i * 3 + offset];

            uiVertex.position = position;

            uiVertices[i * 3 + offset] = uiVertex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DecayVelocity(int i) =>
            triangleVelocity[i] -= Time.deltaTime * triangleVelocity[i] / explosionTime;


        /// <summary>
        /// 
        /// </summary>
        [BehaviourButton]
        public void DebugShatter()
        {
            ResetProcess();

            StartCoroutine(ShatterTheGraphic());
        }

        /// <inheritdoc/>
        protected override void ResetProcess()
        {
            if (resetMesh != null)
                targetMeshFilter.mesh = resetMesh;
        }


        /// <inheritdoc/>
        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            if (finished)
                return;

            vertexHelper.Clear();

            foreach (UIVertex uiVertex in uiVertices)
                vertexHelper.AddVert(uiVertex);

            for (int i = 0; (i + 6) <= uiVertices.Count; i += 6)
            {
                vertexHelper.AddTriangle(i, i + 1, i + 2);

                vertexHelper.AddTriangle(i + 3, i + 4, i + 5);
            }
        }
    }

    // Gizmo Section
    public partial class ShatterMeshGraphic
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector3 sphereOffset = new(960, 540, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float sphereRadius = 50;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int spheresPerColor = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color startingColor = Color.red;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Color> gizmoColors = new() { Color.red, Color.green, Color.blue };


        /// <summary>
        ///
        /// </summary>
        protected Color vColor;

        /// <summary>
        ///
        /// </summary>
        protected int counter = 0;


        protected virtual void OnDrawGizmos()
        {
            if (Application.isPlaying && uiVertices != null)
                StartDrawingGizmos();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartDrawingGizmos()
        {
            SetGizmoColor(startingColor);

            foreach (var uiVertex in uiVertices)
                DrawGizmoSphere(uiVertex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vColor"></param>
        protected virtual void SetGizmoColor(Color vColor)
        {
            this.vColor = vColor;

            Gizmos.color = vColor;

            counter = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uiVertex"></param>
        protected virtual void DrawGizmoSphere(UIVertex uiVertex)
        {
            Gizmos.DrawSphere(uiVertex.position + sphereOffset, sphereRadius);

            if (counter == spheresPerColor)
                SetGizmoColor(GetNextColor());
            else
                counter++;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Color GetNextColor()
        {
            int gizmoColorIndex = gizmoColors.IndexOf(vColor) + 1;

            if (gizmoColorIndex >= gizmoColors.Count)
                gizmoColorIndex = 0;

            return gizmoColors[gizmoColorIndex];
        }
    }
}