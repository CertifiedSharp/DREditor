//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Choices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadable) + ", " + nameof(IClearDialogueSaveData) +
        "\n\n")]
    public class ChoiceSaveHandler : DialogueComponent, IDRESaveable, IDRELoadable, IClearDialogueSaveData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKey = "Choices";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener OnPlayerDidntGivePresentChoice = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int dontGivePresentChoiceIndex = 1;


        /// <summary>
        ///
        /// </summary>
        protected List<int> choices = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => OnPlayerDidntGivePresentChoice.Response.AddListener(OnPlayerNoGivePresent);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => OnPlayerDidntGivePresentChoice.Response.RemoveListener(OnPlayerNoGivePresent);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPlayerNoGivePresent() => AddChoice(dontGivePresentChoiceIndex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedChoiceIndex"></param>
        public virtual void AddChoice(int selectedChoiceIndex) => choices.Add(selectedChoiceIndex);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual List<int> GetChoices() => choices;


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) => saveData.Set(saveKey, choices);

        /// <inheritdoc/>
        public virtual void ClearSaveData() => choices.Clear();

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => choices = new(saveDatas.Get<List<int>>(saveKey));
    }
}