//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FadeVisibility<T> : UIVisibilityComponent where T : Component
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<T> fadeComponents = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float visibleValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeInDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [Range(0f, 1f)]
        [SerializeField] protected float invisibleValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool killTweensOnShow = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool killTweensOnHide = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool ignoreTimeScale = false;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease easeCurve = Ease.Linear;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int loops = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float delay = 0f;

#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected List<Tween> activeTweens = new();


        protected override void ShowUI()
        {
            if (killTweensOnShow)
                KillTweens();

            fadeComponents.ForEach(component =>
            RegisterTween(component.DOFadeAlpha(visibleValue, fadeInDuration)
            .SetUpdate(ignoreTimeScale)
            .SetEase(easeCurve)
            .SetLoops(loops)
            .SetDelay(delay))
            );
        }

        protected override void HideUI()
        {
            if (killTweensOnHide)
                KillTweens();

            fadeComponents.ForEach(component => 
            RegisterTween(component.DOFadeAlpha(invisibleValue, fadeOutDuration)
            .SetUpdate(ignoreTimeScale)
            .SetEase(easeCurve)
            .SetLoops(loops)
            .SetDelay(delay))
            );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tween"></param>
        protected virtual void RegisterTween(Tween tween)
        {
            tween.onComplete = () => { activeTweens.Remove(tween); };

            activeTweens.Add(tween);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void KillTweens()
        {
            activeTweens.ForEach(tween => tween.Kill());

            activeTweens.Clear();
        }
#else
        protected override void ShowUI() { }

        protected override void HideUI() { }
#endif

        protected override IEnumerator WaitForUIToComplete()
        {
#if DOTWEEN
            while (activeTweens.Count != 0)
#endif
                yield return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetFadeComponents() => fadeComponents;
    }
}