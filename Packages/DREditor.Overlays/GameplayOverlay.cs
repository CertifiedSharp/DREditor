//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GameplayOverlay : MonoBehaviour, IOverlayManager, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference observeMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEventListener checkOverlay = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition displayOverlayCondition = null;


        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IDialoguePrepareEvent> dialoguePrepareEvents;

        /// <summary>
        ///
        /// </summary>
        protected bool overlaying = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
            => dialoguePrepareEvents ??= FindObjectsByType<MonoBehaviour>().OfType<IDialoguePrepareEvent>();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            checkOverlay.Response.AddListener(ClearOverlayOnActive);

            foreach (var preparer in dialoguePrepareEvents)
                if (preparer != null)
                    preparer.OnDialoguePrepare += ClearOverlayOnActive;

            observeMode.action.started += EvaluateOverlay;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            checkOverlay.Response.RemoveListener(ClearOverlayOnActive);

            foreach (var preparer in dialoguePrepareEvents)
                if (preparer != null)
                    preparer.OnDialoguePrepare -= ClearOverlayOnActive;

            observeMode.action.started -= EvaluateOverlay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected abstract void EvaluateOverlay(CallbackContext context);

        /// <inheritdoc/>
        public virtual bool IsOverlaying() => overlaying;

        /// <inheritdoc/>
        public virtual void SetOverlay(bool to) => overlaying = to;

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator ToggleOverlay();

        /// <summary>
        ///
        /// </summary>
        protected abstract void TurnOnOverlay();

        /// <summary>
        ///
        /// </summary>
        protected abstract void TurnOffOverlay();


        /// <inheritdoc/>
        public virtual void ClearOverlayOnActive()
        {
            if (overlaying)
                ClearOverlay();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearOverlay() => StartCoroutine(ToggleOverlay());


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => TurnOffOverlay();
    }
}