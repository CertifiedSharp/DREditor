//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IndexSelector) +
        "\n\n")]
    public class ChapterSelectUIHandler : IndexSelector
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
        [SerializeField] protected InputActionReference navigate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChapterSelectCollectionBase chapterSelectDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase menuGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChapterSectionBase modifyingSection = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<ChapterSectionBase> chapterSections = new();


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent OnSubmit;


        public override int SelectionMaximum { get => GetMaxSection(); set => base.SelectionMaximum = value; }


        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            menuGroup.StartEvents.AddListener(ActivateControls);

            menuGroup.EndEvents.AddListener(DeactivateControls);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            menuGroup.StartEvents.RemoveListener(ActivateControls);

            menuGroup.EndEvents.RemoveListener(DeactivateControls);
        }

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            navigate.action.performed += EvaluateChapter;

            submit.action.started += InvokeSubmit;
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            navigate.action.performed -= EvaluateChapter;

            submit.action.started -= InvokeSubmit;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void EvaluateChapter(CallbackContext ctx) => Move((int)ctx.ReadValue<Vector2>().x);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int GetMaxSection() => 
            modifyingSection != null ? modifyingSection.GetMaxSection() : chapterSelectDatabase.GetDataCount() - 1;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void InvokeSubmit(CallbackContext ctx)
        {
            SubmitSectionIndex();

            OnSubmit.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubmitSectionIndex()
        {
            foreach (ChapterSectionBase section in chapterSections)
                section.OnParentSubmittedIndex(SelectedIndex);
        }
    }
}