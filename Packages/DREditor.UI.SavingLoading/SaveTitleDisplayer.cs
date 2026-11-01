//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using DREditor.Saving;
using DREditor.States;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SaveDataDisplayerBase) +
        "\n\n")]
    public class SaveTitleDisplayer : SaveDataDisplayerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ProgressionDatabase progressionDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StateDatabase addStateDatabase = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI title = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string titleNotFoundText = "Free Slot ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string titleFoundText = "Slot ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string scenePrefix = "GYM_";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string nullLocation = "null";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string slotSeparator = " : ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string additionalStatePrefix = "(";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string additionalStateSuffix = ")";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string regexPattern = @"([a-z])([A-Z])|([A-Z]+)([A-Z][a-z])";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string regexReplacement = "$1$3 $2$4";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string sceneKey = "SceneName";
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string addedStateKey = "addedState";
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string locationKey = "location";
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string chapterKey = "chapter";


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas;


        /// <inheritdoc/>
        public override void Display(Dictionary<string, object> saveDatas, int id)
        {
            this.saveDatas = saveDatas;

            string title = GetChapter() + GetArea() + GetAdditionalState();

            this.title.text = titleFoundText + (id + 1) + slotSeparator + title;

            this.saveDatas = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetChapter() 
            => progressionDatabase != null ? 
            progressionDatabase.Chapters[saveDatas.Get<int>(chapterKey)].saveTitle + " ": "";

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetArea()
        {
            string area = saveDatas.Get<string>(sceneKey)?.Replace(scenePrefix, "");

            string location = saveDatas.Get<string>(locationKey);

            if (location != null && location == nullLocation)
                return "";
            else if (location != null && location != "")
                return location + " ";
            else
                return SplitCamelCase(area) + " ";
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetAdditionalState()
        {
            if (addStateDatabase == null)
                return string.Empty;

            string added = SplitCamelCase(addStateDatabase.GetStateName(saveDatas.Get<int>(addedStateKey)).ToString());

            if (added != "")
                added = additionalStatePrefix + added + additionalStateSuffix;

            return added;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual string SplitCamelCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            else
                return Regex.Replace(text, regexPattern, regexReplacement);
        }


        /// <inheritdoc/>
        public override void OnSaveNotFound(int id) => title.text = titleNotFoundText + (id + 1);
    }
}