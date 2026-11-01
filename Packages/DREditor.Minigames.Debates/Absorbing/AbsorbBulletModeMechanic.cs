//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Absorbing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AbsorbMechanic) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class AbsorbBulletModeMechanic : AbsorbMechanic, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inBulletMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent startBulletMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent displayBulletMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent bulletText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition absorbTickCondition = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateContextCondition<PhraseBase> phraseAbsorbTickCondition = null;


        [Header("")]
        [SerializeField] protected UnityEvent<PhraseTextBase> onAbsorbedPhrase;


        /// <summary>
        ///
        /// </summary>
        protected PhraseTextBase aimedText;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseRetriever phraseRetriever = null;


        /// <inheritdoc/>
        protected override void Validate()
        {
            base.Validate();

            phraseRetriever = transform.root.GetComponentInChildren<IPhraseRetriever>();

            if (phraseRetriever == null)
                UnityEngine.Debug.LogError($"ALERT: {nameof(phraseRetriever)} cannot be null!");
        }


        /// <inheritdoc/>
        protected override bool ShouldResetGauge() => base.ShouldResetGauge() && inBulletMode.Value;

        /// <inheritdoc/>
        protected override bool GaugeShouldTick() => base.GaugeShouldTick() &&
            (absorbTickCondition == null || absorbTickCondition.Evaluate()) &&
            phraseRetriever.TryGetAimedPhrase(out aimedText) &&
            (phraseAbsorbTickCondition == null || phraseAbsorbTickCondition.Evaluate(aimedText.textPhrase));

        /// <inheritdoc/>
        protected override float GetTickAmount() => Time.unscaledDeltaTime;

        /// <inheritdoc/>
        protected override bool ShouldTriggerGaugeFilled() => !inBulletMode.Value && base.ShouldTriggerGaugeFilled();

        /// <inheritdoc/>
        protected override void OnGaugeFilled()
        {
            onAbsorbedPhrase?.Invoke(aimedText);

            displayBulletMode.RegisterListener(this);

            startBulletMode.Raise();
        }

        /// <inheritdoc/>
        protected override void OnGaugeNotFilled() => aimedText = null;


        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            displayBulletMode.UnregisterListener(this);

            bulletText.SetValue(aimedText.textPhrase.Text);

            OnGaugeNotFilled();
        }


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            displayBulletMode.UnregisterListener(this);
        }


        protected virtual void OnDestroy() => inBulletMode.SetValue(false);
    }
}