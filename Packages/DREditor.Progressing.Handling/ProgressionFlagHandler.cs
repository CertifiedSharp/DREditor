//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
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
	[ClassHeader("")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionFlagHandler : MonoBehaviour
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
        /// <param name="flagName"></param>
        /// <param name="targetObjective"></param>
        /// <returns></returns>
        public virtual ProgressionFlag GetFlag(string flagName, Objective targetObjective)
        {
            if (SearchCurrentObjective(flagName, targetObjective) is ProgressionFlag flagOne && flagOne != null)
                return flagOne;


            RepairProgressionHistory(ServiceLocator.GetService<IHistory>().GetChapterHistory());

            if (ServiceLocator.GetService<IHistory>().GetFlagFromHistory(flagName) is ProgressionFlag flagTwo && flagTwo != null)
                return flagTwo;


            RepairObjective(ref targetObjective);

            if (SearchCurrentObjective(flagName, targetObjective) is ProgressionFlag flagThree && flagThree != null)
                return flagThree;


            return OnFlagNotRevived(flagName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterHistory"></param>
        protected virtual void RepairProgressionHistory(List<List<Objective>> chapterHistory)
        {
            UnityEngine.Debug.Log($"REPORT: {ToString()}: Repairing progression history.");

            foreach (List<Objective> chapterObjectives in chapterHistory)
                RepairObjectivesOnFlagMismatch(chapterObjectives);

            ServiceLocator.GetService<IHistory>().SetChapterHistory(chapterHistory);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectives"></param>
        protected virtual void RepairObjectivesOnFlagMismatch(List<Objective> objectives)
        {
            for (int i = 0; i < objectives.Count; i++)
                RepairObjectiveOnFlagMismatch(objectives[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        protected virtual void RepairObjectiveOnFlagMismatch(Objective objective)
        {
            if (ObjectiveFlagsDontMatchBase(objective))
                RepairObjective(ref objective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        /// <returns></returns>
        protected virtual bool ObjectiveFlagsDontMatchBase(Objective objective)
            => objective.Flags.Count != progressionDatabase.GetObjective(objective.Description).Flags.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagName"></param>
        /// <returns></returns>
        protected virtual ProgressionFlag OnFlagNotRevived(string flagName)
        {
            UnityEngine.Debug.LogWarning("ALERT: After repairing the current save file objective, the current " +
                "flag couldn't be found. Best to contact Sweden for support.");

            UnityEngine.Debug.LogWarning("ALERT: Since the flag couldn't be found for Flag: " + flagName +
                " the game will just return a triggered flag and use that as the default.");

            return new ProgressionFlag(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        public virtual void RepairObjective(ref Objective objective)
        {
            UnityEngine.Debug.LogWarning("REPORT: The system noticed an issue with the current objective and will now repair it." +
                "\nThis is common in scenarios where objective data from a save file is outdated or manipulated through debugging.");

            Objective currentObjectiveCopy = (Objective)objective.Clone();

            objective = (Objective)progressionDatabase.GetObjective(objective.Description).Clone();

            CopyFlagsToObjective(currentObjectiveCopy, ref objective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentObjectiveCopy"></param>
        /// <param name="targetObjective"></param>
        protected virtual void CopyFlagsToObjective(Objective currentObjectiveCopy, ref Objective targetObjective)
        {
            for (int i = 0; i < currentObjectiveCopy.Flags.Count; i++)
                ApplyFlagStateToObjectivesFlag(currentObjectiveCopy.Flags[i], ref targetObjective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="originalFlag"></param>
        /// <param name="targetObjective"></param>
        protected virtual void ApplyFlagStateToObjectivesFlag(ProgressionFlag originalFlag, ref Objective targetObjective)
        {
            foreach (ProgressionFlag currentObjectiveFlag in targetObjective.Flags)
                if (currentObjectiveFlag.name == originalFlag.name && originalFlag.triggered)
                    currentObjectiveFlag.SetTriggered(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagName"></param>
        /// <param name="currentObjective"></param>
        /// <returns></returns>
        protected virtual ProgressionFlag SearchCurrentObjective(string flagName, Objective currentObjective)
        {
            foreach (ProgressionFlag flag in currentObjective.Flags)
                if (flag.name == flagName)
                    return flag;

            UnityEngine.Debug.LogWarning("ALERT: Flag: " + flagName + " could not be found on Objective: " + 
                currentObjective.Description + " Make sure you spell the flag name correctly!");

            return null;
        }
    }
}