//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public abstract class FlagTriggerHandlerBase : ServiceHolder<FlagTriggerHandlerBase>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="objectiveIndex"></param>
        /// <param name="flagIndexes"></param>
        public abstract void TriggerFlag(int chapterIndex, int objectiveIndex, List<int> flagIndexes);

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagIndexes"></param>
        public abstract void TriggerFlag(List<int> flagIndexes);

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagName"></param>
        /// <returns></returns>
        public abstract bool IsFlagTriggered(string flagName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objective"></param>
        /// <param name="flags"></param>
        /// <param name="falseFlags"></param>
        /// <returns></returns>
        public abstract bool EvaluateVariable(int chapter, int objective, List<int> flags, List<int> falseFlags = null);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool CompletedRequiredFlags();

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objective"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public abstract ProgressionFlag GetFlagFromIndexes(int chapter, int objective, int flag);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public abstract void TriggerAllRequiredFlags();
    }
}