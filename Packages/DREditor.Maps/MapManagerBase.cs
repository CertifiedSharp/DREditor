//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MapManagerBase : ServiceHolder<IMapManager>, IMapManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MapEntry InstanceData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject areaGameObject = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MapAreaManager currentAreaManager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int currentAreaIndex = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MapDatabase mapDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject root = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI areaText = null;


        [Header("")]
        [SerializeField] protected UnityEvent onMapChanging = null;

        
        /// <summary>
        ///
        /// </summary>
        protected int shiftNumber = 0;

        /// <summary>
        ///
        /// </summary>
        protected IPlaceOptionHolder placeOptionHolder;


        protected virtual void Start() => placeOptionHolder = GetComponentInChildren<IPlaceOptionHolder>();


        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateMapUI()
        {
            shiftNumber = 0;

            if (currentAreaManager != null &&
                currentAreaManager.Data.name == mapDatabase.ValidFloorList(InstanceData.UnlockedAreas)[currentAreaIndex].name)
                return;

            StopAllCoroutines();

            onMapChanging?.Invoke();

            StartCoroutine(UpdateMapArea());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator UpdateMapArea()
        {
            DestroyAreaUI();

            yield return null;

            SpawnLoadAreaUI();

            yield return null;

            PopulateMapUIText(0);

            ServiceLocator.GetService<MapTabUIHandler>().InitializeTab();

            if (ServiceLocator.GetService<MapTabUIHandler>().IsOnDefaultTab())
                NavigateSelectVisual(new CallbackContext());

            AnimateToHoveredPlace();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroyAreaUI()
        {
            if (areaGameObject != null)
                Destroy(areaGameObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SpawnLoadAreaUI()
        {
            areaGameObject = Instantiate(mapDatabase.ValidFloorList(InstanceData.UnlockedAreas)[currentAreaIndex].prefab, root.transform);

            if (areaGameObject == null)
                return;

            currentAreaManager = areaGameObject.GetComponent<MapAreaManager>();

            currentAreaManager
                .LoadEntry(InstanceData.GetAreaEntry(currentAreaManager.Data.name), InstanceData.UnlockedAreas);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="shiftNum"></param>
        protected virtual void PopulateMapUIText(int shiftNum)
        {
            areaText.text = currentAreaManager.Data.name;

            var placeNames = currentAreaManager.GetNamesOfUnlockedPlaces(InstanceData.UnlockedAreas);

            placeOptionHolder.PopulatePlaceUIText(shiftNum, placeNames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected void NavigateSelectVisual(CallbackContext ctx)
        {
            float vertical = ctx.ReadValue<Vector2>().y;

            bool movingUp = IsMovingUp(vertical);

            if (vertical == 0 && !movingUp)
                return;

            bool movingDown = IsMovingDown(vertical);

            if (movingUp || movingDown)
                ShiftThenHover(movingUp);

            if (ServiceLocator.GetService<MapTabUIHandler>().IsOnDefaultTab())
                AnimateToHoveredPlace();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertical"></param>
        /// <returns></returns>
        protected virtual bool IsMovingUp(float vertical) =>
                vertical > 0 &&
                shiftNumber > 0 &&
                EventSystem.current.currentSelectedGameObject == placeOptionHolder.GetTopOption();

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertical"></param>
        /// <returns></returns>
        protected virtual bool IsMovingDown(float vertical) =>
                vertical < 0 &&
                shiftNumber < placeOptionHolder.GetMaxShiftIndex() &&
                EventSystem.current.currentSelectedGameObject == placeOptionHolder.GetBottomOption();

        /// <summary>
        ///
        /// </summary>
        /// <param name="movingUp"></param>
        protected virtual void ShiftThenHover(bool movingUp)
        {
            if (movingUp)
                shiftNumber--;
            else
                shiftNumber++;

            PopulateMapUIText(shiftNumber);

            AnimateToHoveredPlace();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateToHoveredPlace() => StartCoroutine(WaitForAreaLoaded());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForAreaLoaded()
        {
            yield return new WaitUntil(() => ServiceLocator.HasService<MapAreaManager>());

            if (!ServiceLocator.HasService(out currentAreaManager))
                yield break;

            ServiceLocator.GetService<MapHoverSelector>().AnimateToHoveredPlace(placeOptionHolder.GetCurrentlyHoveredPlace());
        }


        /// <inheritdoc/>
        public virtual MapEntry GetInstanceData() => InstanceData;

        /// <inheritdoc/>
        public virtual void SetInstanceData(MapEntry InstanceData) => this.InstanceData = InstanceData;

        /// <inheritdoc/>
        public virtual void AddUnlockedArea(string areaName)
        {
            MapAreaData areaData = mapDatabase.GetArea(areaName);

            PlaceData placeData = mapDatabase.GetPlace(areaName);

            if (areaData != null)
                InstanceData.UnlockedAreas.Add(areaData.GetID());
            else if (placeData != null)
                InstanceData.UnlockedAreas.Add(placeData.GetID());
            else
                UnityEngine.Debug.LogWarning($"ALERT: {ToString()}: Couldn't find the area/place to unlock: " + areaName);
        }
    }
}