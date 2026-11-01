//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IReloadBulletUI) +
        "\n\n")]
    public class ReloadBulletUIHandler : MonoBehaviour, IReloadBulletUI
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isReloading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialMistake = null;

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
        [SerializeField] protected SceneEvent onReloadStarted;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent setBulletText = null;

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
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float toRestPositionDuration = 0.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float toRestPositionDelay = 0.1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float reloadDelayTime = 0f;


        [Header("")]
        [SerializeField] protected UnityEvent onReloading = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector2 startOfReloadResetPosition;


        /// <summary>
        ///
        /// </summary>
        protected IReloadResolverRoutine[] reloadResolvers;


        protected virtual void OnEnable() => trialMistake.AddListener(MistakeUpdated);

        protected virtual void OnDisable() => trialMistake.RemoveListener(MistakeUpdated);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inMistake"></param>
        protected virtual void MistakeUpdated(bool inMistake)
        {
            if (!inMistake)
                Reload();
        }

        protected virtual void Start()
        {
            if (fireObject == null)
                fireObject = bulletUI;

            reloadResolvers = transform.root.GetComponentsInChildren<IReloadResolverRoutine>();
        }


        /// <inheritdoc/>
        public virtual void Reload() => StartCoroutine(ReloadRoutine());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ReloadRoutine()
        {
            UnityEngine.Debug.Log(nameof(ReloadBulletUIHandler) + ": REPORT: RELOAD CALLED.");

            yield return new WaitForSeconds(reloadDelayTime);

            isReloading.SetValue(true);

            InvokeReloadStarted();

            yield return ReloadFiredObjectAnimation();

            yield return InvokeResolveUIRoutine();

            isReloading.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeReloadStarted()
        {
            onReloadStarted.Raise();

            setBulletText.Raise();
        }
        
        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ReloadFiredObjectAnimation()
        {
            onReloading?.Invoke();

            ShowBulletOnReload();

            AnimateToRestPosition();

            yield return new WaitForSecondsRealtime(toRestPositionDuration + toRestPositionDelay);

            ResolveReloadAnimation();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowBulletOnReload()
        {
            if (fireObject.TryGetComponent<UIVisibilityComponent>(out var fireObjectUI) && fireObjectUI != null)
                fireObjectUI.Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateToRestPosition()
        {
            startOfReloadResetPosition.x = bulletRestPosition.Value.Pos.x - bulletEndPositionOffset.Value.Pos.x;

            startOfReloadResetPosition.y = bulletRestPosition.Value.Pos.y - bulletEndPositionOffset.Value.Pos.y;
#if DOTWEEN
            fireObject.DOKill();

            fireObject.anchoredPosition = startOfReloadResetPosition;

            fireObject.DOAnchorPos(bulletRestPosition.Value.Pos, toRestPositionDuration)
                .SetUpdate(true)
                .SetDelay(toRestPositionDelay);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveReloadAnimation()
        {
            fireObject.anchoredPosition = bulletRestPosition.Value.Pos;

            if (fireObject != bulletUI && fireObject.anchoredPosition != (Vector2)bulletRestPosition.Value.Pos)
                bulletRestPosition.SetValue(new(fireObject.anchoredPosition, Quaternion.identity));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator InvokeResolveUIRoutine()
        {
            foreach (var reloadResolver in reloadResolvers)
                yield return reloadResolver.OnReloadUIFinished();
        }
    }
}