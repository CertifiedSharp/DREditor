//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class GenericActuator : GenericTriggerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool Triggered = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 position;


        /// <summary>
        ///
        /// </summary>
        /// <param name="Triggered"></param>
        public virtual void SetTriggered(bool Triggered) => this.Triggered = Triggered;

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        public virtual void SetPosition(Vector3 position) => this.position = position;


        protected GenericActuator() { }

        protected GenericActuator(string Type, bool Triggered, Vector3 position) : base(Type)
        {
            this.Triggered = Triggered;

            this.position = position;
        }
    }
}