//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Gates;
using DREditor.Progressing.Handling;
using DREditor.RoomLoading;
using DREditor.Saving;
using DREditor.Services;
using DREditor.States;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.DebugTesting
{
    /// <summary>
    ///
    /// </summary>
    public class DebugTester : DebugTesterBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference startTest = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public StateDatabase stateDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public ScriptableObject pauseData = null;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected SceneEvent newGameSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent startedNewGame = null;

        /// <summary>
        ///
        /// </summary>
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
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string generatePastEvent = "FadeOutGeneratePastOnDebugTest";


        protected virtual void OnEnable() => startTest.action.started += StartNewGame;

        protected virtual void OnDisable() => startTest.action.started -= StartNewGame;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void StartNewGame(CallbackContext ctx)
        {
            FindAnyObjectByType<PlayerDataClearHandler>().Clear();

            if (hasLoadedSaveData.Value)
            {
                startedNewWithLoaded.SetValue(true);

                newGameSelected.Raise();

                startedNewGame.Raise();
            }

            EventSystem.current.SetSelectedGameObject(null);

            StartCoroutine(FadeIntoGame());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator FadeIntoGame()
        {
            newGameSelected.Raise();

            startedNewGame.Raise();

            // This section differs from the normal new game initialization logic.
            currentChapterIndex.SetValue(chapterIndex);

            ServiceLocator.GetService<ProgressionManagerBase>().SetCurrentObjective(objective);

            if (toGate != null || (position == Vector3.zero && rotation == Vector3.zero))
                OverrideFromGate(fromGate);
            else
                playerSpawnPoint.SetValue(new(position, Quaternion.Euler(rotation)));

            currentState.SetValue(state);

            ServiceLocator.GetService<MusicManagerBase>().FadeMusic(1f);

            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(1);

            ServiceLocator.GetService<RoomLoaderBase>().AllowRoomsToLoad();

            if (GoToScene == "" && toGate != null)
                GoToScene = toGate.ToAreaName;

            SceneManager.LoadSceneAsync(GoToScene);

            DialogueEventSystem.TriggerEvent(generatePastEvent);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fromG"></param>
        protected virtual void OverrideFromGate(GateFrom fromG)
        {
            if (fromG != null)
                UnityEngine.Debug.Log($"{ToString()}: OverrideFromGate: FromG was Null , most likely TPFD Area");

            if (fromG != null)
                playerSpawnPoint.SetValue(new(fromG.position, Quaternion.Euler(fromG.rotation)));
            else
                playerSpawnPoint.SetValue(new(Vector3.zero, Quaternion.Euler(Vector3.zero)));
        }
    }
}