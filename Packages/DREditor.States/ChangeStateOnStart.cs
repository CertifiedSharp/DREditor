//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ChangeStateOnStart : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StateDatabase stateDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StateAsset updatedState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useDelay = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float delayTime = 0;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (useDelay)
                StartCoroutine(WaitDelay());
            else
                InvokeChangeState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator WaitDelay()
        {
            yield return new WaitForSeconds(delayTime);

            InvokeChangeState();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeChangeState() => currentState.SetValue(stateDatabase.States.IndexOf(updatedState));
    }
}