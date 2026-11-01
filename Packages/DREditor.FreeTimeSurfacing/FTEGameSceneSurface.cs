//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Fading;
using DREditor.Processables;
using DREditor.Services;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FTESurfaceComponent) + ", " + nameof(ISubmittable) +
        "\n\n")]
    public class FTEGameSceneSurface : FTESurfaceComponent, ISubmittable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string gameScene = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.5f;


        /// <inheritdoc/>
        public virtual void Submit(Action onComplete) => StartCoroutine(MoveToScene());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator MoveToScene()
        {
            if (ServiceLocator.HasService(out GlobalFade fader))
                yield return fader.FadeToRoutine(fadeOutDuration);

            UnityEngine.Debug.LogWarning("REPORT: CHANGING SCENE");

            SceneManager.LoadScene(gameScene);
        }
    }
}