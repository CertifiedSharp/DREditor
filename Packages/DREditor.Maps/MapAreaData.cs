//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MapAreaData : DiscoverableBase
    {
        /// <summary>
        ///
        /// </summary>
        public GameObject prefab = null;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("placeList")]
        public List<PlaceData> places = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="prefab"></param>
        public virtual void SetPrefab(GameObject prefab) => this.prefab = prefab;

        /// <summary>
        ///
        /// </summary>
        /// <param name="places"></param>
        public virtual void SetPlaces(List<PlaceData> places) => this.places = places;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetPlaceNames()
        {
            string[] names = new string[places.Count];

            for (int i = 0; i < places.Count; i++)
                names[i] = places[i].name;

            return names;
        }
    }
}