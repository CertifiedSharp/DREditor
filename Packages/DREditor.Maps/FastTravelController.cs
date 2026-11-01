//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Spawnables;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastTravelController<T> : Spawnable, ISaveable
    {
        public int SubID { get => subID; }

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int subID;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected T data;


        public virtual string Save() => JsonUtility.ToJson(data);

        public virtual void Load(string data) => this.data = JsonUtility.FromJson<T>(data);
    }
}