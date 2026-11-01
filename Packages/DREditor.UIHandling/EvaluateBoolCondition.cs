//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(EvaluateCondition) +
        "\n\n")]
    public class EvaluateBoolCondition : EvaluateCondition
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool any = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<MatchBoolEventState> events = new() { new MatchBoolEventState() };


        /// <inheritdoc/>
        protected override bool OnEvaluateCondition()
        {
            bool result = AllConditionsMatch();

            LogStatesOnFail(result);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AllConditionsMatch()
        {
            foreach (MatchBoolEventState state in events)
                if (!any && !state.Matches())
                    return false;
                else if (any && state.Matches())
                    return true;

            return !any;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        protected virtual void LogStatesOnFail(bool result)
        {
            if (debug && !result)
                LogStates();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LogStates()
        {
            foreach (MatchBoolEventState state in events)
                UnityEngine.Debug.Log(gameObject.name + ": State: " + state.LogState());
        }
    }
}