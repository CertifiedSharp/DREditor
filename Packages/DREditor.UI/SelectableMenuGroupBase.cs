//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SelectableMenuGroupBase : MenuGroupBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [FormerlySerializedAs("butts")]
        [Tooltip("")]
        public List<Selectable> selectables = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject first = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject LastSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool dontUseLastSelected = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool forceUseLastSelected = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useVerticalNavigation = false;
        
        
        /// <summary>
        ///
        /// </summary>
        private GameObject tempFirst = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        public virtual void SetFirst(GameObject go) => first = go;

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <remarks></remarks>
        public virtual void SetTempFirst(GameObject to) => tempFirst = to;

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual void SetLastSelected(GameObject gameObject) => LastSelected = gameObject;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            if (useVerticalNavigation)
                selectables.PopulateVerticalNavigation();
        }

        /// <inheritdoc/>
        public override void SetEvaluateSelect() => StartCoroutine(SetEvaluateSelection());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SetEvaluateSelection()
        {
            if (!ServiceLocator.GetService<MenuState>().CanSelect)
            {
                UnityEngine.Debug.Log("REPORT: Menu Evaluate Selection Disabled.");

                yield break;
            }

            GameObject go = EvaluateSelection();

            yield return OnSelectionEvaluated(go);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        protected virtual IEnumerator OnSelectionEvaluated(GameObject go)
        {
            SetSelection(go);

            ServiceLocator.GetService<MenuState>().MenuEvaluationFinished?.Invoke(this);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void QuickSelection() => SetSelection(EvaluateSelection());

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        protected virtual void SetSelection(GameObject go)
        {
            if (!ServiceLocator.GetService<MenuState>().CanSelect)
                return;

            EventSystem.current.SetSelectedGameObject(go);

            if (ServiceLocator.HasService<UIHandlerBase>())
                ServiceLocator.GetService<UIHandlerBase>().SetCurrent(go);

            if (go == tempFirst)
                tempFirst = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual GameObject EvaluateSelection()
        {
            if (Check(LastSelected) && CheckLastSelected())
                return LastSelected;
            else if (Check(first))
                return first;
            else if (Check(tempFirst))
                return tempFirst;

            foreach (Selectable s in selectables)
                if (Check(s.gameObject))
                    return s.gameObject;

            UnityEngine.Debug.LogWarning("ALERT: Menu Group " + gameObject.name + "'s GetSelection Returned Null!");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckLastSelected() => forceUseLastSelected || (IsMenusSelectables(LastSelected) && !dontUseLastSelected);

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        protected bool Check(GameObject go) => go != null && go.activeSelf && go.GetComponent<Selectable>().interactable;

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        protected bool IsMenusSelectables(GameObject go)
        {
            foreach (Selectable selectable in selectables)
                if (selectable.gameObject == go)
                    return true;

            return false;
        }
    }
}