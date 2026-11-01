//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FlagTriggerHandlerBase) +
        "\n\n")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class FlagTriggerHandler : FlagTriggerHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected ProgressionDatabase progressionDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ProgressionFlagHandler flagHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent flagTriggered = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent requiredFlagsTriggered = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener loadProgressionData = null;


        /// <summary>
        ///
        /// </summary>
        protected ProgressionManagerBase manager = null;

        protected virtual void OnEnable() => loadProgressionData.Response.AddListener(Load);

        protected virtual void OnDisable() => loadProgressionData.Response.RemoveListener(Load);

        /// <summary>
        ///
        /// </summary>
        protected virtual void Load() => StartCoroutine(DelayLoad());

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual IEnumerator DelayLoad()
        {
            yield return null;

            requiredFlagsTriggered.SetValue(CompletedRequiredFlags());
        }


        protected virtual void Start() => manager = ServiceLocator.GetService<ProgressionManagerBase>();

        /// <inheritdoc/>
        public override void TriggerFlag(int chapterIndex, int objectiveIndex, List<int> flagIndexes)
        {
            try
            {
                bool onCurrentChapter = chapterIndex == currentChapterIndex.Value;

                Objective targetObjective = progressionDatabase.Chapters[chapterIndex].Objectives[objectiveIndex];

                bool targetIsCurrentObjective = targetObjective.Description == manager.currentObjective.Description;

                if (onCurrentChapter && targetIsCurrentObjective)
                    TriggerFlagsInCurrentObjective(flagIndexes);
                else
                    TriggerFlagsInHistory(chapterIndex, objectiveIndex, flagIndexes);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning("ALERT: A CRITICAL ERROR HAS OCCURED TRIGGERING A PROGRESSION FLAG " +
                    "\n PLEASE CHECK THE DIALOGUE ASSET TO FIX OR FIND THE ISSUE.");

                UnityEngine.Debug.LogWarning(exception);
            }
        }

        public override void TriggerFlag(List<int> flagIndexes)
        {
            var activeObjective = progressionDatabase.Chapters[currentChapterIndex.Value].GetObjective(manager.currentObjective.Description);

            int objectiveIndex = progressionDatabase.Chapters[currentChapterIndex.Value].Objectives.IndexOf(activeObjective);

            TriggerFlag(currentChapterIndex.Value, objectiveIndex, flagIndexes);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagIndexes"></param>
        protected virtual void TriggerFlagsInCurrentObjective(List<int> flagIndexes)
        {
            Objective objective = manager.currentObjective;

            for (int i = 0; i < flagIndexes.Count; i++)
            {
                if (flagIndexes[i] > objective.Flags.Count)
                {
                    flagHandler.RepairObjective(ref manager.currentObjective);

                    objective = manager.currentObjective;
                }

                objective.Flags[flagIndexes[i]].SetTriggered(true);

                flagTriggered.SetValue(objective.Flags[flagIndexes[i]].name);

                requiredFlagsTriggered.SetValue(CompletedRequiredFlags());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="objectiveIndex"></param>
        /// <param name="flagIndexes"></param>
        protected virtual void TriggerFlagsInHistory(int chapterIndex, int objectiveIndex, List<int> flagIndexes)
        {
            Objective targetObjective = progressionDatabase.Chapters[chapterIndex].Objectives[objectiveIndex];

            for (int i = 0; i < flagIndexes.Count; i++)
                flagHandler.GetFlag(targetObjective.Flags[flagIndexes[i]].name, targetObjective).SetTriggered(true);
        }

        /// <inheritdoc/>
        public override bool IsFlagTriggered(string flagName) => flagHandler.GetFlag(flagName, manager.currentObjective).triggered;

        /// <inheritdoc/>
        public override bool EvaluateVariable(int chapter, int objective, List<int> flags, List<int> falseFlags = null)
        {
            bool checkingCurrentObjective =
                progressionDatabase.Chapters[chapter].Objectives[objective].Description == manager.currentObjective.Description;

            Objective evaluateObjective = checkingCurrentObjective ?
                manager.currentObjective : ServiceLocator.GetService<IHistory>().GetHistoryObjective(chapter, objective);

            return EvaluateVariableFlags(evaluateObjective, flags, falseFlags);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        /// <param name="flags"></param>
        /// <param name="falseFlags"></param>
        /// <returns></returns>
        protected virtual bool EvaluateVariableFlags(Objective objective, List<int> flags, List<int> falseFlags = null)
        {
            for (int i = 0; i < flags.Count; i++)
                if (!objective.Flags[flags[i]].triggered)
                    return false;

            if (falseFlags != null)
                for (int i = 0; i < falseFlags.Count; i++)
                    if (objective.Flags[falseFlags[i]].triggered)
                        return false;

            return true;
        }

        /// <inheritdoc/>
        public override void TriggerAllRequiredFlags()
        {
            List<int> flagIndexes = new();

            for (int i = 0; i < manager.currentObjective.Flags.Count; i++)
                if (!manager.currentObjective.Flags[i].optional && !manager.currentObjective.Flags[i].triggered)
                    flagIndexes.Add(i);

            TriggerFlag(flagIndexes);
        }

        /// <inheritdoc/>
        protected override bool CompletedRequiredFlags()
        {
            int check = 0;

            foreach (ProgressionFlag flag in manager.currentObjective.Flags)
                if (flag.triggered && !flag.optional)
                    check++;

            return check == manager.currentObjective.GetRequiredFlagCount();
        }

        /// <inheritdoc/>
        public override ProgressionFlag GetFlagFromIndexes(int chapter, int objective, int flag)
        {
            bool onCurrentChapter = chapter == currentChapterIndex.Value;

            string sentObjectiveName = progressionDatabase.Chapters[currentChapterIndex.Value].Objectives[objective].Description;

            bool onCurrentObjective = sentObjectiveName == manager.currentObjective.Description;

            if (onCurrentChapter && onCurrentObjective)
                return manager.currentObjective.Flags[flag];
            else
                return ServiceLocator.GetService<IHistory>().GetFlagFromIndexes(chapter, objective, flag);
        }
    }
}