//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<ISplitScreen>) + ", " + nameof(ISplitScreen) + ", " + nameof(IPanelSendable) +
        "\n\n")]
    public class SplitScreenHandler : ServiceHolder<ISplitScreen>, ISplitScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform canvasRectTransform = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int playerIndex = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maskAnimTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float advancePointThreshold = 250;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float midPointThreshold = 50;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float retreatPointThreshold = 150;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float debugDamageAmount = 100f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<RectTransform> masks = new();


        /// <summary>
        ///
        /// </summary>
        protected readonly List<float> maskWidths = new();


        protected virtual void Start() => InitializeMaskWidths();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeMaskWidths()
        {
            foreach (var mask in masks)
                if (mask != null)
                    maskWidths.Add(mask.sizeDelta.x);
        }

        /// <inheritdoc/>
        public virtual int CalculateDomination()
        {
            ClampPlayerMask();

            RectTransform player = masks[playerIndex];

            float playerWidth = player.sizeDelta.x;

            float oldPlayerWidth = maskWidths[playerIndex];

            float advanceWidth = oldPlayerWidth + advancePointThreshold;

            float midWidth = oldPlayerWidth + midPointThreshold;

            float loseWidth = oldPlayerWidth - retreatPointThreshold;

            UnityEngine.Debug.Log($"Player: {playerWidth}, " +
                $"Advance: {oldPlayerWidth * 2} : {advanceWidth}, " +
                $"Midpoint: {advanceWidth} : {midWidth}, " +
                $"Stall: {midWidth} : {loseWidth}, " +
                $"Lose: {loseWidth} : {0}");

            if (playerWidth >= advanceWidth)
                return 1;
            else if (playerWidth >= midWidth)
                return -1;
            else if (playerWidth <= loseWidth)
                return 0;
            else
                return 2;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClampPlayerMask()
            => masks[playerIndex].sizeDelta = new Vector2(Mathf.Round(masks[playerIndex].sizeDelta.x), masks[playerIndex].sizeDelta.y);

        /// <inheritdoc/>
        public virtual void InitializePanelCount(int count)
        {
            if (count <= 0)
            {
                UnityEngine.Debug.LogError($"ALERT: Panel count must be positive, got: {count}");

                return;
            }

            if (playerIndex < 0 || playerIndex >= maskWidths.Count)
            {
                UnityEngine.Debug.LogError($"ALERT: Invalid player index: {playerIndex}");

                return;
            }

            retreatPointThreshold = maskWidths[playerIndex] / count;
        }

        /// <inheritdoc/>
        public virtual bool WillAdvanceOnTie()
        {
            ClampPlayerMask();

            return masks[playerIndex].sizeDelta.x > maskWidths[playerIndex];
        }
#if DOTWEEN
        /// <inheritdoc/>
        public virtual void AnimateToResetState(float time = 1, Ease ease = Ease.Linear)
        {
            foreach (var mask in masks)
                mask.DOSizeDelta(GetUpdatedSize(mask), time).SetEase(ease);
        }
#else
        /// <inheritdoc/>
        public virtual void AnimateToResetState() { }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        protected virtual Vector2 GetUpdatedSize(RectTransform rectTransform)
            => new(maskWidths[masks.IndexOf(rectTransform)], rectTransform.sizeDelta.y);


        /// <inheritdoc/>
        public virtual void PlayerAttack(float damage) => Attack(masks[playerIndex], damage);

        /// <inheritdoc/>
        public virtual void EnemyAttack(int enemyIDX, float damage)
        {
            if (!inputAllowed.Value || enemyIDX == playerIndex)
                return;

            Attack(masks[enemyIDX], damage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="damage"></param>
        protected virtual void Attack(RectTransform rectTransform, float damage)
        {
            if (damage == 0)
            {
                UnityEngine.Debug.LogError("ALERT: Sent split screen damage can never be 0!");

                return;
            }

            int attackerIndex = masks.IndexOf(rectTransform);

            for (int i = 0; i < masks.Count; i++)
                MoveMaskX(attackerIndex, i, damage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attackerIndex"></param>
        /// <param name="targetIndex"></param>
        /// <param name="damage"></param>
        protected virtual void MoveMaskX(int attackerIndex, int targetIndex, float damage)
        {
#if DOTWEEN
            float initialDamage = maskWidths[attackerIndex] / damage;

            float maskDamage = masks[attackerIndex] == masks[targetIndex] ? initialDamage : -initialDamage;

            float newMaskX = Mathf.Clamp(masks[targetIndex].sizeDelta.x + maskDamage, 0, canvasRectTransform.sizeDelta.x);

            masks[targetIndex].DOSizeDelta(new Vector2(newMaskX, masks[targetIndex].sizeDelta.y), maskAnimTime);
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        [BehaviourButton]
        public void DebugPlayerAttack()
        {
            if (Application.isPlaying)
                PlayerAttack(debugDamageAmount);
        }

        /// <summary>
        /// 
        /// </summary>
        [BehaviourButton]
        public void DebugEnemyAttack()
        {
            for (int i = 0; i < masks.Count; i++)
            {
                if (i != playerIndex)
                {
                    EnemyAttack(i, debugDamageAmount);

                    return;
                }
            }
        }
    }
}