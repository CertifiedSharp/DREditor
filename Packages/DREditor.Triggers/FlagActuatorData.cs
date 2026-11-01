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
    public class FlagActuatorData : GenericActuator, IActuatorConvert
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool callIfFlagsAre = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<string> flags = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="flags"></param>
        public virtual void SetStringList(List<string> flags) => this.flags = flags;

        /// <summary>
        ///
        /// </summary>
        /// <param name="callIfFlagsAre"></param>
        public virtual void SetBoolValue(bool callIfFlagsAre) => this.callIfFlagsAre = callIfFlagsAre;


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericActuator oldActuator) => ((Actuator)oldActuator).strings.Count > 0;

        /// <inheritdoc/>
        public virtual void Convert(ref GenericActuator genericActuator, GenericActuator oldActuator)
        {
            var newData = new FlagActuatorData(oldActuator.Type, oldActuator.Triggered, oldActuator.position)
            {
                flags = ((Actuator)oldActuator).strings,
                callIfFlagsAre = ((Actuator)oldActuator).boolValue
            };

            genericActuator = newData;
        }


        public FlagActuatorData() { }

        public FlagActuatorData(string Type, bool Triggered, Vector3 position) : base(Type, Triggered, position) { }
    }
}