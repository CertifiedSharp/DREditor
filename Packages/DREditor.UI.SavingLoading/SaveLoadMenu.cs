//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.SaveManaging;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [DefaultExecutionOrder(1)]
	[ClassHeader("")]
    public class SaveLoadMenu : SaveLoadMenuBase
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool onPauseMenu = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("SlotList")]
        [SerializeField] protected List<SaveSlotBase> saveSlots = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableMenuGroupBase saveLoadGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase savePopUp = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase overwritePopUp = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase loadPopUp = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ReturnToMenuOnPauseMenu ReturnToMenuPauseHandler = null;


        [Header("")]
        [SerializeField] protected UnityEvent OnSelectedLockedSaveSlot;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string loadGameFileEventString = "LoadGameFile";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string applyDataEventString = "ApplyCurrentData";


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            for (int i = 0; i < saveSlots.Count; i++)
                saveSlots[i].SetID(i);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            SetSubscriptionsToSaveSlots(true);

            SetSubscriptionToPopUps(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            SetSubscriptionsToSaveSlots(false);

            SetSubscriptionToPopUps(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscribing"></param>
        protected virtual void SetSubscriptionsToSaveSlots(bool subscribing)
        {
            foreach (var slot in saveSlots)
            {
                if (subscribing)
                {
                    slot.OnSlotClicked += SetCurrentSaveSlot;

                    slot.button.onClick.AddListener(EvaluatePopUp);
                }
                else
                {
                    slot.OnSlotClicked -= SetCurrentSaveSlot;

                    slot.button.onClick.RemoveListener(EvaluatePopUp);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscribing"></param>
        protected virtual void SetSubscriptionToPopUps(bool subscribing)
        {
            SelectSlotOnPopUpEnd(savePopUp, subscribing);

            SelectSlotOnPopUpEnd(overwritePopUp, subscribing);

            SelectSlotOnPopUpEnd(loadPopUp, subscribing);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="popUp"></param>
        /// <param name="subscribing"></param>
        protected virtual void SelectSlotOnPopUpEnd(MenuGroupBase popUp, bool subscribing)
        {
            if (popUp == null)
                return;

            if (subscribing)
                popUp.EndEvents.AddListener(SelectSlot);
            else
                popUp.EndEvents.RemoveListener(SelectSlot);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentSlot"></param>
        protected virtual void SetCurrentSaveSlot(SaveSlotBase currentSlot) => base.currentSlot = currentSlot;

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluatePopUp()
        {
            saveLoadGroup.SetLastSelected(currentSlot.gameObject);

            if (saveMode)
            {
                saveLoadGroup.RemoveBackInput();

                if (currentSlot.HasSaveData())
                    overwritePopUp.Show();
                else
                    savePopUp.Show();
            }
            else if (currentSlot.HasSaveData())
            {
                saveLoadGroup.RemoveBackInput();

                loadPopUp.Show();
            }
            else
                OnSelectedLockedSaveSlot?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SelectSlot()
        {
            EventSystem.current.SetSelectedGameObject(currentSlot.gameObject);

            ServiceLocator.GetService<UIHandlerBase>().SetCurrent(currentSlot.gameObject);
        }


        /// <inheritdoc/>
        public override void LoadCurrentSlot()
        {
            ServiceLocator.GetService<IDialogueManager>().GetManagerComponent<DialogueDataClearer>().ClearSaveData();

            DialogueEventSystem.TriggerEvent(loadGameFileEventString, saveFilePreText + currentSlot.GetID());

            if (onPauseMenu)
                MoveToMenuOnFileSelected();
            else // On the Main Menu Screen
                DialogueEventSystem.TriggerEvent(applyDataEventString);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToMenuOnFileSelected()
        {
            ServiceLocator.GetService<MenuState>().SetSelect(false);

            loadPopUp.Hide();

            loadPopUp.RemoveBackInput();

            EventSystem.current.SetSelectedGameObject(null);

            saveLoadGroup.SetLastSelected(currentSlot.gameObject);

            saveLoadGroup.RemoveBackInput();

            StartCoroutine(ReturnToMenuPauseHandler.FadeToMenu());
        }


        /// <summary>
        ///
        /// </summary>
        protected override void LoadSlotsUI()
        {
            if (useCoroutineFileLoading)
                StartCoroutine(LoadSlotProcess());
            else
                PopulateSlotsUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator LoadSlotProcess()
        {
            for (int i = 0; i < saveSlots.Count; i++)
                if (SaveSystem.SaveFileExists(saveFilePreText + i))
                    yield return FoundSaveRoutine(saveSlots[i]);
                else
                    saveSlots[i].OnSaveNotFound();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="slot"></param>
        /// <remarks></remarks>
        protected virtual IEnumerator FoundSaveRoutine(SaveSlotBase slot)
        {
            slot.OnSaveFound();

            yield return null;

            yield return null;

            yield break;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void PopulateSlotsUI()
        {
            for (int i = 0; i < saveSlots.Count; i++)
                if (SaveSystem.SaveFileExists(saveFilePreText + i))
                    saveSlots[i].OnSaveFound();
                else
                    saveSlots[i].OnSaveNotFound();
        }
    }
}