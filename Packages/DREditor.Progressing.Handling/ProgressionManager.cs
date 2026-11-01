//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<ProgressionManagerBase>) + " for type: " + nameof(ProgressionManagerBase) +
        "\n\n")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionManager : ProgressionManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentObjectiveIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent onLastObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent objectiveKeepsInstanceData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent requiredFlagsTriggered = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentObjectiveName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent moveToObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener newGameSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener startedNewGame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener moveToNextNaturalObjective = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => SubscribeToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnsubscribeToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
            moveToNextNaturalObjective.Response.AddListener(MoveToNextNaturalObjective);

            newGameSelected.Response.AddListener(ResetProgression);

            startedNewGame.Response.AddListener(ResetProgression);

            moveToObjective.AddListener(MoveToObjective);

            currentChapterIndex.AddListener(SetCurrentChapterByIndex);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToEvents()
        {
            moveToNextNaturalObjective.Response.RemoveListener(MoveToNextNaturalObjective);

            newGameSelected.Response.RemoveListener(ResetProgression);

            startedNewGame.Response.RemoveListener(ResetProgression);

            moveToObjective.RemoveListener(MoveToObjective);

            currentChapterIndex.RemoveListener(SetCurrentChapterByIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void SetCurrentChapterByIndex(int index) => SetCurrentChapter(progressionDatabase.Chapters[index]);

        /// <inheritdoc/>
        public override bool PassedPoint(int chapter, string objectiveName)
        {
            if (currentChapterIndex.Value > chapter)
                return true;

            if (currentChapterIndex.Value < chapter)
                return false;

            return ServiceLocator.GetService<IHistory>().ContainsObjective(objectiveName);
        }

        /// <inheritdoc/>
        public override void SetCurrentChapter(Chapter chapter)
        {
            base.SetCurrentChapter(chapter);

            currentChapterIndex.SetValue(progressionDatabase.Chapters.IndexOf(chapter));

            UnityEngine.Debug.Log($"REPORT: {nameof(ProgressionManager)}: The Chapter index has changed to {chapter}.");
        }

        /// <inheritdoc/>
        public override void SetCurrentObjective(Objective objective)
        {
            base.SetCurrentObjective(objective);

            bool isFinalObjective = currentObjective.Description == currentChapter.Objectives[^1].Description;

            onLastObjective.SetValue(isFinalObjective);

            currentObjectiveIndex.SetValue(currentChapter.Objectives.IndexOf(currentObjective));

            currentObjectiveName.SetValue(currentObjective.Description);

            objectiveKeepsInstanceData.SetValue(progressionDatabase.GetObjective(currentObjective.Description).KeepInstanceData);

            requiredFlagsTriggered.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetProgression()
        {
            SetCurrentChapter(progressionDatabase.Chapters[0]);

            currentChapterIndex.SetValue(0);

            SetCurrentObjective((Objective)currentChapter.Objectives[0].Clone());

            ServiceLocator.GetService<IHistory>().Clear();

            UnityEngine.Debug.Log($"REPORT: {nameof(ProgressionManager)}: Progression Reset.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objective"></param>
        /// <returns></returns>
        protected virtual Objective GetNextObjective(Objective objective)
        {
            for (int i = 0; i < currentChapter.Objectives.Count; i++)
            {
                Objective iteratedObjective = currentChapter.Objectives[i];

                if (iteratedObjective.Description == objective.Description)
                {
                    if (i + 1 < currentChapter.Objectives.Count)
                        return currentChapter.Objectives[i + 1];
                    else
                    {
                        MoveToNextChapter();

                        return currentChapter.Objectives[0];
                    }
                }
            }

            UnityEngine.Debug.Log($"REPORT: {ToString()}: GetNextObjective Evaluated Null");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToNextChapter()
        {
            for (int i = 0; i < progressionDatabase.Chapters.Count; i++)
                if (progressionDatabase.Chapters[i] == currentChapter && (i + 1) < progressionDatabase.Chapters.Count)
                    SetCurrentChapter(progressionDatabase.Chapters[i + 1]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objectiveName"></param>
        protected virtual void MoveToObjective(string objectiveName)
        {
            if (string.IsNullOrEmpty(objectiveName))
                return;

            ServiceLocator.GetService<IHistory>().Add(currentChapterIndex.Value, currentObjective);

            Objective objective = progressionDatabase.GetObjective(objectiveName);

            if (objective == null)
            {
                UnityEngine.Debug.LogError($"ALERT: {ToString()}: " +
                    $"The Move to Objective called name does not match any valid objectives, " +
                    $"for: " + objectiveName + " . Please make sure the string is spelled correctly and has correct case.");

                return;
            }
            else
            {
                ServiceLocator.GetService<IHistory>().SetPreviousObjective((Objective)currentObjective.Clone());

                SetCurrentObjective((Objective)objective.Clone());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToNextNaturalObjective()
        {
            bool isFTEObjective = currentChapter.GetObjective(currentObjective.Description) == null;

            if (isFTEObjective)
                SetCurrentObjective((Objective)GetNextObjective(ServiceLocator.GetService<IHistory>().GetLastObjective()).Clone());
            else
                ChangeObjective();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeObjective()
        {
            UnityEngine.Debug.Log($"REPORT: {nameof(ProgressionManager)}: Changing objective from current objective: " 
                + currentObjective.Description);

            ServiceLocator.GetService<IHistory>().Add(currentChapterIndex.Value, currentObjective);

            string overrideFlagObjectiveName = currentObjective.GetOverrideFlag();

            if (!string.IsNullOrEmpty(overrideFlagObjectiveName))
                SetCurrentObjective((Objective)progressionDatabase.GetObjective(overrideFlagObjectiveName).Clone());
            else
                SetCurrentObjective((Objective)GetNextObjective(currentObjective).Clone());
        }
    }
}