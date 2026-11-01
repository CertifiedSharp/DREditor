// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ShatterProcess<T> : ShatterProcessBase where T : ShatterShape
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float maxAlpha = 1;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float minAlpha = 0;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float airResistince = 0.8f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float hitForce = .025f;// 0.125

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maxHitArea = 3f;// 10

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float angularVelocityMultiplier = 100;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 hitPointOffset;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool fades = false; // true

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeTime = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeDelay = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool freezeXYRot = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool freezeZPos = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector3[] newVertices;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int[] newTriangles;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector2[][] newUVs;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color[] vertexColors;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3[] triangleVelocity;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3[] rotationVelocity;


        /// <summary>
        ///
        /// </summary>
        protected T[] shapes;

        /// <summary>
        ///
        /// </summary>
        protected MonoBehaviour runner;


        /// <summary>
        ///
        /// </summary>
        /// <param name="mesh"></param>
        protected virtual void InitializeArrays(Mesh mesh)
        {
            int triCount = mesh.triangles.Length / 3 - 2;

            int newVertexCount = triCount * 3;

            triangleVelocity = new Vector3[triCount];

            rotationVelocity = new Vector3[triCount];

            newTriangles = new int[newVertexCount];

            newUVs = new Vector2[2][];

            newUVs[0] = new Vector2[newVertexCount];

            newUVs[1] = new Vector2[newVertexCount];
        }


        public override void Shatter(MonoBehaviour runner, Vector3 hitPoint, Mesh mesh)
        {
            this.runner = runner;

            ShapeAngulate(mesh);

            ShatterShapes(hitPoint);

            FadeShapes();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mesh"></param>
        protected abstract void ShapeAngulate(Mesh mesh);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitPoint"></param>
        protected virtual void ShatterShapes(Vector3 hitPoint)
        {
            hitPoint += hitPointOffset;

            for (int i = 0; i < shapes.Length; i++)
            {
                Vector3 direction = (shapes[i].Center - hitPoint).normalized;

                float dist = Vector3.Distance(shapes[i].Center, hitPoint);

                float force = Mathf.Lerp(hitForce, 0, dist / maxHitArea);

                triangleVelocity[i] = direction * force;

                if (freezeXYRot)
                    rotationVelocity[i] = angularVelocityMultiplier * force * new Vector3(0, 0, UnityEngine.Random.Range(0, 1f));
                else
                    rotationVelocity[i] =
                        angularVelocityMultiplier *
                        force *
                        new Vector3(
                            UnityEngine.Random.Range(0f, 1f),
                            UnityEngine.Random.Range(0f, 1f),
                            UnityEngine.Random.Range(0f, 1f));
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeShapes()
        {
            if (fades)
                runner.StartCoroutine(FadeShape());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FadeShape()
        {
            yield return new WaitForSeconds(fadeDelay);

            float initial = vertexColors[0].a;

            float counter = 0;

            while (counter <= fadeTime)
            {
                for (int i = 0; i < vertexColors.Length; i++)
                    vertexColors[i].a = Mathf.Lerp(initial, 0, counter / fadeTime);

                counter += Time.deltaTime;

                yield return null;
            }

            yield break;
        }

        public override ShatterShape[] GetShapes() => shapes;

        public override IEnumerator UpdateShatter(Action<Vector3[], Color[]> updateCallback)
        {
            Ready();

            while (ready)
            {
                UpdateShape();

                updateCallback(newVertices, vertexColors);

                yield return null;
            }

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateShape()
        {
            for (int i = 0; i < shapes.Length; i++)
            {
                shapes[i].Rotate(rotationVelocity[i], newVertices);

                shapes[i].Move(triangleVelocity[i], newVertices, freezeZPos);

                rotationVelocity[i] *= (1 - (airResistince * Time.deltaTime));

                triangleVelocity[i] *= (1 - (airResistince * Time.deltaTime));
            }
        }
    }
}