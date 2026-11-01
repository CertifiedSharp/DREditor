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
    public class ScriptableObjectsSubsequentData : GenericSubsequent, ISubsequentConvert
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public ScriptableObject scriptableObject;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<ScriptableObject> scriptableObjects = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptableObject"></param>
        /// <remarks></remarks>
        public virtual void SetScriptableObject(ScriptableObject scriptableObject) => this.scriptableObject = scriptableObject;


        /// <inheritdoc/>
        public virtual bool CanConvert(GenericSubsequent genericSubsequent)
            => ((Subsequent)genericSubsequent).ScriptableObject != null || ((Subsequent)genericSubsequent).ScriptableObjects.Count > 0;

        /// <inheritdoc/>
        public virtual void Convert(List<GenericSubsequent> genericSubsequents, GenericSubsequent genericSubsequent)
        {
            var subsequent = (Subsequent)genericSubsequent;

            var newData = new ScriptableObjectsSubsequentData(subsequent.Type) { 
                scriptableObjects = subsequent.ScriptableObjects, scriptableObject = subsequent.ScriptableObject };

            genericSubsequents.Add(newData);
        }

        /// <inheritdoc/>
        public override GenericSubsequent MergeInstance(GenericSubsequent instance) => this;


        public ScriptableObjectsSubsequentData() { }

        public ScriptableObjectsSubsequentData(string Type) : base(Type) { }
    }
}