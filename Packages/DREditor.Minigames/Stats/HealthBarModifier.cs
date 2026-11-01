//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Saving.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HealthStatBase) +
        "\n\n")]
    public class HealthBarModifier : HealthStatBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image fillImage = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float regenHealthFillDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float takeHealthFillDuration = 1;


        /// <summary>
        ///
        /// </summary>
        protected HealthDataHandler<int> healthDataHandler = null;


        protected virtual void Start() => healthDataHandler = transform.root.GetComponentInChildren<HealthDataHandler<int>>();


        /// <inheritdoc/>
        public override void ResetHealth()
        {
#if DOTWEEN
            fillImage.DOFillAmount(1, regenHealthFillDuration);
#else
            fillImage.fillAmount = 1;
#endif
        }

        /// <inheritdoc/>
        public override void Refresh() { }

        /// <inheritdoc/>
        public override void TakeDamage(float damage)
        {
            if (!HandlerValid())
                return;
#if DOTWEEN
            fillImage.DOFillAmount(fillImage.fillAmount - (1f / healthDataHandler.GetMaxHealth()), takeHealthFillDuration);
#else
            fillImage.fillAmount = fillImage.fillAmount - (1f / healthDataHandler.GetMaxHealth());
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool HandlerValid()
        {
            bool result = healthDataHandler != null;

            if (!result)
                UnityEngine.Debug.LogError("ALERT: Health data handler is Missing!");

            return result;
        }
    }
}