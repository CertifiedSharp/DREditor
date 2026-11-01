//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure
#if DOTWEEN
using DG.Tweening;
#endif
using System;
using UnityEngine;

namespace DREditor.DialogueEventHandling.Flashing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FlashEventHandlerBase : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float endValue = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float duration = 0.25f;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease ease = Ease.Linear;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int loops = 2;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LoopType loopType = LoopType.Yoyo;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setUpdate = true;

#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected Tween flashTween;
#endif

        /// <summary>
        ///
        /// </summary>
        /// <param name="onValueUpdated"></param>
        /// <param name="onComplete"></param>
        protected virtual void FlashAnimation(Action<float> onValueUpdated, Action onComplete)
        {
#if DOTWEEN
            float whiteValue = 0;

            flashTween = DOTween.To(() => whiteValue, x => whiteValue = x, endValue, duration)
                .SetLoops(loops, LoopType.Yoyo)
                .OnUpdate(() =>
                {
                    onValueUpdated(whiteValue);
                })
                .SetUpdate(setUpdate)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    onComplete();
                });
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        protected abstract void FlashUpdate(float value);

        /// <summary>
        ///
        /// </summary>
        protected abstract void ResetFlash();

        /// <summary>
        ///
        /// </summary>
        /// <param name="onComplete"></param>
        protected virtual void KillActiveProcess(Action onComplete)
        {
#if DOTWEEN
            if (flashTween != null)
            {
                flashTween.Kill();

                onComplete();
            }
#else
            onComplete();
#endif
        }
    }
}