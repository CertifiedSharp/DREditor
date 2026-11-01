//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionInfo
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<Objective> objectives = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="objectives"></param>
        public virtual void SetObjectiveHistory(List<Objective> objectives) => this.objectives = objectives;

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        public virtual void Add(Objective objective)
        {
            if (ContainsObjective(objective.Description))
                return;

            objectives.Add(objective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectiveName"></param>
        /// <returns></returns>
        public virtual bool ContainsObjective(string objectiveName)
        {
            for (int i = 0; i < objectives.Count; i++)
                if (objectives[i].Description == objectiveName)
                    return true;

            return false;
        }
    }
}