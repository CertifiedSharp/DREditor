//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CGHandlerBase : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGStateMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgUIVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgDialogueUIVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgStateEnded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isGoingToMainMenu = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            cgUIVisible.SetValue(false);

            cgStateEnded.SetValue(false);

            inCGStateMode.SetValue(false);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void TurnOffOtherHandlers()
        {
            var handlers = GetOtherActiveHandlers();

            foreach (var handler in handlers)
                handler.TransitionOffState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OtherHandlersActive() => GetOtherActiveHandlers().Count() > 0;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ICGHandler> GetOtherActiveHandlers()
            => FindObjectsByType<MonoBehaviour>()
            .OfType<ICGHandler>()
            .Where(handler => handler.InCGStateMode().Value && handler.InCGStateMode() != inCGStateMode);


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => Initialize();
    }
}