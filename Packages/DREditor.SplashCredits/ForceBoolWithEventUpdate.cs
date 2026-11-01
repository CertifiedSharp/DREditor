//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.SplashCredits
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class ForceBoolWithEventUpdate : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent valueToForce = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool forcedValueTo = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => ForceSetValue();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Update() => ForceSetValue();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ForceSetValue() => valueToForce.SetValue(forcedValueTo);
    }
}