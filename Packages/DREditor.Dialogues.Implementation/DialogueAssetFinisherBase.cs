//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueAssetFinisherBase : DialogueFinisher
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasStartedDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isWaitingForDialogueToFinish = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent SetControlsUIBasedOnCurrentMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> FinishableComponents = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void SetNoLongerInDialogueValues()
        {
            hasStartedDialogue.SetValue(false);

            inDialogue.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableMenuAccessIfNotInTrial()
        {
            if (!inTrialMode.Value && !isWaitingForDialogueToFinish.Value)
                hasMenuAccess.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetControlsBasedOnCurrentMode() => SetControlsUIBasedOnCurrentMode.Raise();
    }
}