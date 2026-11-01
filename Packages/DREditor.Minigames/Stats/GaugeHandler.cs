//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GaugeHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent gaugeState = null;

        /// <summary>
        ///
        /// </summary>
        [SelectType]
        [Tooltip("")]
        [SerializeReference] protected IGauge gauge = null;


        protected virtual void Awake() => Validate();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Validate()
        {
            if (gauge == null)
                UnityEngine.Debug.LogError($"ALERT: {nameof(gauge)} cannot be null!");
        }


        protected virtual void Update()
        {
            if (GaugeShouldTick())
                OnGaugeTick();
            else
                OnFailedTick();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool GaugeShouldTick() => gaugeState.Value;

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnGaugeTick();

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnFailedTick();

        /// <summary>
        ///
        /// </summary>
        protected virtual void TickGauge() => gauge.Tick(GetTickAmount());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract float GetTickAmount();
    }
}