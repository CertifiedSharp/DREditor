//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class HitboxActuatorData : GenericActuator, IActuatorConvert
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 HitboxSize;


        /// <summary>
        ///
        /// </summary>
        /// <param name="HitboxSize"></param>
        public virtual void SetHitboxSize(Vector3 HitboxSize) => this.HitboxSize = HitboxSize;


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericActuator oldActuator) => ((Actuator)oldActuator).HitBoxSize != Vector3.zero;

        /// <inheritdoc/>
        public virtual void Convert(ref GenericActuator genericActuator, GenericActuator oldActuator)
        {
            var newData = new HitboxActuatorData(oldActuator.Type, oldActuator.Triggered, oldActuator.position)
            { 
                HitboxSize = ((Actuator)oldActuator).HitBoxSize 
            };

            genericActuator = newData;
        }


        public HitboxActuatorData() { }

        public HitboxActuatorData(string Type, bool Triggered, Vector3 position) : base(Type, Triggered, position) { }
    }
}