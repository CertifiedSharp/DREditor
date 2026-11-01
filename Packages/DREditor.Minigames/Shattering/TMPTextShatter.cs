// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    [ClassHeader(nameof(IShatterText) +
        "\n\n")]
    public class TMPTextShatter : MonoBehaviour, IShatterText
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ShatterProcessBase shatterProcess;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ShatterProcessBase shatterInstance;


        /// <summary>
        ///
        /// </summary>
        protected bool started;

        /// <summary>
        ///
        /// </summary>
        protected Mesh resetMesh;

        /// <summary>
        ///
        /// </summary>
        protected TMP_Text Container => GetComponent<TMP_Text>();


        /// <inheritdoc/>
        public virtual void SetMesh()
        {
            resetMesh = new Mesh
            {
                vertices = Container.mesh.vertices,
                triangles = Container.mesh.triangles,
                uv = Container.mesh.uv,
                normals = Container.mesh.normals,
                colors = Container.mesh.colors,
                tangents = Container.mesh.tangents
            };
        }

        /// <inheritdoc/>
        public virtual void ResetMesh()
        {
            if (resetMesh == null)
                return;

            started = false;

            StopAllCoroutines();

            ResolveShatterInstance();

            ResolveContainer();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveContainer()
        {
            Container.mesh.triangles = resetMesh.triangles;

            Container.mesh.vertices = resetMesh.vertices;

            Container.mesh.uv = resetMesh.uv;

            Container.mesh.normals = resetMesh.normals;

            Container.mesh.colors = resetMesh.colors;

            Container.mesh.tangents = resetMesh.tangents;

            Container.UpdateGeometry(Container.mesh, 0);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveShatterInstance()
        {
            if (shatterInstance != null)
            {
                shatterInstance.Unready();

                Destroy(shatterInstance);

                shatterInstance = null;
            }
        }


        /// <inheritdoc/>
        public virtual void Shatter(Vector3 hitPoint)
        {
            if (started)
                return;

            Container.ForceMeshUpdate(); // Force mesh update to get valid data

            if (shatterInstance != null)
                Destroy(shatterInstance);

            shatterInstance = Instantiate(shatterProcess);

            shatterInstance.Shatter(this, hitPoint, Container.mesh);

            started = true;
        }

        /// <inheritdoc/>
        public virtual void OverrideProcess(ScriptableObject shatterProcess)
        {
            if (shatterProcess is ShatterProcessBase process)
                this.shatterProcess = process;
        }

        protected virtual void Update() => CheckShatter();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckShatter()
        {
            if (started)
            {
                started = false;

                StartCoroutine(shatterInstance.UpdateShatter(UpdateShatterCallback));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newVertices"></param>
        /// <param name="vertexColors"></param>
        protected virtual void UpdateShatterCallback(Vector3[] newVertices, Color[] vertexColors)
        {
            Container.mesh.vertices = newVertices;

            Container.mesh.colors = vertexColors;

            Container.UpdateGeometry(Container.mesh, 0);
        }


        protected virtual void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawWireMesh(Container.mesh, Container.transform.position);
            }
        }

        #region Moving Char by Char Anim Study

        //void Cope(Vector3 v)
        //{
        //    if (v == Vector3.zero)
        //        v = new(-5, -1, 0);
        //    StartCoroutine(CopeRoutine(v));

        //}
        //Vector3 L(Vector3 point, Vector3 offset, float t)
        //{
        //    Debug.Log(Vector3.Lerp(point, point + offset, t));

        //    return Vector3.Lerp(point, point + offset, t);

        //}
        //IEnumerator CopeRoutine(Vector3 to, float duration = 1)
        //{

        //    Vector3[] iniV = new Vector3[Container.mesh.vertices.Length];
        //    for (int i = 0; i < iniV.Length; i++)
        //    {
        //        iniV[i] = Container.mesh.vertices[i];
        //    }
        //    Vector3[] newV = new Vector3[Container.mesh.vertices.Length];
        //    for (int i = 0; i < iniV.Length; i++)
        //    {
        //        newV[i] = iniV[i];
        //    }

        //    for (int i = 0; i < Container.mesh.vertices.Length; i += 4) // 4 should be a letter
        //    {
        //        StartCoroutine(MoveLetter(i, iniV, newV, to, duration));
        //        yield return new WaitForSeconds(Time.deltaTime);
        //    }

        //    yield break;
        //}
        //IEnumerator MoveLetter(int ind, Vector3[] iniV, Vector3[] newV, Vector3 to, float duration = 1)
        //{
        //    float counter = 0;
        //    while (counter <= duration || iniV[ind] != iniV[ind] + to)
        //    {

        //        //Vector3[] newV = new Vector3[Container.mesh.vertices.Length];
        //        for (int i = ind; i < ind + 4; i += 4) // 4 should be a letter
        //        {

        //            newV[i] = L(iniV[i], to, counter);
        //            newV[i + 1] = L(iniV[i + 1], to, counter);
        //            newV[i + 2] = L(iniV[i + 2], to, counter);
        //            newV[i + 3] = L(iniV[i + 3], to, counter);


        //        }

        //        Container.mesh.vertices = newV;
        //        Container.UpdateGeometry(Container.mesh, 0);
        //        if (counter >= duration)
        //            yield break;

        //        counter += Time.deltaTime;
        //        yield return null;
        //    }
        //    yield break;
        //}

        #endregion
    }
}