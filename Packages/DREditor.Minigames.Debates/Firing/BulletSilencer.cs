//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.EventObjects;
using DREditor.UIHandling;
using DREditor.UIHandling.Reticles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) +
        "\n\n")]
    public class BulletSilencer : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference silence = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputModifierType silencable = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition canFireCondition = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float timeRemovedOnHitPhrase = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float timeAddedOnHitWN = 5;


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent OnHitWhiteNoise = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnHitNonWhiteNoise = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnMissedWhiteNoise = null;


        /// <summary>
        ///
        /// </summary>
        protected TrialTimerBase TrialTimer => transform.root.GetComponentInChildren<TrialTimerBase>();

        /// <summary>
        ///
        /// </summary>
        protected HoverReticle reticle = null;

        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera trialCameraComponent;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseRetriever phraseRetriever = null;

        /// <summary>
        ///
        /// </summary>
        protected PhraseTextBase aimedText;

        /// <summary>
        ///
        /// </summary>
        protected InputModifier inputableModifier;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseShatterable shatterable;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            trialCameraComponent = FindAnyObjectByType<DRTrialCamera>().GetComponentInChildren<UnityEngine.Camera>();

            reticle = transform.root.GetComponentInChildren<HoverReticle>();

            phraseRetriever = transform.root.GetComponentInChildren<IPhraseRetriever>();
        }


        /// <inheritdoc/>
        public override void ActivateControls()
            => silence.action.started += FireSilencer;

        /// <inheritdoc/>
        public override void DeactivateControls()
            => silence.action.started -= FireSilencer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void FireSilencer(CallbackContext context)
        {
            if (!SilencerCanFire())
                return;

            if (SilencerHitSomething())
                OnSilencerFired();
            else
                OnSilencerMissed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool SilencerCanFire() => canFireCondition != null && canFireCondition.Evaluate();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SilencerHitSomething() => 
            phraseRetriever.TryGetAimedPhrase(out aimedText) && 
            aimedText.TryGetComponent(out shatterable) &&
            !shatterable.IsShattering();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSilencerFired()
        {
            if (CorrectTargetTypeHit())
                OnSilencerCorrectTargetHit();
            else
                OnSilencerWrongTargetHit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CorrectTargetTypeHit() =>
            aimedText != null &&
            aimedText.textPhrase != null &&
            aimedText.textPhrase.TryGetModifier(out inputableModifier) &&
            inputableModifier.inputModifiers.Contains(silencable) &&
            reticle.HitPoint.transform.parent != null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSilencerCorrectTargetHit()
        {
            TrialTimer.AddTime(timeAddedOnHitWN);

            OnHitWhiteNoise?.Invoke();

            shatterable?.Shatter(reticle.HitPoint.point);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSilencerWrongTargetHit()
        {
            TrialTimer.RemoveTime(timeRemovedOnHitPhrase);

            OnHitNonWhiteNoise?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSilencerMissed() => OnMissedWhiteNoise?.Invoke();
    }
}