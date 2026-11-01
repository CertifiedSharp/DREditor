//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ShatterGraphic : Graphic, IShatterGraphic
    {
        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent onPreShatter;

        [Header("")]
        [SerializeField] protected UnityEvent onMeshShattering;

        [Header("")]
        [SerializeField] protected UnityEvent onPostShatter;


        /// <summary>
        ///
        /// </summary>
        public UnityEvent OnPreShatter => onPreShatter;

        /// <summary>
        ///
        /// </summary>
        public UnityEvent OnMeshShattering => onMeshShattering;

        /// <summary>
        ///
        /// </summary>
        public UnityEvent OnPostShatter => onPostShatter;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Texture mainTex;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float shatterDuration = 4f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float delayResetDuration = 2f;


        public override Texture mainTexture => mainTex;


        /// <summary>
        ///
        /// </summary>
        protected bool finished = false;


        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator ShatterTheGraphic()
        {
            PreShatter();

            yield return null;

            ShatterMesh();

            yield return new WaitForSeconds(shatterDuration);

            PostShatter();

            yield return new WaitForSeconds(delayResetDuration);

            SetFinished(true);

            yield return null;

            SetFinished(false);

            ResetProcess();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PreShatter() => onPreShatter?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShatterMesh()
        {
            SetVerticesDirty();

            onMeshShattering?.Invoke();

            StartCoroutine(ShatterMeshProcess());
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator ShatterMeshProcess();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PostShatter() => onPostShatter?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetFinished(bool finished) => this.finished = finished;

        /// <summary>
        ///
        /// </summary>
        protected abstract void ResetProcess();
    }
}