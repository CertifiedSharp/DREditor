//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class OnModeVisibilityHandler : UIVisibilityHandler, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [SelectType]
        [SerializeReference]
        [Tooltip("")]
        [SerializeField] protected List<IModeVisibilityEntry> entries = new();


        protected virtual void Start()
        {
            foreach (var entry in entries)
                entry.Initialize(visibleUI.SetVisibility);
        }

        protected virtual void OnDisable()
        {
            foreach (var entry in entries)
                entry.Resolve();
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => visibleUI.SetVisibility(false);
    }
}