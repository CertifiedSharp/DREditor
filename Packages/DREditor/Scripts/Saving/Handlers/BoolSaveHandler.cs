//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class BoolSaveHandler : ValueEventHandler<bool, BoolEvent>
    {
        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Default Data")]
        public virtual void PopulateDefaultData()
        {
            entries.Add(new("InvertX"));

            entries.Add(new("LookInvert"));

            entries.Add(new("MovementBob"));

            entries.Add(new("DRCameraPan"));
        }
    }
}