//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
#endif
using DREditor.EventObjects;
using System;
using UnityEngine.Rendering;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates.Mechanics
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SpeedHandlerBase : ControlledComponent, IPanelsResolvedListener,
        IPanelsStoppedListener, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialTimeUp = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isChangingSpeed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onPhraseShot = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Volume velocityVolume = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float invokedSpeed = 5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float defaultSpeed = 1;


#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected StudioEventEmitter speedAudio = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string focusParamName = "Focus";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int invokedParamValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int defaultParamValue = 0;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioSource speedAudio = null;
#endif

        /// <summary>
        ///
        /// </summary>
        protected bool velocityCanChange = false;


        /// <summary>
        ///
        /// </summary>
        protected abstract event Action<CallbackContext> StartInput;

        /// <summary>
        ///
        /// </summary>
        protected abstract event Action<CallbackContext> StopInput;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (trialTimeUp != null)
                trialTimeUp.AddListener(OnTimeUp);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            if (trialTimeUp != null)
                trialTimeUp.RemoveListener(OnTimeUp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeUp"></param>
        protected virtual void OnTimeUp(bool timeUp)
        {
            if (timeUp)
                SetChangingSpeedState(false);
        }

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            if (inMenu != null)
                inMenu.AddListener(ToggleVelocity);

            onPhraseShot.RegisterListener(this);

            SetVelocityChangability(true);

            StartInput += StartSpeedChange;
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            if (inMenu != null)
                inMenu.RemoveListener(ToggleVelocity);

            onPhraseShot.UnregisterListener(this);

            SetVelocityChangability(false);

            StartInput -= StartSpeedChange;

            StopInput -= StopSpeedChange;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        protected virtual void ToggleVelocity(bool mode) => SetVelocityChangability(!mode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="velocityCanChange"></param>
        protected virtual void SetVelocityChangability(bool velocityCanChange) => this.velocityCanChange = velocityCanChange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void StartSpeedChange(CallbackContext context)
        {
            if (SpeedChangeShouldStart())
                SetChangingSpeedState(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SpeedChangeShouldStart() => velocityCanChange && !isChangingSpeed.Value;//&& !BulletSwapper.InCycle)

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void StopSpeedChange(CallbackContext context)
        {
            if (SpeedChangeShouldStop())
                SetChangingSpeedState(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SpeedChangeShouldStop() => isChangingSpeed.Value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSpeedState"></param>
        protected virtual void SetChangingSpeedState(bool inSpeedState)
        {
#if FMOD_UNITY
            UpdateFMODParameter(inSpeedState ? invokedParamValue : defaultParamValue);
#else
            if (inSpeedState)
                speedAudio.Play();
            else
                speedAudio.Stop();
#endif
            isChangingSpeed.SetValue(inSpeedState);

            if (velocityVolume != null)
                velocityVolume.enabled = inSpeedState;

            SetGameSpeed(inSpeedState ? invokedSpeed : defaultSpeed);

            if (inSpeedState)
                StopInput += StopSpeedChange;
            else
                StopInput -= StopSpeedChange;
        }

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterValue"></param>
        protected virtual void UpdateFMODParameter(int parameterValue)
        {
            if (focusParamName != "")
                RuntimeManager.StudioSystem.setParameterByName(focusParamName, parameterValue);

            if (speedAudio == null)
                return;

            if (parameterValue != defaultParamValue)
                speedAudio.Play();
            else
                speedAudio.Stop();
        }
#endif

        /// <summary>
        ///
        /// </summary>
        /// <param name="speed"></param>
        protected virtual void SetGameSpeed(float speed) => Time.timeScale = speed;


        /// <inheritdoc/>
        public virtual void OnPanelsStopped() => SetChangingSpeedState(false);

        /// <inheritdoc/>
        public virtual void OnPanelsResolved() => SetChangingSpeedState(false);

        /// <inheritdoc/>
        public virtual void OnEventRaised() => SetChangingSpeedState(false);
    }
}