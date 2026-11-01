//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing.Handling;
using DREditor.Saving;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HistoricalManagerBase<IChapterSelectData, ChapterSelectHistoryManager>) + 
        " SERVICE for type: " + nameof(IChapterSelectData) +
        "\n" + nameof(IDRESaveable) + ", " + nameof(IDRELoadableManager) + ", " +
        nameof(IDRELoadable) + ", " + nameof(ILoadOnChapterSelect) +
        "\n\n")]
    public class ChapterSelectHistoryManager : HistoricalManagerBase<IChapterSelectData, ChapterSelectHistoryManager> { }
}