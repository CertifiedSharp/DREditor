//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public abstract class ProgressionManagerBase : ServiceHolder<ProgressionManagerBase>
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
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Chapter currentChapter = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Objective currentObjective = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            InitializeCurrentChapter();

            CleanChapterAsset();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeCurrentChapter()
        {
            if (CurrentChapterCanBeAssigned())
                currentChapter = progressionDatabase.Chapters[0];
            else
                UnityEngine.Debug.LogError("ALERT: You must have a valid progression database with a chapter and objective assigned!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool CurrentChapterCanBeAssigned() =>
            progressionDatabase != null && progressionDatabase.Chapters.Count > 0 && progressionDatabase.Chapters[0] != null && currentChapter == null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void CleanChapterAsset()
        {
            if (currentChapter == null)
                return;

            foreach (Objective objective in currentChapter.Objectives)
                foreach (ProgressionFlag flag in objective.Flags)
                    flag.SetTriggered(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Chapter GetCurrentChapter() => currentChapter;

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objectiveName"></param>
        /// <returns></returns>
        public abstract bool PassedPoint(int chapter, string objectiveName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        public virtual void SetCurrentChapter(Chapter chapter) => currentChapter = chapter;

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        /// <remarks>
        ///
        /// </remarks>
        public virtual void SetCurrentObjective(Objective objective) => currentObjective = (Objective)objective.Clone();

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseObjective"></param>
        /// <param name="saveObjective"></param>
        /// <remarks></remarks>
        public virtual void ResetCurrentObjective(Objective baseObjective, Objective saveObjective)
        {
            SetCurrentObjective((Objective)baseObjective.Clone());

            CopyFlagTriggers(saveObjective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveObjective"></param>
        /// <remarks></remarks>
        protected virtual void CopyFlagTriggers(Objective saveObjective)
        {
            for (int i = 0; i < saveObjective.Flags.Count && i < currentObjective.Flags.Count; i++)
                currentObjective.Flags[i].SetTriggered(saveObjective.Flags[i].triggered);
        }
    }
}