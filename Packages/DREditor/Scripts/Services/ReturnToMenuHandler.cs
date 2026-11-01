//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ReturnToMenuHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string titleScreenSceneName = "GYM_MainMenu";


        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneName"></param>
        public virtual void ReturnToMenu(string sceneName = "")
        {
            CallToTitleScreen();

            LoadToScene(sceneName);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CallToTitleScreen()
        {
            UnityEngine.Debug.Log($"REPORT: {nameof(ReturnToMenuHandler)}: Calling to title screen.");

            WaitForTitleScreenToLoad();

            EventSystem.current.SetSelectedGameObject(null);

            ServiceLocator.GetService<UIHandlerBase>().CallToTitle();

            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void WaitForTitleScreenToLoad()
        {
            ServiceLocator.GetService<UIHandlerBase>().MovingToTitleScreen = true;

            SceneManager.sceneLoaded += OnTitleScreenLoaded;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected virtual void OnTitleScreenLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnTitleScreenLoaded;

            ServiceLocator.GetService<UIHandlerBase>().MovingToTitleScreen = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneName"></param>
        protected virtual void LoadToScene(string sceneName = "")
        {
            UnityEngine.Debug.Log($"REPORT: {nameof(ReturnToMenuHandler)}: CHANGING SCENE.");

            string sceneToLoad = string.IsNullOrEmpty(sceneName) ? titleScreenSceneName : sceneName;

            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}