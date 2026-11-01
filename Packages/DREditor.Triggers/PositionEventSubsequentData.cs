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
    public class PositionEventSubsequentData : GenericSubsequent, ISubsequentConvert
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("String")]
        public string eventString;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 position;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Quaternion rotation;


        /// <summary>
        ///
        /// </summary>
        /// <param name="eventString"></param>
        public virtual void SetEventString(string eventString) => this.eventString = eventString;

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public virtual void SetPositionRotation(Vector3 position, Quaternion rotation)
        {
            this.position = position;

            this.rotation = rotation;
        }


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericSubsequent genericSubsequent)
        {
            var subsequent = (Subsequent)genericSubsequent;

            return subsequent.String != "" || subsequent.position != Vector3.zero;
        }

        /// <inheritdoc/>
        public virtual void Convert(List<GenericSubsequent> genericSubsequents, GenericSubsequent genericSubsequent)
        {
            var subsequent = (Subsequent)genericSubsequent;

            var newData = new PositionEventSubsequentData(subsequent.Type)
            {  
                eventString = subsequent.String,
                position = subsequent.position, 
                rotation = subsequent.rotation
            };

            genericSubsequents.Add(newData);
        }

        /// <inheritdoc/>
        public override GenericSubsequent MergeInstance(GenericSubsequent instance) => this;


        public PositionEventSubsequentData() { }

        public PositionEventSubsequentData(string Type) : base(Type) { }
    }
}