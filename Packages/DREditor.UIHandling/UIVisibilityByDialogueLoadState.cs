//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class UIVisibilityByDialogueLoadState : ConditionOnSceneEvent, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveDialogueNameKey = "DialogueName";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool notLoadingDialogueSaveEvaluation = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool visibilityOnConditionPassed = true;


        /// <summary>
        ///
        /// </summary>
        protected string saveDialogueName = "";


        /// <inheritdoc/>
        protected override void OnPassedCondition()
        {
            bool enabledValue = notLoadingDialogueSaveEvaluation ? !LoadingGameFileWithDialogue() : visibilityOnConditionPassed;

            if (inInstant != null && inInstant.Value)
                enabledValue = true;

            SetVisibility(enabledValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="toBeVisible"></param>
        protected virtual void SetVisibility(bool toBeVisible)
        {
            if (uiComponent is IUIVisibility visibilityComponent)
                if (toBeVisible)
                    visibilityComponent.Show();
                else
                    visibilityComponent.Hide();
        }

        private bool LoadingGameFileWithDialogue() => isLoadingFile.Value && saveDialogueName != "";


        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => saveDialogueName = saveDatas.Get<string>(saveDialogueNameKey);
    }
}