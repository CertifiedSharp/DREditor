//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(SaveSlotBase) +
        "\n\n")]
    public class SaveSlot : SaveSlotBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveFilePreText = "save";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveFileFoundEvent = "OnSaveFileFound";
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string requestSaveFileEvent = "RequestSaveFile";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SaveDataDisplayerBase> displayers = new();


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas;


        /// <inheritdoc/>
        public override void OnSaveFound()
        {
            hasSaveData = true;

            DialogueEventSystem.StartListening(saveFileFoundEvent, LoadSaveFileSlotUI);

            DialogueEventSystem.TriggerEvent(requestSaveFileEvent, saveFilePreText + id);
        }

        /// <inheritdoc/>
        public override void OnSaveNotFound()
        {
            foreach (var displayer in displayers)
                if (displayer != null)
                    displayer.OnSaveNotFound(id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void LoadSaveFileSlotUI(object o = null)
        {
            DialogueEventSystem.StopListening(saveFileFoundEvent, LoadSaveFileSlotUI);

            saveDatas = (Dictionary<string, object>)o;

            foreach (var displayer in displayers)
                if (displayer != null)
                    displayer.Display(saveDatas, id);

            saveDatas = null;
        }
    }
}