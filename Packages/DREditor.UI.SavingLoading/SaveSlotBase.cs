//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SaveSlotBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public Button button = null;

        /// <summary>
        ///
        /// </summary>
        public event Action<SaveSlotBase> OnSlotClicked;


        /// <summary>
        ///
        /// </summary>
        protected int id = -1;

        /// <summary>
        ///
        /// </summary>
        protected bool hasSaveData = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => InitializeButton();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeButton()
        {
            if (!TryGetComponent(out button))
                button = GetComponentInChildren<Button>();

            if (button == null)
                UnityEngine.Debug.LogError("The button for a save slot couldn't be initialized! Please ensure a button is either on " +
                    "or a child of the save slot!");
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => button.onClick.AddListener(InvokeSlotClicked);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => button.onClick.RemoveListener(InvokeSlotClicked);

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeSlotClicked() => OnSlotClicked?.Invoke(this);


        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public virtual void SetID(int id) => this.id = id;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetID() => id;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool HasSaveData() => hasSaveData;


        /// <summary>
        ///
        /// </summary>
        public abstract void OnSaveFound();

        /// <summary>
        ///
        /// </summary>
        public abstract void OnSaveNotFound();
    }
}