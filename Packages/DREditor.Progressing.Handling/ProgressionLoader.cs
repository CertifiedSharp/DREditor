//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionLoader : SceneEventListener, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected ProgressionDatabase progressionDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string chapterKey = "chapter";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string objectiveKey = "CurrentObjective";


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas;

        /// <summary>
        ///
        /// </summary>
        protected ProgressionManagerBase manager = null;



        protected virtual void Start() => manager = ServiceLocator.GetService<ProgressionManagerBase>();


        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => this.saveDatas = saveDatas;

        /// <inheritdoc/>
        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            LoadOnSceneEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadOnSceneEvent()
        {
            currentChapterIndex.SetValue(saveDatas.Get<int>(chapterKey));

            manager.SetCurrentChapter(progressionDatabase.Chapters[currentChapterIndex.Value]);


            Objective saveObjective = saveDatas.Get<Objective>(objectiveKey);

            manager.SetCurrentObjective(saveObjective);


            LoadBaseObjective(saveObjective);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveObjective"></param>
        protected void LoadBaseObjective(Objective saveObjective)
        {
            Objective baseObjective = progressionDatabase.GetObjective(manager.currentObjective.Description);

            if (baseObjective == null)
            {
                UnityEngine.Debug.LogWarning($"REPORT: {ToString()}: " +
                    $"Attempting to Repair the Objective since it's not found in any chapter.");

                int finishedObjectives = ServiceLocator.GetService<IHistory>().GetAmountOfFinishedObjectives();

                baseObjective = progressionDatabase
                    .GetObjective(progressionDatabase.Chapters[currentChapterIndex.Value].Objectives[finishedObjectives].Description);

                manager.ResetCurrentObjective(baseObjective, saveObjective);
            }
            else if (saveObjective.KeepInstanceData != baseObjective.KeepInstanceData || FlagCountUpdated(baseObjective, saveObjective))
                manager.ResetCurrentObjective(baseObjective, saveObjective);

            if (baseObjective != null && baseObjective.ProgressionGate != null)
                manager.currentObjective.SetProgressionGate(baseObjective.ProgressionGate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseObjective"></param>
        /// <param name="saveObjective"></param>
        /// <returns></returns>
        protected virtual bool FlagCountUpdated(Objective baseObjective, Objective saveObjective) =>
            baseObjective.Description == saveObjective.Description && baseObjective.Flags.Count != saveObjective.Flags.Count;
    }
}