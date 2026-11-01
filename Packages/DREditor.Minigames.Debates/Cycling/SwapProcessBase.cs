//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
#endif
using DREditor.EventObjects;
using DREditor.Input;
using DREditor.Services;
using DREditor.Investigations;
using DREditor.UIHandling;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SwapProcessBase : ControlledComponent, IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference change = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference navigation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFire = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canCycle = null;


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
        [SerializeField] protected bool scrollOnly = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float changeBulletYMidpoint = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string scrollActionName = "scroll";


        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> truthBullets = new();

        /// <summary>
        ///
        /// </summary>
        protected bool changingBullet = false;


        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        protected virtual void SetChangingBullet(bool to) => changingBullet = to;


        /// <inheritdoc/>
        public override void ActivateControls() => change.action.started += ChangeBullet;

        /// <inheritdoc/>
        public override void DeactivateControls() => change.action.started -= ChangeBullet;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ChangeBullet(CallbackContext context)
        {
            if (CancelSwap(context))
                return;

            bool changePrevented = preventCycleCondition != null && preventCycleCondition.Evaluate();

            if (!changingBullet && (canFire.Value && !changePrevented || inCycle.Value && canCycle.Value))
            {
                float yInput;

                if (inCycle.Value && canCycle.Value)
                    yInput = change.action.ReadValue<Vector2>().y;
                else if (!ServiceLocator.GetService<IInputManager>().UsingGamepadScheme())
                    yInput = navigation.action.ReadValue<Vector2>().y;
                else
                    return;

                if (yInput > changeBulletYMidpoint)
                    SwapBullet(true);
                else if (yInput < -changeBulletYMidpoint)
                    SwapBullet(false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool CancelSwap(CallbackContext context) =>
            truthBullets.Count == 1 || (!inCycle.Value && scrollOnly && context.control.name != scrollActionName);


        /// <summary>
        ///
        /// </summary>
        /// <param name="isUp"></param>
        protected abstract void SwapBullet(bool isUp);


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets) => truthBullets = bullets;
    }
}