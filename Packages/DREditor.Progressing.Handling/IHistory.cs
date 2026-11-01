//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public interface IHistory : IPersistentData, IClearableOnNewGameSelected
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="objective"></param>
        void Add(int chapterIndex, Objective objective);

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectiveName"></param>
        /// <returns></returns>
        bool ContainsObjective(string objectiveName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        int GetAmountOfFinishedObjectives();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<List<Objective>> GetChapterHistory();

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagName"></param>
        /// <returns></returns>
        ProgressionFlag GetFlagFromHistory(string flagName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objective"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        ProgressionFlag GetFlagFromIndexes(int chapter, int objective, int flag);

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objective"></param>
        /// <returns></returns>
        Objective GetHistoryObjective(int chapter, int objective);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Objective GetLastObjective();

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterHistory"></param>
        void SetChapterHistory(List<List<Objective>> chapterHistory);

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        void SetChapterIndex(int chapter);

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        void SetPreviousObjective(Objective objective);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedChapter"></param>
        /// <returns></returns>
        int GetObjectiveCountFromHistory(int selectedChapter);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        int LatestChapterIndex();
    }
}