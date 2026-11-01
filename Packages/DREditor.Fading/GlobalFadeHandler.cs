//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Fading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<GlobalFade>) + " for type: " + nameof(GlobalFade) +
        "\n\n")]
    public class GlobalFadeHandler : GlobalFade, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugLogs = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadeOutComplete = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image Fade = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float toTitleWaitTime = 0.3f;


        /// <summary>
        ///
        /// </summary>
        protected string lastFadeCaller;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine fadeToRoutine;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine fadeOutRoutine;


        /// <inheritdoc/>
        protected override bool IsUIDark() => Fade.color.a != 0;

        /// <inheritdoc/>
        public override void FadeTo(float time, bool white = false, bool overrule = false) =>
            fadeToRoutine = StartCoroutine(FadeToRoutine(time, white, overrule));

        
        /// <inheritdoc/>
        public override IEnumerator FadeToRoutine(float time, bool white = false, bool overrule = false)
        {
            lastFadeCaller = StackTraceUtility.ExtractStackTrace();

            return FadeToRoutineInternal(time, white, overrule);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="time"></param>
        /// <param name="white"></param>
        /// <param name="overrule"></param>
        protected virtual IEnumerator FadeToRoutineInternal(float time, bool white = false, bool overrule = false)
        {
            if (FailedFade(overrule, "TO"))
                yield break;

            if (isTransitioning && overrule && fadeOutRoutine != null)
            {
                KillFade();

                StopCoroutine(fadeOutRoutine);
            }

            LogWarning($"Called Fade {gameObject.name} {isTransitioning} {overrule}");

            isTransitioning = true;

            SetColor(white);

            if (inFastForwardMode.Value)
                time /= 2;

            IsDark = true;
#if DOTWEEN
            Fade.DOFade(1, time)
                .SetUpdate(!inFastForwardMode.Value);
#endif
            yield return new WaitForSecondsRealtime(time);

            isTransitioning = false;
        }


        /// <inheritdoc/>
        public override void FadeOut(float time, bool white = false, bool overrule = false) =>
            fadeOutRoutine = StartCoroutine(FadeOutRoutine(time, white, overrule));

        /// <inheritdoc/>
        public override IEnumerator FadeOutRoutine(float time, bool white, bool overrule)
        {
            lastFadeCaller = StackTraceUtility.ExtractStackTrace();

            return FadeOutRoutineInternal(time, white, overrule);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="time"></param>
        /// <param name="white"></param>
        /// <param name="overrule"></param>
        protected virtual IEnumerator FadeOutRoutineInternal(float time, bool white = false, bool overrule = false)
        {
            if (FailedFade(overrule, "OUT"))
                yield break;

            if (overrule)
            {
                KillFade();

                if (isTransitioning && fadeToRoutine != null)
                    StopCoroutine(fadeToRoutine);
            }

            isTransitioning = true;

            SetColor(white);

            LogWarning("FADE OUT WAS CALLED, OVERRULING WAS: " + overrule);

            if (inFastForwardMode.Value)
                time /= 2;

            IsDark = false;
#if DOTWEEN
            Fade.DOFade(0, time)
                .SetUpdate(!inFastForwardMode.Value);
#endif
            yield return new WaitForSecondsRealtime(time);

            isTransitioning = false;

            if (onFadeOutComplete != null)
                onFadeOutComplete.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="overrule"></param>
        /// <param name="fadeType"></param>
        /// <returns></returns>
        protected virtual bool FailedFade(bool overrule, string fadeType)
        {
            bool result = isTransitioning && !overrule;

            if (result)
                LogWarning($"Failed Attempt to Fade " + fadeType);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void KillFade()
        {
#if DOTWEEN
            Fade.DOKill();
#endif
            LogWarning("FADE Killed");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="white"></param>
        protected virtual void SetColor(bool white)
        {
            if (white)
                Fade.color = new Color(1f, 1f, 1f, Fade.color.a);
            else
                Fade.color = new Color(0f, 0f, 0f, Fade.color.a);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        protected virtual void LogWarning(string message)
        {
            if (debugLogs)
                UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}: {message} \n{lastFadeCaller}");
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            if (!IsDark)
                FadeTo(toTitleWaitTime);
        }


        /// <summary>
        /// 
        /// </summary>
        [BehaviourButton]
        protected virtual void ToggleFadeState() => ForceVisibility(!IsDark);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        protected virtual void ForceVisibility(bool visible)
        {
            float alpha = visible ? 1f : 0f;

            Fade.color = Fade.color.WithAlpha(alpha);

            IsDark = visible;
        }
    }
}