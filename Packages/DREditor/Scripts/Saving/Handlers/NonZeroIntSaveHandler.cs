//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class NonZeroIntSaveHandler : ValueEventHandler<int, IntEvent>
    {
        /// <inheritdoc/>
        public override void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            base.LoadManager(saveDatas, inChapterSelect);

            foreach (var entry in entries)
                if (entry.value.Value == 0)
                    entry.value.SetValue(1);
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Default Data")]
        public virtual void PopulateDefaultData() => entries.Add(new("health"));
    }
}