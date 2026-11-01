//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.AutomaticLine
{
    /// <summary>
    ///
    /// </summary>
    public abstract class AutoMechanicBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inAutoMode = null;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator WaitForAutoMechanicToResolve();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator AutoMechanicProcess();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool InAutoMode() => inAutoMode.Value;

        /// <summary>
        ///
        /// </summary>
        public virtual void ToggleAutoMode() => inAutoMode.SetValue(!inAutoMode.Value);
    }
}