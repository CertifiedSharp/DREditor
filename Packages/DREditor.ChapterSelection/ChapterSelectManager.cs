//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Progressing;
using DREditor.Saving;
using DREditor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class ChapterSelectManager : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChapterSelectDatabase chapterSelectDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Button chapSelectButton = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ProgressionDatabase progressionDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<ChapterSectionBase> chapterSections = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasLoadedSaveData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase backGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string loadGameEventString = "LoadGameFromData";


        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            backGroup.BeforeStart.AddListener(SetChapterSelectButtonInteractibility);

            chapSelectButton.onClick.AddListener(EvaluateAvailableChapterSelection);

            Response?.AddListener(StartChapterSelectGame);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            backGroup.BeforeStart.RemoveListener(SetChapterSelectButtonInteractibility);

            chapSelectButton.onClick.RemoveListener(EvaluateAvailableChapterSelection);

            Response?.RemoveListener(StartChapterSelectGame);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetChapterSelectButtonInteractibility()
            => chapSelectButton.interactable = PlayerCanUseChapterSelect();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayerCanUseChapterSelect() => 
            DatabaseIsValid() && (HandlersAllowChapterSelect() || PlayerHasLoadedSaveData());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool DatabaseIsValid() => chapterSelectDatabase != null && !chapterSelectDatabase.IsEmpty();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool HandlersAllowChapterSelect()
        {
            foreach (ChapterSectionBase section in chapterSections)
                if (!section.AllowsChapterSelect())
                    return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayerHasLoadedSaveData() => hasLoadedSaveData.Value;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void EvaluateAvailableChapterSelection()
        {
            foreach (ChapterSectionBase section in chapterSections)
                section.OnParentSubmittedIndex(0);

            if (hasLoadedSaveData.Value)
                LoadedSaveChapterSelect();
            else
                DefaultChapterSelect();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadedSaveChapterSelect()
        {
            UnityEngine.Debug.Log("Chapter Select: Used Load File Evaluation.");

            foreach (ChapterSectionBase section in chapterSections)
                section.InitializeLoaded();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DefaultChapterSelect()
        {
            UnityEngine.Debug.Log("Chapter Select: Used Default Max Settings");

            foreach (ChapterSectionBase section in chapterSections)
                section.InitializeDefault();
        }


        // Investigate whether this code is to be used.
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public virtual void StartChapterSelectGame()
        {
            Queue<int> queue = new(chapterSections.Select(section => section.GetSelectedSection()));

            SceneDataProviderBase data = (SceneDataProviderBase)chapterSelectDatabase.GetChapterSelectData(queue);

            try
            {
                DialogueEventSystem.TriggerEvent(loadGameEventString, (SerializableDictionary)data.GetData());
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError("Couldn't set Chapter select data: Selection order: " + string.Join(", ", queue) +
                    " \n " + exception.ToString());

                UnityEngine.Debug.LogException(exception);
            }
        }
    }
}