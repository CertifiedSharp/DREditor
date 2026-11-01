//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing.Handling;
using DREditor.Saving;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ChapterSectionBase : MonoBehaviour, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChapterSelectDatabase chapterSelectDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string chapterKey = "chapter";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int defaultUnlockedSections = -1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Button> sectionButtons = new();


        /// <summary>
        ///
        /// </summary>
        protected int maxSection = 0;

        /// <summary>
        ///
        /// </summary>
        protected int selectedSection = 0;

        /// <summary>
        ///
        /// </summary>
        protected int loadedChapter = 0;


        /// <summary>
        ///
        /// </summary>
        public virtual int DefaultUnlockedSections { get => defaultUnlockedSections; set => defaultUnlockedSections = value; }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetSelectedSection() => selectedSection;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaxSection() => maxSection;

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxSection"></param>
        protected virtual void SetMaxSection(int maxSection) => this.maxSection = maxSection;


        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedIndex"></param>
        public abstract void OnParentSubmittedIndex(int selectedIndex);

        /// <summary>
        ///
        /// </summary>
        public abstract void InitializeDefault();

        /// <summary>
        ///
        /// </summary>
        public abstract void InitializeLoaded();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool AllowsChapterSelect() => DefaultUnlockedSections >= 0;

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <param name="button"></param>
        public virtual void SetSelectedSection(Button button) => selectedSection = sectionButtons.IndexOf(button);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool SectionIsDefaultUnlocked() => selectedSection == DefaultUnlockedSections;


        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect) => 
            StartCoroutine(BufferLoad(saveDatas, inChapterSelect));

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        /// <param name="inChapterSelect"></param>
        protected virtual IEnumerator BufferLoad(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            yield return null;

            var historyChapter = ServiceLocator.GetService<IHistory>().LatestChapterIndex();

            loadedChapter = historyChapter >= 0 ? historyChapter : saveDatas.Get<int>(chapterKey);
        }
    }
}