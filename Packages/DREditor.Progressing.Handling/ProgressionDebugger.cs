//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [DefaultExecutionOrder(1)]
	[ClassHeader("")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionDebugger : MonoBehaviour
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
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onGeneratePastData = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool testLatestObjective = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int startObjective = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int clearedFlags = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool generatePastData = false;


        /// <summary>
        ///
        /// </summary>
        protected ProgressionManagerBase manager = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => onGeneratePastData.Response.AddListener(GeneratePast);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => onGeneratePastData.Response.RemoveListener(GeneratePast);

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            manager = ServiceLocator.GetService<ProgressionManagerBase>();

            InitializeStartObjective();

            TriggerFlagsInCurrentObjective();

            InitializePastData();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeStartObjective()
        {
            if (manager == null)
                manager = ServiceLocator.GetService<ProgressionManagerBase>();

            if (manager.GetCurrentChapter() == null)
                return;

            startObjective = testLatestObjective ? manager.GetCurrentChapter().Objectives.Count - 1 : startObjective;

            if (manager.GetCurrentChapter().Objectives.Count > 0)
                manager.SetCurrentObjective((Objective)manager.GetCurrentChapter().Objectives[startObjective].Clone());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TriggerFlagsInCurrentObjective()
        {
            if (manager.currentObjective != null && clearedFlags > 0 && clearedFlags <= manager.currentObjective.Flags.Count)
                for (int i = 0; i < clearedFlags; i++)
                    manager.currentObjective.Flags[i].SetTriggered(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePastData()
        {
            if (generatePastData)
                GeneratePast();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void GeneratePast()
        {
            for (int chapterIndex = 0; chapterIndex <= currentChapterIndex.Value; chapterIndex++)
                GenerateChapterObjectives(chapterIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        protected void GenerateChapterObjectives(int chapterIndex)
        {
            foreach (Objective objective in progressionDatabase.Chapters[chapterIndex].Objectives)
            {
                if (objective.Description == manager.currentObjective.Description)
                    break;

                GenerateCompletedObjectiveToHistory(chapterIndex, objective);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="objective"></param>
        protected void GenerateCompletedObjectiveToHistory(int chapterIndex, Objective objective)
        {
            Objective clonedObjective = (Objective)objective.Clone();

            TriggerAllRequiredFlags(clonedObjective.Flags);

            ServiceLocator.GetService<IHistory>().Add(chapterIndex, clonedObjective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flags"></param>
        protected void TriggerAllRequiredFlags(List<ProgressionFlag> flags)
        {
            foreach (ProgressionFlag flag in flags)
                if (!flag.optional)
                    flag.SetTriggered(true);
        }
    }
}