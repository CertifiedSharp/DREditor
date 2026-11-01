// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.CA.Stocks
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class ShootCAStockVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform whitePanel = null;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector3 startingLocalPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 startingLocalScale = new(0.8f, 0.8f, 1);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent caPanelTarget = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 targetLocalScale = new(0.2f, 0.2f, 1);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float positionSpeedDivisor = 0.33333f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float scaleSpeedMultiplier = 2.7f;


        /// <summary>
        ///
        /// </summary>
        protected float positionSpeed;


        /// <inheritdoc/>
        protected virtual void Start() => HideUI();

        protected override void HideUI() => whitePanel.gameObject.SetActive(false);

        /// <inheritdoc/>
        protected override void ShowUI()
        {
            whitePanel.gameObject.SetActive(true);

            whitePanel.localPosition = startingLocalPosition;

            whitePanel.localScale = startingLocalScale;
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            InitializePositionSpeed();

            while (NotAtTargetPosition())
            {
                UpdateAnimation();

                yield return null;
            }

            HideUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePositionSpeed() =>
            positionSpeed = (caPanelTarget.Value.Pos - whitePanel.position).magnitude / positionSpeedDivisor;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool NotAtTargetPosition() => whitePanel.position != caPanelTarget.Value.Pos;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateAnimation()
        {
            UpdateLocalPosition();

            UpdateLocalScale();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateLocalPosition() =>
            whitePanel.position = Vector3
            .MoveTowards(whitePanel.position, caPanelTarget.Value.Pos, positionSpeed * Time.deltaTime);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateLocalScale() =>
             whitePanel.localScale = Vector3
            .MoveTowards(whitePanel.localScale, targetLocalScale, scaleSpeedMultiplier * Time.deltaTime);
    }
}