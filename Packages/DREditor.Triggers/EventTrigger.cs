//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class EventTrigger
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeReference]
        public GenericActuator GenericActuator;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeReference]
        public List<GenericSubsequent> GenericSubsequents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Actuator Actuator;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<Subsequent> Subsequents = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericActuator"></param>
        public virtual void SetActuator(GenericActuator GenericActuator) => this.GenericActuator = GenericActuator;
    }
}