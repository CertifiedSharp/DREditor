//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionData : IHistory
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int chapter;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Objective previousObjective;


        /// <remarks></remarks>
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("Info")]
        public List<ProgressionInfo> Infos = new();

        public ProgressionData() { }

        /// <inheritdoc/>
        public virtual void CarryOver(IPersistentData persistentData)
        {
            if (persistentData is ProgressionData oldData)
                Infos = oldData.Infos;
            else
                UnityEngine.Debug.LogWarning("ALERT: TRIED TO SET PROGRESSION DATA OF A DIFFERENT TYPE!");
        }

        /// <inheritdoc/>
        public virtual void SetChapterIndex(int chapter) => this.chapter = chapter;


        /// <inheritdoc/>
        public virtual void SetPreviousObjective(Objective objective) => previousObjective = objective;

        /// <inheritdoc/>
        public virtual void Add(int chapterIndex, Objective objective)
        {
            RecursiveAdd(chapterIndex);

            Infos[chapterIndex].Add(objective);
        }

        protected virtual void RecursiveAdd(int chapterIndex)
        {
            if (chapterIndex >= Infos.Count)
            {
                Infos.Add(new ProgressionInfo());

                RecursiveAdd(chapterIndex);
            }
        }

        /// <inheritdoc/>
        public virtual void Clear() => Infos.Clear();

        /// <inheritdoc/>
        public virtual Objective GetLastObjective()
        {
            if (previousObjective != null)
                return previousObjective;

            if (Infos.Count == 0)
                return null;

            return Infos.Last().objectives.Last();
        }

        /// <inheritdoc/>
        public virtual bool ContainsObjective(string objectiveName)
        {
            for (int i = 0; i < Infos.Count; i++)
                if (Infos[i].ContainsObjective(objectiveName))
                    return true;

            return false;
        }

        /// <inheritdoc/>
        public virtual ProgressionFlag GetFlagFromHistory(string flagName)
        {
            IEnumerable<ProgressionInfo> chaptersContainingFlag =
                Infos.Where(chapter => chapter.objectives.Where(objective => objective.GetFlagNames()
                .Contains(flagName)).Count() > 0);

            if (chaptersContainingFlag.Count() > 0)
            {
                var firstObjectiveContainingFlag =
                    chaptersContainingFlag.ElementAt(0).objectives.Where(n => n.GetFlagNames().Contains(flagName)).ElementAt(0);

                var matchingFlags = firstObjectiveContainingFlag.Flags.Where(n => n.name == flagName);

                if (matchingFlags.Count() > 0 && matchingFlags.ElementAt(0) != null)
                    return matchingFlags.ElementAt(0);
            }

            UnityEngine.Debug.LogWarning("After Checking this save files previously completed Objectives, " +
                    "that flag couldn't be found. \n" +
                "Which means one has occured: There's been user error building progression, " +
                "The Searched progression flag string used was misspelt, OR the save file never completed or had the objective " +
                "that contained the searched flag. \n" +
                "If you still encounter this error after making sure Debug Viewing " +
                "Progression data contains the flag, and the searched flag " +
                "was not misspelt, you should contact Sweden in order to debug and find the problem.");

            return null;
        }

        /// <inheritdoc/>
        public virtual List<List<Objective>> GetChapterHistory()
        {
            List<List<Objective>> chapterHistory = new();

            foreach (var chapter in Infos)
                chapterHistory.Add(chapter.objectives);

            return chapterHistory;
        }

        /// <inheritdoc/>
        public virtual void SetChapterHistory(List<List<Objective>> chapterHistory)
        {
            foreach (var chapter in Infos)
                chapter.SetObjectiveHistory(chapterHistory[Infos.IndexOf(chapter)]);
        }

        /// <inheritdoc/>
        public virtual Objective GetHistoryObjective(int chapter, int objective) => Infos[chapter].objectives[objective];

        /// <inheritdoc/>
        public virtual int GetAmountOfFinishedObjectives()
            => Infos.Count > chapter ? Infos[chapter].objectives.Count : 0;

        /// <inheritdoc/>
        public virtual int GetObjectiveCountFromHistory(int selectedChapter)
        {
            if (selectedChapter < Infos.Count && selectedChapter >= 0)
                return Infos[selectedChapter].objectives.Count;

            UnityEngine.Debug.LogWarning($"REPORT: The selected chapter index: {selectedChapter} " +
                $"does not have progression history! Is there Progression data missing?");

            return 0;
        }

        /// <inheritdoc/>
        public virtual int LatestChapterIndex()
        {
            int history = Infos.Count;

            --history;

            return Math.Max(chapter, history);
        }

        /// <inheritdoc/>
        public virtual ProgressionFlag GetFlagFromIndexes(int chapter, int objective, int flag)
            => GetHistoryObjective(chapter, objective).Flags[flag];
    }
}