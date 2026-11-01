//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Shader That made this possible + Idea by LeoTheDev
using DREditor.EventObjects;
using System.Linq;
using UnityEngine;

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class ControlOverlaysOnSceneEvent : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool setStateTo = false;


        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(OnEventInvoked);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(OnEventInvoked);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEventInvoked()
        {
            var overlayManagers = FindObjectsByType<MonoBehaviour>().OfType<IOverlayManager>();

            foreach (var overlay in overlayManagers)
                if (overlay != null && overlay.IsOverlaying())
                    overlay.SetOverlay(setStateTo);
        }
    }
}