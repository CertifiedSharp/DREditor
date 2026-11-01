//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.RoomLoading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<IMapManager>) + " for type: " + nameof(IMapManager) + 
        "\n\n")]
    public class MapManager : MapManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Space]
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference navigate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleLeft = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleRight = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference rightClick = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool giveAllMapsOnStart = false;


        [Header("")]
        [SerializeField] protected UnityEvent OnTeleportMapUI;


        /// <summary>
        ///
        /// </summary>
        protected bool changingArea = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateControls()
        {
            submit.action.performed += OnPlaceSelected;

            navigate.action.performed += NavigateSelectVisual;

            shuffleLeft.action.started += DecreaseArea;

            shuffleRight.action.started += IncreaseArea;

            rightClick.action.started += OnChangeTab;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DeactivateControls()
        {
            submit.action.performed -= OnPlaceSelected;

            navigate.action.performed -= NavigateSelectVisual;

            shuffleLeft.action.started -= DecreaseArea;

            shuffleRight.action.started -= IncreaseArea;

            rightClick.action.started -= OnChangeTab;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void OnChangeTab(CallbackContext ctx)
        {
            if (!ServiceLocator.HasService(out MapTabUIHandler tabHandler))
                return;

            if (tabHandler.IsOnDefaultTab())
                placeOptionHolder.DisableSelectableOptions();

            tabHandler.UpdateTab();

            if (tabHandler.IsOnDefaultTab())
            {
                tabHandler.InitializeTab();

                if (tabHandler.IsOnDefaultTab())
                    AnimateToHoveredPlace();

                PopulateMapUIText(0);
            }
            else
                UpdateMapUI();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void OnPlaceSelected(CallbackContext ctx)
        {
            UnityEngine.Debug.Log("REPORT: TELEPORT REQUESTED");

            if (ServiceLocator.GetService<MapHoverSelector>().PlaceIsFastTravelable())
                EvaluateTeleportingTo(ServiceLocator.GetService<MapHoverSelector>().GetCurrentPlace());
            else
                UnityEngine.Debug.Log("REPORT: TELEPORT REQUEST UNSUCCESSFUL, PLACE WAS NOT FAST TRAVELABLE.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPlace"></param>
        protected virtual void EvaluateTeleportingTo(PlaceManager currentPlace)
        {
            DeactivateControls();
            
            if (ServiceLocator.HasService(out ITeleporter teleporter) && 
                teleporter.RequestTeleport(currentPlace.Data.sceneName, currentPlace.Data.fastTravelIndex))
            {
                UnityEngine.Debug.Log("REPORT: TELEPORT REQUEST SUCCESSFUL");

                LeaveProcess();

                OnTeleportMapUI?.Invoke();

                Time.timeScale = 1;
            }
            else
            {
                UnityEngine.Debug.Log("REPORT: TELEPORT REQUEST UNSUCCESSFUL");

                ActivateControls();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void IncreaseArea(CallbackContext ctx)
        {
            if (changingArea || currentAreaIndex == mapDatabase.ValidFloorList(InstanceData.UnlockedAreas).Count - 1)
                return;

            currentAreaIndex++;

            ChangeArea();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void DecreaseArea(CallbackContext ctx)
        {
            if (changingArea || currentAreaIndex == 0)
                return;

            currentAreaIndex--;

            ChangeArea();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator AreaWait()
        {
            yield return new WaitForEndOfFrame();

            changingArea = false;
        }


        /// <summary>
        ///
        /// </summary>
        protected override void Start()
        {
            base.Start();

            if (giveAllMapsOnStart)
                DebugGivePlayerAllMaps();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DebugGivePlayerAllMaps()
        {
            foreach (var o in mapDatabase.floors)
                InstanceData.UnlockedAreas.Add(o.name);
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void StartDisplayingUI()
        {
            if (mapDatabase != null && mapDatabase.floors.Count > 0)
                ChangeArea();
            else
                UnityEngine.Debug.LogWarning($"ALERT: {ToString()}: Couldn't Populate UI due to Map Database not having information.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeArea()
        {
            changingArea = true;

            DeactivateControls();

            UpdateMapUI();

            ActivateControls();

            StartCoroutine(AreaWait());
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void LeaveProcess()
        {
            DeactivateControls();

            EventSystem.current.SetSelectedGameObject(null);

            DestroyAreaUI();

            shiftNumber = 0;

            ServiceLocator.GetService<MapTabUIHandler>().ShowDefaultTab();

            placeOptionHolder.DisableSelectableOptions();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="shiftNum"></param>
        protected override void PopulateMapUIText(int shiftNum)
        {
            base.PopulateMapUIText(shiftNum);

            placeOptionHolder.EnableSelectablesOnValidText();
        }
    }
}