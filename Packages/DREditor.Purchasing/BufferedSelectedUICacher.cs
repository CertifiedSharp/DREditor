//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    public class BufferedSelectedUICacher : SelectedUICacher
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected float selectBufferDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected bool waitingThroughBuffer = false;


        /// <inheritdoc/>
        public override void CacheObject() => CacheObjectResponse()?.Invoke();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Action CacheObjectResponse() => waitingThroughBuffer ? ResetBuffer : base.CacheObject;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetBuffer()
        {
            StopAllCoroutines();

            SetWaitingThroughBuffer(false);
        }


        /// <inheritdoc/>
        protected override bool ShouldSelectCachedObject() => !waitingThroughBuffer && base.ShouldSelectCachedObject();

        /// <inheritdoc/>
        protected override void SelectCachedObject() => StartCoroutine(BufferSelection());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferSelection()
        {
            SetWaitingThroughBuffer(true);

            yield return new WaitForSeconds(selectBufferDuration);

            base.SelectCachedObject();

            ResetBuffer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waitingThroughBuffer"></param>
        protected virtual void SetWaitingThroughBuffer(bool waitingThroughBuffer) => this.waitingThroughBuffer = waitingThroughBuffer;
    }
}