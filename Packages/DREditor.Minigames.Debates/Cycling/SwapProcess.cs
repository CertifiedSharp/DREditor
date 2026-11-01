//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
#endif
#if DOTWEEN
using DG.Tweening;
using DREditor.Services;
using DREditor.Audio;
#endif
using DREditor.EventObjects;
using DREditor.Investigations;
using DREditor.Minigames.Debates.Mechanics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SwapProcessBase) + ", " + nameof(ControlledComponent) + ", " +
        nameof(IBulletSendable) + ", " + nameof(IBulletSwapperUI) +
        "\n\n")]
    public class SwapProcess : SwapProcessBase, IBulletSwapperUI
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentBulletIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent bulletFired = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent setBulletText = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useSqueezeAnim = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maskStartX = 90;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maskEndX = 1050;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(UsingSqueezeAnimation))]
        [Tooltip("")]
        [SerializeField] protected float squeezeStartX = 537.57f;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(UsingSqueezeAnimation))]
        [Tooltip("")]
        [SerializeField] protected float squeezeEndX = 705.13f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float changeDuration = 0.2f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RectTransform bulletMask;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(UsingSqueezeAnimation))]
        [Tooltip("")]
        [SerializeField] protected Image mainBullet = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BulletIndicatorsBase indicators = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
#if FMOD_UNITY
        [Tooltip("")]
        [EventRef]
        [SerializeField] protected string bulletChangeSound;
#else
        [Tooltip("")]
        [SerializeField] protected AudioClip bulletChangeSound;
#endif

        /// <summary>
        ///
        /// </summary>
        protected bool isUp;

        /// <summary>
        ///
        /// </summary>
        protected ICycleListener[] cycleListeners;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            if (debug)
                canFire.SetValue(true);
#endif
            SetBulletIndex(0);
        }

        /// <inheritdoc/>
        protected override void SwapBullet(bool isUp)
        {
            if (truthBullets.Count == 1)
                return;

            this.isUp = isUp;

            canFire.SetValue(false);

            SetChangingBullet(true);

            InvokeCyclables();

            StartCoroutine(SwapRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeCyclables()
        {
            cycleListeners ??= transform.root.GetComponentsInChildren<ICycleListener>();

            foreach (var cyclable in cycleListeners)
                cyclable.OnBulletCycled(isUp);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SwapRoutine()
        {
            if (inCycle.Value)
                canCycle.SetValue(false);

            DisplayBulletActive(false);

            SwapActiveBullet();

            DisplayBulletActive(true);

            yield return StartCoroutine(ChangeBulletAnim());

            if (inCycle.Value)
                canCycle.SetValue(true);
            else if (!bulletFired.Value)
                canFire.SetValue(true);

            SetChangingBullet(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isActive"></param>
        protected virtual void DisplayBulletActive(bool isActive)
        {
            if (indicators != null)
                indicators.SetIndicator(currentBulletIndex.Value, isActive);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SwapActiveBullet() => SetBulletIndex(currentBulletIndex.Value + (isUp ? -1 : 1));

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ChangeBulletAnim()
        {
#if DOTWEEN
            bulletMask.DOSizeDelta(new Vector2(maskStartX, bulletMask.sizeDelta.y), changeDuration)
                .SetUpdate(true);

            ServiceLocator.GetService<SFXManagerBase>().PlaySFX(bulletChangeSound);

            if (UsingSqueezeAnimation())
                mainBullet.rectTransform.DOSizeDelta(new Vector2(squeezeStartX, mainBullet.rectTransform.sizeDelta.y), changeDuration)
                    .SetUpdate(true);
#endif

            yield return new WaitForSecondsRealtime(changeDuration / 2);

            setBulletText.Raise();
#if DOTWEEN
            bulletMask.DOSizeDelta(new Vector2(maskEndX, bulletMask.sizeDelta.y), changeDuration)
                .SetUpdate(true);

            if (UsingSqueezeAnimation())
                mainBullet.rectTransform.DOSizeDelta(new Vector2(squeezeEndX, mainBullet.rectTransform.sizeDelta.y), changeDuration)
                    .SetUpdate(true);
#endif
            yield return new WaitForSecondsRealtime(changeDuration / 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool UsingSqueezeAnimation() => useSqueezeAnim;

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        protected virtual void SetBulletIndex(int index)
        {
            int finalIndex;

            if (index >= truthBullets.Count)
                finalIndex = 0;
            else if (index <= -1)
                finalIndex = truthBullets.Count - 1;
            else
                finalIndex = index;

            currentBulletIndex.SetValue(finalIndex);
        }


        /// <inheritdoc/>
        public virtual TruthBullet GetCurrentBullet()
        {
            if (truthBullets == null || truthBullets.Count == 0)
                return null;

            return truthBullets[currentBulletIndex.Value];
        }


        protected virtual void OnDestroy() => SetBulletIndex(0);
    }
}