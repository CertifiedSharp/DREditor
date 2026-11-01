//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
#endif
using DREditor.Audio;
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Services;
using DREditor.Investigations;
using DREditor.UIHandling;
using DREditor.UIHandling.Reticles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(IBulletSendable) + ", " + nameof(IPanelsResolvedListener) +
        "\n\n")]
    public class BulletSwapper : ControlledComponent, IBulletSendable, IPanelsResolvedListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference cycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canCycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFire = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cyclePass = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialTimeUp = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected EvaluateBoolCondition preventCycleCondition = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setBoolsOnControlActivate = false;


        [Header("")]
        [Divider]
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [EventRef]
        [SerializeField] protected string bulletSelectInSFX;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [EventRef]
        [SerializeField] protected string bulletSelectOutSFX;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip bulletSelectInSFX;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip bulletSelectOutSFX;
#endif

        /// <summary>
        ///
        /// </summary>
        protected HoverReticle nsdReticle;

        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> truthBullets = new();

        /// <summary>
        ///
        /// </summary>
        protected IBulletCycleUI[] cyclables;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            var allComponents = transform.root.GetComponentsInChildren<Component>();

            cyclables = allComponents.OfType<IBulletCycleUI>().ToArray();

            nsdReticle = allComponents.OfType<HoverReticle>().FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            InitializeDebugging();
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDebugging()
        {
            if (debug)
            {
                EnableInput();

                canCycle.SetValue(true);

                canFire.SetValue(true);

                cyclePass.SetValue(true);
            }
        }
#endif

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            cycle.action.started += StartCycle;

            cycle.action.canceled += EndCycle;

            cyclePass.SetValue(true);

            if (setBoolsOnControlActivate)
            {
                canCycle.SetValue(true);

                canFire.SetValue(true);
            }

            inMenu.AddListener(ToggleCanFire);

            trialTimeUp.AddListener(OnTimeUp);
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            cycle.action.started -= StartCycle;

            cycle.action.canceled -= EndCycle;

            if (inCycle.Value)
                EndCycle(new CallbackContext());

            inMenu.RemoveListener(ToggleCanFire);

            trialTimeUp.RemoveListener(OnTimeUp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        protected virtual void ToggleCanFire(bool mode) => canFire.SetValue(!mode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeUp"></param>
        protected virtual void OnTimeUp(bool timeUp)
        {
            if (timeUp && inCycle.Value)
                EndCycle(new CallbackContext());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void StartCycle(CallbackContext context)
        {
            if (truthBullets.Count == 1 || 
                !cyclePass.Value || 
                (preventCycleCondition != null && preventCycleCondition.Evaluate()) || 
                inMenu.Value)
                return;

            InvokeStartCycle();

            ServiceLocator.GetService<SFXManagerBase>().PlaySFX(bulletSelectInSFX);

            UpdateCycleStates(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeStartCycle()
        {
            foreach (var cyclable in cyclables)
                cyclable.StartCycle();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void EndCycle(CallbackContext context)
        {
            if (truthBullets.Count == 1 || 
                !cyclePass.Value ||
                (preventCycleCondition != null && preventCycleCondition.Evaluate()) ||
                inMenu.Value)
                return;

            InvokeEndCycle();

            ServiceLocator.GetService<SFXManagerBase>().PlaySFX(bulletSelectOutSFX);

            UpdateCycleStates(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeEndCycle()
        {
            foreach (var cyclable in cyclables)
                cyclable.EndCycle();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cycleActive"></param>
        protected virtual void UpdateCycleStates(bool cycleActive)
        {
            if (nsdReticle != null && InputAllowed())
                nsdReticle.SetLock(cycleActive);

            canFire.SetValue(!cycleActive);

            inCycle.SetValue(cycleActive);

            canCycle.SetValue(cycleActive);
        }


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets) => truthBullets = bullets;

        /// <inheritdoc/>
        public virtual void OnPanelsResolved()
        {
            canFire.SetValue(false);

            if (inCycle.Value)
                EndCycle(new CallbackContext());
        }
    }
}