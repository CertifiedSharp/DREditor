// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<MapAreaManager>) + " for type: " + nameof(MapAreaManager) +
        "\n\n")]
    public class MapAreaManager : ServiceHolder<MapAreaManager>
    {
        /// <summary>
        ///
        /// </summary>
        [Space]
        [Space]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MapAreaEntry Entry;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public MapAreaData Data;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<PlaceManager> Places = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="unlocks"></param>
        public virtual void LoadEntry(MapAreaEntry entry, List<string> unlocks = null)
        {
            if (entry != null)
            {
                Entry = entry;

                PopulatePlaces(Entry.PlaceEntries, unlocks);
            }
            else
                UnityEngine.Debug.Log("No Entry Data For: " + Data.name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="placeEntries"></param>
        /// <param name="unlocks"></param>
        protected virtual void PopulatePlaces(List<PlaceEntry> placeEntries, List<string> unlocks = null)
        {
            for (int i = 0; i < placeEntries.Count; i++)
            {
                PlaceEntry entry = placeEntries[i];

                var places = Places.Where(place => 
                place.Data.name == entry.name ||
                place.Data.GetID() == entry.GetID())
                    .ToList();

                foreach (var place in places)
                {
                    if (unlocks != null && unlocks.Contains(place.Data.GetID()))
                        place.SetPlaceVisibilityTo(true);

                    place.LoadPlace(entry);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="place"></param>
        public virtual void AddPlaceManager(PlaceManager place)
        {
            Data.places.Add(place.Data);

            Places.Add(place);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<PlaceData> GetPlaceDatas()
        {
            List<PlaceData> placeDatas = new();

            foreach (var place in Places)
                placeDatas.Add(place.Data);

            return placeDatas;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<PlaceManager> GetPlaceManagers() => Places;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PlaceManager GetPlaceFromName(string name)
        {
            if (Places.Where(place => place.Data.name == name).ToList() is List<PlaceManager> list && list.Count > 0)
                return list[0];

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="unlocks"></param>
        /// <returns></returns>
        public virtual List<string> GetNamesOfUnlockedPlaces(List<string> unlocks)
        {
            List<string> list = new();

            for (int i = 0; i < Places.Count; i++)
                if (Places[i].IsUnlocked(unlocks))
                    list.Add(Places[i].Data.name);

            return list;
        }


        #region Unused but available public functions

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual MapAreaEntry GetInstanceData()
        {
            MapAreaEntry areaEntry = new(Data);

            for (int i = 0; i < Places.Count; i++)
                areaEntry.PlaceEntries.Add(Places[i].GetEntryInstance());

            return areaEntry;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="placeName"></param>
        /// <param name="data"></param>
        public virtual void AddMarker(string placeName, MarkerVessel data)
        {
            var places = Places.Where(place => place.Data.name == placeName).ToList();

            if (places.Count > 0)
                places[0].CreateMarker(data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="marker"></param>
        public virtual void RemoveMarker(MarkerVessel marker)
        {
            var places = Places.Where(place => place.GetEntryInstance().MarkerVessels.Contains(marker)).ToList();

            foreach (var placeManager in places)
                placeManager.DestroyMarker(marker.sprite);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Load() => Places.ForEach(place => place.Load());

        #endregion
    }
}