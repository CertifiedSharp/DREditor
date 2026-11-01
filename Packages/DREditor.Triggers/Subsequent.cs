//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.LocalDialogues;
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
    public class Subsequent : GenericSubsequent, ISubsequentConvert
    {
        public ScriptableObject ScriptableObject;

        [FormerlySerializedAs("ScriptableObjectList")]
        public List<ScriptableObject> ScriptableObjects = new();

        public bool Locked = false;

        public string String;

        public Vector3 position;

        public Quaternion rotation;

        public LocalDialogue localDialogue;


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericSubsequent genericSubsequent) => false;

        /// <inheritdoc/>
        public virtual void Convert(List<GenericSubsequent> genericSubsequents, GenericSubsequent genericSubsequent) { }

        /// <inheritdoc/>
        public override GenericSubsequent MergeInstance(GenericSubsequent instance) => this;

        public Subsequent() { }
        public Subsequent(string Type) : base(Type) { }
    }
}