//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public static class MapUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Import/Map/Selected Area into Database")]
        public static void ImportAreas()
        {
            MapDatabase database = DatabaseUtility.LoadDatabase<MapDatabase>();

            if (database == null)
                return;

            List<MapAreaManager> managers = GetSelectedAreaManagers();

            for (int i = 0; i < managers.Count; i++)
            {
                UpdateAreaPlaces(managers[i]);

                if (CheckForExistingAreas(database, managers[i]))
                    continue;
            }

            EditorUtility.SetDirty(database);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="area"></param>
        /// <param name="places"></param>
        static void MissingPlacesWindow(MapAreaManager area, PlaceManager[] places)
        {
            if (area.Data.places.Count != places.Length &&
                EditorUtility.DisplayDialog("Missing Places Detected",
                            "Missing Places detected, import?", "Yes", "No"))
            {
                UnityEngine.Debug.Log(area.Data.name + " Had Missing ref's : " + area.Data.places.Count + " To Found: " + places.Length);

                foreach (var place in places)
                    area.AddPlaceManager(place);

                EditorUtility.SetDirty(area);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="database"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        static bool CheckForExistingAreas(MapDatabase database, MapAreaManager area)
        {
            List<MapAreaData> dataList = database.floors.Where(databaseArea => databaseArea.GetID() == area.Data.GetID()).ToList();

            if (dataList.Count == 0)
                database.floors.Add(area.Data);
            else if (!ExistingAreaWindow(database, area, dataList))
                return true;

            var areaPrefab = PrefabUtility.GetCorrespondingObjectFromSource(area.gameObject);

            area.Data.SetPrefab(areaPrefab);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="database"></param>
        /// <param name="area"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        static bool ExistingAreaWindow(MapDatabase database, MapAreaManager area, List<MapAreaData> dataList)
        {
            if (EditorUtility.DisplayDialog("Existing Area Data Detected",
                    "Database contains Data for area " + area.Data.name + ", replace it?", "Yes", "No"))
            {
                int areaIDX = database.floors.IndexOf(dataList[0]);

                database.floors[areaIDX] = area.Data;

                return false;
            }
            else
            {
                UnityEngine.Debug.Log("Skipping Area " + area.Data.name + " Import Process");

                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        [MenuItem("DREditor/Import/Map/Area Places")]
        public static void ImportAreaPlaces()
        {
            List<MapAreaManager> managers = GetSelectedAreaManagers();

            for (int i = 0; i < managers.Count; i++)
                UpdateAreaPlaces(managers[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        static List<MapAreaManager> GetSelectedAreaManagers()
        {
            List<MapAreaManager> managers = new();

            foreach (var selectedObject in Selection.objects)
                if (selectedObject is GameObject go && go.TryGetComponent<MapAreaManager>(out var area))
                    managers.Add(area);

            return managers;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        static MapAreaData UpdateAreaPlaces(MapAreaManager area)
        {
            PlaceManager[] places = area.gameObject.GetComponentsInChildren<PlaceManager>();

            GenerateMissingPlaceIDs(places);

            MissingPlacesWindow(area, places);

            EditorUtility.SetDirty(area);

            return area.Data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="places"></param>
        static void GenerateMissingPlaceIDs(PlaceManager[] places)
        {
            foreach (var place in places)
                if (place.Data.GetID() == "")
                    place.Data.GenerateID();
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Map/Update Map Database Data From Prefabs")]
        public static void UpdateMapData()
        {
            MapDatabase database = DatabaseUtility.LoadDatabase<MapDatabase>();

            if (database == null)
                UnityEngine.Debug.LogWarning($"{nameof(MapUtility)}: UpdateMapData: " +
                    $"No Map Database could be found to use this function!");
            else
                for (int i = 0; i < database.floors.Count; i++)
                    if (database.floors[i].prefab != null)
                        database.floors[i] = UpdateAreaPlaces(database.floors[i].prefab.GetComponent<MapAreaManager>());
        }
    }
}
#endif