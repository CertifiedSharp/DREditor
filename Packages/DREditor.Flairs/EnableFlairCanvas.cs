//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.Services;
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class EnableFlairCanvas : IFlairStep, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Canvas canvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform parent = null;


        /// <inheritdoc/>
        public virtual void ApplyStep(IFlairEvent eventInstance, GameObject spawnedFlair)
        {
            SetCanvasEnabled(true);

            if (spawnedFlair.TryGetComponent(out IFlairDestroyable destroyable))
                destroyable.OnFlairDestroyed.AddListener(OnFlairDestroyed);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="destroyable"></param>
        protected virtual void OnFlairDestroyed(IFlairDestroyable destroyable)
        {
            destroyable.OnFlairDestroyed.RemoveListener(OnFlairDestroyed);

            DisableOnCanvasEmpty();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableOnCanvasEmpty()
        {
            if (canvas != null && parent != null && parent.transform.childCount == 1)
                SetCanvasEnabled(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enabled"></param>
        protected virtual void SetCanvasEnabled(bool enabled)
        {
            if (canvas != null)
                canvas.enabled = enabled;
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            var children = parent.GetComponentsInChildren<Transform>();

            foreach (var child in children)
                MonoBehaviour.Destroy(child.gameObject);
        }
    }
}