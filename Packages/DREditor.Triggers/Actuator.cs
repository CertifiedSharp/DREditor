//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class Actuator : GenericActuator, IActuatorConvert
    {
        public Vector3 HitBoxSize;

        [FormerlySerializedAs("stringList")]
        public List<string> strings = new();

        public bool boolValue;


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericActuator genericActuator) => false;

        /// <inheritdoc/>
        public virtual void Convert(ref GenericActuator genericSubsequents, GenericActuator oldActuator) { }


        // Kept for archival purposes, as this class will eventually be deleted.
        ///// <summary></summary>
        ///// <param name="a"></param>
        ///// <param name="i"></param>
        ///// <returns></returns>
        //public static Actuator MergeInstance(Actuator a, Actuator i) // Use data of i to apply instance data
        //{
        //    Actuator act = new();
        //    act.Type = a.Type;
        //    act.Triggered = i.Triggered;
        //    act.position = i.position;
        //    act.HitBoxSize = i.HitBoxSize;
        //    act.stringList = i.stringList;
        //    act.boolValue = i.boolValue;
        //    return null;
        //}
    }
}