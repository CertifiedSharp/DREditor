// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using DREditor.Utility;
using DREditor.Editor.Utility;
#endif

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class MapEntryChange : IDataCapsulate
    {
        /// <summary>
        ///
        /// </summary>
        public MapEntry Entry = new();

        /// <summary>
        ///
        /// </summary>
        public string unlockArea = "";

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        protected MarkerDatabase markerDB = null;

        /// <summary>
        ///
        /// </summary>
        protected MapDatabase mapDB = null;

        /// <summary>
        ///
        /// </summary>
        protected int areaNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected int placeNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected int markerNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected PlaceData placeData = null;

        /// <summary>
        ///
        /// </summary>
        protected MarkerVessel selectedVessel = null;
#endif

        /// <inheritdoc/>
        public virtual void Display(object o)
        {
#if UNITY_EDITOR
            InitializeDatabases();

            if (FailedToInitializeDatabases())
                return;

            if (GUILayout.Button("Add Fast Travel Places", GUILayout.Width(150)))
                foreach (var place in mapDB.GetFastTravelablePlaces())
                    CreateAreaEntry(mapDB.GetAreaDataFromPlace(place), place);

            GUILayout.Space(10);

            unlockArea = HandyFields.StringField("Unlock Area: ", unlockArea);

            GUILayout.Space(10);

            DisplayAreasSection();

            GUILayout.Space(10);

            DisplayCreateEntrySection();
#endif
        }

#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDatabases()
        {
            if (markerDB == null)
                markerDB = DatabaseUtility.LoadDatabase<MarkerDatabase>();

            if (mapDB == null)
                mapDB = DatabaseUtility.LoadDatabase<MapDatabase>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool FailedToInitializeDatabases()
        {
            if (markerDB == null || mapDB == null)
                EditorGUILayout.LabelField("Could not find databases of type " + nameof(MarkerDatabase) +
                    " or of type " + nameof(MapDatabase) + ". \nPlease ensure that both of these databases exist in your project.");

            return markerDB == null || mapDB == null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAreasSection()
        {
            using (new GUILayout.VerticalScope(GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth)))
                for (int i = 0; i < Entry.AreaEntries.Count; i++)
                    DisplayArea(Entry.AreaEntries[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="area"></param>
        protected virtual void DisplayArea(MapAreaEntry area)
        {
            EditorGUILayout.LabelField(area.Data.name);

            using (new GUILayout.VerticalScope())
                DisplayPlacesSection(area);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="area"></param>
        protected virtual void DisplayPlacesSection(MapAreaEntry area)
        {
            for (int i = 0; i < area.PlaceEntries.Count; i++)
            {
                GUILayout.Space(10);

                PlaceEntry place = area.PlaceEntries[i];

                using (new GUILayout.HorizontalScope())
                    DisplayPlace(place);

                using (new GUILayout.VerticalScope())
                    DisplayMarkersSection(area, place);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="place"></param>
        protected virtual void DisplayPlace(PlaceEntry place)
        {
            GUILayout.Space(20);

            EditorGUILayout.LabelField(place.name, GUILayout.MaxWidth(100));

            if (mapDB.GetPlace(place.name, place.GetID()).fastTravelable)
                place.SetFastTravel(HandyFields.BoolField(place.enableFastTravelForThisEntryInstance, "FT: "));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="area"></param>
        /// <param name="place"></param>
        protected virtual void DisplayMarkersSection(MapAreaEntry area, PlaceEntry place)
        {
            for (int i = 0; i < place.MarkerVessels.Count; i++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(40);

                    if (place.MarkerVessels[i] != null)
                    {
                        place.Save();

                        EditorGUILayout.LabelField(place.MarkerDatas[i].name);

                        if (GUILayout.Button("Remove", GUILayout.Width(100)))
                            RemoveMarker(place.MarkerVessels[i], area, place);
                    }
                    else
                        place.MarkerVessels[i] = markerDB.GetVesselByData(place.MarkerDatas[i]);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="markerVessel"></param>
        /// <param name="area"></param>
        /// <param name="place"></param>
        protected virtual void RemoveMarker(MarkerVessel markerVessel, MapAreaEntry area, PlaceEntry place)
        {
            place.MarkerVessels.Remove(markerVessel);

            if (place.MarkerVessels.Count == 0)
                area.PlaceEntries.Remove(place);

            if (area.PlaceEntries.Count == 0)
                Entry.AreaEntries.Remove(area);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCreateEntrySection()
        {
            using (new GUILayout.HorizontalScope(GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth)))
            {
                areaNum = EditorGUILayout.IntPopup(areaNum, mapDB.GetAreaNames(),
                        ContainerUtil.Iota(mapDB.GetAreaNames().Length, 0), GUILayout.MaxWidth(130));

                MapAreaData areaData = mapDB.floors[areaNum];


                if (placeNum > areaData.places.Count)
                    placeNum = 0;

                placeNum = EditorGUILayout.IntPopup(placeNum, areaData.GetPlaceNames(),
                            ContainerUtil.Iota(areaData.GetPlaceNames().Length, 0), GUILayout.MaxWidth(130));

                if (placeNum >= areaData.places.Count)
                    placeNum = 0;

                placeData = areaData.places[placeNum];


                markerNum = EditorGUILayout.IntPopup(markerNum, markerDB.GetVesselNames(),
                            ContainerUtil.Iota(markerDB.GetVesselNames().Length, 0), GUILayout.MaxWidth(130));

                selectedVessel = markerDB.Entries[markerNum];


                if (GUILayout.Button("Add"))
                    CreateValidEntry(areaData);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="areaData"></param>
        protected virtual void CreateValidEntry(MapAreaData areaData)
        {
            if (Entry.Contains(areaData) is MapAreaEntry existingAreaEntry)
                AddEntryToExistingArea(existingAreaEntry);
            else
                CreateAreaEntry(areaData, placeData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="existingAreaEntry"></param>
        protected virtual void AddEntryToExistingArea(MapAreaEntry existingAreaEntry)
        {
            if (existingAreaEntry.ContainsPlace(placeData.GetID()) is PlaceEntry existingPlaceEntry)
                existingPlaceEntry.MarkerVessels.Add(selectedVessel);
            else
                CreatePlaceEntry(existingAreaEntry, placeData, "Had Area but not place entry, creating...");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="areaData"></param>
        /// <param name="placeData"></param>
        protected virtual void CreateAreaEntry(MapAreaData areaData, PlaceData placeData)
        {
            MapAreaEntry existingAreaEntry = new(areaData);

            CreatePlaceEntry(existingAreaEntry, placeData, "Didn't have area entry, creating...");

            Entry.AreaEntries.Add(existingAreaEntry);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="existingAreaEntry"></param>
        /// <param name="placeData"></param>
        /// <param name="log"></param>
        protected virtual void CreatePlaceEntry(MapAreaEntry existingAreaEntry, PlaceData placeData, string log)
        {
            PlaceEntry placeEntry = new();

            placeEntry.SetData(placeData);

            if (selectedVessel != null)
                placeEntry.MarkerVessels.Add(selectedVessel);

            existingAreaEntry.PlaceEntries.Add(placeEntry);

            placeEntry.SetFastTravel(placeData.fastTravelable);

            UnityEngine.Debug.Log(nameof(MapEntryChange) + ": " + log);
        }

#endif
    }
}