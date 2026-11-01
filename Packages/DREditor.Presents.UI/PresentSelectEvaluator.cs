//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PresentSelectEvaluator : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentResolver presentResolver = null;


        [Header("")]
        public UnityEvent onGavePresentSelected;

        [Header("")]
        public UnityEvent onDidntGivePresentSelected;


        /// <summary>
        ///
        /// </summary>
        public event Action OnChoiceMenuStarted;


        /// <summary>
        ///
        /// </summary>
        protected bool gavePresent = false;

        /// <summary>
        ///
        /// </summary>
        protected int selectedPresentIndex = -1;


        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedPresentIndex"></param>
        public virtual void UpdateSelectedPresentIndex(int selectedPresentIndex) => this.selectedPresentIndex = selectedPresentIndex;


        /// <summary>
        ///
        /// </summary>
        protected abstract void OnEnable();

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnDisable();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnChoiceMenuStart() => OnChoiceMenuStarted?.Invoke();

        /// <summary>
        ///
        /// </summary>
        public virtual void SubscribeCheckPresentSelect()
            => submit.action.performed += CheckPresentSelection;

        /// <summary>
        ///
        /// </summary>
        public virtual void UnsubscribeCheckPresentSelect()
            => submit.action.performed -= CheckPresentSelection;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void CheckPresentSelection(CallbackContext ctx)
        {
            if (SelectedPresentIsValid())
            {
                submit.action.performed -= CheckPresentSelection;

                submit.action.performed += EvaluatePresentChoice;

                OnSelectedValidPresent();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnSelectedValidPresent();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SelectedPresentIsValid()
        {
            if (presentDatabase.presents.Count == 0)
                return false;

            Present present = presentDatabase.presents[selectedPresentIndex];

            IAmountablePresent presentData = ServiceLocator.GetService<IPresentManager>().Get<IAmountablePresent>(present.ID);

            return present != null && presentData != null && presentData.Amount > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void EvaluatePresentChoice(CallbackContext ctx) 
            => submit.action.performed -= EvaluatePresentChoice;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnGivePresentSelected()
        {
            gavePresent = selectedPresentIndex < presentDatabase.presents.Count && presentDatabase.presents[selectedPresentIndex] != null;

            onGavePresentSelected?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDontGivePresentSelected() => onDidntGivePresentSelected?.Invoke();


        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedPresentIndex"></param>
        /// <returns></returns>
        public virtual IEnumerator HandlePresentSelection(int selectedPresentIndex)
        {
            UpdateSelectedPresentIndex(selectedPresentIndex);

            yield return presentResolver.ResolvePresentSelection(gavePresent, selectedPresentIndex);

            gavePresent = false;
        }
    }
}