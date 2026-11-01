//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISceneEventListener) +
        "\n\n")]
    public class MainMenu : MonoBehaviour, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameObject firstObjectToBeSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase firstMenuGroupSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase loadGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string applyDataEventString = "ApplyCurrentData";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasLoadedSaveData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent startedNewWithLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent firstTimeLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadeOutComplete = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float firstInitializeBufferTime = 1.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutBufferTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float firstGroupRevealBufferTime = 1;

        [Space]

        [Space]

        [Header("")]
        [SerializeField] protected UnityEvent OnLoadIntoMainMenu;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => EvaluateMainMenuState();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateMainMenuState()
        {
            if (!firstTimeLoaded.Value && !hasLoadedSaveData.Value)
                FirstTimeReachedMainMenu();
            else if (startedNewWithLoaded.Value)
                StartedNewGameOnLoadedFile();
            else if (firstTimeLoaded.Value && hasLoadedSaveData.Value)
                LoadedFile();
            else
                FirstTimeReachedMainMenu();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FirstTimeReachedMainMenu()
        {
            if (!firstTimeLoaded.Value && !hasLoadedSaveData.Value)
                UnityEngine.Debug.Log($"{ToString()}: First Time Reached Main Menu");
            else
                UnityEngine.Debug.LogWarning($"{ToString()}: MainMenu Start: No CurrentDRESaveData");

            firstTimeLoaded.SetValue(true);

            StartCoroutine(OnFirstTimeRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator OnFirstTimeRoutine()
        {
            yield return new WaitForSecondsRealtime(firstInitializeBufferTime);

            if (ServiceLocator.HasService<GlobalFade>() && ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutBufferTime);

            OnLoadIntoMainMenu?.Invoke();

            yield return new WaitForSeconds(firstGroupRevealBufferTime);

            InitializeFirstGroupStartUp();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeFirstGroupStartUp()
        {
            firstMenuGroupSelected.Show();

            EventSystem.current.SetSelectedGameObject(firstObjectToBeSelected);

            ServiceLocator.GetService<UIHandlerBase>().SetCurrent(firstObjectToBeSelected);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void StartedNewGameOnLoadedFile()
        {
            UnityEngine.Debug.Log($"{ToString()}: Player started New Game on Loaded File");

            startedNewWithLoaded.SetValue(false);

            LoadedFile();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadedFile() => StartCoroutine(LoadGroupStart());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LoadGroupStart()
        {
            UnityEngine.Debug.Log($"{ToString()}: Player Loaded in with save data");

            if (ServiceLocator.GetService<GlobalFade>().isTransitioning)
                yield return new WaitUntil(() => !ServiceLocator.GetService<GlobalFade>().isTransitioning);

            yield return null;

            LoadGroupStartProcess();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadGroupStartProcess()
        {
            ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutBufferTime);

            OnLoadIntoMainMenu?.Invoke();

            onFadeOutComplete.RegisterListener(this);

            if (hasLoadedSaveData.Value)
            {
                DialogueEventSystem.TriggerEvent(applyDataEventString);

                hasLoadedSaveData.Invoke();
            }
            else
                UnityEngine.Debug.LogError($"ALERT: {nameof(MainMenu)}: CRITICAL ERROR: DRESaveData Not found. :(");
        }

        
        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            onFadeOutComplete.UnregisterListener(this);

            loadGroup.Show();
        }
    }
}