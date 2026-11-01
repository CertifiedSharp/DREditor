//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FPC;
using System.Collections;
using UnityEngine;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
    public class OutOfBounds : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected Transform safeSpot = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float givePlayerControlBufferTime = 0.5f;


        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerEnter(Collider other) => StartCoroutine(OnOutOfBoundsRoutine(other));

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual IEnumerator OnOutOfBoundsRoutine(Collider other)
        {
            UnityEngine.Debug.LogWarning($"{ToString()}: OBJECT FELL THROUGH FLOOR: " + other.gameObject.name);

            if (MovePlayerComponentInvalid(other))
                yield break;

            other.GetComponent<MovePlayer>().enabled = false;

            other.gameObject.transform.position = safeSpot.position;

            yield return new WaitForSeconds(givePlayerControlBufferTime);

            other.GetComponent<MovePlayer>().enabled = true;

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool MovePlayerComponentInvalid(Collider other)
        {
            bool isInvalid = other.GetComponent<MovePlayer>() == null;

            if (isInvalid)
                UnityEngine.Debug.LogWarning($"{ToString()}: The fallen game object does not contain a {typeof(MovePlayer)} component.");

            return isInvalid;
        }
    }
}