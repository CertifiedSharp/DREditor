//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SimpleActuatorData : GenericActuator, IActuatorConvert
    {
        /// <inheritdoc/>
        public virtual bool CanConvert(GenericActuator oldActuator) => oldActuator.Type == nameof(OnLoadActuator);

        /// <inheritdoc/>
        public virtual void Convert(ref GenericActuator genericActuator, GenericActuator oldActuator)
        {
            var newData = new SimpleActuatorData(oldActuator.Type, oldActuator.Triggered, oldActuator.position);

            genericActuator = newData;
        }


        public SimpleActuatorData() { }

        public SimpleActuatorData(string Type, bool Triggered, Vector3 position) : base(Type, Triggered, position) { }
    }
}