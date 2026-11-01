//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<MapTabUIHandler>) + " for type: " + nameof(MapTabUIHandler) +
        "\n\n")]
    public class MapTabUIHandler : ServiceHolder<MapTabUIHandler>
    {
        /// <summary>
        ///
        /// </summary>
        [Space]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MapUITab defaultTab;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<MapUITab> tabs = new();


        /// <summary>
        ///
        /// </summary>
        protected MapUITab currentTab = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            currentTab = defaultTab;

            ShowDefaultTab();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void ShowDefaultTab()
        {
            if (currentTab != defaultTab)
            {
                currentTab.HideTab();

                currentTab = defaultTab;
            }
            
            defaultTab.ShowTab();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void InitializeTab()
        {
            foreach (var tab in tabs)
                if (currentTab == tab)
                    tab.InitializeTab();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOnDefaultTab() => currentTab == defaultTab;

        /// <summary>
        ///
        /// </summary>
        public virtual void UpdateTab()
        {
            EventSystem.current.SetSelectedGameObject(null);

            foreach (var tab in tabs)
                tab.HideTab();

            int nextIndex = tabs.IndexOf(currentTab) + 1;

            if (nextIndex < 0 || nextIndex >= tabs.Count)
                nextIndex = 0;

            currentTab = tabs[nextIndex];

            currentTab.ShowTab();
        }
    }
}