//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Investigations;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.UIHandling;
using DREditor.UIHandling.Reticles;
using DREditor.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(IFireBulletUIEvent) + ", " + nameof(IReloadResolverRoutine) +
        "\n\n")]
    public class FireBulletUIHandler : ControlledComponent, IFireBulletUIEvent, IReloadResolverRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

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
        [SerializeField] protected BoolWithEvent bulletFired = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cyclePass = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent bulletEndPositionOffset = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent bulletRestPosition = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform bulletUI;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform fireObject = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float toEndPosDuration = 0.5f;


        [Header("")]
        [SerializeField] protected UnityEvent<TruthBullet> onUIBulletFired;

        public UnityEvent<TruthBullet> OnUIBulletFired { get => onUIBulletFired; set => onUIBulletFired = value; }


        /// <summary>
        ///
        /// </summary>
        protected HoverReticle nsdReticle;

        /// <summary>
        ///
        /// </summary>
        protected IBulletSwapperUI bulletHandler;


        // Functions unused because we're really just using the reverse event.
        /// <inheritdoc/>
        public override void ActivateControls() { }

        /// <inheritdoc/>
        public override void DeactivateControls() { }


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            bulletFired.SetValue(false);

            bulletRestPosition.SetValue(new(bulletRestPosition.InitialValue.Pos.DeepCopy(), Quaternion.identity));
        }


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            bulletRestPosition.Init();

            if (fireObject == null)
                fireObject = bulletUI;
            else if (fireObject != bulletUI)
                bulletRestPosition.SetValue(new(Vector3.zero, Quaternion.identity));

            nsdReticle = transform.root.GetComponentInChildren<HoverReticle>();

            bulletHandler = transform.root.GetComponentInChildren<IBulletSwapperUI>();
        }


        /// <inheritdoc/>
        public virtual void FireUITruthBullet()
        {
            if (fireObject == bulletUI && (Vector2)bulletRestPosition.Value.Pos == Vector2.zero)
                bulletRestPosition.SetValue(new(bulletRestPosition.InitialValue.Pos.DeepCopy(), Quaternion.identity));

            UnityEngine.Debug.Log(nameof(FireBulletUIHandler) + ": REPORT: FIRING BULLET.");

            canFire.SetValue(false);

            cyclePass.SetValue(false);

            Vector2 firePosition = new(
                fireObject.anchoredPosition.x + bulletEndPositionOffset.Value.Pos.x,
                fireObject.anchoredPosition.y + bulletEndPositionOffset.Value.Pos.y);
#if DOTWEEN
            fireObject.DOAnchorPos(firePosition, toEndPosDuration)
                .SetUpdate(true)
                .OnComplete(HideBulletOnComplete);
#endif
            if (nsdReticle != null)
                nsdReticle.SetLock(true);

            OnUIBulletFired?.Invoke(bulletHandler.GetCurrentBullet());

            bulletFired.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideBulletOnComplete()
        {
            if (fireObject.TryGetComponent<UIVisibilityComponent>(out var fireObjectUI) && fireObjectUI != null)
                fireObjectUI.Hide();
        }


        /// <inheritdoc/>
        public virtual IEnumerator OnReloadUIFinished()
        {
            if (!inCycle.Value && !inDialogue.Value && nsdReticle != null)
                nsdReticle.SetLock(false);

            bulletFired.SetValue(false);

            if (!inCycle.Value)
                canFire.SetValue(true);

            cyclePass.SetValue(true);

            yield break;
        }
    }
}