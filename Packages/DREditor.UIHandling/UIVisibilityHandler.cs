//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class UIVisibilityHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiComponent = null;


        /// <summary>
        ///
        /// </summary>
        protected IUIVisibility visibleUI = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => InitializeUIComponent();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeUIComponent()
        {
            if (uiComponent is IUIVisibility visibilityUI)
                visibleUI = visibilityUI;
            else
                UnityEngine.Debug.LogError($"{ToString()}: The UI component field does not inherit {typeof(IUIVisibility)}.");
        }
    }
}