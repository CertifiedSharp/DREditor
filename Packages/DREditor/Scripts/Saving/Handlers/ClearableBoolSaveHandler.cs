//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class ClearableBoolSaveHandler : ValueEventHandler<bool, BoolEvent>, IPlayerDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool valueWhenCleared = false;

        
        /// <inheritdoc/>
        public virtual void ClearData()
        {
            foreach (var entry in entries)
                entry.value.SetValue(valueWhenCleared);
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Clearable Data")]
        public virtual void PopulateClearableData() => entries.Add(new("pauseAccess"));
    }
}