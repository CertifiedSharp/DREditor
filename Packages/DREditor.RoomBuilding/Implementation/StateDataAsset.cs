//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class StateDataAsset<T> : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("data")]
        public List<T> datas = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="datas"></param>
        public virtual void SetDatas(List<T> datas) => this.datas = datas;
    }
}