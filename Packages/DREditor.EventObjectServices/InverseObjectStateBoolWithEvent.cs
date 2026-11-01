//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class InverseObjectStateBoolWithEvent : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent boolWithEvent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject objectToSet = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => boolWithEvent.AddListener(OnLeavingState);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => boolWithEvent.RemoveListener(OnLeavingState);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        protected virtual void OnLeavingState(bool state)
        {
            if (objectToSet != null)
                objectToSet.SetActive(!state);
        }
    }
}