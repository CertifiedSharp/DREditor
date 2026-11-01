//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MapBaseData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string name;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IDHolder Id = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(MapBaseData data)
        {
            name = data.name;

            Id.SetID(data.GetID());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string GetID() => Id.value;


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public virtual void SetName(string name) => this.name = name;


#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        public virtual void GenerateID()
        {
            if (Id.GenerateID())
                UnityEngine.Debug.Log($"GenerateID Called For {name} and ID is: {Id.value}");
            else
                UnityEngine.Debug.LogWarning($"Map Base Data ID for: {name} has already been Generated! ID: {Id.value} ");
        }
        
    #endif
    }
}