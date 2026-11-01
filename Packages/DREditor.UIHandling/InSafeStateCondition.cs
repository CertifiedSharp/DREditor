//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public class InSafeStateCondition : EvaluateCondition
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inVideoMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;


        /// <inheritdoc/>
        protected override bool OnEvaluateCondition()
        {
            bool notInTrial = !inTrialMode.Value;

            bool notInCGOrVideo = !inCGMode.Value && !inVideoMode.Value;

            bool notInDialogueOrCGState = !inDialogue.Value || notInCGOrVideo;

            bool notInUnsafeState = notInTrial && notInDialogueOrCGState;

            return notInUnsafeState;
        }
    }
}