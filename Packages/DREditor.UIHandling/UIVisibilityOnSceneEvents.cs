//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class UIVisibilityOnSceneEvents : UIVisibilityHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected List<SceneEventListener> showListeners = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SceneEventListener> hideListeners = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            foreach (SceneEventListener listener in showListeners)
                listener.Response.AddListener(visibleUI.Show);

            foreach (SceneEventListener listener in hideListeners)
                listener.Response.AddListener(visibleUI.Hide);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach (SceneEventListener listener in showListeners)
                listener.Response.RemoveListener(visibleUI.Show);

            foreach (SceneEventListener listener in hideListeners)
                listener.Response.RemoveListener(visibleUI.Hide);
        }
    }
}