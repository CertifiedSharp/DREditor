//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class FadeOutGeneratePastOnDebugTest : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onGeneratePastData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string debugEventString = "FadeOutGeneratePastOnDebugTest";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 1;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => DialogueEventSystem.StartListening(debugEventString, DebugFadeOutGeneratePast);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueEventSystem.StopListening(debugEventString, DebugFadeOutGeneratePast);


        /// <summary>
        ///
        /// </summary>
        public virtual void DebugFadeOutGeneratePast(object o = null) 
            => onFinishedLoadingRoom.Response.AddListener(FadeOutGeneratePast);

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeOutGeneratePast()
        {
            onFinishedLoadingRoom.Response.RemoveListener(FadeOutGeneratePast);

            UnityEngine.Debug.Log("REPORT: Fading Out of black then Generating Past.");

            if (ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutDuration);

            StartCoroutine(WakeUpMonobehaviourControl());

            if (onGeneratePastData != null)
                onGeneratePastData.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WakeUpMonobehaviourControl()
        {
            yield return null;

            inMenu.Value = true;

            yield return null;

            inMenu.Value = false;
        }
    }
}