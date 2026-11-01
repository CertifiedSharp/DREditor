//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using DREditor.Progressing.Handling;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ChapterSelectFallbackEvaluator) + ", " + nameof(IDRELoadableManager) +
        "\n\n")]
    public class ChapterSceneByLatestObjective : ChapterSelectFallbackEvaluator, IDRELoadableManager
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
        [SerializeField] protected ProgressionDatabase progressionDatabase = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string objectiveKey = "CurrentObjective";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string progressionDataKey = "ProgressionData";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string deathsKey = "deadList";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string chapterKey = "chapter";


        /// <summary>
        ///
        /// </summary>
        protected Objective loadedCurrentObjective = null;

        /// <summary>
        ///
        /// </summary>
        protected int loadedProgressionIndex = -1;

        /// <summary>
        ///
        /// </summary>
        protected List<string> loadedDeaths = new();

        /// <summary>
        ///
        /// </summary>
        protected int loadedChapter = 0;


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

            loadedChapter = saveDatas.Get<int>(chapterKey);

            loadedCurrentObjective = saveDatas.Get<Objective>(objectiveKey);

            loadedProgressionIndex = ServiceLocator.GetService<IHistory>().GetObjectiveCountFromHistory(loadedChapter) - 1;

            if (saveDatas.TryGetValue(deathsKey, out object dead) && dead is List<string> deaths)
                loadedDeaths = deaths;
        }


        /// <inheritdoc/>
        public override int GetMaxScene(ChapterSectionBase parentSection)
        {
            int loadedObjectiveIndex = progressionDatabase.GetObjectiveInt(loadedChapter, loadedCurrentObjective, loadedProgressionIndex);

            if (loadedObjectiveIndex < 0)
                UnityEngine.Debug.LogError($"ALERT: Chapter Select: A problem has arisen in the {nameof(ChapterSceneHandler)}");

            int maxChapter = parentSection.GetMaxSection();

            Chapter chapter = progressionDatabase.Chapters[maxChapter];

            for (int i = 0; i < chapterSelectDatabase[maxChapter].GetDataCount(); i++)
            {
                SceneDataProviderBase data = chapterSelectDatabase[maxChapter][i];

                var saveDict = ((SerializableDictionary)data.GetData()).ToDictionary();

                Objective saveObjective = chapter.GetObjective(saveDict.Get<Objective>(objectiveKey).Description);

                int sceneObjectiveIndex = chapter.Objectives.IndexOf(saveObjective);

                List<string> sceneDeaths = saveDict.Get<List<string>>(deathsKey);

                if (sceneObjectiveIndex > loadedObjectiveIndex ||
                    (sceneObjectiveIndex <= loadedObjectiveIndex && HasMoreDead(sceneDeaths)))
                    return chapterSelectDatabase[maxChapter][i - 1].selectScene;
                else if (sceneObjectiveIndex == loadedObjectiveIndex && MatchesDead(sceneDeaths))
                    return data.selectScene;
            }

            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneDeaths"></param>
        /// <returns></returns>
        protected virtual bool HasMoreDead(List<string> sceneDeaths) => sceneDeaths == null || sceneDeaths.Count > loadedDeaths.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneDeaths"></param>
        /// <returns></returns>
        protected virtual bool MatchesDead(List<string> sceneDeaths) => sceneDeaths == null || sceneDeaths.Count == loadedDeaths.Count;


        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneData"></param>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool SceneIsValid(SceneDataProviderBase sceneData, ChapterSectionBase parentSection)
        {
            if (sceneData.GetData() is SerializableDictionary dataEntries &&
                dataEntries.ToDictionary() is Dictionary<string, object> sceneSaveDatas)
            {
                var sceneObjective = sceneSaveDatas.Get<Objective>(objectiveKey);

                int sceneObjectiveIndex = progressionDatabase.GetObjectiveIndex(loadedChapter, sceneObjective);

                try
                {
                    int historyObjectiveCount = ServiceLocator.GetService<IHistory>()
                        .GetObjectiveCountFromHistory(parentSection.GetSelectedSection());

                    return loadedChapter >= parentSection.DefaultUnlockedSections && sceneObjectiveIndex <= historyObjectiveCount;
                }
                catch (Exception exception)
                {
                    UnityEngine.Debug.LogWarning(ToString() + ": ALERT: There was an issue evaluating the progression history.");

                    UnityEngine.Debug.LogException(exception);

                    return false;
                }
            }

            return false;
        }
    }
}