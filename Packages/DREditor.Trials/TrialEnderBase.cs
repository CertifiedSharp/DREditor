//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TrialEnderBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        public virtual IEnumerator EndTrial(ScriptableObject builder)
        {
            Initialize(builder);

            if (TrialEnderWillFail())
                yield break;

            yield return PreEndTrialProcess();

            EvaluateLeaveTrial();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void Initialize(ScriptableObject builder);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool TrialEnderWillFail();

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator PreEndTrialProcess();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateLeaveTrial()
        {
            if (PlayerCanLeaveTrial())
                LeaveTrial();
            else
                CannotLeaveTrial();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool PlayerCanLeaveTrial();

        /// <summary>
        ///
        /// </summary>
        protected abstract void LeaveTrial();

        /// <summary>
        ///
        /// </summary>
        protected abstract void CannotLeaveTrial();
    }
}