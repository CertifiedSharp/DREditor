//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    public class SelectedUICacher : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected GameObject cachedSelection;


        /// <summary>
        ///
        /// </summary>
        public virtual void CacheObject()
        {
            if (ShouldCacheObject())
                CacheSelectedObject();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldCacheObject() => EventSystem.current.currentSelectedGameObject != null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void CacheSelectedObject()
        {
            cachedSelection = EventSystem.current.currentSelectedGameObject;

            UnityEngine.Debug.Log("REPORT: Caching: " + cachedSelection.name);
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void SelectObject()
        {
            if (ShouldSelectCachedObject())
                SelectCachedObject();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldSelectCachedObject() =>
            cachedSelection != null && EventSystem.current.currentSelectedGameObject == null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void SelectCachedObject()
        {
            EventSystem.current.SetSelectedGameObject(cachedSelection);

            UnityEngine.Debug.Log("REPORT: Selecting: " + cachedSelection.name);
        }
    }
}