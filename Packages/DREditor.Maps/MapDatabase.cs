//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * TO-DO: CREATE EDITOR MENU ITEM THAT TAKES EACH FLOOR DATA AND GAMEOBJECT AND DO THE IMPORT DATA 
 * TO REFRESH STUFF DONE TO PREFABS LETS GOOO
 * 
 * Problem : If anything in the map data changes , entries containing map data would not be changed
 * 
 */

//All of these classes in this script might need to be a scriptable object or at least have a better structure to be more dynamic.
//MapObjectiveData
//DiscoverableData
//MapAreaData
//PlaceData

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Map/Map Database", fileName = "Map_Database")]
    public class MapDatabase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("floorList")]
        public List<MapAreaData> floors = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual MapAreaData GetArea(string name)
        {
            foreach (MapAreaData area in floors)
                if (area.name == name)
                    return area;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="unlocks"></param>
        /// <returns></returns>
        public virtual List<MapAreaData> ValidFloorList(List<string> unlocks)
        {
            List<MapAreaData> list = new();

            foreach (var floor in floors)
                if (!floor.Discoverable.isValid ||
                    floor.Discoverable.isValid &&
                    (unlocks.Contains(floor.GetID()) || unlocks.Contains(floor.name)))
                    list.Add(floor);

            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual PlaceData GetPlace(string name, string ID = "")
        {
            foreach (MapAreaData area in floors)
                foreach (var place in area.places)
                    if (place.name == name || place.GetID() == ID)
                        return place;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetAreaNames()
        {
            string[] names = new string[floors.Count];

            for (int i = 0; i < floors.Count; i++)
                names[i] = floors[i].name;

            return names;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<PlaceData> GetFastTravelablePlaces()
        {
            List<PlaceData> fastTravelablePlaces = new();

            foreach (var area in floors)
                foreach (var place in area.places)
                    if (place.fastTravelable)
                        fastTravelablePlaces.Add(place);

            return fastTravelablePlaces;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="placeData"></param>
        /// <returns></returns>
        public virtual MapAreaData GetAreaDataFromPlace(PlaceData placeData)
        {
            foreach (var area in floors)
                foreach (var place in area.places)
                    if (place == placeData)
                        return area;

            return null;
        }
    }
}