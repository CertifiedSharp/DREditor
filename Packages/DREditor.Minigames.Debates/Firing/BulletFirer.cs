//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
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
    public class BulletFirer : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shoot = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFire = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateBoolCondition fireBulletCondition;


        [Header("")]
        [SerializeField] protected UnityEvent onBulletFired;


        /// <summary>
        ///
        /// </summary>
        protected IFireBulletUI[] fireBulletHandlers = null;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if (debug && canFire != null)
                canFire.SetValue(true);
#endif
            fireBulletHandlers = transform.root.GetComponentsInChildren<IFireBulletUI>();
        }


        /// <inheritdoc/>
        public override void ActivateControls() => shoot.action.started += FireTruthBullet;

        /// <inheritdoc/>
        public override void DeactivateControls() => shoot.action.started -= FireTruthBullet;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void FireTruthBullet(CallbackContext context)
        {
            if (fireBulletCondition.Evaluate())
            {
                FireBulletUI();

                onBulletFired?.Invoke();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FireBulletUI()
        {
            foreach (var handler in fireBulletHandlers)
                handler.FireUITruthBullet();
        }
    }
}