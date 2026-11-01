//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.RoomLoading;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SavePointUIBase) + ", " + nameof(SceneEventListener) +
        "\n\n")]
    public class SavePointUI : SavePointUIBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent requiredFlagsTriggered = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent playHideDialogueSFX = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent threeDUIVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent moveToNextNaturalObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentObjectiveName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentAddState = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float delayLoadPointDuration = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToPointDuration = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected PostSavePoint[] savePoints;


        /// <summary>
        ///
        /// </summary>
        protected readonly Dictionary<string, string> scenePoints = new();


        protected virtual void Awake() => InitializeSavePointDictionary();

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            hasMenuAccess.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSavePointDictionary()
        {
            foreach (PostSavePoint savePoint in savePoints)
                scenePoints.TryAdd(savePoint.objective, savePoint.scene);
        }

        
        /// <inheritdoc/>
        protected override void ShowSavePointUI()
        {
            if (inDialogue.Value)
                StartCoroutine(WaitForDialogueToFinish());
            else
                base.ShowSavePointUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForDialogueToFinish()
        {
            playHideDialogueSFX.SetValue(false);

            yield return new WaitUntil(() => !inDialogue.Value);

            playHideDialogueSFX.SetValue(true);

            threeDUIVisible.SetValue(false);

            savePointGroup.Show();
        }
        
        
        /// <inheritdoc/>
        protected override void StartSavePointTransition() => StartCoroutine(BufferLoadSavePoint());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferLoadSavePoint()
        {
            yield return new WaitForSeconds(delayLoadPointDuration);

            if (ServiceLocator.HasService<GlobalFade>())
                ServiceLocator.GetService<GlobalFade>().FadeTo(fadeToPointDuration);

            ExecuteValidSavePoint();
        }

        /// <inheritdoc/>
        protected override bool SavePointIsValid() => scenePoints.ContainsKey(currentObjectiveName.Value);

        /// <inheritdoc/>
        protected override void LoadSavePoint()
        {
            if (SceneManager.GetActiveScene().name == scenePoints[currentObjectiveName.Value])
                return;
            
            string savePointObjective = currentObjectiveName.Value;

            ExecuteSavePointSettings();

            LoadScene(scenePoints[savePointObjective]);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ExecuteSavePointSettings()
        {
            PostSavePoint savePoint = savePoints.Where(n => n.objective == currentObjectiveName.Value).FirstOrDefault();

            if (savePoint == null)
                return;

            if (inTrialMode.Value)
                savePoint.mode.SetValue(true);

            if (savePoint.changeObjective && requiredFlagsTriggered.Value)
                moveToNextNaturalObjective.Raise();

            if (savePoint.dialogueEvent != "")
                DialogueEventSystem.TriggerEvent(savePoint.dialogueEvent);

            if (savePoint.roomLoadingOptions != null)
                ServiceLocator.GetService<RoomChangerBase>().SetOptions(savePoint.roomLoadingOptions);

            currentAddState.SetValue(savePoint.addstate);

            inLeaveProcess.SetValue(true);

            hasMenuAccess.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        protected virtual void LoadScene(string scene)
        {
            if (SceneManager.GetActiveScene().name == scene)
                return;

            UnityEngine.Debug.LogWarning("REPORT: CHANGING SCENE.");

            if (!isLoadingFile.Value)
                ServiceLocator.GetService<GlobalFade>().FadeTo(fadeToPointDuration);

            SceneManager.LoadSceneAsync(scene);
        }
    }
}