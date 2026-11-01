//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.Saving;
using DREditor.Services;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ChapterSectionBase) + ", " + nameof(IDRELoadableManager) + 
        "\n\n")]
    public class ChapterSelectHandler : ChapterSectionBase
    {
        /// <inheritdoc/>
        public override void OnParentSubmittedIndex(int selectedIndex) => selectedSection = selectedIndex;

        /// <inheritdoc/>
        public override void InitializeDefault()
        {
            SetMaxSection(defaultUnlockedSections);

            ActivateValidChapterButtons();
        }

        /// <inheritdoc/>
        public override void InitializeLoaded()
        {
            SetMaxSection(GetMaxLoadedScene());

            ActivateValidChapterButtons();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected int GetMaxLoadedScene()
        {
            ServiceLocator.HasService(out IChapterSelectData chapterSelectData);

            int maxLoadedIndex = -1;

            foreach (var section in chapterSelectDatabase)
                foreach (var scene in section)
                    if (chapterSelectData is IIdentifiableValidator validator && validator.IDValid(scene.ID))
                        maxLoadedIndex = chapterSelectDatabase.Details.IndexOf(section);

            return maxLoadedIndex != -1? maxLoadedIndex : loadedChapter;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateValidChapterButtons()
        {
            for (int i = 0; i < sectionButtons.Count; i++)
                sectionButtons[i].gameObject.SetActive(i <= maxSection);
        }
    }
}