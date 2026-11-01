//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PlaceManager : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected PlaceEntry Entry;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Image> markers = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IPlaceMarker[] placeMarkers = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public PlaceData Data;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MarkerDatabase database = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image RoomImage = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => InitializePlaceMarkers();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePlaceMarkers()
        {
            placeMarkers = GetComponentsInChildren<MonoBehaviour>().OfType<IPlaceMarker>().ToArray();

            foreach (var markers in placeMarkers)
                markers.Initialize(Data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual PlaceEntry GetEntryInstance() => Entry;

        /// <summary>
        ///
        /// </summary>
        /// <param name="visible"></param>
        public virtual void SetPlaceVisibilityTo(bool visible)
        {
            if (RoomImage != null)
                RoomImage.enabled = visible;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool HasRoomImage() => RoomImage != null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        public virtual void LoadPlace(PlaceEntry entry)
        {
            if (entry != Entry)
            {
                Entry = entry;

                Load();

                ClearMarkerList();

                LoadMarkerData();

                LoadHandlers();
            }
            else
                UnityEngine.Debug.Log("Place Marker: " + Data.name + " has Identified no change in state.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadMarkerData()
        {
            ValidateEntry();

            foreach (var markerVessel in Entry.MarkerVessels)
                CreateMarker(markerVessel);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ValidateEntry()
        {
            if (Entry.MarkerDatas.Count > Entry.MarkerVessels.Count)
                foreach (var markerData in Entry.MarkerDatas)
                    Entry.MarkerVessels.Add(database.GetVesselByData(markerData));

            if (Entry.MarkerDatas.Count < Entry.MarkerVessels.Count)
                foreach (var markerVessel in Entry.MarkerVessels)
                    if (markerVessel != null)
                        Entry.MarkerDatas.Add(markerVessel.markerData);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearMarkerList()
        {
            foreach (Image image in markers)
                Destroy(image.gameObject);

            markers.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadHandlers()
        {
            foreach (IEntryLoader entryLoader in GetComponentsInChildren<MonoBehaviour>().OfType<IEntryLoader>())
                entryLoader.LoadEntry(Entry, markers);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="markerName"></param>
        /// <returns></returns>
        public virtual bool ContainsMarker(string markerName)
        {
            foreach (var marker in markers)
                if (marker.name == markerName)
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Load()
        {
            if (Entry.MarkerDatas.Count == 0)
                return;

            Entry.MarkerVessels.Clear();

            foreach (var marker in Entry.MarkerDatas)
                Entry.MarkerVessels.Add(database.GetVesselByData(marker));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="markerVessel"></param>
        public virtual void CreateMarker(MarkerVessel markerVessel)
        {
            if (markerVessel != null && markerVessel.sprite != null)
            {
                GameObject go = new();

                Image image = go.AddComponent<Image>();

                image.sprite = markerVessel.sprite;

                if (markerVessel.markerData.sizeDelta == Vector2.zero)
                    image.SetNativeSize();
                else
                    image.rectTransform.sizeDelta = markerVessel.markerData.sizeDelta;

                go.transform.SetParent(gameObject.transform);

                go.transform.localPosition = markerVessel.markerData.localPosition;

                go.name = markerVessel.markerData.name;

                markers.Add(image);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        public virtual void DestroyMarker(Sprite sprite)
        {
            if (GetMarkerImage(sprite) is Image image && image != null)
            {
                MarkerVessel marker = GetMarkerData(image.sprite);

                Entry.MarkerVessels.Remove(marker);

                markers.Remove(image);

                Destroy(image.gameObject);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        protected virtual Image GetMarkerImage(Sprite sprite)
        {
            foreach (var image in markers)
                if (image.sprite == sprite)
                    return image;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        protected virtual MarkerVessel GetMarkerData(Sprite sprite)
        {
            var markerVessels = Entry.MarkerVessels.Where(marker => marker.sprite == sprite).ToList();

            if (markerVessels.Count > 0)
                return markerVessels[0];
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="unlocks"></param>
        /// <returns></returns>
        public virtual bool IsUnlocked(List<string> unlocks) => !HasRoomImage() || HasRoomImage() && unlocks.Contains(Data.GetID());


        /// <summary>
        ///
        /// </summary>
        public virtual void Select()
        {
            foreach (var place in placeMarkers)
                place.Select();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Deselect()
        {
            foreach (var place in placeMarkers)
                place.Deselect();
        }
    }
}