//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ValueEventHandler<int, IntEvent>) + " for types: int and " + nameof(IntEvent) +
        "\n\n")]
    public class IntSaveHandler : ValueEventHandler<int, IntEvent>
    {
        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Default Data")]
        public virtual void PopulateDefaultData() => entries.Add(new("currency"));

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Difficulty Data")]
        public virtual void PopulateDifficultyData()
        {
            entries.Add(new("logic"));

            entries.Add(new("action"));
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate State Data")]
        public virtual void PopulateStateData()
        {
            entries.Add(new("currentState"));

            entries.Add(new("addedState"));
        }
    }
}