//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class CGFlashbackEventHandler : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGStateMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgStateEnded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgUIVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgDialogueUIVisible = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image CG = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image SubCG = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float nextFadeTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float stayDuration = 1.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startFadeToTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startFadeOutTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endFadeToTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endFadeOutTime = 0.5f;


        [Header("")]
        [SerializeField] protected UnityEvent onFlashbackStart;

        [Header("")]
        [SerializeField] protected UnityEvent onFlashbackEnd;


        /// <summary>
        ///
        /// </summary>
        protected Flashback data;


        protected virtual void Awake()
        {
            if (CG == null)
                UnityEngine.Debug.LogError($"ALERT: You must assign the {nameof(CG)} value " +
                    $"in the inspector in order for CG's to work!");

            if (SubCG == null)
                UnityEngine.Debug.LogError($"ALERT: You must assign the {nameof(SubCG)} value " +
                    $"in the inspector in order for CG's to work!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => StartCoroutine(FlashbackEventRoutine(o));

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        protected virtual IEnumerator FlashbackEventRoutine(object o)
        {
            data = (Flashback)o;

            if (FlashbacksEmpty())
                yield break;

            yield return StartFlashback();

            for (int i = 0; i < data.Flashbacks.Count; i++)
                yield return DisplayFlashback(data.Flashbacks[i]);

            yield return EndFlashback();

            ResolveFlashback();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool FlashbacksEmpty()
        {
            bool result = data.Flashbacks.Count == 0;

            if (result)
            {
                UnityEngine.Debug.LogWarning($"{ToString()}: Flashbacks were empty.");

                cgStateEnded.SetValue(true);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator StartFlashback()
        {
            onFlashbackStart?.Invoke();

            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(startFadeToTime, true);

            EnableCGUI();

            yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(startFadeOutTime, true);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableCGUI()
        {
            inCGStateMode.SetValue(true);

            cgUIVisible.SetValue(true);

            if (!cgDialogueUIVisible.Value)
                cgDialogueUIVisible.Invoke();
            else
                cgDialogueUIVisible.SetValue(false);

            CG.sprite = data.Flashbacks[0];
#if DOTWEEN
            CG.DOFade(1, 0);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        protected virtual IEnumerator DisplayFlashback(Sprite sprite)
        {
            SubCG.sprite = sprite;
#if DOTWEEN
            SubCG.DOFade(1, 0);

            CG.DOFade(0, nextFadeTime);
#endif
            yield return new WaitForSeconds(nextFadeTime);

            CG.sprite = sprite;
#if DOTWEEN
            CG.DOFade(1, 0);
#endif
            yield return new WaitForSeconds(stayDuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator EndFlashback()
        {
            onFlashbackEnd?.Invoke();

            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(endFadeToTime, true);

            DisableCGUI();

            yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(endFadeOutTime, true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableCGUI()
        {
            cgUIVisible.SetValue(false);
#if DOTWEEN
            CG.DOFade(0, 0);

            SubCG.DOFade(0, 0);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveFlashback()
        {
            cgStateEnded.SetValue(true);

            inCGStateMode.SetValue(false);
        }


        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("Flashback");
    }
}