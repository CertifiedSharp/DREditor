//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class InstanceListWrapper<T>
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("list")]
        public List<T> wrappedInstances = new();

        public InstanceListWrapper() { }

        public InstanceListWrapper(List<T> wrappedInstances) => this.wrappedInstances = wrappedInstances;
    }
}