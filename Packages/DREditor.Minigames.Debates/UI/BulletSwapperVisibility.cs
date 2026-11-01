//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(IDebateUIVisibility) +
        "\n\n")]
    public class BulletSwapperVisibility : ControlledComponent, IDebateUIVisibility
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool showHideOnControlChange = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RectTransform bulletUIParent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float selectionDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bulletMaskDelay = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 swapperUIShowEndPos = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 swapperUIHideEndPos = new(-1000, 0, 0);

#if DOTWEEN
        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            inCycle.AddListener(OnCycleStateUpdated);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            inCycle.RemoveListener(OnCycleStateUpdated);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        protected virtual void OnCycleStateUpdated(bool active)
        {
            if (active)
                HideSelect();
            else
                ShowSelect();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowSelect()
        {
            if (!InputAllowed())
                return;

            bulletUIParent.DOAnchorPos(swapperUIShowEndPos, selectionDuration)
                .SetUpdate(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideSelect()
            => bulletUIParent.DOAnchorPos(swapperUIHideEndPos, selectionDuration)
                .SetUpdate(true);
#endif

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            if (showHideOnControlChange)
                Show();
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            if (!inCycle.Value && showHideOnControlChange)
                Hide();
        }


        /// <inheritdoc/>
        public virtual void Show()
        {
#if DOTWEEN
            bulletUIParent.DOKill();

            bulletUIParent.DOAnchorPos(swapperUIShowEndPos, selectionDuration);
#endif
        }

        /// <inheritdoc/>
        public virtual void Hide()
        {
#if DOTWEEN
            bulletUIParent.DOKill();

            bulletUIParent.DOAnchorPos(swapperUIHideEndPos, selectionDuration);
#endif
        }

        /// <inheritdoc/>
        public virtual IEnumerator WaitForChangeToComplete()
        {
#if DOTWEEN
            yield return new WaitUntil(() => !DOTween.IsTweening(this));
#else
            yield return null;
#endif
        }
    }
}