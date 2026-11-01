//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MapUITab) +
        "\n\n")]
    public class CharacterMapUITab : MapUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject studentTabHighlight = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CanvasGroup studentGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MapMarkerSelectable[] mapMarkerSelectables = new MapMarkerSelectable[0];


        protected virtual void OnEnable() => SubscribeMarkerHover();

        protected virtual void OnDisable() => UnsubscribeMarkerHover();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeMarkerHover()
        {
            foreach (MapMarkerSelectable marker in mapMarkerSelectables)
                marker.OnHover.AddListener(OnMarkerHovered);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeMarkerHover()
        {
            foreach (MapMarkerSelectable marker in mapMarkerSelectables)
                marker.OnHover.RemoveListener(OnMarkerHovered);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="markerName"></param>
        protected virtual void OnMarkerHovered(string markerName)
        {
            var places = ServiceLocator.GetService<MapAreaManager>()
                .GetPlaceManagers()
                .Where(place => place.ContainsMarker(markerName));

            if (places.Count() > 0)
            {
                PlaceManager place = places.ToArray()[0];

                ServiceLocator.GetService<MapHoverSelector>().AnimateToHoveredPlace(place);
            }
        }


        protected virtual void Start() => DisableMarkers();

        /// <inheritdoc/>
        public override void InitializeTab() => EnableAllMarkers();

        /// <inheritdoc/>
        public override void HideTab()
        {
            studentGroup.alpha = 0;

            studentTabHighlight.SetActive(false);

            DisableMarkers();
        }

        /// <inheritdoc/>
        public override void ShowTab()
        {
            studentGroup.alpha = 1;

            studentTabHighlight.SetActive(true);

            EnableAllMarkers();

            EventSystem.current.SetSelectedGameObject(mapMarkerSelectables[0].gameObject);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableMarkers()
        {
            for (int i = 0; i < mapMarkerSelectables.Length; i++)
            {
                mapMarkerSelectables[i].enabled = false;

                mapMarkerSelectables[i].interactable = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableAllMarkers()
        {
            EnableMarkers();

            EnableMarkersInArea();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableMarkers()
        {
            for (int i = 0; i < mapMarkerSelectables.Length; i++)
            {
                mapMarkerSelectables[i].enabled = true;

                mapMarkerSelectables[i].interactable = true;

                mapMarkerSelectables[i].SetUnavailable();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableMarkersInArea()
        {
            var places = ServiceLocator.GetService<MapAreaManager>().GetPlaceManagers();

            var names = GetMarkerSelectableNames();

            foreach (var place in places)
                foreach (string name in names)
                    if (place.ContainsMarker(name))
                        mapMarkerSelectables[names.IndexOf(name)].SetAvailable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetMarkerSelectableNames()
        {
            List<string> names = new();

            for (int i = 0; i < mapMarkerSelectables.Length; i++)
                names.Add(mapMarkerSelectables[i].GetMarkerName());

            return names;
        }
    }
}