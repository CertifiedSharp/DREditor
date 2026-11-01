//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MainMenuButton) +
        "\n\n")]
    public class MoveToScene : MainMenuButton
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string sceneName = "GYM_Start";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToSceneDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutMusicDuration = 1;


        /// <inheritdoc/>
        public override void OnButtonClicked() => MoveToGameScene();

        /// <summary>
        ///
        /// </summary>
        public virtual void MoveToGameScene() => StartCoroutine(FadeToScene(sceneName));

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        protected virtual IEnumerator FadeToScene(string scene)
        {
            EventSystem.current.SetSelectedGameObject(null);

            var menuGroups = transform.root.GetComponentsInChildren<MenuGroupBase>();

            foreach (var group in menuGroups)
                group.RemoveBackInput();

            if (ServiceLocator.GetService<GlobalFade>().isTransitioning)
                yield return new WaitUntil(() => !ServiceLocator.GetService<GlobalFade>().isTransitioning);

            ServiceLocator.GetService<MusicManagerBase>().FadeMusic(fadeOutMusicDuration);

            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeToSceneDuration);

            UnityEngine.Debug.Log($"REPORT: {nameof(MoveToScene)}: Changing scene to: {scene}");

            SceneManager.LoadSceneAsync(sceneName);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneName"></param>
        protected virtual void SetSceneName(string sceneName) => this.sceneName = sceneName;
    }
}