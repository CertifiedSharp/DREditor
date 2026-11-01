//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SaveLoadMenuBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveFilePreText = "save";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useCoroutineFileLoading = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveGameFileEvent = "SaveGameFile";


        [Header("")]
        [Divider]
        [SerializeField] protected UnityEvent OnSave;


        /// <summary>
        ///
        /// </summary>
        protected SaveSlotBase currentSlot = null;

        /// <summary>
        ///
        /// </summary>
        protected bool saveMode = false;


        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void AddOnSaveListener(UnityAction action) => OnSave.AddListener(action);

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void RemoveOnSaveListener(UnityAction action) => OnSave.RemoveListener(action);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void SetSaveMode()
        {
            saveMode = true;

            LoadSlotsUI();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void SetLoadMode()
        {
            saveMode = false;

            LoadSlotsUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void LoadSlotsUI();


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void SaveCurrentSlot()
        {
            DialogueEventSystem.TriggerEvent(saveGameFileEvent, saveFilePreText + currentSlot.GetID());

            StartCoroutine(BufferPostSave());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator BufferPostSave()
        {
            yield return null;

            LoadSlotsUI();

            OnSave?.Invoke();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public abstract void LoadCurrentSlot();
    }
}