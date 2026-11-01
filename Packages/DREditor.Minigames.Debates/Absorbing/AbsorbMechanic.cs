//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Stats;

namespace DREditor.Minigames.Debates.Absorbing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class AbsorbMechanic : GaugeHandler
    {
        protected virtual void OnEnable() => gaugeState.AddListener(OnAbsorbStateChanged);

        protected virtual void OnDisable() => gaugeState.RemoveListener(OnAbsorbStateChanged);

        /// <summary>
        ///
        /// </summary>
        /// <param name="absorbing"></param>
        protected virtual void OnAbsorbStateChanged(bool absorbing)
        {
            if (ShouldResetGauge())
                gauge?.Reset();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldResetGauge() => !gaugeState.Value;

        /// <summary>
        ///
        /// </summary>
        protected override void OnGaugeTick()
        {
            TickGauge();

            CheckGauge();
        }

        /// <inheritdoc/>
        protected override void OnFailedTick() { }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckGauge()
        {
            if (ShouldTriggerGaugeFilled())
                OnGaugeFilled();
            else
                OnGaugeNotFilled();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldTriggerGaugeFilled() => gauge.IsCharged;

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnGaugeFilled();

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnGaugeNotFilled();
    }
}