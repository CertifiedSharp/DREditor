//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(OptionUIGroupBase) +
        "\n\n")]
    public class OptionUIGroup : OptionUIGroupBase
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("navGroups")]
        [SerializeField] protected List<NavigationGroup> navigationGroups = new() { new NavigationGroup() };

        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("navSelects")]
        [SerializeField] protected List<NavigationSelect> navigationSelects = new() { new NavigationSelect() };


        [Header("")]
        [Divider]
        [Header("")]
        public UnityEvent OnBeforeSetup;

        [Header("")]
        public UnityEvent OnSetup;

        [Header("")]
        public UnityEvent OnUnSetup;


        [Header("")]
        [Divider]
        [Header("")]
        public UnityEvent Selection;

        [Header("")]
        public UnityEvent Deselection;


        /// <summary>
        ///
        /// </summary>
        protected Selectable lastOption = null;

        /// <summary>
        ///
        /// </summary>
        protected bool isSetup = false;

        /// <summary>
        ///
        /// </summary>
        protected bool selected = false;


        /// <summary>
        ///
        /// </summary>
        public virtual void BeforeSetupGroup() => OnBeforeSetup?.Invoke();

        /// <summary>
        ///
        /// </summary>
        public virtual void SetupGroup()
        {
            ServiceLocator.GetService<UIHandlerBase>().OnChange.AddListener(Evaluate);

            SelectLastOptionOnEmpty();

            isSetup = true;

            if (navigationGroups.Count == 0 && navigationSelects.Count == 0)
            {
                ClearNavigation();

                InitializeNavigation();
            }
            else if (lastOption != null)
                RewireNavigation(lastOption);
            else
                RewireNavigation(options[0]);

            OnSetup?.Invoke();

            Evaluate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeNavigation()
        {
            var firstOption = options[0];

            List<Selectable> firstSelectables = new()
            {
                firstOption.navigation.selectOnDown,
                firstOption.navigation.selectOnUp,
                firstOption.navigation.selectOnRight,
                firstOption.navigation.selectOnLeft,
            };

            for (int i = 0; i < firstSelectables.Count; i++)
            {
                if (options.Contains(firstSelectables[i]) || firstSelectables[i] == null)
                    continue;

                if (firstSelectables[i].GetComponentInParent<OptionUIGroupBase>() is OptionUIGroupBase group)
                    navigationGroups.Add(new NavigationGroup(group, (Direction)i));
                else
                    navigationSelects.Add(new NavigationSelect(firstSelectables[i], (Direction)i));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void UnSetupGroup()
        {
            ServiceLocator.GetService<UIHandlerBase>().OnChange.RemoveListener(Evaluate);

            DisplayLastOptionSelected();

            isSetup = false;

            foreach (var deselect in deselectHandlers)
                deselect.OnDeselect(null);

            if (navigationGroups.Count == 0 && navigationSelects.Count == 0)
                ClearNavigation();

            OnUnSetup?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearNavigation()
        {
            navigationGroups.Clear();

            navigationSelects.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Evaluate() => CheckSelectionInGroup();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckSelectionInGroup()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}: The Current Selected object was null");

            foreach (Selectable selectable in options)
                if (SelectedInGroup(selectable))
                    return true;

            NotInSelection();

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        /// <returns></returns>
        protected virtual bool SelectedInGroup(Selectable selectable)
        {
            if (EventSystem.current.currentSelectedGameObject == selectable.gameObject)
            {
                SetGroupSelectedTo(true);

                RewireNavigation(selectable);

                foreach (var select in selectHandlers)
                    select.OnSelect(null);

                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void NotInSelection()
        {
            SetGroupSelectedTo(false);

            DisplayLastOptionSelected();

            foreach (var deselect in deselectHandlers)
                deselect.OnDeselect(null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isSelected"></param>
        protected virtual void SetGroupSelectedTo(bool isSelected)
        {
            if (selected == !isSelected)
            {
                selected = isSelected;

                UnityEvent selectionEvent = selected ? Selection : Deselection;

                selectionEvent?.Invoke();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        public virtual void RewireNavigation(Selectable selectable)
        {
            lastOption = selectable;

            RewireNavigationGroups();

            RewireNavigationSelects();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RewireNavigationGroups()
        {
            if (navigationGroups.Count != 0)
                foreach (NavigationGroup nav in navigationGroups)
                    nav.group.options.ChangeGroupNav(lastOption, nav.direction);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RewireNavigationSelects()
        {
            if (navigationSelects.Count != 0)
                foreach (NavigationSelect nav in navigationSelects)
                    nav.selectable.ChangeNavSelects(lastOption, nav.direction);
        }
        
        /// <summary>
        ///
        /// </summary>
        protected virtual void SelectLastOptionOnEmpty()
        {
            if (lastOption == null)
            {
                SetLastOption(options[0]);

                DisplayLastOptionSelected();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayLastOptionSelected()
        {
            if (lastOption == null)
                return;

            var lastSelectables = lastOption.GetComponents<SelectHandler>();

            foreach (var selectable in lastSelectables)
                if (selectable != null)
                    selectable.Select();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        protected virtual void SetLastOption(Selectable selectable) => lastOption = selectable;
    }
}