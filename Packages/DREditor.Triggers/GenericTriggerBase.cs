//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class GenericTriggerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string Type;


        /// <summary>
        ///
        /// </summary>
        /// <param name="Type"></param>
        public virtual void SetType(string Type) => this.Type = Type;


        // Because the class type name might change namespaces, having this would 
        // help at least store and allow for a converter to be made.
        // But at the moment the TriggerLoader class just scans assemblies to always find the right class,
        // therefore this isn't needed at the moment, but it uses system.reflection, so it is only a 
        // temporary solution and should eventually be fixed in a future update.
        //public string classTypeName;

        //public void SetClassTypeName(string classTypeName) => this.classTypeName = classTypeName;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone() => MemberwiseClone();


        protected GenericTriggerBase() { }

        protected GenericTriggerBase(string Type)
        {
            this.Type = Type;

            // See classTypeName comments
            //classTypeName = GetType().ToString();
        }
    }
}