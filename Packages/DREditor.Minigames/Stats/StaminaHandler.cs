//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(GaugeHandler) +
        "\n\n")]
    public class StaminaHandler : GaugeHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasStamina = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float staminaRatio = 2f;


        /// <summary>
        ///
        /// </summary>
        protected IShatterGraphic shatter;

        /// <summary>
        ///
        /// </summary>
        protected InfluenceGaugeBase influenceGauge;


        protected virtual void Start()
        {
            influenceGauge = ServiceLocator.GetService<InfluenceGaugeBase>();

            shatter = transform.root.GetComponentInChildren<IShatterGraphic>();

            shatter?.OnPostShatter?.AddListener(influenceGauge.ResetStamina);
        }

        protected virtual void OnDisable() => shatter?.OnPostShatter?.RemoveListener(influenceGauge.ResetStamina);


        /// <summary>
        ///
        /// </summary>
        protected override void OnGaugeTick()
        {
            if (CanDrainStamina())
            {
                TickGauge();

                DrainStaminaVisual();
            }
            else
                UpdateStaminaEmpty();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanDrainStamina() => !gauge.IsEmpty;

        /// <inheritdoc/>
        protected override float GetTickAmount()
        {
            if (GaugeShouldTick() && CanDrainStamina())
                return (staminaRatio * Time.unscaledDeltaTime) * -1;
            else if (!GaugeShouldTick() && CanRestoreStamina())
                return staminaRatio * Time.unscaledDeltaTime;

            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrainStaminaVisual()
        {
            if (influenceGauge != null)
                influenceGauge.DrainStaminaVisual();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateStaminaEmpty()
        {
            if (hasStamina.Value)
                hasStamina.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnFailedTick()
        {
            if (CanRestoreStamina())
            {
                TickGauge();

                RegenStaminaVisual();
            }
            
            if (CanDrainStamina())
                UpdateHasStamina();

            CheckBrokenStamina();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanRestoreStamina() => !gauge.IsCharged;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateHasStamina()
        {
            if (!hasStamina.Value)
                hasStamina.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RegenStaminaVisual()
        {
            if (influenceGauge != null)
                influenceGauge.RegenStaminaVisual();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckBrokenStamina()
        {
            if (gauge.IsCharged && influenceGauge != null && influenceGauge.StaminaVisualWrong())
                influenceGauge.RegenStaminaVisual();
        }
    }
}