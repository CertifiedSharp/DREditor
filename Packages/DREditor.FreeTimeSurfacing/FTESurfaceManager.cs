//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor;
using DREditor.Services;
using DREditor.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<IFTESurfaceManager>) + " for type: " + nameof(IFTESurfaceManager) +
        "\n\n")]
    public class FTESurfaceManager : ServiceHolder<IFTESurfaceManager>, IFTESurfaceManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IndexSelector surfaceMover = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<FTESurfaceComponent> surfaces = new();


        protected virtual void Start() => surfaceMover.SelectionMaximum = surfaces.Count - 1;


        /// <inheritdoc/>
        public virtual FTESurfaceComponent GetSurfaceAt(int index) => surfaces[index];

        /// <inheritdoc/>
        public virtual FTESurfaceComponent GetSelectedSurface() => surfaces[surfaceMover.SelectedIndex];

        /// <inheritdoc/>
        public virtual bool TryGetSelectionHandler<T>(out T value) => (value = GetSelectionHandler<T>()) != null;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetSelectionHandler<T>() => surfaceMover is T output ? output : default;
    }
}