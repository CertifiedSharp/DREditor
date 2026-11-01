//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HistoricalManagerBase<IHistory, ProgressionHistoryManager>) + " SERVICE for type: " + nameof(IHistory) +
        "\n" + nameof(IDRESaveable) + ", " + nameof(IDRELoadableManager) + ", " + 
        nameof(IDRELoadable) + ", " + nameof(ILoadOnChapterSelect) +
        "\n\n")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionHistoryManager : HistoricalManagerBase<IHistory, ProgressionHistoryManager>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;


        /// <inheritdoc/>
        protected override object GetInstanceData()
        {
            historicalData.SetChapterIndex(currentChapterIndex.Value);

            return base.GetInstanceData();
        }
    }
}