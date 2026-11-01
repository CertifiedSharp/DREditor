//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.Saving;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ChapterSubSectionBase) + ", " + nameof(ChapterSectionBase) + ", " + nameof(IDRELoadableManager) +
        "\n\n")]
    public class ChapterSceneHandler : ChapterSubSectionBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChapterSectionBase parentSection = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChapterSelectFallbackEvaluator fallbackEvaluator = null;


        /// <summary>
        ///
        /// </summary>
        protected bool inDefaultMode = false;

        /// <summary>
        ///
        /// </summary>
        protected ChapterSelectData chapterSelectData = null;

        /// <summary>
        ///
        /// </summary>
        protected List<int> moments = new();


        /// <inheritdoc/>
        public override void InitializeDefault()
        {
            SetMaxSection(defaultUnlockedSections);

            inDefaultMode = true;
        }

        /// <inheritdoc/>
        public override void InitializeLoaded()
        {
            SetMaxSection(GetMaxLoadedScene());

            inDefaultMode = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected int GetMaxLoadedScene()
        {
            ServiceLocator.HasService(out IChapterSelectData chapterSelectData);

            var chapterScenes = chapterSelectDatabase[parentSection.GetMaxSection()].Details;

            foreach (var scene in chapterScenes)
                if (chapterSelectData is IIdentifiableValidator validator && validator.IDValid(scene.ID))
                    return chapterScenes.IndexOf(scene);

            return fallbackEvaluator.GetMaxScene(parentSection);
        }


        #region Evaluate Chapter Scene Moment

        /// <inheritdoc/>
        public override void OnParentSubmittedIndex(int selectedIndex) => EvaluateChapterScene(selectedIndex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentButtonIndex"></param>
        public virtual void EvaluateChapterScene(int currentButtonIndex)
        {
            chapterSelectData = chapterSelectDatabase[currentButtonIndex];

            moments.Clear();

            EvaluateChapterSelectMoments();

            ActivateValidSceneMoments();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateChapterSelectMoments()
        {
            foreach (SceneDataProviderBase sceneData in chapterSelectData)
                CheckValidChapterSceneData(sceneData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneData"></param>
        protected virtual void CheckValidChapterSceneData(SceneDataProviderBase sceneData)
        {
            bool shouldAdd = chapterSelectData.chapter < parentSection.GetMaxSection() ||
                (inDefaultMode && parentSection.SectionIsDefaultUnlocked() && sceneData.selectScene <= maxSection) ||
                (!inDefaultMode && SceneIsValid(sceneData)) ||
                (!inDefaultMode && sceneData.selectScene <= maxSection);

            if (shouldAdd)
                moments.Add(sceneData.selectScene);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneData"></param>
        /// <returns></returns>
        protected virtual bool SceneIsValid(SceneDataProviderBase sceneData)
        {
            if (ServiceLocator.HasService(out IChapterSelectData chapterSelectData) &&
                chapterSelectData is IIdentifiableValidator validator)
                return validator.IDValid(sceneData.ID);

            UnityEngine.Debug.LogWarning("ANALYSIS: CHAPTER SELECT IS USING ITS FALLBACK EVALUATOR.");

            return fallbackEvaluator != null && fallbackEvaluator.SceneIsValid(sceneData, parentSection);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateValidSceneMoments()
        {
            for (int i = 0; i < sectionButtons.Count; i++)
                sectionButtons[i].gameObject.SetActive(moments.Contains(i));
        }

        #endregion
    }
}